using System.Collections.Generic;

namespace GraphAlgorithm.Services
{
    public interface IMethod
    {
        List<List<double>> Resolve(List<List<double>> matrix, bool isIncidentMatrix);
    }
}
