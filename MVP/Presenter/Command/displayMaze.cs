using MVP.Model;
using MVP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Presenter.Command
{
    /// displayMaze<summary>
    /// implement for Acommand that displaing maze
    /// </summary>
    class displayMaze : ACommand
    {
        public displayMaze(IModel model, IView view)
            : base(model, view) { }

        /// <summary>
        /// displaying given maze fom view class
        /// </summary>
        /// <param name="parameters">maze name</param>
        public override void DoCommand(params string[] parameters)
        {
            try
            {
                m_view.DisplayMaze(m_model.GetMaze(parameters[1]));
            }
            catch (Exception)
            {

                m_view.Output("no maze to display");
            }

        }
    }
}
