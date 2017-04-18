using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MVP;

namespace MVP.View
{
    /// ThreadedCLI<summary>
    /// holds threds of CLI objct 
    /// by objct adpet dising
    /// </summary>
    class ThreadedCLI
    {

        private CLI cli;
        public ThreadedCLI(CLI cli)
        {
            this.cli = cli;
        }

        ///RunCLI <summary>
        /// run CLI in new threds
        /// </summary>
        public void RunCLI()
        {
            Thread CliRun = new Thread(() => cli.Start());
            CliRun.Start();
        }

        /// GeTCliParams<summary>
        /// 
        /// </summary>
        /// <returns>return CLI commands</returns>
        public string[] GeTCliParams()
        {
            return cli.getCommand();

        }
        
        /// getClicommand<summary>
        /// 
        /// </summary>
        /// <returns>return CLI user commands</returns>
        public ICommand getClicommand()
        {
            return this.cli.GetUserCommand();
        }

        /// setCliEvent<summary>
        /// seting new event that recive from CLI
        /// </summary>
        /// <param name="viewFunc"></param>
        internal void setCliEvent(ViewFunc viewFunc)
        {
            this.cli.setEvent(viewFunc);
        }

        /// Output<summary>
        /// writ to consol by adpting thrds
        /// </summary>
        /// <param name="s">sting to output</param>
        public void Output(string s)
        {
            cli.Output(s);
        }

    }
}
