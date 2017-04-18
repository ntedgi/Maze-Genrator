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
    /// <summary>
    /// Interaction logic for MazeCell.xaml
    /// </summary>
    public partial class MazeCell : UserControl
    {
        public MazeCell(int mazeCellSize, bool upWall, bool leftWall, bool bottomWall, bool rightWall)
        {
            InitializeComponent();
          
            userControl.Width = mazeCellSize;
            lineUpWall.Visibility = (upWall) ? Visibility.Visible : Visibility.Hidden;
            lineLeftWall.Visibility = (leftWall) ? Visibility.Visible : Visibility.Hidden;
            lineDownWall.Visibility = (bottomWall) ? Visibility.Visible : Visibility.Hidden;
            lineRightWall.Visibility = (rightWall) ? Visibility.Visible : Visibility.Hidden;
            back.Visibility = (upWall) ? Visibility.Visible : Visibility.Hidden;

        }
    }
}
