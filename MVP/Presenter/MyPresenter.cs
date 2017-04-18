using MVP.Model;
using MVP.Presenter.Command;
using MVP.View;
using ProjectSrc.Model.Search.Domains.Maze;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP
{



    public class MyPresenter
    {
        #region Fields

        private IModel m_model;
        private IView m_view;
        private Dictionary<string, ICommand> commands;
        #endregion



        #region Configure Settings
        /// <summary>
        /// fields for given setting by user
        /// </summary>
        public int N = Settings.Default.Threads; //Thrades
        public string Genrator = Settings.Default.MazeGenerator[1];
        public string Alg = Settings.Default.SolvingAlgorithm[0];
        public string lHeuristic = Settings.Default.lHeuristic[0];
        public bool IsDiagonal = Settings.Default.Diagonal;

        #endregion

        /// MyPresenter<summary>
        /// initialze commands in dictonary
        /// </summary>
        /// <param name="model">model of algorithems</param>
        /// <param name="view">user view</param>
        public MyPresenter(IModel model, IView view)
        {
            this.m_model = model;
            this.m_view = view;
            this.commands = new Dictionary<string, ICommand>();
            commands.Add("generate", new GenerateMaze(this.m_model, this.m_view));
            commands.Add("display_maze", new displayMaze(this.m_model, this.m_view));
            commands.Add("solve", new solveMaze(this.m_model, this.m_view));
            commands.Add("display_solution", new displaySolution(this.m_model, this.m_view));
            commands.Add("test", new TestMVPCommand(this.m_model, this.m_view));
            commands.Add("exit", new ExitCommand(this.m_model, this.m_view));
            commands.Add("Delete", new DeleteMaze(this.m_model, this.m_view));
            commands.Add("Save", new Save(this.m_model, this.m_view));
            commands.Add("Load", new LoadMaze(this.m_model, this.m_view));
            commands.Add("ChangeParams", new ChangeParams(this.m_model, this.m_view));
            commands.Add("moveItem", new MoveItem(this.m_model, this.m_view));


            SetEvents();


        }

        /// SetEvents<summary>
        /// event init: print view start and setiing init
        /// </summary>
        private void SetEvents()
        {
            m_model.PrintEvent += PrintModel;

            if (this.m_view is MyView)
            {
                m_view.ViewChanged += viewEvent;
            }
            else
                m_view.ViewChanged += ChangeParams;
            m_view.StartEvent += StartProgram;
            m_view.SetCommands(commands);
            m_model.SetParams(N, IsDiagonal, Alg, lHeuristic, Genrator);
            m_model.PropertyChanged += delegate(Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };

        }
        private void NotifyPropertyChanged(string propertyName)
        {
            m_view.MoveIt(propertyName);

        }

        /// viewEvent<summary>
        /// get event from CLI, geting command from vew and active the command
        /// </summary>
        private void viewEvent()
        {
            m_view.GetUserCommand().DoCommand(((MyView)(this.m_view)).cThreadedCLI.GeTCliParams());

        }

        ///StartProgram <summary>
        /// StartProgram from model
        /// </summary>
        private void StartProgram()
        {
            m_model.StartProgram();
        }

        ///PrintModel <summary>
        /// geting status from model and send oit to output
        /// </summary>
        private void PrintModel()
        {
            string s = m_model.GetStatus();
            m_view.Output(s);
        }
        private void ChangeParams()
        {

        }


    }
}