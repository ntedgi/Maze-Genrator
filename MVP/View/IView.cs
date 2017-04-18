using Model.Search;
using MVP.Model.Maze;
using MVP.Presenter.Command;
using ProjectSrc.Model.Search.Domains.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.View
{
    public delegate void ViewFunc();

    public interface IView
    {

        #region Event
        event ViewFunc ViewChanged;
        event ViewFunc StartEvent;
        #endregion


        /// Start<summary>
        /// run the console interface
        /// </summary>
        /// 
        void Start();

        ///SetCommands <summary>
        /// set the commands suppurted in the currnt arcithcture
        /// </summary>
        /// <param name="commands">dictionary commands</param>
        void SetCommands(Dictionary<string, ICommand> commands);

        /// GetUserCommand<summary>
        ///  return from the cli the command need to do
        /// </summary>
        /// <returns></returns>
        ICommand GetUserCommand();

        ///DisplayMaze <summary>
        /// display the cgiven maze
        /// </summary>
        /// <param name="maze">maze name</param>
        void DisplayMaze(extendedMaze maze);

        ///DisplaySolution <summary>
        /// diplay the given solotion
        /// </summary>
        /// <param name="solution">sultion to display</param>
        void DisplaySolution(Solution solution);

        /// SetCommands<summary>
        /// set the command suported by the app
        /// </summary>
        /// <param name="testMVPCommand"></param>
        /// <returns></returns>

        void Output(string p);

        void Delete(string p);

        void MoveIt(string propertyName);
        
        
    }
}
