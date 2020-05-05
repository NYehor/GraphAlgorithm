using System;

namespace dijkstra
{
    class Program
    {
        static void Main(string[] args)
        {
            var dijkstra = new Dijkstra();
            dijkstra.Resolve(1);
            Console.Read();
        }
    }
}
