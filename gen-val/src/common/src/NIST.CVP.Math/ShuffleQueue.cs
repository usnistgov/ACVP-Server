using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Math
{
    public class ShuffleQueue<T>
    {
        private readonly Queue<T> _queue;
        private readonly List<T> _fullList;
            
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
}