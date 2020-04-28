using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithm
{
    interface IMethod
    {
        string Resolve(List<List<int>> matrix, bool isIncidentMatrix);
    }
}
