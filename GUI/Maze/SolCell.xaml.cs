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

namespace GUI.Maze
{
    ///class SolCell <summary>
    /// stend for sulotion cells
    /// </summary>
    public partial class SolCell : UserControl
    {
        public SolCell(int mazeCellSize)
        {
            InitializeComponent();
            userControl.Width = mazeCellSize;
        }
    }
}
