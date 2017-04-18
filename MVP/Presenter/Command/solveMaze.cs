using MVP.Model;
using MVP.View;
using ProjectSrc.Model.Search.Domains.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Presenter.Command
{
    class solveMaze : ACommand
    {
        public solveMaze(IModel model, IView view)
            : base(model, view) { }

        public override void DoCommand(params string[] parameters)
        {
            try
            {
                m_model.SolveMaze(parameters[1]);
            }
            catch (Exception)
            {

                m_view.Output("Maze Name  Not  Exists");
            }
            
        }
    }
}
