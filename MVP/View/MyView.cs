using Model.Search;
using MVP.Presenter.Command;
using ProjectSrc.Model.Search.Domains.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using ProjectSrc.Model.Search.Domains.Maze.Algorithms.Dfs;
using MVP.Model.Maze;
namespace MVP.View
{
    /// MyView <summary>
    /// implmant for IView
    /// </summary>
    class MyView : IView
    {

        #region Fields

        public event ViewFunc ViewChanged;
        public event ViewFunc StartEvent;
        public ThreadedCLI cThreadedCLI;
        #endregion

        public MyView() { }

        ///Start <summary>
        /// set start event 
        /// set CLIi threds pool
        /// </summary>
        public void Start()
        {
            StartEvent();

            cThreadedCLI.RunCLI();
        }

        ///SetCommands <summary>
        /// seting the commnds vyh giving dictionary
        /// </summary>
        /// <param name="commands">commmnds dictionary</param>
        public void SetCommands(Dictionary<string, ICommand> commands)
        {
            cThreadedCLI = new ThreadedCLI(new CLI(commands));

            cThreadedCLI.setCliEvent(this.ViewChanged);

        }

        /// <summary>
        /// get user commnsa from runing threds
        /// </summary>
        /// <returns></returns>
        public ICommand GetUserCommand()
        {
            return this.cThreadedCLI.getClicommand();



        }

        /// DisplayMaze<summary>
        /// send to outout given maze to display
        /// </summary>
        /// <param name="maze">maze name to display</param>
        public void DisplayMaze(extendedMaze maze)
        {
            string ans = Environment.NewLine + (string)maze.print();
            Output(ans);
        }

        /// <summary>
        /// display soltion by cordinate from al AState
        /// </summary>
        /// <param name="solution">soul to display</param>
        public void DisplaySolution(Solution solution)
        {
            if (solution != null)
            {
                ArrayList list = solution.GetSolutionPath();

                foreach (AState item in list)
                {
                    ((AState)item).PrintState();

                }
            }
            else
                Output("solution not exist");

        }




        /// Output<summary>
        /// writng to consol by runing threds only!!
        /// </summary>
        /// <param name="p">string to output</param>
        public void Output(string p)
        {
            cThreadedCLI.Output(p);
        }

        public void Delete(string p)
        {
            throw new NotImplementedException();
        }

        public void MoveIt(string propertyName)
        {

        }
      
    }
}