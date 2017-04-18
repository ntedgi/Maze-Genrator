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
using MVP;
using ProjectSrc.Model.Search.Domains.Maze;


namespace GUI.Maze
{
    ///MazeBoard class <summary>
    ///build a mazegmae bord from cells
    /// </summary>
    public partial class MazeBoard : UserControl
    {
        #region field
        private ItemPoint item;
        int m_mazeCellSize;
        private int m_itemLocationX;
        private int m_itemLocationY;
        public int ItemLocationX
        {
            get { return m_itemLocationX; }
            set { m_itemLocationX = value; }
        }
        public int ItemLocationY
        {
            get { return m_itemLocationY; }
            set { m_itemLocationY = value; }

        }
        #endregion field
        public MazeBoard() { }

        ///constractor <summary>
        /// 
        /// </summary>
        /// <param name="mazeHight"></param>
        /// <param name="mazeLength"></param>
        /// <param name="mazeCellSize"></param>
        /// <param name="maze"></param>
        public MazeBoard(int mazeHight, int mazeLength, int mazeCellSize, maze maze)
        {
            InitializeComponent();
            m_mazeCellSize = mazeCellSize;
            m_itemLocationX = 1;
            m_itemLocationY = 1;
            item = new ItemPoint(m_mazeCellSize);
            Goal G = new Goal(mazeCellSize);
            mazeBoard.Children.Add(item);
            Canvas.SetLeft(item, m_mazeCellSize);
            Canvas.SetTop(item, m_mazeCellSize);
            mazeBoard.Children.Add(G);
            Canvas.SetLeft(G,( mazeHight-2) * m_mazeCellSize);
            Canvas.SetTop(G, (mazeLength-2) * m_mazeCellSize);
            CreateMaze(mazeHight, mazeLength, m_mazeCellSize, maze);




        }

        ///CreateMaze <summary>
        /// creating the maze cell bay given maze 
        /// </summary>
        /// <param name="mazeHight">maze Hight</param>
        /// <param name="mazeLength">maze Length</param>
        /// <param name="mazeCellSize">cell size</param>
        /// <param name="_Maze">maze Name</param>
        private void CreateMaze(int mazeHight, int mazeLength, int mazeCellSize, ProjectSrc.Model.Search.Domains.Maze.maze _Maze)
        {
            MazeCell mazeCell = null;

            for (int i = 0; i < mazeHight; i++)
            {
                for (int j = 0; j < mazeLength; j++)
                {
                    if (_Maze._Maze[i, j] == 1)
                    {
                        mazeCell = new MazeCell(mazeCellSize, true, true, true, true);
                    }
                    else
                        mazeCell = new MazeCell(mazeCellSize, false, false, false, false);
                    mazeBoard.Children.Add(mazeCell);
                    Canvas.SetLeft(mazeCell, mazeCellSize * i);
                    Canvas.SetTop(mazeCell, mazeCellSize * j);
                }
            }

            Image BodyImage = new Image
            {
                Width = mazeCellSize,
                Height = mazeCellSize,
               // Source = new BitmapImage(new Uri(@"/Images\lego.jpg", UriKind.Relative)),
            };
          //  mazeBoard.Children.Add(BodyImage);
            Canvas.SetLeft(BodyImage, mazeCellSize * mazeHight);
            Canvas.SetTop(BodyImage, mazeCellSize * mazeLength);
        }

        ///Move <summary>
        /// moivng th item in the bord
        /// </summary>
        public void Move()
        {
            mazeBoard.Children.Remove(item);
            mazeBoard.Children.Add(item);
            Canvas.SetLeft(item, m_mazeCellSize * ItemLocationX);
            Canvas.SetTop(item, m_mazeCellSize * ItemLocationY);
        }
    }
}