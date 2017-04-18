using MVP.Model;
using MVP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Presenter.Command
{
    class DeleteMaze : ACommand
    {


        public DeleteMaze(IModel model, IView view)
            : base(model, view) { }

        /// <summary>
        /// displaying given maze fom view class
        /// </summary>
        /// <param name="parameters">maze name</param>
        public override void DoCommand(params string[] parameters)
        {
            this.m_model.Delete(parameters[0]);
            this.m_view.Delete(parameters[0]);
        }
    }
}
