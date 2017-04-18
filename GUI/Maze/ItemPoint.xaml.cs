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
    /// Interaction logic for ItemPoint.xaml
    /// </summary>
    public partial class ItemPoint : UserControl
    {
        int m_mazeCellSize;
        public ItemPoint(int mazeCellSize)
        {
          InitializeComponent();
           m_mazeCellSize = mazeCellSize;
          //ell_item.Width = 20;
          //ell_item.Height = 20;
          //Canvas.SetLeft(ell_item, 20);
          //Canvas.SetTop(ell_item, 20);
          //
          //ImageBrush i = new ImageBrush(new BitmapImage(new Uri("lego.jpg", UriKind.Relative)));

           rec.Width = mazeCellSize;
           rec.Height = mazeCellSize;
           rec.Fill = new ImageBrush(new BitmapImage(new Uri(@"Images\runner4.png", UriKind.Relative)));
        }
    }
}
