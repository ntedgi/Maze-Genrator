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
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>

    public delegate void PrintF();

    public delegate void SetsEvent();

    public partial class SettingWindow : Window
    {
        public event PrintF PrintEvent;
        public event SetsEvent Setter;
       public string Return_State;
       public string Solving;
       public string Alg;
       public string Huristic;
       public bool diagonel;
       public string M_t;

       ///SettingWindow <summary>
        /// the class stend for the sttting wondow of all propties as:
        /// solving , gnarate , huirstic, and diagonal
        /// </summary>
        public SettingWindow()
        {
            InitializeComponent();

            M_generator.Items.Add("Dfs");
            M_generator.Items.Add("Random");
            M_Heuristic.Items.Add("AirDistance");
            M_Heuristic.Items.Add("ManhattanDistance");
            M_solveAlg.Items.Add("UCS");
            M_solveAlg.Items.Add("A*");
            DataContext = this;
            

        }
        /// <summary>
        /// sving buttton 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

            Exception Missing = new Exception("Missing Argument Exception");
            //if some thing not chouse, set the defult
            try
            {
                if ((Solving == "") | (Alg == "") | (Huristic == "") | (M_t == ""))
                {
                    throw Missing;
                }
                else
                {
                    Solving = M_solveAlg.Text;
                    Alg = M_generator.Text;
                    Huristic = M_Heuristic.Text;
                    diagonel = (M_Diagonal.IsChecked == true);
                    M_t = M_TheredNUM.Text;
                    this.Visibility = Visibility.Hidden;
                    Setter();
                }

            }
            catch (System.Exception ex)
            {
                Return_State = ex.Message;
                PrintEvent();
            }



        }
    }
}
