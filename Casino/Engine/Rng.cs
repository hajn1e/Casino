using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Engine
{
    public sealed class Rng
    {
        private readonly Random _r = new Random();

        public int Next(int minInclusive, int maxExclusive)
        {
            return _r.Next(minInclusive, maxExclusive);
        }

        public double NextDouble()
        {
            return _r.NextDouble();
        }
    }
}
