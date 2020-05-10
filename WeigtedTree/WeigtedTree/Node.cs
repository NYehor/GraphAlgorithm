using System;
using System.Collections.Generic;
using System.Text;

namespace WeigtedTree
{
    class Node<T> : IComparable
    {
        internal Node(double probability, T value)
        {
            Probability = probability;
            LeftSon = RightSon = Parent = null;
            Value = value;
            IsLeaf = true;
        }

        internal Node(Node<T> leftSon, Node<T> rightSon)
        {
            LeftSon = leftSon;
            RightSon = rightSon;
            Probability = leftSon.Probability + rightSon.Probability;
            leftSon.IsZero = true;
            rightSon.IsZero = false;
            leftSon.Parent = rightSon.Parent = this;
            IsLeaf = false;
        }

        internal Node<T> LeftSon { get; set; }
        internal Node<T> RightSon { get; set; }
        internal Node<T> Parent { get; set; }
        internal T Value { get; set; }
        internal bool IsLeaf { get; set; }
        internal  bool IsZero { get; set; }
        internal int Bit
        {
            get { return IsZero ? 0 : 1; }
        }

        internal bool IsRoot
        {
            get { return Parent == null; }
        }

       double Probability { get; set; }

        public int CompareTo(object obj)
        {
            return -Probability.CompareTo(((Node<T>)obj).Probability);
        }
    }
}
	