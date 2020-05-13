using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphAlgorithm.Services.FloydWarshall
{
    public class FloydWarshallFirtsAlgorithm
    {
        public int RowCount { get; private set; }

        public List<List<double>> Resolve(List<List<double>> adjacencyMatrix)
        {
            Validate(adjacencyMatrix);

            RowCount = adjacencyMatrix.First().Count;
           
            var weightMatrix = PrepareWeightMatrix(adjacencyMatrix);

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                for (int colIndex = 0; colIndex < RowCount; colIndex++)
                {
                    var currentNumber = weightMatrix[colIndex][rowIndex];

                    if (currentNumber.IsInt() && currentNumber > 0)
                    {
                        var Aij = weightMatrix[rowIndex].AddNumber(currentNumber);

                        weightMatrix[colIndex] = Aij.ElementwiseMinimum(weightMatrix[colIndex]);
                    }
                }
            }

            ReplaceInfinityWithZero(weightMatrix);

            return weightMatrix;
        }

        private void ReplaceInfinityWithZero(List<List<double>> matrix)
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    if (double.IsPositiveInfinity(matrix[i][j]))
                    {
                        matrix[i][j] = 0;
                    }
                }
            }
        }

        private List<List<double>> PrepareWeightMatrix(List<List<double>> adjacencyMatrix)
        {
            var algorithmMatrix = new List<List<double>>();

            for (int i = 0; i < RowCount; i++)
            {
                algorithmMatrix.Add(new List<double>());
                for (int j = 0; j < RowCount; j++)
                {
                    if (i != j && adjacencyMatrix[i][j] == 0)
                    {
                        algorithmMatrix[i].Add(double.PositiveInfinity);
                    }
                    else
                    {
                        algorithmMatrix[i].Add(adjacencyMatrix[i][j]);
                    }
                }
            }

            return algorithmMatrix;
        }

        private void Validate(List<List<double>> adjacencyMatrix)
        {
            if (adjacencyMatrix == null) throw new ArgumentNullException(nameof(adjacencyMatrix));

            if (adjacencyMatrix.Count == 0)
                throw new MethodException("Value cannot be an empty collection.");

            var rowCount = adjacencyMatrix.First().Count;

            if (adjacencyMatrix.Any(e => e.Count != rowCount)) throw new MethodException("The matrix must be square");

            // check if all values are integers
            if (adjacencyMatrix.SelectMany(x => x.ToList()).Any(e => !e.IsInt()))
            {
                throw new MethodException($"{nameof(FloydWarshallFirtsAlgorithm)} supports integers only.");
            }

            if (adjacencyMatrix.SelectMany(x => x.ToList()).Any(e => e < 0))
            {
                throw new MethodException($"{nameof(FloydWarshallFirtsAlgorithm)} supports positive integers only.");
            }
        }
    }
}
