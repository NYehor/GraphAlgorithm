using System.Collections.Generic;

namespace SearchTree
{
    interface IMethod
    {
        List<int> StartDeepSearch(int[,] matrix, int startNodeId);

        List<int> StartWideSearch(int[,] matrix, int startNodeId);
    }
}
