using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Math
{
    public class ShuffleQueue<T>
    {
        private readonly ConcurrentQueue<T> _queue;
        private readonly List<T> _fullList;
            
        public ShuffleQueue(List<T> list, int requestedValues = 0)
        {
            _queue = new ConcurrentQueue<T>();
            _fullList = list.Shuffle();

            // Pre-load the queue
            if (_fullList.Any())
            {
                do
                {
                    AddElements(_fullList);
                } while (_queue.Count < requestedValues);
            }
        }

        public T Pop()
        {
            lock (_queue)
            {
                if (_queue.TryDequeue(out var result))
                {
                    return result;
                }
            }
            
            if (_fullList.Any())
            {
                AddElements(_fullList);
                return Pop();
            }

            throw new ArgumentException("Initial list was empty.");
        }

        private void AddElements(List<T> list)
        {
            var shuffledList = list.Shuffle();
            lock (_queue)
            {
                foreach (var element in shuffledList)
                {
                    _queue.Enqueue(element);
                }                
            }
        }
    }
}