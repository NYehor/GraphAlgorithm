using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphAlgorithm.Services.SearchTree
{
    public class SearchTree
    {
        public List<Node> nodes = new List<Node>();

        public List<int> resultSequence = new List<int>();


        public List<int> StartWideSearch(int[,] matrix, int startNodeId)
        {
            ConvertMatrixToNodes(matrix);
            if (startNodeId < 0 && startNodeId >= nodes.Count)
            {
                throw new ArgumentOutOfRangeException("Node id out of range.");
            }

            SetNodesStatusToNew();

            Node node = nodes[startNodeId];
            resultSequence.Add(node.Id);
            node.IsNew = false;

            List<int> newNodes = new List<int>();
            newNodes.AddRange(node.Nodes);

            while (newNodes.Count != 0)
            {
                newNodes = FindAllWideNodes(newNodes);
            }



            for (int i = 0; i < resultSequence.Count; i++)
            {
                Console.WriteLine(resultSequence[i]);
            }
            return resultSequence;

        }


        public List<int> StartDeepSearch(int[,] matrix, int startNodeId)
        {
            ConvertMatrixToNodes(matrix);
            if (startNodeId < 0 && startNodeId >= nodes.Count)
            {
                throw new ArgumentOutOfRangeException("Node id out of range.");
            }

            SetNodesStatusToNew();

            Node node = nodes[startNodeId];
            resultSequence.Add(node.Id);
            node.IsNew = false;

            FindDeepNode(node);

            for (int i = 0; i < resultSequence.Count; i++)
            {
                Console.WriteLine(resultSequence[i]);
            }

            return resultSequence;
        }


        private void FindDeepNode(Node node)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                Node current = nodes[node.Nodes[i]];

                if (!current.IsNew)
                {
                    continue;
                }
                resultSequence.Add(current.Id);
                current.IsNew = false;
                FindDeepNode(current);
            }
        }

        private void ConvertMatrixToNodes(int[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new ArgumentException("Incorrect matrix");
            }
            int lenght = matrix.GetLength(0);

            for (int i = 0; i < lenght; i++)
            {
                Node node = new Node(i, true);
                for (int j = 0; j < lenght; j++)
                {
                    if (matrix[i, j] > 0)
                    {
                        node.AddNode(j);
                    }
                }

                nodes.Add(node);
            }

        }

        private List<int> FindAllWideNodes(List<int> nodeIds)
        {
            List<int> result = new List<int>();


            for (int i = 0; i < nodeIds.Count; i++)
            {
                Node node = nodes[nodeIds[i]];
                if (!node.IsNew)
                {
                    continue;
                }
                resultSequence.Add(node.Id);
                node.IsNew = false;
                result.AddRange(node.Nodes);
            }


            return result;
        }

        private void SetNodesStatusToNew()
        {
            resultSequence = new List<int>();
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].IsNew = true;
            }
        }

    }
}