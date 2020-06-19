using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Math
{
    public class ShuffleQueue<T>
    {
        private readonly Queue<T> _queue;
        private readonly List<T> _fullList;
            
        public ShuffleQueue(List<T> list, int requestedValues = 0)
        {
            _queue = new Queue<T>();
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

        public T Pop() => _queue.Dequeue();
        
        private void AddElements(List<T> list)
        {
            var shuffledList = list.Shuffle();
            foreach (var element in shuffledList)
            {
                _queue.Enqueue(element);
            }
        }
    }
}