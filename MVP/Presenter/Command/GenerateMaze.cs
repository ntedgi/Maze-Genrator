using MVP.Model;
using MVP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Presenter.Command
{
    class GenerateMaze : ACommand
    {
        public GenerateMaze(IModel model, IView view)
            : base(model, view) { }

        public override void DoCommand(params string[] parameters)
        {
            int H, W, CellSize;
            string Name;
            try
            {
                H = Convert.ToInt32(parameters[1]);
                W = Convert.ToInt32(parameters[2]);

                Name = parameters[3];
                if ((this.m_view is MyView))
                {
                    m_model.GenerateMaze(H, W, Name, 0);
                }
                else
                {
                    CellSize = Convert.ToInt32(parameters[4]);

                    m_model.GenerateMaze(H, W, Name, CellSize);
                }




            }
            catch (System.Exception ex)
            {
                m_view.Output(ex.Message);
            }
        }
    }
}
