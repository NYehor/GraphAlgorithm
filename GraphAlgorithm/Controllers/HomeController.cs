using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GraphAlgorithm.Services;

namespace GraphAlgorithm.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdjacencyMatrix()
        {
            return View();
        }

        public ActionResult IncindenceMatrix()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult KruskalAlgorithm()
        {
            var INF = int.MaxValue;

            /* Let us create the following graph 
                2   3 
            (0)--(1)--(2) 
            |    / \   | 
           6|  8/   \5 |7 
            |  /     \ | 
            (3)-------(4) 
                  9         */

            var adjacencyMatrix = new List<List<int>>
            {
                new List<int>
                {
                    INF, 2, INF, 6, INF
                },
                new List<int>
                {
                    2, INF, 3, 8, 5
                },
                new List<int>
                {
                    INF, 3, INF, INF, 7
                },
                new List<int>
                {
                    6, 8, INF, INF, 9
                },
                new List<int>
                {
                    INF, 5, 7, 9, INF
                }
            };

            var kruskalAlgorithm = new KruskalAlgorithmService();

            var resultMatrix = kruskalAlgorithm.Resolve(adjacencyMatrix, false);
            Console.WriteLine(kruskalAlgorithm.MinimalCost);

            for (var i = 0; i < resultMatrix[0].Count; i++)
            {
                Console.WriteLine();

                for (var j = 0; j < resultMatrix[i].Count; j++)
                {
                    Console.Write(resultMatrix[i][j] + "       ");
                }
            }

            //some view or Json response
            return View("Index");
        }
    }
}