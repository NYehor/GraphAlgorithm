using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace GraphAlgorithm.Services.FloydWarshall
{
    public static class ImmutableListExtensions
    {
        public static List<double> Clone(this IList<double> listToClone)
        {
            return listToClone.Select(item => item).ToList();
        }

        public static List<double> AddNumber(this IList<double> list, double number)
        {
            return Clone(list).Select(e => e + number).ToList();
        }

        public static List<double> ElementwiseMinimum(this IList<double> list1, IList<double> list2)
        {
            if (list1.Count != list2.Count) throw new Exception("Lists have to be the same size.");

            var resultList = new List<double>();
            for (int i = 0; i < list1.Count; i++)
            {
                resultList.Add(Min(list1[i], list2[i]));
            }

            return resultList;
        }
    }
}