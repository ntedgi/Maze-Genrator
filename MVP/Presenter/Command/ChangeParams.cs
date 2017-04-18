using MVP.Model;
using MVP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Presenter.Command
{
    class ChangeParams : ACommand
    {
        public ChangeParams(IModel model, IView view)
            : base(model, view) { }
        public override void DoCommand(params string[] parameters)
        {
            bool Diagonal;
            int N = Convert.ToInt32(parameters[0]);
            if (parameters[1] == "true")
                Diagonal = true;
            else
                Diagonal = false;
            m_model.SetParams(N, Diagonal, parameters[2], parameters[3], parameters[4]);
        }
    }
}
