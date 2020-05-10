using System.Collections.Generic;
using System.Linq;

namespace GraphAlgorithm.Services
{
    /// <summary>
    /// Kruskal Algorithm returns List<List<double>> and main minimal value - property MinimalCost
    /// </summary>
    public class KruskalAlgorithmService
    {
        public int MinimalCost { get; set; }

        private List<int> Parent = new List<int>();

        public KruskalAlgorithmService()
        {
        }

        public List<List<double>> Resolve(List<List<double>> adjacencyMatrix, bool isIncidentMatrix)
        {
            var vertexCount = adjacencyMatrix[0]?.Count;
            if (!vertexCount.HasValue)
            {
                return new List<List<double>>();
            }

            for (var i = 0; i < vertexCount; i++)
            {
                Parent.Add(i);
            }

            var adjacencyMatrixResult = new List<List<double>>();
            for (var i = 0; i < vertexCount.Value; i++)
            {
                adjacencyMatrixResult.Add(Enumerable.Repeat(double.PositiveInfinity, vertexCount.Value).ToList());
            }

            var edgeCount = 0;
            while (edgeCount < vertexCount - 1)
            {
                var min = double.PositiveInfinity;
                var a = -1;
                var b = -1;

                for (var i = 0; i < vertexCount; i++)
                {
                    for (var j = 0; j < adjacencyMatrix[i].Count; j++)
                    {
                        if (Find(i) == Find(j) || adjacencyMatrix[i][j] >= min)
                        {
                            continue;
                        }

                        min = adjacencyMatrix[i][j];
                        a = i;
                        b = j;
                    }
                }

                Union(a, b);

                adjacencyMatrixResult[a][b] = min;

                edgeCount++;

                MinimalCost += (int)min;
            }

            return adjacencyMatrixResult;
        }

        private int Find(int i)
        {
            while (Parent[i] != i)
            {
                i = Parent[i];
            }

            return i;
        }

        private void Union(int i, int j)
        {
            Parent[Find(i)] = Find(j);
        }
    }
}