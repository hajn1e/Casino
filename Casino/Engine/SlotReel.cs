using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Casino.Engine
{
    public sealed class SlotReel
    {
        private readonly Canvas _canvas;
        private readonly int _symbolWidth;
        private readonly int _symbolHeight;
        private readonly int _visibleRows;
        private readonly Func<Symbol, FrameworkElement> _factory;

        private readonly List<FrameworkElement> _items = new List<FrameworkElement>();
        private readonly List<Symbol> _symbols = new List<Symbol>();

        private DispatcherTimer _timer;
        private double _offsetY;
        private double _speed;
        private DateTime _stopAt;
        private Symbol _finalSymbol;
        private Action _onCompleted;

        public SlotReel(Canvas canvas, int symbolWidth, int symbolHeight, int visibleRows, Func<Symbol, FrameworkElement> factory)
        {
            _canvas = canvas;
            _symbolWidth = symbolWidth;
            _symbolHeight = symbolHeight;
            _visibleRows = visibleRows;
            _factory = factory;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16);
            _timer.Tick += OnTick;
        }

        public void FillInitial(Rng rng)
        {
            Clear();
            for (int i = 0; i < _visibleRows + 3; i++)
            {
                var sym = (Symbol)rng.Next(0, 5);
                AddSymbolVisual(sym);
            }
            LayoutItems();
        }

        public void SpinFor(TimeSpan duration, Rng rng, Symbol finalSymbol, Action onCompleted = null)
        {
            _onCompleted = onCompleted;
            _finalSymbol = finalSymbol;

            // Kezdő sebesség
            _speed = 18.0;
            _stopAt = DateTime.Now + duration;

            if (!_timer.IsEnabled)
            {
                _timer.Start();
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            _offsetY += _speed;

            // Fokozatos lassítás
            var remaining = (_stopAt - DateTime.Now).TotalMilliseconds;
            if (remaining < 400)
            {
                _speed = Math.Max(4.0, _speed * 0.92);
            }

            // Ha elértük egy elem magasságát, “léptetés”
            if (_offsetY >= _symbolHeight)
            {
                _offsetY -= _symbolHeight;
                // legfelső elem kicsúszott -> távolít, új véletlen alulra
                PopTopPushBottom();
            }

            ApplyTransform();

            if (DateTime.Now >= _stopAt && _speed <= 4.05)
            {
                // Finom ráigazítás a cella rácsra
                SnapToGrid();
                // Középső látható sor legyen a kért végső szimbólum
                SetMiddleTo(_finalSymbol);
                ApplyTransform();
                _timer.Stop();

                var done = _onCompleted;
                _onCompleted = null;
                if (done != null)
                {
                    done();
                }
            }
        }

        private void PopTopPushBottom()
        {
            if (_items.Count == 0) return;

            var top = _items[0];
            _items.RemoveAt(0);
            _canvas.Children.Remove(top);

            // Új elem alulra – a változatosság kedvéért rotáljuk az eddigi listát
            var newSym = NextCycleSymbol();
            AddSymbolVisual(newSym);
        }

        private Symbol NextCycleSymbol()
        {
            // Egyszerű körforgás: a készletből választunk, majd változtatjuk.
            // Itt nem súlyozunk, az utolsó beállás adja a végső eredményt.
            var idx = _symbols.Count % 5;
            var sym = (Symbol)idx;
            return sym;
        }

        private void SetMiddleTo(Symbol s)
        {
            // Középső látható sor mindig a lista 1. indexén legyen megjelenítve
            // Trükk: töröljük és újrarakjuk úgy, hogy középen s legyen.
            Clear();

            // Láthatóság: 3 sor. Legyen [előtte, közép, utána] + két extra felül és alul a gördüléshez
            var seq = new List<Symbol>
            {
                RandomOther(s),
                RandomOther(s),
                s,
                RandomOther(s),
                RandomOther(s)
            };

            foreach (var it in seq)
            {
                AddSymbolVisual(it);
            }

            LayoutItems();
            _offsetY = 0;
        }

        private Symbol RandomOther(Symbol s)
        {
            // Kis keverés
            var v = (int)s;
            var n = (v + 1) % 5;
            return (Symbol)n;
        }

        private void Clear()
        {
            _items.Clear();
            _symbols.Clear();
            _canvas.Children.Clear();
            _offsetY = 0;
        }

        private void AddSymbolVisual(Symbol s)
        {
            var fe = _factory(s);
            _items.Add(fe);
            _symbols.Add(s);
            _canvas.Children.Add(fe);
        }

        private void LayoutItems()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                var fe = _items[i];
                Canvas.SetLeft(fe, 10);
                Canvas.SetTop(fe, i * _symbolHeight - _offsetY);
            }
        }

        private void ApplyTransform()
        {
            LayoutItems();
        }

        private void SnapToGrid()
        {
            var mod = _offsetY % _symbolHeight;
            if (mod >= _symbolHeight / 2.0)
            {
                _offsetY += (_symbolHeight - mod);
            }
            else
            {
                _offsetY -= mod;
            }
            if (_offsetY < 0) _offsetY = 0;
        }
    }
}
