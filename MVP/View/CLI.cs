//using ConsoleApplication4.Command;
using MVP;
using MVP.Presenter;
using MVP.Presenter.Command;
using MVP.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MVP
{
    internal class CLI
    {
        #region Fields

        public event ViewFunc m_ViewChanged;
        private Stream m_input;
        private Stream m_output;
        private Dictionary<string, ICommand> m_commands;
        private string[] linePartsC = new string[1];
        private StreamWriter M_Writer;
        private ICommand TempCommand;
        private Queue<string[]> CommandQueue;
        private Mutex m_Mutex;

        #endregion

        /// CLI<summary>
        /// Constructor
        /// </summary>
        /// <param name="m_commands">commands dictonary</param>
        public CLI(Dictionary<string, ICommand> m_commands)
        {
            m_input = Console.OpenStandardInput();
            m_output = Console.OpenStandardOutput();
            this.m_commands = m_commands;
            M_Writer = new StreamWriter(m_output);
            CommandQueue = new Queue<string[]>();
            WritePossibleCommands();
            m_Mutex = new Mutex();
        }

        /// Start<summary>
        /// Open A terminal that Suppurt An pre Initilized Commands
        /// </summary>
        public void Start()
        {
            
            while (true)
            {
                string line;
                string[] linePartsC;

                m_Mutex.WaitOne();
                Console.Write(">>");
                byte[] bytes = new byte[100];
                int outputLength = m_input.Read(bytes, 0, 100);
                m_Mutex.ReleaseMutex();

                char[] chars = Encoding.UTF7.GetChars(bytes, 0, outputLength);
                line = new string(chars);
                line = line.Replace("\r\n", string.Empty);
                linePartsC = line.Split(' ');
                CommandQueue.Enqueue(linePartsC);
                if (line == "exit")
                {
                    TempCommand = m_commands[linePartsC[0]];
                    m_ViewChanged();
                    break;
                }
                else
                {
                    if (m_commands.ContainsKey(linePartsC[0]))
                    {
                        TempCommand = m_commands[linePartsC[0]];
                        m_ViewChanged();
                    }
                    else
                        Console.WriteLine("Command Not Found");
                }
                Thread.Sleep(300);
            }
        }

        /// GetUserCommand<summary>
        /// returnd to notification that change
        /// </summary>
        /// <returns>notification that change</returns>      
        public ICommand GetUserCommand()
        {
            return TempCommand;

        }

        ///getCommand <summary>
        /// return the parameters for the corrnt command
        /// </summary>
        /// <returns></returns>
        public string[] getCommand()
        {
            return CommandQueue.Dequeue();
        }


        /// Output<summary>
        /// the output Function get a Return statment after each process and display the Results 
        /// </summary>
        /// <param name="output">String To print</param>
        public void Output(string output)
        {
            try
            {
                m_Mutex.WaitOne();
                M_Writer.Flush();

                M_Writer.WriteLine(">>" + output);

            }
            finally
            {
                if (M_Writer != null)
                    M_Writer.Flush();
                m_Mutex.ReleaseMutex();
            }
        }

        ///setEvent <summary>
        /// event of recining commands
        /// </summary>
        /// <param name="viewFunc"></param>
        internal void setEvent(ViewFunc viewFunc)
        {
            this.m_ViewChanged = viewFunc;
        }

        /// WritePossibleCommands<summary>
        /// print to termonal all possible commands
        /// </summary>
        public void WritePossibleCommands()
        {
            Console.WriteLine("Possible Commands:");
            Console.WriteLine("         Generate Maze : generate <Maze Hight> <Maze Length> <Maze Name>");
            Console.WriteLine("         Display Maze : display_maze <Maze Name>");
            Console.WriteLine("         Solve Maze : solve <Maze Name>");
            Console.WriteLine("         Display Solution : display_solution <Maze Name>");
            Console.WriteLine("         Exit App : exit");
        }

    }
}