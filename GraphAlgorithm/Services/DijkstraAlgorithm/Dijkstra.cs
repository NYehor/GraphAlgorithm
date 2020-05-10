using System;
using System.Collections.Generic;
using System.Text;

namespace GraphAlgorithm.Services
{
    class Dijkstra
    {
        const double INF = double.PositiveInfinity;
        public List<List<double>> AdjacencyMatrix = new List<List<double>>
        {
            new List<double> { INF, 2,   INF, 6,   INF  },
            new List<double> { 2,   INF, 4,   INF, 8   },
            new List<double> { INF, 4,   INF, 6,   2   },
            new List<double> { 6,   INF, 6,   INF, 4    },
            new List<double> { INF, 8,   2,   4,   INF }
        };

        private Graph graph;

        //TODO: set matrix here
        public Dijkstra()
        {
            graph = new Graph(AdjacencyMatrix);
        }

        public object Resolve(int startVertice)
        {
            var currentVertice = graph.Vertices[startVertice - 1];
            double currentPathLength = 0;
            currentVertice.ShortestPathLength = 0;

            while(!graph.AreAllNodesRemoved())
            {
                foreach (var sibling in currentVertice.Siblings)
                {
                    if (sibling.IsRemoved)
                    {
                        continue;
                    }

                    var transitionWeight = getTransitionWeight(currentVertice, sibling);
                    var fullLength = currentPathLength + transitionWeight;

                    if (fullLength < sibling.ShortestPathLength)
                    {
                        sibling.ShortestPathLength = fullLength;
                        sibling.NearestVertice = currentVertice;
                    }
                }

                // move to next Vertice
                var nextMinVertice = getMinVertice(graph.Vertices);
                currentVertice.IsRemoved = true;
                if (nextMinVertice == null)
                {
                    continue;
                }

                currentPathLength = nextMinVertice.ShortestPathLength;
                currentVertice = nextMinVertice;
            }

            var result = RestoreShortestPaths();

            string strResult = string.Empty;
            foreach (var r in result)
            {
                strResult += r.ToString();
                Console.WriteLine(r.ToString());
            }

            
            return result;
        }

        private double getTransitionWeight(Vertice from, Vertice to)
        {
            if (from == null || to == null)
            {
                return double.PositiveInfinity;
            }
            return AdjacencyMatrix[from.Number][to.Number];
        }

        private Vertice getMinVertice(List<Vertice> vertices)
        {
            var minVertice = vertices.Find(v => !v.IsRemoved);
            foreach (var v in vertices)
            {
                if (!v.IsRemoved && v.ShortestPathLength < minVertice.ShortestPathLength)
                {
                    minVertice = v;
                }
            }
            return minVertice;
        }

        private List<AlgorithmResult> RestoreShortestPaths()
        {
            var shortestPaths = new List<AlgorithmResult>();
            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                var shortestPath = new List<int>();
                var currentVertice = graph.Vertices[i];
                while (currentVertice.NearestVertice != null)
                {
                    shortestPath.Add(currentVertice.Number + 1);
                    currentVertice = currentVertice.NearestVertice;
                }
                shortestPath.Add(currentVertice.Number + 1);
                shortestPath.Reverse();
                shortestPaths.Add(new AlgorithmResult(shortestPath, graph.Vertices[i].ShortestPathLength, i + 1));
            }
            return shortestPaths;
        }

        private class AlgorithmResult
        {
            public List<int> Path { get; private set; }
            public double PathLength { get; private set; }
            public int VerticeNumber { get; private set; }
            public AlgorithmResult(List<int> path, double length, int number)
            {
                Path = path;
                PathLength = length;
                VerticeNumber = number;
            }

            public override string ToString()
            {
                var stringPath = $"[ ";

                for (int i = 0; i < Path.Count; i++)
                {
                    stringPath += Path[i];
                    stringPath += i < Path.Count - 1 ? ", " : " ]";
                }
                return $"Вершина: {VerticeNumber} - Длина: {PathLength} - Путь: {stringPath}";
            }
        }
    }
}
