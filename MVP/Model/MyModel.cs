using Model.Search;
using MVP.Model;
using ProjectSrc.Model.Search.Domains.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectSrc.Model.Search.Domains.Maze.Algorithms;
using ProjectSrc.Model.Search.Domains.Maze.Algorithms.Dfs;
using System.Threading;
using MVP.Model.Maze;
using Model.Search.Algorithms;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using ProjectSrc.Model;
using ProjectSrc.Model.Search.Heuristic;
using System.Collections;
using System.Security.Permissions;
using System.ComponentModel;


namespace MVP
{



    public class MyModel : IModel
    {

        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;
        public Mutex Mutex_Add_Maze = new Mutex();
        public Mutex Mutex_Add_Sol = new Mutex();
        public Mutex foreach_Mutex = new Mutex();
        public string m_Status;
        public event PrintF PrintEvent;
        public List<IStoppable> Workers;
        public Dictionary<extendedMaze, Solution> SolutionDictionary;
        public Dictionary<string, extendedMaze> MazeDictionary;


        #endregion

        #region Configure Settings

        public string Curr_Genrator;
        public string Curr_Heuristic;
        public string Curr_Alg;
        public bool IsDiagonal;
        public int N;

        /// SetParams<summary>
        /// stiing configurtion from Setttings
        /// </summary>
        /// <param name="_N">maze number</param>
        /// <param name="_IsDiagonal">solveing with diagonal</param>
        /// <param name="Alg">algorithe type to generate</param>
        /// <param name="Heuristic">hurstic to solv</param>
        /// <param name="Genrator">generator type</param>
        public void SetParams(int _N, bool _IsDiagonal, string Alg, string Heuristic, string Genrator)
        {
            N = _N;
            IsDiagonal = _IsDiagonal;
            Curr_Genrator = Genrator;
            Curr_Heuristic = Heuristic;
            Curr_Alg = Alg;


        }

        #endregion

        /// MyModel<summary>
        /// defult constractor
        /// </summary>
        public MyModel()
        {
            SolutionDictionary = new Dictionary<extendedMaze, Solution>();
            MazeDictionary = new Dictionary<string, extendedMaze>();
            ThreadPool.SetMaxThreads(N, 0);                                         //// Change from Configure
            Workers = new List<IStoppable>();
            m_Status = "";

        }


        #region  ThreadPool Method

        ///GenerateMaze <summary>
        /// gnatrating maze by given parmas
        /// </summary>
        /// <param name="H">maze hight</param>
        /// <param name="W">maze width</param>
        /// <param name="Name">maze name</param>
        public void GenerateMaze(int H, int W, string Name, int Cell)
        {
            string[] parametrs = new string[4];
            parametrs[0] = Convert.ToString(H);
            parametrs[1] = Convert.ToString(W);
            parametrs[2] = Name;
            parametrs[3] = Convert.ToString(Cell);

            ThreadPool.QueueUserWorkItem(ThreadPoolGeneratMaze, parametrs);
        }
        /// SolveMaze<summary>
        /// solving maze by name
        /// </summary>
        /// <param name="p">maze name to solve</param>
        public void SolveMaze(string p)
        {
            if (MazeDictionary.Count != 0)
            {
                ThreadPool.QueueUserWorkItem(ThreadPoolSolveMaze, p);
            }
            else
            {
                m_Status = string.Format("{0} Does not Exist ,generate first...", p);
                PrintEvent();
            }

        }

