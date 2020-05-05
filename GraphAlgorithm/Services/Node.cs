using System.Collections.Generic;

namespace SearchTree
{
    class Node
    {
        public int Id { get; set; }
        public List<int> Nodes = new List<int>();

        public bool IsNew { get; set; }

        public Node(int id, bool isNew)
        {
            Id = id;
            IsNew = isNew;
        }

        public void AddNode(int nodeId)
        {
            this.Nodes.Add(nodeId);
        }
    }
}
