using MVP.Model;
using MVP.View;
using ProjectSrc.Model.Search.Domains.Maze.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MVP.Presenter.Command
{
    /// ExitCommand<summary>
    /// class implemnt for Acommand.
    /// exiting from program
    /// </summary>
    class ExitCommand:ACommand
    {
        public ExitCommand(IModel model, IView view)
            : base(model, view) { }

        /// <summary>
        /// command to exit.
        /// stoping all runing thrads
        /// exit
        /// </summary>
        /// <param name="parameters">sting exit</param>
        public override void DoCommand(params string[] parameters)
        {
            m_model.Stop();

            m_model.Exit();
        }


    }
}
