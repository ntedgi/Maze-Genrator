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
    ///class Goal <summary>
    ///goal cell
    /// </summary>
    public partial class Goal : UserControl
    {
        int m_mazeCellSize;

        ///Goal <summary>
        /// put image in goal place
        /// </summary>
        /// <param name="mazeCellSize"></param>
        public Goal(int mazeCellSize)
        {
            InitializeComponent();
            m_mazeCellSize = mazeCellSize;
            rec.Width = mazeCellSize;
            rec.Height = mazeCellSize;
            rec.Fill = new ImageBrush(new BitmapImage(new Uri(@"Images\mas1.jpg", UriKind.Relative)));
        }
    }
}