        /// ThreadPoolGeneratMaze<summary>
        /// thrading to genrate maze by give algorithem
        /// </summary>
        /// <param name="param">'hight' 'width' 'name'</param>
        public void ThreadPoolGeneratMaze(object param)
        {
            string[] p = (string[])param;

            IStoppable NewMazeGenerator;
            if (Curr_Genrator == "Dfs")//// Change from Configure
                NewMazeGenerator = new DfsMazeGenerator();
            else
                NewMazeGenerator = new RandomMazeGenerator();

            Workers.Add(NewMazeGenerator);
            maze m_Maze = new maze();
            m_Status = string.Format("Maze {0} started Genarate...", p[2]);
            PrintEvent();
            
                m_Maze = (maze)(((MazeGenerator)NewMazeGenerator).generatMaze(Convert.ToInt32(p[0]), Convert.ToInt32(p[1])));
          
            if (m_Maze != null)
            {
                extendedMaze ex_Maze = new extendedMaze(m_Maze);
                ex_Maze.setName(p[2]);
                if (p[3] != "0")
                {
                    ex_Maze.SaveGuiParams(p[3]);
                }
                addMaze(ex_Maze, p[2]);
                m_Status = string.Format("Maze {0} is ready...", p[2]);
                PrintEvent();
            }
        }

        ///ThreadPoolSolveMaze <summary>
        /// thrding to solve maze
        /// </summary>
        /// <param name="p">name to solve</param>
        public void ThreadPoolSolveMaze(object p)
        {
            bool flag = false;
            bool exict = false;
            string Name = (string)p;
            extendedMaze m_Maze = new extendedMaze();
            m_Status = string.Format("started Solving Maze {0}...", p);
            PrintEvent();
            try
            {
                m_Maze = MazeDictionary[Name];
                if (SolutionDictionary.ContainsKey(m_Maze))
                {
                    m_Status = string.Format("Solution For {0} Allready Exict In Dictionary", p);
                    PrintEvent();
                    exict = true;
                }
            }
            catch (System.Exception ex)
            {
                m_Status = ex.Message;
                PrintEvent();

            }


            if (!exict)
            {
                IStoppable Alg;
                if (Curr_Alg == "A*")                                           //Setting Cunfigure
                {                                                               //*****************
                    if (Curr_Heuristic == "MazeAirDistance")                    //3 Diffrent Algorithm
                        Alg = new Astar(new MazeAirDistance());                 //Astar->MazeAirDistance
                    else                                                        //Astar->MazeManhattanDistance
                        Alg = new Astar(new MazeManhattanDistance());           //UCS
                }
                else
                    Alg = new UCS();
                Workers.Add(Alg);
                try
                {
                    foreach_Mutex.WaitOne();
                    foreach (KeyValuePair<extendedMaze, Solution> item in SolutionDictionary)
                    {
                        if (item.Key.getGrid() == m_Maze.getGrid())                                                                               // Ovaride equals to chack if 
                        {                                                                                                                         //a sol is Allready  Exict
                            m_Status = string.Format("Solution  for {0} Allready Exict In {1}", m_Maze.getName(), item.Key.getName());             // if he does copy it 
                            PrintEvent();
                            Solution sol = SolutionDictionary[item.Key];
                            if (sol != null && sol.GetSolutionPath().Count > 0)
                                addSol(m_Maze, sol);
                            flag = true;
                            break;
                        }
                    }
                    foreach_Mutex.ReleaseMutex();

                    if (!flag)
                    {
                        ISearchable Searchable_Maze;
                        if (!IsDiagonal)                                                               //Diagonal Setting
                            Searchable_Maze = new SearchableMaze(m_Maze, false);                       //****************
                        else                                                                           //False->Without
                            Searchable_Maze = new SearchableMaze(m_Maze, true);                        //True->With
                        Solution sol = (Solution)(((ASearchingAlgorithm)Alg).Solve(Searchable_Maze));
                        ArrayList list = sol.GetSolutionPath();
                        ArrayList states = new ArrayList();
                        foreach (AState item in list)
                        {
                            states.Add(item.GetState());
                        }
                        m_Maze.m_Solution = m_Maze.printAfterSolution(states);

                        m_Status = string.Format("{0} result Finished...", p);
                        PrintEvent();
                        if (sol != null && sol.GetSolutionPath().Count > 0)
                            addSol(m_Maze, sol);
                    }
                }
                catch
                {
                    m_Status = "Cant Find Maze";
                    PrintEvent();

                }
            }

        }

