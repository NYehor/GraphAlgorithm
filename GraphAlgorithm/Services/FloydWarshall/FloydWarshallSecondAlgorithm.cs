using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace GraphAlgorithm.Services.FloydWarshall
{
    public class FloydWarshallSecondAlgorithm : IMethod
    {
        public int RowCount { get; private set; }

        public List<List<double>> Resolve(List<List<double>> adjacencyMatrix, bool isIncidentMatrix = false)
        {
            if (isIncidentMatrix)
            {
                // TODO: convert
                throw new NotImplementedException("Only adjacency matrices are supported");
            }

            Validate(adjacencyMatrix);
            RowCount = adjacencyMatrix.First().Count;

            var weightMatrix = PrepareWeightMatrix(adjacencyMatrix.ToArray2D());

            for (int k = 0; k < RowCount; k++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < RowCount; j++)
                    {
                        weightMatrix[i, j] = Min(weightMatrix[i, j], weightMatrix[i, k] + weightMatrix[k, j]);
                    }
                }
            }

            // TODO: should I return 0 or Infinity
            ReplaceInfinityWithZero(weightMatrix);

            return weightMatrix.ToListOfLists();
        }

        private void ReplaceInfinityWithZero(double[,] matrix)
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    if (double.IsPositiveInfinity(matrix[i, j]))
                    {
                        matrix[i, j] = 0;
                    }
                }
            }
        }

        private double[,] PrepareWeightMatrix(double[,] adjacencyMatrix)
        {
            var algorithmMatrix = new double[RowCount, RowCount];

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    if (i == j || adjacencyMatrix[i, j] == 0)
                    {
                        algorithmMatrix[i, j] = double.PositiveInfinity;
                    }
                    else
                    {
                        algorithmMatrix[i, j] = adjacencyMatrix[i, j];
                    }
                }
            }

            return algorithmMatrix;
        }

        private void Validate(List<List<double>> adjacencyMatrix)
        {
            if (adjacencyMatrix == null) throw new ArgumentNullException(nameof(adjacencyMatrix));

            if (adjacencyMatrix.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(adjacencyMatrix));

            var rowCount = adjacencyMatrix.First().Count;

            if (adjacencyMatrix.Any(e => e.Count != rowCount)) throw new Exception("The matrix must be square");
        }
    }
}
