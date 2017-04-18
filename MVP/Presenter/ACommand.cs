using MVP.Model;
using MVP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Presenter
{
    abstract class ACommand : ICommand
    {
        
        #region Fields
        protected IModel m_model;
        protected IView m_view;
        #endregion

        /// ACommand<summary>
        /// absrct class for user commands
        /// </summary>
        /// <param name="model"></param>
        /// <param name="view"></param>
        public ACommand(IModel model, IView view)
        {
            this.m_model = model;
            this.m_view = view;
        }

        abstract public void DoCommand(params string[] parameters);
    }
}
