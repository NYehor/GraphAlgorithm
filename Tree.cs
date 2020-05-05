using System;
using System.Collections.Generic;
using System.Text;

namespace WeigtedTree
{
	class Tree<T> where T : IComparable
    {
        private readonly Dictionary<T, Node<T>> _leafDictionary = new Dictionary<T, Node<T>>();
        private readonly Node<T> _root;

        public Tree(IEnumerable<T> values)
        {
            var counts = new Dictionary<T, int>();
            var priorityQueue = new PriorityQueue<Node<T>>();
            int valueCount = 0;

            foreach (T value in values)
            {
                if (!counts.ContainsKey(value))
                {
                    counts[value] = 0;
                }
                counts[value]++;
                valueCount++;
            }

            foreach (T value in counts.Keys)
            {
                var node = new Node<T>((double)counts[value] / valueCount, value);
                priorityQueue.Add(node);
                _leafDictionary[value] = node;
            }

            while (priorityQueue.Count > 1)
            {
                Node<T> leftSon = priorityQueue.Pop();
                Node<T> rightSon = priorityQueue.Pop();
                var parent = new Node<T>(leftSon, rightSon);
                priorityQueue.Add(parent);
            }

            _root = priorityQueue.Pop();
            _root.IsZero = false;
        }

        public List<int> Encode(T value)
        {
            var returnValue = new List<int>();
            Encode(value, returnValue);
            return returnValue;
        }

        public void Encode(T value, List<int> encoding)
        {
            if (!_leafDictionary.ContainsKey(value))
            {
                throw new ArgumentException("Invalid value in Encode");
            }
            Node<T> nodeCur = _leafDictionary[value];
            var reverseEncoding = new List<int>();
            while (!nodeCur.IsRoot)
            {
                reverseEncoding.Add(nodeCur.Bit);
                nodeCur = nodeCur.Parent;
            }

            reverseEncoding.Reverse();
            encoding.AddRange(reverseEncoding);
        }

        public List<int> Encode(IEnumerable<T> values)
        {
            var returnValue = new List<int>();

            foreach (T value in values)
            {
                Encode(value, returnValue);
            }
            return returnValue;
        }

        public T Decode(List<int> bitString, ref int position)
        {
            Node<T> nodeCur = _root;
            while (!nodeCur.IsLeaf)
            {
                if (position > bitString.Count)
                {
                    throw new ArgumentException("Invalid bitstring in Decode");
                }
                nodeCur = bitString[position++] == 0 ? nodeCur.LeftSon : nodeCur.RightSon;
            }
            return nodeCur.Value;
        }

        public List<T> Decode(List<int> bitString)
        {
            int position = 0;
            var returnValue = new List<T>();

            while (position != bitString.Count)
            {
                returnValue.Add(Decode(bitString, ref position));
            }
            return returnValue;
        }
    }

}

