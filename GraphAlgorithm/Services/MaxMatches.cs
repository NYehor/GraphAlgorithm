using System;
using System.Collections.Generic;

namespace GraphAlgorithm.Services
{
    public class MaxMatches
    {
        bool FindPath(int u1, List<List<double>> matrix, List<int> matching, List<bool> visited)
        {
            visited[u1] = true;
            for (int v = 0; v < matching.Count; ++v)
            {
                int u2 = matching[v];

                if (matrix[u1][v] == 1 && (u2 == -1 || !visited[u2] && FindPath(u2, matrix, matching, visited)))
                {
                    matching[v] = u1;
                    return true;
                }
            }
            return false;
        }

        public List<List<double>> Resolve(List<List<double>> matrix, bool isIncidentMatrix)
        {
            if (!ValidateMatrix(matrix))
            {
                throw new MethodException("Matrix is not valid.");
            }

            int leftPart = matrix.Count;
            int rightPart = matrix[0].Count;

            List<int> matching = new List<int>();

            matching.FillInt(-1, rightPart);

            for (int i = 0; i < leftPart; i++)
                FindPath(i, matrix, matching, new List<bool>().FillBool(false, rightPart));

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    matrix[i][j] = 0;

                    int index = matching.IndexOf(j);
                    if (index >= 0)
                    {
                        matrix[index][j] = 1;
                    }
                }
            }
            return matrix;
        }

        private bool ValidateMatrix(List<List<double>> matrix)
        {
            if (matrix.Count == 0)
                return false;

            for (int i = 0; i < matrix.Count; i++)
            {
                if (matrix[i].Count == 0)
                    return false;
            }

            return true;
        }
    }

    public static class ListExtension
    {
        public static List<int> FillInt(this List<int> matrix, int value, int length)
        {
            for (int i = 0; i < length; i++)
            {
                matrix.Add(value);
            }
            return matrix;
        }

        public static List<bool> FillBool(this List<bool> matrix, bool value, int length)
        {
            for (int i = 0; i < length; i++)
            {
                matrix.Add(value);
            }
            return matrix;
        }
    }
}