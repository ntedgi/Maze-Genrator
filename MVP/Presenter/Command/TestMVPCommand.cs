
using MVP.Model;
using MVP.View;
using ProjectSrc.Model.Search.Domains.Maze.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MVP.Presenter.Command
{
    class TestMVPCommand : ACommand
    {
        ///TestMVPCommand <summary>
        /// class of testing code!!!
        /// sulde not be use by newb!!
        /// </summary>
        /// <param name="model"></param>
        /// <param name="view"></param>
        public TestMVPCommand(IModel model, IView view)
            : base(model, view) { }

        public override void DoCommand(params string[] parameters)
        {
            Console.WriteLine();
            Console.WriteLine("TestMVPCommand - doCommand");
            Console.WriteLine("**************************");
            bool ans = false;
            for (int i = 0; i < 150; i++)
            {
                if (!ans)
                    this.m_model.GenerateMaze(5, 5, i + "",0);
                if (i == 96)
                {
                    m_model.Stop();
                    ans = true;
                }

            }

            Thread.Sleep(2500);
            Console.WriteLine();
            this.m_model.GetData();
            Console.WriteLine();
            Thread.Sleep(5000);
            Console.WriteLine("TestMVPCommand - SolveMaze");
            Console.WriteLine("**************************");
            for (int i = 0; i < 90; i++)
            {

                this.m_model.SolveMaze(i + "");

            }
            Thread.Sleep(10000);
            Console.WriteLine();
            this.m_model.GetData();
            Console.WriteLine();
            Thread.Sleep(1000);
            m_model.PrintAll();
            Thread.Sleep(10000);

            
        }
    }
}
