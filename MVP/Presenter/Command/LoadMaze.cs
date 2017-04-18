using MVP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP.View;

namespace MVP.Presenter.Command
{
    class LoadMaze:ACommand
    {
        public LoadMaze(IModel model, IView view)
            : base(model, view) { }
        public override void DoCommand(params string[] parameters)
        {
            m_model.LoadMaze(parameters[0]);
     }
    }
}
