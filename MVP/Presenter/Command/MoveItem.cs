using MVP.Model;
using MVP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Presenter.Command
{
    class MoveItem: ACommand
    {
        public MoveItem(IModel model, IView view)
            : base(model, view) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">maze name, poit postion , up/down/left/right</param>
        public override void DoCommand(params string[] parameters)
        {
            m_model.move(parameters);
        }
    }
}
