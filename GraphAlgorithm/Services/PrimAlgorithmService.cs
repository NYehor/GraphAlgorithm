using System.Collections.Generic;
using System.Linq;

namespace GraphAlgorithm.Services
{
    /// <summary>
    /// Prim's Algorithm returns List<List<double>> and main minimal value - property MinimalCost
    /// </summary>
    public class PrimAlgorithmService
    {
        public int MinimalCost { get; set; }

        public PrimAlgorithmService()
        {
        }

        public List<List<double>> Resolve(List<List<double>> adjacencyMatrix, bool isIncidentMatrix)
        {
            var vertexCount = adjacencyMatrix[0]?.Count;
            if (!vertexCount.HasValue)
            {
                return new List<List<double>>();
            }

            var inMST = new bool[vertexCount.Value];

            // Include first vertex in MST 
            inMST[0] = true;

            var adjacencyMatrixResult = new List<List<double>>();
            for (var i = 0; i < vertexCount.Value; i++)
            {
                adjacencyMatrixResult.Add(Enumerable.Repeat(double.PositiveInfinity, vertexCount.Value).ToList());
            }

            var edgeCount = 0;
            while (edgeCount < vertexCount - 1)
            {
                // Find minimum weight valid edge.  
                var min = double.PositiveInfinity;
                var a = -1;
                var b = -1;
                for (var i = 0; i < vertexCount; i++)
                {
                    for (var j = 0; j < adjacencyMatrix[i].Count; j++)
                    {
                        if (!(adjacencyMatrix[i][j] < min) || !IsValidEdge(i, j, inMST))
                        {
                            continue;
                        }

                        min = adjacencyMatrix[i][j];
                        a = i;
                        b = j;
                    }
                }

                if (a == -1 || b == -1)
                {
                    continue;
                }

                adjacencyMatrixResult[a][b] = min;

                MinimalCost += (int)min;

                edgeCount++;

                inMST[b] = inMST[a] = true;
            }

            return adjacencyMatrixResult;
        }

        private bool IsValidEdge(int u, int v, bool[] inMST)
        {
            return u != v && ((inMST[u] || inMST[v]) && (!inMST[u] || !inMST[v]));
        }
    }
}