using System.Collections.Generic;
using System.Linq;

namespace GraphAlgorithm.Services
{
    /// <summary>
    /// Kruskal Algorithm returns List<List<int>> and main minimal value - property MinimalCost
    /// </summary>
    public class KruskalAlgorithmService : IMethod
    {
        public int MinimalCost { get; set; }

        private List<int> Parent = new List<int>();

        public KruskalAlgorithmService()
        {
        }

        public List<List<int>> Resolve(List<List<int>> adjacencyMatrix, bool isIncidentMatrix)
        {
            var vertexCount = adjacencyMatrix[0]?.Count;
            if (!vertexCount.HasValue)
            {
                return new List<List<int>>();
            }

            for (var i = 0; i < vertexCount; i++)
            {
                Parent.Add(i);
            }

            var adjacencyMatrixResult = new List<List<int>>();
            for (var i = 0; i < vertexCount.Value; i++)
            {
                adjacencyMatrixResult.Add(Enumerable.Repeat(int.MaxValue, vertexCount.Value).ToList());
            }

            var edgeCount = 0;
            while (edgeCount < vertexCount - 1)
            {
                var min = int.MaxValue;
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

                MinimalCost += min;
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