        #endregion

        /// Sttoping all working process
        /// </summary>
        public void Stop()
        {
            foreach (IStoppable Work in Workers)
            {
                Work.Stop();
            }
        }

        /// <summary>
        /// adding a new maze for dictionary
        /// </summary>
        /// <param name="Maze"></param>
        /// <param name="name"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
      
        public void addMaze(extendedMaze Maze, string name)
        {
            Mutex_Add_Maze.WaitOne();
            try
            {
                MazeDictionary.Add(name, Maze);

            }
            catch (System.Exception ex)
            {
                m_Status = ex.Message;
                PrintEvent();
                Mutex_Add_Maze.ReleaseMutex();

                Thread.CurrentThread.Abort();

            }
            Mutex_Add_Maze.ReleaseMutex();
        }

        /// addSol<summary>
        /// adding soltion to dictionary
        /// </summary>
        /// <param name="maze">maze</param>
        /// <param name="sol">maze soltion</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public void addSol(extendedMaze maze, Solution sol)
        {
            Mutex_Add_Sol.WaitOne();
            try
            {
                SolutionDictionary.Add(maze, sol);

            }
            catch (System.Exception ex)
            {
                m_Status = ex.Message;
                PrintEvent();
                Mutex_Add_Maze.ReleaseMutex();

                Thread.CurrentThread.Abort();

            }



            Mutex_Add_Sol.ReleaseMutex();
        }

        ///PrintAll <summary>
        /// print all given maze
        /// </summary>
        public void PrintAll()
        {
            foreach (KeyValuePair<string, extendedMaze> item in MazeDictionary)
            {
                Console.WriteLine(item.Value.print());
            }
        }

        ///GetSolution <summary>
        /// geting soluting for maze by name
        /// </summary>
        /// <param name="name">given name to solve</param>
        /// <returns>Solution for maze</returns>
        public Solution GetSolution(string name)
        {

            Solution sol = new Solution();
            try
            {
                sol = SolutionDictionary[MazeDictionary[name]];
                return sol;
            }
            catch (System.Exception ex)
            {
                m_Status = string.Format(ex.Message);
                return null;

            }

        }

        /// GetData<summary>
        /// geting count of maze in dictionary
        /// </summary>
        public void GetData()
        {
            Console.WriteLine("{0} Maze Generate ", MazeDictionary.Count);
            Console.WriteLine("{0} Maze Solved ", SolutionDictionary.Count);

        }

        /// GetMaze<summary>
        /// maze geter form dictionary by name
        /// </summary>
        /// <param name="Name">name to get</param>
        /// <returns>maze</returns>
        public extendedMaze GetMaze(string Name)
        {
            extendedMaze Ans = new extendedMaze();
            try
            {
                Ans = MazeDictionary[Name];

            }
            catch (System.Exception ex)
            {
                m_Status = string.Format(ex.Message);
                PrintEvent();

            }
            return Ans;

        }

        /// GetStatus<summary>
        /// geting process statos
        /// </summary>
        /// <returns></returns>
        public string GetStatus()
        {
            return m_Status;
        }




        #region UnPacking Program Start

