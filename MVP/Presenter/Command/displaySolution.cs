using MVP.Model;
using MVP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP.Model.Maze;
using Model.Search;
using System.Collections;

namespace MVP.Presenter.Command
{
    /// displaySolution<summary>
    /// implament fo Acommand for diplaing solution
    /// </summary>
    class displaySolution : ACommand
    {
        public displaySolution(IModel model, IView view)
            : base(model, view) { }

        /// DoCommand<summary>
        /// using view displayin maze soultion by given name 
        /// </summary>
        /// <param name="parameters">name of maze</param>
        public override void DoCommand(params string[] parameters)
        {
            m_view.DisplaySolution(m_model.GetSolution(parameters[1]));



        }
    }
}

