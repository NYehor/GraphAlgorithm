using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GraphAlgorithm.Models;
using GraphAlgorithm.Services;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using GraphAlgorithm.Services.FloydWarshall;
using GraphAlgorithm.Services.SearchTree;

namespace GraphAlgorithm.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
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

        [HttpGet]
        public JsonResult KruskalAlgorithm(string data)
        {
            List<List<double>> matrix = JsonConvert.DeserializeObject< List < List<double> >>(data);

            Object result;
            try
            {
                if (matrix == null)
                    throw new MethodException("TEST EXSEPTION");
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
            catch (MethodException ex) {
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
            List<List<double>> matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            Object result;
            try
            {
                if (matrix == null)
                    throw new MethodException("TEST EXSEPTION");

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
            List<List<double>> matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);
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
                    path = hamiltonianCycle.Path
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
            List<List<double>> matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            Object result;
            try
            {
                if (matrix == null)
                    throw new MethodException("TEST EXSEPTION");

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
            List<List<double>> matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);
            Object result;
            try
            {
                if (matrix == null)
                    throw new MethodException("TEST EXSEPTION");

                var method = new MaxMatches();
                var resultMatrix = method.Resolve(matrix, false);

                result = new
                {
                    exception="",
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

        public ActionResult WideSearchTreeAlgorithm(string data, int start)
        {
            int[,] matrix = JsonConvert.DeserializeObject<int[,]>(data);
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
            int[,] matrix = JsonConvert.DeserializeObject<int[,]>(data);
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
            List<List<double>> matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            Object result;
            try
            {
                if (matrix == null)
                    throw new MethodException("TEST EXSEPTION");

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

            for (int i = 0; i < matrix.Count; i++)
                for (int j = 0; j < matrix.Count; j++)
                    if (matrix[i][j] == 0)
                        matrix[i][j] = INF;

            return matrix;
        }

        private List<List<double>> replaceInfToZero(List<List<double>> matrix)
        {
            var INF = double.PositiveInfinity;

            for (int i = 0; i < matrix.Count; i++)
                for (int j = 0; j < matrix.Count; j++)
                    if (matrix[i][j] == INF)
                        matrix[i][j] = 0;

            return matrix;
        }
    }
}