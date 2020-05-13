namespace GraphAlgorithm.Services.FloydWarshall
{
    public static class DoubleExtensions
    {
        public static bool IsInt(this double d)
        {
            return (d % 1) == 0;
        }
    }
}