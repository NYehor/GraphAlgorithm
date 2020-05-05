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

        public ActionResult Matrix()
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
            var INF = double.PositiveInfinity;

            /* Let us create the following graph  dgfdgdfg
                2   3 
            (0)--(1)--(2) 
            |    / \   | 
           6|  8/   \5 |7 
            |  /     \ | 
            (3)-------(4) 
                  9         */

            var adjacencyMatrix = new List<List<double>>
            {
                new List<double>
                {
                    INF, 2, INF, 6, INF
                },
                new List<double>
                {
                    2, INF, 3, 8, 5
                },
                new List<double>
                {
                    INF, 3, INF, INF, 7
                },
                new List<double>
                {
                    6, 8, INF, INF, 9
                },
                new List<double>
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

        public ActionResult PrimAlgorithm()
        {
            var INF = double.PositiveInfinity;

            /* Let us create the following graph 
                2   3 
            (0)--(1)--(2) 
            |    / \   | 
           6|  8/   \5 |7 
            |  /     \ | 
            (3)-------(4) 
                  9         */

            var adjacencyMatrix = new List<List<double>>
            {
                new List<double>
                {
                    INF, 2, INF, 6, INF
                },
                new List<double>
                {
                    2, INF, 3, 8, 5
                },
                new List<double>
                {
                    INF, 3, INF, INF, 7
                },
                new List<double>
                {
                    6, 8, INF, INF, 9
                },
                new List<double>
                {
                    INF, 5, 7, 9, INF
                }
            };

            var primAlgorithm = new PrimAlgorithmService();

            var resultMatrix = primAlgorithm.Resolve(adjacencyMatrix, false);
            Console.WriteLine(primAlgorithm.MinimalCost);

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