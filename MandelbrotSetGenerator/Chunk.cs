using Mandelbrot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotSetGenerator
{
    public class Chunk
    {
        public List<int> Columns { get; }

        public Chunk()
        {
            Columns = new List<int>();
        }
        override public string ToString()
        {
            string result = $"Count: {Columns.Count} - [";

            for (int i = 0; i < Columns.Count - 1; i++)
            {
                result += Columns[i] + ", ";
            }

            result += Columns[Columns.Count - 1] + "]";

            return result;
        }
    }
}
