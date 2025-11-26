using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Casino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

        public partial class MainWindow : Window
        {
            private int _balance = 1000;
            private int _bet = 100;

            public MainWindow()
            {
                InitializeComponent();
                UpdateHud();
                Slot.SetGetBetFunc(() => _bet);
                Slot.SetOnWin(AddWinToBalance);
                Slot.SetCanSpendFunc(SpendBalance);
            }

            private void UpdateHud()
            {
                TxtBalance.Text = _balance.ToString();
                TxtBet.Text = _bet.ToString();
                // Nyeremény mezőt a Slot állítja, de nullázzuk pörgetés előtt is.
            }

            private bool SpendBalance(int amount)
            {
                if (_balance >= amount)
                {
                    _balance -= amount;
                    UpdateHud();
                    return true;
                }
                return false;
            }

            private void AddWinToBalance(int win)
            {
                _balance += win;
                TxtWin.Text = win.ToString();
                UpdateHud();
            }

            private void BtnSpin_Click(object sender, RoutedEventArgs e)
            {
                TxtWin.Text = "0";
                Slot.Spin();
            }

            private void BtnAddCredit_Click(object sender, RoutedEventArgs e)
            {
                _balance += 500;
                UpdateHud();
            }

            private void BtnBetMinus_Click(object sender, RoutedEventArgs e)
            {
                if (_bet > 10)
                {
                    _bet -= 10;
                    UpdateHud();
                }
            }

            private void BtnBetPlus_Click(object sender, RoutedEventArgs e)
            {
                if (_bet < 1000)
                {
                    _bet += 10;
                    UpdateHud();
                }
            }
        }
    }

