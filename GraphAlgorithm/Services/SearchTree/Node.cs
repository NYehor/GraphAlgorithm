using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphAlgorithm.Services.SearchTree
{
    public class Node
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