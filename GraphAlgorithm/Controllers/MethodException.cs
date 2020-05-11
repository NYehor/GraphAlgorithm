using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphAlgorithm
{
    public class MethodException: ArgumentException
    {
        public MethodException(string message)
            : base(message)
        {
          
        }
    }
}