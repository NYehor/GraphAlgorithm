using System.Collections.Generic;

namespace GraphAlgorithm.Services
{
    public class MaxMatches
    {
        List<List<double>> edgesMatrix;
        List<int> u = new List<int>();
        List<int> v = new List<int>();

        bool FindPath(int u1, List<List<double>> matrix, List<int> matching, List<bool> visited)
        {
            visited[u1] = true;
            foreach (int vi in v)
            {
                int u2 = matching[vi];

                if (matrix[u1][vi] == 1 && (u2 == -1 || !visited[u2] && FindPath(u2, matrix, matching, visited)))
                {
                    matching[vi] = u1;
                    return true;
                }
            }
            return false;
        }

        public List<List<double>> Resolve(List<List<double>> matrix, bool isIncidentMatrix)
        {
            ValidateMatrix(matrix);

            int leftPart = u.Count;
            int rightPart = v.Count;

            List<int> matching = new List<int>();

            matching.FillInt(-1, rightPart + leftPart);

            foreach (int ui in u)
                FindPath(ui, matrix, matching, new List<bool>().FillBool(false, leftPart));

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    matrix[i][j] = 0;

                    int index = matching.IndexOf(j);
                    if (index >= 0)
                    {
                        matrix[index][j] = 1;
                        matrix[j][index] = 1;
                    }
                }
            }
            return matrix;
        }

        private void ValidateMatrix(List<List<double>> matrix)
        {
            if (matrix.Count == 0)
                throw new MethodException("Матриця має некоректні дані.");

            for (int i = 0; i < matrix.Count; i++)
            {
                if (matrix[i].Count == 0)
                    throw new MethodException("Матриця має некоректні дані.");

                for (int j = 0; j < matrix[i].Count; j++)
                {
                    if (matrix[i][j] != 1 && matrix[i][j] != 0)
                    {
                        throw new MethodException("Матриця повинна складатися тільки з 0 або 1.");
                    }
                }
            }

            if (!CheckDicotyledonousGraph(matrix))
            {
                throw new MethodException("Граф не є двоїстим.");
            }
        }

        public bool CheckDicotyledonousGraph(List<List<double>> g)
        {
            Queue<int> q = new Queue<int>();
            int u = g.Count - 1;
            bool[] used = new bool[g.Count];
            int[] paintedVertexes = new int[g.Count];
            bool Dicotyledonous = true;
            int color = 2;

            used[u] = true;

            q.Enqueue(u);

            while (q.Count != 0 && Dicotyledonous)
            {
                u = q.Peek();
                q.Dequeue();

                if (paintedVertexes[u] == 0)
                {
                    paintedVertexes[u] = color;
                    color = color == 1 ? 2 : 1;
                }

                for (int i = 0; i < g.Count; i++)
                {
                    if (g[u][i] == 1)
                    {
                        if (paintedVertexes[i] == paintedVertexes[u])
                        {
                            Dicotyledonous = false;
                            break;
                        }

                        if (!used[i])
                        {
                            used[i] = true;
                            paintedVertexes[i] = paintedVertexes[u] == 1 ? 2 : 1;
                            q.Enqueue(i);
                        }
                    }
                }
            }

            if (Dicotyledonous)
            {
                BuildEdgeMatrix(paintedVertexes);
            }

            return Dicotyledonous;
        }

        private void BuildEdgeMatrix(int[] paintedVertexes)
        {
            for (int i = 0; i < paintedVertexes.Length; i++)
            {
                if (paintedVertexes[i] == 1)
                {
                    u.Add(i);
                }
                else
                {
                    v.Add(i);
                }
            }
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