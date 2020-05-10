using System;
using System.Collections.Generic;

namespace WeigtedTree
{
	class Program
    {
        private static void Main()
        {
            Console.WriteLine("Please enter the string:");
            string Example = Console.ReadLine();
            var tree = new Tree<char>(Example);
            List<int> encoding = tree.Encode(Example);
            List<char> decoding = tree.Decode(encoding);
            var outString = new string(decoding.ToArray());
            Console.WriteLine(outString == Example ? "Encoding/decoding worked" : "Encoding/Decoding failed");

            var chars = new HashSet<char>(Example);
            foreach (char c in chars)
            {
                encoding = tree.Encode(c);
                Console.Write("{0}:  ", c);
                foreach (int bit in encoding)
                {
                    Console.Write("{0}", bit);
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
    
	
}
