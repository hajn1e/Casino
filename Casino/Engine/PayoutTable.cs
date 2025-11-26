using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Engine
{
    public sealed class PayoutTable
    {
        // Súlyok a szimbólum választáshoz (gyakoribb = könnyebb)
        private readonly (Symbol symbol, int weight)[] _weights =
        {
            (Symbol.Cherry, 30),
            (Symbol.Lemon, 30),
            (Symbol.Bar, 20),
            (Symbol.Diamond, 12),
            (Symbol.Seven, 8)
        };

        public Symbol PickWeighted(Rng rng)
        {
            var total = _weights.Sum(w => w.weight);
            var roll = rng.Next(0, total);
            var acc = 0;
            for (int i = 0; i < _weights.Length; i++)
            {
                acc += _weights[i].weight;
                if (roll < acc)
                {
                    return _weights[i].symbol;
                }
            }
            return Symbol.Cherry;
        }

        // Egyszerű kifizetési tábla: csak 3 azonos számít
        public int CalculateWin(int bet, Symbol[] line)
        {
            if (line.Length != 3) return 0;

            if (line[0] == line[1] && line[1] == line[2])
            {
                switch (line[0])
                {
                    case Symbol.Seven:
                        // nagy nyeremény
                        return bet * 20;
                    case Symbol.Diamond:
                        return bet * 10;
                    case Symbol.Bar:
                        return bet * 6;
                    case Symbol.Cherry:
                        return bet * 4;
                    case Symbol.Lemon:
                        return bet * 3;
                }
            }

            // 2 azonos esetén apró visszatérítés
            if (line[0] == line[1] || line[1] == line[2] || line[0] == line[2])
            {
                return bet; // tét vissza
            }

            return 0;
        }
    }
}
