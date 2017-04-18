using GUI.Maze;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MVP.View;
using System.Linq;
using MVP;
using System.Threading;
using MVP.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using ProjectSrc.Model.Search.Domains.Maze;
using MVP.Model.Maze;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;
using System.Windows.Input;

namespace GUI
{
   
    public delegate void MoveEvent(string s);

    /// MainWindow class<summary>
    /// Interaction logic for MainWindow.xaml
    /// rap all GUI , and intraction with the present
    /// </summary>
    public partial class MainWindow : Window, IView
    {

        #region Fields

        public Dictionary<string, MVP.ICommand> m_commands;
        List<setting> items = new List<setting>();
        public event ViewFunc ViewChanged;
        public event ViewFunc StartEvent;
        public Dictionary<string, int> MazeCellsize;
        public int clickCount = 0;
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        public SettingWindow M_setting;
        public MazeBoard m_mazeBoard;
        bool Gen_Flag;
        DateTime dt;
        #endregion

        #region User Setting
        public string Solving;
        public string Alg;
        public string Huristic;
        public bool diagonel;
        public string M_t;
        #endregion


        /// class setting<summary>
        /// Hellping Object To create A Dynamic GridView
        /// </summary>
        public class setting
        {
            public string Name { get; set; }
            public TextBox optines { get; set; }

            public int CompareTo(setting b)
            {
                return this.Name.CompareTo(b.Name);
            }

        }

       
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            IModel model = new MyModel();
            MyPresenter presenter = new MyPresenter(model, (IView)this);
            MazeCellsize = new Dictionary<string, int>();
            StartEvent();
            items.Add(new setting() { Name = "Maze Name", optines = new TextBox() });
            items.Add(new setting() { Name = "Height", optines = new TextBox() });
            items.Add(new setting() { Name = "Width", optines = new TextBox() });
            items.Add(new setting() { Name = "Cell Size", optines = new TextBox() });
            items[0].optines.Name = "Name";
            this.RegisterName("Name", items[0].optines);
            items[1].optines.Name = "Height";
            this.RegisterName("Height", items[1].optines);
            items[2].optines.Name = "Width";
            this.RegisterName("Width", items[2].optines);
            items[3].optines.Name = "Cell";
            this.RegisterName("Cell", items[3].optines);

            DataContext = this;
            myGrid.DataContext = this;
            lvUsers.ItemsSource = items;
            Gen_Flag = false;
            m_mazeBoard = new MazeBoard();
        }

        #endregion


        #region Xaml

        ///indexOf <summary>
        /// Find The Currant Parameter in The GridView An Internal methood
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int indexOf(string s)
        {
            for (int i = 0; i < items.Capacity; i++)
            {
                if (items[i].Name == s)
                {
                    return i;
                }
            }
            return 0;

        }

        private void lvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #endregion


        #region IView Implement

        /// Delete<summary>
        /// Delete The Choosing Maze From The maze List
        /// </summary>
        /// <param name="p"></param>
        public void Delete(string p)
        {
            cnvs_main.Children.Clear();
            M_list.Text = "Maze List";
            M_list.Items.Remove(p);
            MazeCellsize.Remove(p);
            ViewChanged();
        }
        public void Start()
        {
        }

        /// SetCommands<summary>
        /// Sets The Command
        /// </summary>
        /// <param name="commands">commend dictionary</param>
        public void SetCommands(Dictionary<string, MVP.ICommand> commands)
        {
            m_commands = commands;
        }

        /// GetUserCommand<summary>
        /// using in interface
        /// </summary>
        /// <returns></returns>
        public MVP.ICommand GetUserCommand()
        {
            throw new NotImplementedException();
        }

        /// DisplayMaze<summary>
        /// Display The Maze by droew he on bord and set hem on the canvas
        /// </summary>
        /// <param name="maze">maze to display</param>
        public void DisplayMaze(extendedMaze maze)
        {

            int mazeCellSize = MazeCellsize[maze.getName()];

            m_mazeBoard = new MazeBoard(maze._Maze.GetLength(0), maze._Maze.GetLength(1), mazeCellSize, maze);
            cnvs_main.Children.Add(m_mazeBoard);
            Canvas.SetLeft(m_mazeBoard, 0);
            Canvas.SetTop(m_mazeBoard, 0);
            cnvs_main.Focus();
            m_mazeBoard.Focus();
        }

