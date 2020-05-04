using System.Collections.Generic;

namespace GraphAlgorithm.Services
{
    public interface IMethod
    {
        List<List<int>> Resolve(List<List<int>> matrix, bool isIncidentMatrix);
    }
}
