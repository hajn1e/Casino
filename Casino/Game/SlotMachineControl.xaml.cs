using Casino.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Casino.Game
{
    /// <summary>
    /// Interaction logic for SlotMachineControl.xaml
    /// </summary>
    public partial class SlotMachineControl : UserControl
    {
        private const int SymbolWidth = 200;
        private const int SymbolHeight = 60;
        private const int VisibleRows = 3;

        private readonly SlotReel _reelA;
        private readonly SlotReel _reelB;
        private readonly SlotReel _reelC;
        private readonly PayoutTable _payout;
        private readonly Rng _rng;

        public SlotMachineControl()
        {
            InitializeComponent();

            _rng = new Rng();
            _payout = new PayoutTable();
            _reelA = new SlotReel(Reel1, SymbolWidth, SymbolHeight, VisibleRows, CreateSymbolVisual);
            _reelB = new SlotReel(Reel2, SymbolWidth, SymbolHeight, VisibleRows, CreateSymbolVisual);
            _reelC = new SlotReel(Reel3, SymbolWidth, SymbolHeight, VisibleRows, CreateSymbolVisual);

            _reelA.FillInitial(_rng);
            _reelB.FillInitial(_rng);
            _reelC.FillInitial(_rng);
        }

        private FrameworkElement CreateSymbolVisual(Symbol symbol)
        {
            var info= SymbolInfo.Get(symbol);

            var root = new Grid
            {
                Width = SymbolWidth,
                Height = SymbolHeight,
                Background = new SolidColorBrush(info.Background),
            };

            root.Effect = new System.Windows.Media.Effects.DropShadowEffect
            {
                Color = Color.FromRgb(20,20,20),
                ShadowDepth = 0,
                Opacity = 6,
                BlurRadius = 8
            };

            var rect = new Rectangle
            {
                RadiusX = 10,
                RadiusY = 10,
                Fill = new LinearGradientBrush(info.Background, Color.Multiply(info.Background,0.85f),90)
            };
            root.Children.Add(rect);

            var text = new TextBlock
            {
                Text = info.Labe,
                Foreground = new SolidColorBrush(info.Foreground),
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            root.Children.Add(text);
            return root;

        }
    }
}
