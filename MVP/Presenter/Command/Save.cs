using MVP.Model;
using MVP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Presenter.Command
{
    class Save:ACommand
    {
        public Save(IModel model, IView view)
            : base(model, view) { }
        public override void DoCommand(params string[] parameters)
        {
            m_model.SaveFile(parameters[0],parameters[1]);
        }

    }
}
