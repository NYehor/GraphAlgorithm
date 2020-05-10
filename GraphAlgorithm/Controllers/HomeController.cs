using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GraphAlgorithm.Models;
using GraphAlgorithm.Services;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using GraphAlgorithm.Services.FloydWarshall;

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
            /* Let us create the following graph 
                2   3 
            (0)--(1)--(2) 
            |    / \   | 
           6|  8/   \5 |7 
            |  /     \ | 
            (3)-------(4) 
                  9         */
            if (matrix == null) return null;

            matrix = replaceZeroToInf(matrix);
            var kruskalAlgorithm = new KruskalAlgorithmService();
            var resultMatrix = kruskalAlgorithm.Resolve(matrix, false);
            resultMatrix = replaceInfToZero(resultMatrix);

            Object result = new
            {
                matrix = resultMatrix,
                minimalCost= kruskalAlgorithm.MinimalCost
            }; 

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrimAlgorithm(string data)
        {
            List<List<double>> matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);
            /* Let us create the following graph 
                2   3 
            (0)--(1)--(2) 
            |    / \   | 
           6|  8/   \5 |7 
            |  /     \ | 
            (3)-------(4) 
                  9         */
            if (matrix == null) return null;

            matrix = replaceZeroToInf(matrix);
            var primAlgorithm = new PrimAlgorithmService();
            var resultMatrix = primAlgorithm.Resolve(matrix, false);
            resultMatrix = replaceInfToZero(resultMatrix);

            Object result = new
            {
                matrix = resultMatrix,
                minimalCost = primAlgorithm.MinimalCost
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HamiltonianCycleAlgorithm(string data)
        {
            List<List<double>> matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            if (matrix == null) return null;

            matrix = replaceZeroToInf(matrix);
            var hamiltonianCycle = new HamiltonianCycleAlgirithmService();
            var resultMatrix = hamiltonianCycle.Resolve(matrix, false);
            resultMatrix = replaceInfToZero(resultMatrix);

            Object result = new
            {
                matrix = resultMatrix,
                path = hamiltonianCycle.Path
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FloydWarshallSecondAlgorithm(string data)
        {
            List<List<double>> matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            if (matrix == null) return null;
            var method = new FloydWarshallSecondAlgorithm();
            var resultMatrix = method.Resolve(matrix, false);

            return Json(JsonConvert.SerializeObject(resultMatrix), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MaxMatchesAlgorithm(string data)
        {
            List<List<double>> matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            if (matrix == null) return null;
            var method = new MaxMatches();
            var resultMatrix = method.Resolve(matrix, false);

            return Json(JsonConvert.SerializeObject(resultMatrix), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DijkstraAlgorithm(string data, int start)
        {
            List<List<double>> matrix = JsonConvert.DeserializeObject<List<List<double>>>(data);

            if (matrix == null) return null;

            matrix = replaceZeroToInf(matrix);
            var method = new Dijkstra(matrix);
            var resultMatrix = method.Resolve(start);  

            return Json(resultMatrix, JsonRequestBehavior.AllowGet);
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