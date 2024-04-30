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
using System.Windows.Threading;

namespace DiceRoll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<int> dice = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRoll_Click(object sender, RoutedEventArgs e)
        {
            btnRoll.Content = "Rolling Dice...";
            btnRoll.IsEnabled = false;
            int roll = 1;
            dice.Clear();
            txtRoll.Text = "Dice Rolls:";

            for (int i = 0; i < cmbDice.Items.Count; i++)
            {
                dice.Add(0);
                if ((int)cmbDice.SelectedIndex == i)
                {
                    dice.Add(0); //have to add an extra dice due to the index numbers
                    break;
                }
            }

            if (dice.Count > 3)
                lblWarn.Visibility = Visibility.Visible;

            UpdateUI();
            
            Random rnd = new Random();

            do
            {
                txtRoll.Text += String.Format($"\nRoll {roll}:");

                for (int i=0; i < dice.Count; i++)
                {
                    dice[i] = rnd.Next(1, 6);
                    txtRoll.Text += String.Format($"\nDice {i+1}: {dice[i]}");
                }
                txtRoll.Text += "\n---------------";



                txtRoll.ScrollToEnd();
                
                if (dice.All(c => c == dice[0])) //if all dice are equal to dice[0]
                {
                    txtRoll.Text += String.Format($"\nRolling ended on roll {roll} with the number {dice[0]}.");
                    break;
                }

                roll++;

                if (chkUpdate.IsChecked == true)
                    UpdateUI();

            } while (true);

            btnRoll.Content = "Roll";
            btnRoll.IsEnabled = true;
            lblWarn.Visibility = Visibility.Hidden;
        }

        void UpdateUI() //Taken from Stack Overflow to update the UI elements whenever needed.
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate (object parameter)
            {
                frame.Continue = false;
                return null;
            }), null);

            Dispatcher.PushFrame(frame);
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }
    }
}