using System.Collections.Generic;
using System.Linq;

namespace GraphAlgorithm.Services
{
    public class HamiltonianCycleAlgirithmService
    {
        private const double INF = double.PositiveInfinity;

        private int _n;
        private List<bool> _isVisitedVertexes;
        private List<int> _path;
        List<List<double>> _adjacencyMatrix;
        private string _failureReason;

        public int[] Path
        {
            get
            {
                return _path.ToArray();
            }
        }

        public List<List<double>> Resolve(List<List<double>> adjacencyMatrix, bool isIncidentMatrix)
        {
            var adjacencyMatrixResult = new List<List<double>>();
            _n = adjacencyMatrix.Count;
            _failureReason = string.Empty;
            _adjacencyMatrix = adjacencyMatrix;
            _isVisitedVertexes = Enumerable.Repeat(false, _n).ToList();
            _path = new List<int>();
            if (CheckConnectivity())
            {
                _isVisitedVertexes = Enumerable.Repeat(false, _n).ToList();
                FindHamiltonianCycle(0);
            }

            if (_path.Count > 0)
            {
                adjacencyMatrixResult = ConvertPathToAdjacencyMatrix();
            }
            else
            {
                throw new MethodException("Гамільтонів цикл не існує. " + _failureReason);
            }

            return adjacencyMatrixResult;
        }

        private bool FindHamiltonianCycle(int curVertex)
        {
            _path.Add(curVertex);
            if (_path.Count == _n)
            {
                if (_adjacencyMatrix[_path[_n - 1]][_path[0]] < INF) // check if the first and last vertexes are adjacent
                {
                    return true;
                }
                else
                {
                    _path.RemoveAt(_n - 1);
                    return false;
                }
            }

            _isVisitedVertexes[curVertex] = true;
            for (int nextVertex = 0; nextVertex < _n; nextVertex++)
            {
                if (_adjacencyMatrix[curVertex][nextVertex] < INF && !_isVisitedVertexes[nextVertex]
                    && FindHamiltonianCycle(nextVertex))
                {
                    return true;
                }
            }

            _isVisitedVertexes[curVertex] = false;
            _path.RemoveAt(_path.Count - 1);
            return false;
        }

        private bool CheckConnectivity()
        {
            bool result;
            int nComp = 0;
            for (int curVertex = 0; curVertex < _n; curVertex++)
            {
                if (!_isVisitedVertexes[curVertex])
                {
                    nComp++;
                    dfs(curVertex);
                }
            }

            result = nComp == 1;

            if (!result)
            {
                _failureReason = "Граф не є зв'язним.";
            }

            return result;
        }

        private void dfs(int curVertex)
        {
            _isVisitedVertexes[curVertex] = true;
            for (int nextVertex = 0; nextVertex < _n; nextVertex++)
            {
                if (_adjacencyMatrix[curVertex][nextVertex] < INF && !_isVisitedVertexes[nextVertex])
                {
                    dfs(nextVertex);
                }
            }
        }

        private List<List<double>> ConvertPathToAdjacencyMatrix()
        {
            var adjacencyMatrixResult = new List<List<double>>();
            for (var i = 0; i < _n; i++)
            {
                adjacencyMatrixResult.Add(Enumerable.Repeat(double.PositiveInfinity, _n).ToList());            
            }

            for (var i = 0; i < _n - 1; i++)
            {
                adjacencyMatrixResult[i][_path[i + 1]] = _adjacencyMatrix[_path[i]][_path[i + 1]];
            }

            adjacencyMatrixResult[_n - 1][_path[0]] = _adjacencyMatrix[_path[_n - 1]][_path[0]];

            return adjacencyMatrixResult;
        }      
    }
}