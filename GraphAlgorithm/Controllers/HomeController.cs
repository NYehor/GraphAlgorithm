using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GraphAlgorithm.Models;
using GraphAlgorithm.Services;
using Newtonsoft.Json;
using GraphAlgorithm.Services.FloydWarshall;
using GraphAlgorithm.Services.SearchTree;

namespace GraphAlgorithm.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult SetMatrix(IndexViewModel indexViewModel = null)
        {
            return View("Index", indexViewModel);
        }
        public ActionResult Index()
        {
            return View("Index", new IndexViewModel());
        }

        public ActionResult Wiki()
        {
            return View();
        }

        public ActionResult AdjacencyMatrix()
        {
            return View();
        }

        public ActionResult IncindenceMatrix()
        {
            return View();
        }
        public ActionResult AddVertices()
        {
            return View("Wiki/AddVertices");
        }

        public ActionResult AddEdges()
        {
            return View("Wiki/AddEdges");
        }

        public ActionResult AlgorithmSelection()
        {
            return View("Wiki/AlgorithmSelection");
        }

        public ActionResult MovingVertices()
        {
            return View("Wiki/MovingVertices");
        }

        public ActionResult Graph()
        {
            return View("Wiki/Graph");
        }

        public ActionResult AddIncMatrix()
        {
            return View("Wiki/AddIncMatrix");
        }

        public ActionResult AddAdjMatrix()
        {
            return View("Wiki/AddAdjMatrix");
        }
        public ActionResult Kruskal()
        {
            return View("Algorithms/Kruskal");
        }

        public ActionResult Prim()
        {
            return View("Algorithms/Prim");
        }

        public ActionResult HamiltonianCycle()
        {
            return View("Algorithms/HamiltonianCycle");
        }

        public ActionResult DeepSearchTree()
        {
            return View("Algorithms/DeepSearchTree");
        }

        public ActionResult MaxMatches()
        {
            return View("Algorithms/MaxMatches");
        }

        public ActionResult FloydWarshall()
        {
            return View("Algorithms/FloydWarshall");
        }

        public ActionResult Dijkstra()
        {
            return View("Algorithms/Dijkstra");
        }

        public ActionResult WideSearchTree()
        {
            return View("Algorithms/WideSearchTree");
        }


        [HttpGet]
        public JsonResult KruskalAlgorithm(string data)
        {
            var matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            Object result;

            try
            {
                if (matrix == null || !IsSymmetricMatrix(matrix))
                {
                    throw new MethodException("Матриця не валiдна або задано орiєнтовний граф. Даний алгоритм може працювати лише iз неорiєнтовними графами");
                }

                if (!IsConnectivityGraph(matrix))
                {
                    throw new MethodException("Граф не є зв'язним");
                }

                matrix = replaceZeroToInf(matrix);

                var kruskalAlgorithm = new KruskalAlgorithmService();
                var resultMatrix = kruskalAlgorithm.Resolve(matrix, false);

                resultMatrix = replaceInfToZero(resultMatrix);

                result = new
                {
                    exception = "",
                    matrix = resultMatrix,
                    minimalCost = kruskalAlgorithm.MinimalCost
                };
            }
            catch (MethodException ex)
            {
                result = new
                {
                    exception = ex.Message
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrimAlgorithm(string data)
        {
            var matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            Object result;

            try
            {
                if (matrix == null || !IsSymmetricMatrix(matrix))
                {
                    throw new MethodException("Матриця не валiдна або задано орiєнтовний граф. Даний алгоритм може працювати лише iз неорiєнтовними графами");
                }

                if (!IsConnectivityGraph(matrix))
                {
                    throw new MethodException("Граф не є зв'язним");
                }

                matrix = replaceZeroToInf(matrix);

                var primAlgorithm = new PrimAlgorithmService();
                var resultMatrix = primAlgorithm.Resolve(matrix, false);

                resultMatrix = replaceInfToZero(resultMatrix);

                result = new
                {
                    exception = "",
                    matrix = resultMatrix,
                    minimalCost = primAlgorithm.MinimalCost
                };
            }
            catch (MethodException ex)
            {
                result = new
                {
                    exception = ex.Message
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HamiltonianCycleAlgorithm(string data)
        {
            var matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            Object result;

            try
            {
                if (matrix == null)
                    throw new MethodException("TEST EXSEPTION");

                matrix = replaceZeroToInf(matrix);

                var hamiltonianCycle = new HamiltonianCycleAlgirithmService();
                var resultMatrix = hamiltonianCycle.Resolve(matrix, false);

                resultMatrix = replaceInfToZero(resultMatrix);
                result = new
                {
                    matrix = resultMatrix,
                    path = hamiltonianCycle.Path,
                    exception = string.Empty
                };
            }
            catch (MethodException ex)
            {
                result = new
                {
                    exception = ex.Message
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FloydWarshallFirstAlgorithm(string data)
        {
            var matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            Object result;

            try
            {
                if (matrix == null || !IsSymmetricMatrix(matrix))
                {
                    throw new MethodException("Матриця не валiдна або задано орiєнтовний граф. Даний алгоритм може працювати лише iз неорiєнтовними графами");
                }

                var method = new FloydWarshallFirtsAlgorithm();
                var resultMatrix = method.Resolve(matrix);
                result = new
                {
                    exception = "",
                    matrix = resultMatrix
                };
            }
            catch (MethodException ex)
            {
                result = new
                {
                    exception = ex.Message
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FloydWarshallSecondAlgorithm(string data)
        {
            var matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            Object result;

            try
            {
                if (matrix == null || !IsSymmetricMatrix(matrix))
                {
                    throw new MethodException("Матриця не валiдна або задано орiєнтовний граф. Даний алгоритм може працювати лише iз неорiєнтовними графами");
                }

                var method = new FloydWarshallSecondAlgorithm();
                var resultMatrix = method.Resolve(matrix, false);
                result = new
                {
                    exception = "",
                    matrix = resultMatrix
                };
            }
            catch (MethodException ex)
            {
                result = new
                {
                    exception = ex.Message
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MaxMatchesAlgorithm(string data)
        {
            var matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            Object result;

            try
            {
                if (matrix == null)
                    throw new MethodException("TEST EXSEPTION");

                var method = new MaxMatches();
                var resultMatrix = method.Resolve(matrix, false, out int countMatches);

                result = new
                {
                    exception = "",
                    matrix = resultMatrix,
                    countMatches = countMatches
                };
            }
            catch (MethodException ex)
            {
                result = new
                {
                    exception = ex.Message
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WideSearchTreeAlgorithm(string data, int start)
        {
            var matrix = JsonConvert.DeserializeObject<int[,]>(data);

            Object result;

            try
            {
                if (matrix == null)
                    throw new MethodException("TEST EXSEPTION");
                var method = new SearchTree();
                var resultMatrix = method.StartWideSearch(matrix, start);

                result = new
                {
                    exception = "",
                    matrix = resultMatrix
                };
            }
            catch (MethodException ex)
            {
                result = new
                {
                    exception = ex.Message
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeepSearchTreeAlgorithm(string data, int start)
        {
            var matrix = JsonConvert.DeserializeObject<int[,]>(data);

            Object result;

            try
            {
                if (matrix == null) return null;
                var method = new SearchTree();
                var resultMatrix = method.StartDeepSearch(matrix, start);
                result = new
                {
                    exception = "",
                    matrix = resultMatrix
                };
            }
            catch (MethodException ex)
            {
                result = new
                {
                    exception = ex.Message
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DijkstraAlgorithm(string data, int start)
        {
            var matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            Object result;

            try
            {
                if (matrix == null || !IsConnectivityGraph(matrix))
                {
                    throw new MethodException("Граф не є зв'язним");
                }

                matrix = replaceZeroToInf(matrix);
                var method = new Dijkstra(matrix);
                var resultMatrix = method.Resolve(start);
                result = new
                {
                    exception = "",
                    matrix = resultMatrix
                };
            }
            catch (MethodException ex)
            {
                result = new
                {
                    exception = ex.Message
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<List<double>> replaceZeroToInf(List<List<double>> matrix)
        {
            var INF = double.PositiveInfinity;

            for (var i = 0; i < matrix.Count; i++)
            {
                for (var j = 0; j < matrix.Count; j++)
                {
                    if (matrix[i][j] == 0)
                    {
                        matrix[i][j] = INF;
                    }
                }
            }

            return matrix;
        }

        private List<List<double>> replaceInfToZero(List<List<double>> matrix)
        {
            var INF = double.PositiveInfinity;

            for (var i = 0; i < matrix.Count; i++)
            {
                for (var j = 0; j < matrix.Count; j++)
                {
                    if (matrix[i][j] == INF)
                    {
                        matrix[i][j] = 0;
                    }
                }
            }

            return matrix;
        }

        private bool IsSymmetricMatrix(List<List<double>> matrix)
        {
            for (var i = 0; i < matrix[0].Count; i++)
            {
                for (var j = 0; j < matrix[i].Count; j++)
                {
                    if (matrix[i][j] != matrix[j][i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsConnectivityGraph(List<List<double>> matrix)
        {
            for (int i = 0; i < matrix[0].Count; i++)
            {
                var nodeIsConnected = false;

                for (int j = 0; j < matrix[i].Count; j++)
                {
                    if (matrix[i][j] != 0 || matrix[j][i] != 0)
                    {
                        nodeIsConnected = true;
                        break;
                    }
                }

                if (!nodeIsConnected)
                {
                    return false;
                }
            }

            return true;
        }
    }
}