        /// <summary>
        /// deserialas for .dll objct
        /// </summary>
        /// <param name="arr">dctionary to desirlstion</param>
        /// <returns></returns>
        private object Deserialize(byte[] arr)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(arr))
            {
                object obj = bf.Deserialize(memoryStream);
                return obj;
            }
        }

        ///StartProgram <summary>
        /// unpecank .zip file to array byte
        /// and diliting exsisting file
        /// </summary>
        public void StartProgram()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "MazeVsString.zip") && File.Exists(Directory.GetCurrentDirectory() + "MazeVsString.zip"))
            {
                DoUnZip(Directory.GetCurrentDirectory() + "MazeVsString.zip");
                DoUnZip(Directory.GetCurrentDirectory() + "MazeVsSol.zip");
                byte[] mazes = File.ReadAllBytes(Directory.GetCurrentDirectory() + "MazeVsString\\DebugMazeVsString");
                byte[] solutions = File.ReadAllBytes(Directory.GetCurrentDirectory() + "MazeVsSol\\DebugMazeVsSol");
                MazeDictionary = (Dictionary<string, extendedMaze>)Deserialize(mazes);
                SolutionDictionary = (Dictionary<extendedMaze, Solution>)Deserialize(solutions);
                File.Delete(Directory.GetCurrentDirectory() + "MazeVsString.zip");
                File.Delete(Directory.GetCurrentDirectory() + "MazeVsSol.zip");
                Directory.Delete(Directory.GetCurrentDirectory() + "MazeVsString", true);
                Directory.Delete(Directory.GetCurrentDirectory() + "MazeVsSol", true);
            }

        }

        /// <summary>
        /// unzipnig giving path
        /// </summary>
        /// <param name="parameters">path of file</param>
        public void DoUnZip(params string[] parameters)
        {
            try
            {
                ZipFile.ExtractToDirectory(parameters[0], parameters[0].Substring(0, parameters[0].IndexOf(".")));
                Console.WriteLine("Extracted!");
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("The process failed: " + e.ToString()));
            }

        }
        #endregion



        #region Exit Program Methoods

        ///Exit <summary>
        /// save solutions and maze dictionary to zip
        /// </summary>
        public void Exit()
        {
            byte[] MazeVsString = ObjetToByteArr(MazeDictionary);
            byte[] MazeVsSol = ObjetToByteArr(SolutionDictionary);
            File.WriteAllBytes(Directory.GetCurrentDirectory() + "MazeVsString", MazeVsString);
            File.WriteAllBytes(Directory.GetCurrentDirectory() + "MazeVsSol", MazeVsSol);
            DoZip(Directory.GetCurrentDirectory() + "MazeVsString");
            DoZip(Directory.GetCurrentDirectory() + "MazeVsSol");
            File.Delete(Directory.GetCurrentDirectory() + "MazeVsString");
            File.Delete(Directory.GetCurrentDirectory() + "MazeVsSol");
        }

        /// <summary>
        /// compress to .zip file by giving params
        /// </summary>
        /// <param name="parameters">dictionary to compresss</param>
        public void DoZip(params string[] parameters)
        {
            try
            {
                string[] split = parameters[0].Split('.');
                string tempPath = split[0] + "_temp";
                Directory.CreateDirectory(tempPath);
                string[] split2 = parameters[0].Split('\\');
                System.IO.File.Copy(parameters[0], tempPath + "\\" + split2[split2.Length - 1], true);
                ZipFile.CreateFromDirectory(tempPath, split[0] + ".zip");
                Directory.Delete(tempPath, true);
                Console.WriteLine("Zip file created!");
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("The process failed: " + e.ToString()));
            }

        }

        /// ObjetToByteArr<summary>
        /// convert an objct to byte array
        /// </summary>
        /// <param name="obj">dictionary to convert</param>
        /// <returns>obj by byte arr</returns>
        private byte[] ObjetToByteArr(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bf.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        #endregion


        #region appUtil

        ///Delete <summary>
        /// delet maze from dictionay
        /// </summary>
        /// <param name="p">maze name to delete</param>
        public void Delete(string p)
        {
            MazeDictionary.Remove(p);
            extendedMaze toDel = new extendedMaze();

            foreach (KeyValuePair<extendedMaze, Solution> item in SolutionDictionary)
            {
                if (item.Key.getName() == p)
                {
                    toDel = item.Key;
                    SolutionDictionary.Remove(toDel);
                    break;
                }

            }
        }

        ///SaveFile<summary>
        /// seving given maze
        /// </summary>
        /// <param name="p">path</param>
        /// <param name="Name">save name</param>
        public void SaveFile(string p, string Name)
        {
            string[] linePart;
            string S;
            linePart = p.Split('\\');
            S = linePart[linePart.Length - 1];
            p = p.Remove(p.Length - S.Length, S.Length);
            if (MazeDictionary.ContainsKey(Name))
            {
                extendedMaze MazeToSave = MazeDictionary[Name];
                byte[] m_File = ObjetToByteArr(MazeToSave);
                File.WriteAllBytes(Path.GetFullPath(p) + S, m_File);
            }
        }

        ///LoadMaze <summary>
        /// load saved maze from given path
        /// </summary>
        /// <param name="p">path</param>
        public void LoadMaze(string p)
        {
            byte[] mazes = File.ReadAllBytes(Path.GetFullPath(p));
            extendedMaze MazeToSave = (extendedMaze)Deserialize(mazes);
            MazeDictionary.Add(MazeToSave.getName(), MazeToSave);
            m_Status = "Maze_Cell " + MazeToSave.getName() + " " + Convert.ToString(MazeToSave.m_CellSize);
            PrintEvent();

        }

        ///NotifyPropertyChanged <summary>
        /// event from Presnt, that somthing has ching in View
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        ///move <summary>
        ///check fo vialid move, from given arguments 
        /// </summary>
        /// <param name="parameters">maze name (1), poit postion (x,y)(1,2) , up/down/left/right (3)</param>
        public void move(string[] parameters)
        {
            AState Agoal;
            string[] goal = null;

            extendedMaze maze = null;
            int x = 0;
            int y = 0;
            string m_command = "";

            Exception Missing = new Exception("Missing or incorrect Argument Exception");

            try
            {

                maze = MazeDictionary[parameters[0]];
                x = Convert.ToInt32(parameters[1]);
                y = Convert.ToInt32(parameters[2]);
                m_command = parameters[3];
                Agoal = maze.GetGoalState();
                goal = Agoal.GetState().Split(',');
              //  if (x == Convert.ToInt32(goal[0]) & y == Convert.ToInt32(goal[1]))
              //      m_command = "win";

            }
            catch (System.Exception ex)
            {
                m_Status = ex.Message;
                PrintEvent();
            }

            if (x == Convert.ToInt32(goal[0]) & y == Convert.ToInt32(goal[1]))
                NotifyPropertyChanged("win");

            switch (m_command)
            {
                case "up":
                    if (y > 1)
                    {
                        if (x == Convert.ToInt32(goal[0]) & (y - 1) == Convert.ToInt32(goal[1]))
                        {
                            NotifyPropertyChanged("win");
                            NotifyPropertyChanged("left");


                        }
                        else if (maze._Maze[x, y - 1] != 1)
                            NotifyPropertyChanged("left");
                    }
                        break;
                    
                case "down":
                        if (x == Convert.ToInt32(goal[0]) & (y + 1) == Convert.ToInt32(goal[1]))
                        {
                            NotifyPropertyChanged("win");
                            NotifyPropertyChanged("right");

                        }
                        else if (y < maze._Maze.GetLength(1) & maze._Maze[x, y + 1] != 1)
                            NotifyPropertyChanged("right");
                    break;
                case "left":
                    if (x - 1 == Convert.ToInt32(goal[0]) & (y) == Convert.ToInt32(goal[1]))
                    {
                        NotifyPropertyChanged("win");
                        NotifyPropertyChanged("up");

                    }
                    else if (x > 1 & maze._Maze[x - 1, y] != 1)
                        NotifyPropertyChanged("up");
                    break;

                case "right":
                    if (x + 1 == Convert.ToInt32(goal[0]) & (y) == Convert.ToInt32(goal[1]))
                    {
                        NotifyPropertyChanged("win");
                        NotifyPropertyChanged("down");

                    }
                    else if (x < maze._Maze.GetLength(0) & maze._Maze[x + 1, y] != 1)
                    {
                        NotifyPropertyChanged("down");
                    }
                    break;

            }





        }

        #endregion appUtil

    }
}
