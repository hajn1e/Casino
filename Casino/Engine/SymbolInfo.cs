using System;
using System.Collections.Generic;
using System.Windows.Media;
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
                    return ("Cseresznye", Color.FromRgb(239,68,68), Colors.White);
                case Symbol.Lemon:
                    return ("Citrom", Color.FromRgb(250, 204, 21), Colors.Black);
                case Symbol.Bar:
                    return ("BAR", Color.FromRgb(107, 114, 128), Colors.White);
                case Symbol.Diamond:
                    return("Gyémánt", Color.FromRgb(59, 130, 246), Colors.Black);
                case Symbol.Seven:
                    return ("777", Color.FromRgb(16, 185, 129), Colors.Black);
                default:
                    return("?", Colors.DarkGray, Colors.White);

            }
        }
    }
}
