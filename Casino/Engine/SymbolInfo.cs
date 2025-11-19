using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Engine
{
    public class SymbolInfo
    {
        public static (string Labe, Color Background, Color Foreground) Get(Symbol s)
        {
            switch (s)
            {
                case Symbol.Cherry:
                    return ("Cseresznye", Color.Red, Color.White);
                case Symbol.Lemon:
                    return ("Citrom", Color.Yellow, Color.Black);
                case Symbol.Bar:
                    return ("BAR", Color.Gray, Color.White);
                case Symbol.Diamond:
                    return("Gyémánt", Color.LightBlue, Color.Black);
                case Symbol.Seven:
                    return ("777", Color.Green, Color.Black);
                default:
                    return("?", Color.DarkGray, Color.White);

            }
        }
    }
}
