using System.Collections.Generic;

namespace GraphAlgorithm.Services.FloydWarshall
{
    // Please, be patient
    public static class ListArray2DConverter
    {
        public static double[,] ToArray2D(this List<List<double>> matrix)
        {
            var rowCount = matrix.Count;

            var array2D = new double[rowCount, rowCount];

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    array2D[i, j] = matrix[i][j];
                }
            }

            return array2D;
        }

        public static List<List<double>> ToListOfLists(this double[,] matrix)
        {
            var rowCount = matrix.GetLength(0);

            var output = new List<List<double>>();

            for (int i = 0; i < rowCount; i++)
            {
                output.Add(new List<double>());
                for (int j = 0; j < rowCount; j++)
                {
                    output[i].Add(matrix[i, j]);
                }
            }

            return output;
        }
    }
}