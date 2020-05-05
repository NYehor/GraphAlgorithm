using System;
using System.Collections.Generic;

namespace dijkstra
{
    public class Graph
    {
        public List<List<double>> AdjacencyMatrix { get; set; }
        public List<Vertice> Vertices { get; set; }

        public Graph(List<List<double>> matrix)
        {
            AdjacencyMatrix = matrix;
            Vertices = new List<Vertice>();
            for (int i = 0; i < matrix.Count; i++)
            {
                Vertices.Add(new Vertice(i));
            }
            LinkNodes();
        }

        public bool AreAllNodesRemoved()
        {
            var allremoved = true;
            foreach (var item in Vertices)
            {
                if (!item.IsRemoved)
                {
                    allremoved = false;
                }
            }
            return allremoved;
        }

        private void LinkNodes()
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                for (int j = 0; j < Vertices.Count; j++)
                {   
                    if (!double.IsInfinity(AdjacencyMatrix[i][j]))
                    {
                        Vertices[i].Siblings.Add(Vertices[j]);
                    }
                }
            }
        }
    }

    public class Vertice
    {
        public int Number { get; set; }
        public bool IsRemoved { get; set; } = false;
        public List<Vertice> Siblings { get; set; } = new List<Vertice>();
        public Vertice NearestVertice { get; set; } = null;
        public double ShortestPathLength { get; set; } = double.PositiveInfinity;



        public Vertice(int number)
        {
            Number = number;
        }
    }
}
