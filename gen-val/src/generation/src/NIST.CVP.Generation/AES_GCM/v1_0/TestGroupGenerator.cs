using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_GCM.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

            parameters.IvLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
            parameters.PayloadLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
            parameters.AadLen.SetRangeOptions(RangeDomainSegmentOptions.Random);

            // iv length of 96 is a special case, if it's in the domain, include it.
            var ivLengths = new List<int>();
            var ivLengthMinMax = parameters.IvLen.GetDomainMinMax();
            const int ivLenSpecialCase = 96;
            if (ivLengthMinMax.Minimum < ivLenSpecialCase)
            {
                ivLengths.AddRangeIfNotNullOrEmpty(parameters.IvLen.GetValues(ivLengthMinMax.Minimum, ivLenSpecialCase - 1, 2));
            }
            
            if (ivLengthMinMax.Maximum >= ivLenSpecialCase)
            {
                ivLengths.AddRangeIfNotNullOrEmpty(parameters.IvLen.GetValues(ivLenSpecialCase, ivLenSpecialCase, 1));
            }
            
            if (ivLengthMinMax.Maximum > ivLenSpecialCase)
            {
                ivLengths.AddRangeIfNotNullOrEmpty(parameters.IvLen.GetValues(ivLenSpecialCase + 1, ivLengthMinMax.Maximum, 2));
            }

            var ptLengths = new List<int>();
            ptLengths.AddRangeIfNotNullOrEmpty(parameters.PayloadLen.GetDomainMinMaxAsEnumerable());
            
            // Get block length values
            ptLengths.AddRangeIfNotNullOrEmpty(parameters.PayloadLen.GetValues(g => g % 128 == 0 && !ptLengths.Contains(g), 2, true));
            
            // Get non block length values
            ptLengths.AddRangeIfNotNullOrEmpty(parameters.PayloadLen.GetValues(g => g % 8 == 0 && g % 128 != 0 && !ptLengths.Contains(g), 2, true));

            var aadLengths = GetTestableValuesFromCapability(parameters.AadLen);
            var tagLengths = ParameterValidator.VALID_TAG_LENGTHS.Where(validTagLength => parameters.TagLen.IsWithinDomain(validTagLength)).ToList();

            // sanity check, should be caught by parameter validator
            if (tagLengths.Count == 0)
            {
                throw new ArgumentException("No valid tag lengths found within parameters");
            }

            var lengths = new List<int> { ivLengths.Count, ptLengths.Count, aadLengths.Count, tagLengths.Count };
            var maxLengthParameter = lengths.Max();
            var ivQueue = new ShuffleQueue<int>(ivLengths);
            var ptQueue = new ShuffleQueue<int>(ptLengths);
            var aadQueue = new ShuffleQueue<int>(aadLengths);
            var tagQueue = new ShuffleQueue<int>(tagLengths);

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    for (var i = 0; i < maxLengthParameter; i++)
                    {
                        var testGroup = new TestGroup
                        {
                            AlgoMode = algoMode,
                            Function = function,
                            IvLength = ivQueue.Pop(),
                            PayloadLength = ptQueue.Pop(),
                            KeyLength = keyLength,
                            AadLength = aadQueue.Pop(),
                            TagLength = tagQueue.Pop(),
                            IvGeneration = parameters.IvGen,
                            IvGenerationMode = parameters.IvGenMode
                        };
                        
                        testGroups.Add(testGroup);
                    }
                }
            }

            return Task.FromResult(testGroups);
        }

        private class ShuffleQueue<T>
        {
            private Queue<T> _queue;
            private List<T> _fullList;
            
            public ShuffleQueue(List<T> list)
            {
                _queue = new Queue<T>();
                _fullList = list.Shuffle();
                AddElements(_fullList);
            }

            public T Pop()
            {
                // If the queue is empty, re-queue the stored elements
                if (!_queue.Any())
                {
                    // It is possible for the list coming in to be empty too. 
                    if (!_fullList.Any())
                    {
                        return default;
                    }
                    
                    var shuffledList = _fullList.Shuffle();
                    AddElements(shuffledList);
                }

                return _queue.Dequeue();
            }

            private void AddElements(List<T> list)
            {
                foreach (var element in _fullList)
                {
                    _queue.Enqueue(element);
                }
            }
        }

        private List<int> GetTestableValuesFromCapability(MathDomain capability)
        {
            var minMaxDomain = capability.GetDomainMinMaxAsEnumerable();
            var randomCandidates = capability.GetValues(10).ToList();
            var testValues = new List<int>();
            testValues.AddRangeIfNotNullOrEmpty(minMaxDomain.Distinct());
            testValues
                .AddRangeIfNotNullOrEmpty(
                    randomCandidates
                        .Except(testValues)
                        .OrderBy(ob => Guid.NewGuid())
                        .Take(2)
                );

            return testValues;
        }
    }
}