        /// DisplaySolution<summary>
        /// drien the soltion on the maze
        /// </summary>
        /// <param name="solution">solution to display</param>
        public void DisplaySolution(Model.Search.Solution solution)
        {
            string[] m_arrTemp;
            int x;
            int y;
            try
            {
                ArrayList solPath = solution.GetSolutionPath();

                SolCell solCell;
                string s = M_list.Text;

                // int ind = indexOf("Cell Size");
                int mazeCellSize = MazeCellsize[s];
                // items[ind].optines.Clear();


                foreach (MazeState item in solPath)
                {
                    m_arrTemp = item.GetState().Split(',');
                    x = Convert.ToInt32(m_arrTemp[0]);
                    y = Convert.ToInt32(m_arrTemp[1]);
                    solCell = new SolCell(mazeCellSize);
                    cnvs_main.Children.Add(solCell);
                    Canvas.SetLeft(solCell, mazeCellSize * x);
                    Canvas.SetTop(solCell, mazeCellSize * y);
                }

            }
            catch (System.Exception ex)
            {
                Output(ex.Message);
            }



        }

        ///Output <summary>
        /// stend for the output consol
        /// getiing nothfiction from some event 
        /// and prting the meaning on the consol
        /// </summary>
        /// <param name="p">the string event</param>
        public void Output(string p)
        {
            string[] Lparts;
            if (this.Dispatcher.CheckAccess())
            {

                if (p.Contains("Maze_Cell"))
                {
                    Lparts = p.Split(' ');
                    MazeCellsize.Add(Lparts[1], Convert.ToInt32(Lparts[2]));
                    p = string.Format("Maze {0} Added Succesfuly..", Lparts[1]);
                    MessageBox.Text += p + Environment.NewLine;
                    MessageBox.ScrollToEnd();
                }
                else
                {
                    MessageBox.Text += p + Environment.NewLine;
                    MessageBox.ScrollToEnd();

                    if (p.Contains("is ready..."))
                    {
                       Lparts = p.Split(' ');
                        M_list.Items.Add(Lparts[1]);
                    }
                    if (p.Contains("result Finished..."))
                    {
                        Lparts = p.Split(' ');
                        m_commands["display_solution"].DoCommand("solve", Lparts[0]);

                    }
                    if (p.Contains("Allready Exict In Dictionary"))
                    {
                        Lparts = p.Split(' ');
                        m_commands["display_solution"].DoCommand("solve", Lparts[2]);
                    }
                }

            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() => Output(p)));
            }
        }

        #endregion



        #region Buttons


        ///Save <summary>
        /// saveing maze by zip algoritehm to user platform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click5(object sender, RoutedEventArgs e) //save
        {
            string s = M_list.Text;

            string[] FileName = new string[1];
            if (s == "Maze List")
            {
                Output("Select A Maze From The List...");
            }
            else
            {
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.InitialDirectory = @"C:\Users\naor\Desktop\project3_b\MVP\GUI\bin";
                    saveFileDialog.Filter = "Maze file (*.Maze)|*.Maze";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.FileName = M_list.Text;

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        FileName = saveFileDialog.FileNames;

                    }
                    s = FileName[0];

                    FileName = s.Split('\\');
                    m_commands["Save"].DoCommand(s, M_list.Text);

                    s = string.Format("Maze {0} is Saved...", M_list.Text);
                    Output(s);


                }


                catch
                {
                    s = string.Format("Fail Saving {0}", s);
                    Output(s);

                }
            }
        }

        ///Help <summary>
        /// help button. showing user how to play in App
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click6(object sender, RoutedEventArgs e)
        {
            string m_Message = string.Format("Its Not That Hard.. " + Environment.NewLine +

                "1.) Fill The Arguments Above The Generate Button (all args must be more then 2)" + Environment.NewLine + "2.)Press The Generate Button .. Now Your Maze Added To The Maze List" + Environment.NewLine + "3.)Choose A Maze From The List..." + Environment.NewLine + "4.)Choose Display To Try Solve It Or Solve To See The Solution" + Environment.NewLine + "Enjoy..");
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(m_Message, "Help", System.Windows.MessageBoxButton.OK);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                return;

            }
        }

        ///Delete <summary>
        /// delete maze button, deleting the chosing maze
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            string s = M_list.Text;
            //M_list.Text = "";
            if (s != "Maze List")
            {

                string m_Message = string.Format("Are you sure you want to Delete Maze {0} ?", s);
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(m_Message, "Delete Maze Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {



                    m_commands["Delete"].DoCommand(s);
                    s = string.Format("Maze {0} Deleted From the List...  ", s);
                    Gen_Flag = false;
                    Output(s);

                }
            }
            else
                Output("Select A Maze To Delete...");

            return;
        }

        ///Display <summary>
        /// Display button, displaing choising mazefrom list to user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click3(object sender, RoutedEventArgs e) 
        {

            cnvs_main.Children.Clear();

            string s = M_list.Text;
            if (s == "Maze List")
            {
                if (M_list.Items.Count == 1)
                {
                    Output("First You Should Generate A Maze...");

                }
                else
                    Output("First Pick A Maze From The List...");
            }
            else
            {
                m_commands["display_maze"].DoCommand("Dis", s);

            }
            cnvs_main.Focus();
            m_mazeBoard.Focus();
            dt = DateTime.Now;






        }

        ///Generate<summary>
        /// generate maze button clik.
        /// take all argument from data. and notfay fo the prsenter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cnvs_main.Children.Clear();
            string Name, mazeHight, mazeWight, m_CellSize;
            bool invalidInput = false;
            Name = "";
            Gen_Flag = true;
            mazeHight = "";
            mazeWight = "";
            m_CellSize = "";
            int ind = 0;
            Exception Missing = new Exception("Missing Argument Exception");
            Exception InvalidMesseg = new Exception("Invalid Argument Exception");
            try
            {
                ind = indexOf("Maze Name");
                Name = Convert.ToString(items[ind].optines.Text);
                ind = indexOf("Height");
                mazeHight = Convert.ToString(items[ind].optines.Text);
                ind = indexOf("Width");
                mazeWight = Convert.ToString(items[ind].optines.Text);


                if ((mazeWight == "") | (Name == "") | (mazeHight == ""))
                {
                    throw Missing;
                }

                ind = indexOf("Cell Size");
                m_CellSize = Convert.ToString(items[ind].optines.Text);
                ind = Convert.ToInt32(items[ind].optines.Text);
                //check for invalid input
                if (ind < 2 | Convert.ToInt32(mazeHight) < 2 | Convert.ToInt32(mazeWight) < 2)
                {
                    invalidInput = true;
                    throw InvalidMesseg;
                      
                }

            }
            catch (System.Exception ex)
            {
                Output(ex.Message);
            }

            if (!invalidInput)
            {
                try
                {

                    m_commands["generate"].DoCommand("gen", mazeWight, mazeHight, Name, m_CellSize);

                    MazeCellsize.Add(Name, ind);


                }
                catch (System.Exception ex)
                {
                    Output(ex.Message);
                }
                for (int i = 0; i < 4; i++)
                {
                    items[i].optines.Clear();
                }

            }

        } 

        ///Display solution <summary>
        /// displaying sultion for maze from list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click1(object sender, RoutedEventArgs e) 
        {
            if (Gen_Flag)
            {
                string s = M_list.Text;
                if (s == "Maze List")
                {
                    if (M_list.Items.Count == 1)
                    {
                        Output("First You Should Generate A Maze....");

                    }
                    else
                        Output("First Pick A Maze From The List....");
                }
                else
                {
                    m_commands["solve"].DoCommand("solve", s);
                }
            }
            else
            {
                Output("Display/Generate The Maze First");
            }
        }

        ///sort<summary>
        /// sorting the argument cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            if (clickCount % 2 == 0)
                lvUsers.ItemsSource = items.OrderBy(x => x.Name);
            else
                lvUsers.ItemsSource = items.OrderBy(x => x.Name.Length % 2);
            clickCount++;

        } 

        ///Load<summary>
        /// loading given maze
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click7(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            string[] linePart;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Maze.."; // Default file name 
            dlg.DefaultExt = ".Maze"; // Default file extension 
            dlg.Filter = "Maze File (.Maze)|*.Maze"; // Filter files by extension 

            // Show open file dialog box 
            Nullable<bool> result = dlg.ShowDialog();
            Exception Invaild = new Exception("Invaild Type Of FILE..");
            if (result == true)
            {

                string filename = dlg.FileName;
                linePart = filename.Split('\\');

                try
                {
                    if (!filename.Contains(".Maze"))
                    {
                        throw Invaild;
                    }

                    if (!M_list.Items.Contains(linePart[linePart.Length - 1].Remove(linePart[linePart.Length - 1].Length - 5)))
                    {
                        m_commands["Load"].DoCommand(filename);
                        M_list.Items.Add(linePart[linePart.Length - 1].Remove(linePart[linePart.Length - 1].Length - 5));

                        Gen_Flag = true;
                    }
                    else
                    {
                        string o = string.Format("Maze {0}... ,Another Maze With The Same Name In The List..", linePart[linePart.Length - 1].Remove(linePart[linePart.Length - 1].Length - 5));
                        Output(o);
                    }
                }
                catch (System.Exception ex)
                {
                    Output(ex.Message);
                }






            }
        }

        ///Setting <summary>
        /// the main setting argument
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Setting_Click(object sender, EventArgs e)
        {
           

            M_setting = new SettingWindow();
            M_setting.PrintEvent += SetOutppt;
            M_setting.Setter += ChangeSetting;
            M_setting.Show();

        }

        ///Exit <summary>
        /// exiting the app by event to present
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, EventArgs e)
        {
            string m_Message = string.Format("Are You Sure You Want To Close Maze Generator?..");
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(m_Message, "Exit", System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                this.Close();
                Application.Current.Shutdown();
            }
        }

        ///Window_KeyDown <summary>
        /// hendling with all the KeyDown events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            var key = e.Key;

            switch (key)
            {
                case Key.Down:
                    m_commands["moveItem"].DoCommand(M_list.Text, m_mazeBoard.ItemLocationX.ToString(), m_mazeBoard.ItemLocationY.ToString(), "down");
                    e.Handled = true;
                    break;
                case Key.Up:
                    m_commands["moveItem"].DoCommand(M_list.Text, m_mazeBoard.ItemLocationX.ToString(), m_mazeBoard.ItemLocationY.ToString(), "up");
                    e.Handled = true;
                    break;
                case Key.Right:
                    m_commands["moveItem"].DoCommand(M_list.Text, m_mazeBoard.ItemLocationX.ToString(), m_mazeBoard.ItemLocationY.ToString(), "right");
                    e.Handled = true;
                    break;
                case Key.Left:
                    m_commands["moveItem"].DoCommand(M_list.Text, m_mazeBoard.ItemLocationX.ToString(), m_mazeBoard.ItemLocationY.ToString(), "left");
                    e.Handled = true;
                    break;
                default:
                    break;
            }
            if (!e.Handled)
                base.OnKeyDown(e);
        }

        ///SetOutppt <summary>
        /// seting output event to consol
        /// </summary>
        public void SetOutppt()
        {
            Output(this.M_setting.Return_State);
        }

        ///ChangeSetting <summary>
        /// change parmente comend
        /// </summary>
        public void ChangeSetting()
        {
            NewSetting();
            string b_d;
            if (diagonel)
            {
                b_d = "true";
            }
            else
                b_d = "false";
            m_commands["ChangeParams"].DoCommand(M_t, b_d, Solving, Huristic, Alg);
        }

        ///NewSetting<summary>
        /// retrive setting argument from setting box
        /// </summary>
        public void NewSetting()
        {
            Solving = this.M_setting.Solving;
            Alg = this.M_setting.Alg;
            Huristic = this.M_setting.Huristic;
            diagonel = this.M_setting.diagonel;
            M_t = this.M_setting.M_t;
        }

        /// messege<summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click9(object sender, RoutedEventArgs e)
        {
            MessageBox.Text = "";
        }

        #endregion



        #region Aid func
        /// MoveIt<summary>
        /// aid to move poit on maze bood. the func connectin betwen View and the MazeBord class
        /// </summary>
        /// <param name="propertyName"></param>

        public void MoveIt(string propertyName)
        {
            switch (propertyName)
            {
                case "up":
                    m_mazeBoard.ItemLocationX -= 1;
                    m_mazeBoard.Move();
                    break;
                case "down":
                    m_mazeBoard.ItemLocationX += 1;
                    m_mazeBoard.Move();
                    break;
                case "left":
                    m_mazeBoard.ItemLocationY -= 1;
                    m_mazeBoard.Move();
                    break;
                case "right":
                    m_mazeBoard.ItemLocationY += 1;
                    m_mazeBoard.Move();
                    break;
                case "win":
                    Thread t = new Thread(() => WinFunc());
                    t.Start();
                    t.Join();

                    break;
            }


        }

        ///WinFunc <summary>
       /// active ween Box for user with sound
       /// </summary>
        private void WinFunc()
        {


            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Images\Mario.wav");
            player.Play();
            TimeSpan ts = DateTime.Now - dt;
            string s = ts.Milliseconds.ToString();
            string m = string.Format("You Managed To Solve The Maze ...");
            string m_Message = string.Format(m + Environment.NewLine + "Your Solving Time: " + s + " Ms");
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(m_Message, "You Win!!!...", System.Windows.MessageBoxButton.OK);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                return;

            }
            Output("*****You-Win*****");
        }

    }

        #endregion Aid func 

}


