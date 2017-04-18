using ProjectSrc.Model.Search.Domains.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ProjectSrc.Model.Search.Domains.Maze.Algorithms;
using Model.Search;
using Model.Search.Algorithms;
using ProjectSrc.Model;
using ProjectSrc.Model.Search.Domains.Maze.Algorithms.Dfs;
using System.Collections;

namespace MVP.Model.Maze
{
    ///extendedMaze <summary>
    ///extingding the maze class
    ///doing object adapt to calls maze 
    /// </summary>

    [Serializable]

    public class extendedMaze : maze
    {
        #region Fields

        private string m_Name;
        private string m_GridView;
        public string m_Solution;
        public int m_CellSize;
        #endregion

        #region Constructors
        public extendedMaze()
            : base()
        {
            m_Name = "";
        }
        public extendedMaze(maze maze)
            : base(maze)
        {
        }
        #endregion

        #region Getters

        public string getName()
        {
            return m_Name;
        }
        public string getGrid()
        {
            return m_GridView;
        }
     
        #endregion
        ///setName <summary>
        /// stting a name for bulding maze
        /// </summary>
        /// <param name="name">name for maze</param>
        public void setName(string name)
        {
            m_Name = name;

             for (int i = 0; i < _Maze.GetLength(0); i++)
                 for (int j = 0; j < _Maze.GetLength(1); j++)
                     m_GridView += Convert.ToString(_Maze[i, j]);
        }

        ///printAfterSolution <summary>
        /// printing the solutin by cordination
        /// </summary>
        /// <param name="sol">giving sol</param>
        /// <returns>sting of cordination</returns>
        public string printAfterSolution(ArrayList sol)
        {
            string s = "";
            string s_sol = Environment.NewLine;
            for (int i = 0; i < this._Maze.GetLength(0); i++)
            {
                for (int j = 0; j < this._Maze.GetLength(1); j++)
                {
                    if (_Maze[i, j] == 0)
                    {
                        s = i + "," + j;
                        if (sol.Contains(s))
                            s_sol += "+";
                        else
                            s_sol += " ";
                    }
                    else
                        s_sol += "0";
                }
                s_sol += Environment.NewLine;
            }
            return s_sol;
        }

        /// Equals<summary>
        /// 
        /// </summary>comper beteen soltion
        /// <param name="obj">sol soltion</param>
        /// <returns>bool</returns>
    //   public override bool Equals(System.Object obj)
    //   {
    //       // If parameter is null return false.
    //       if (obj == null)
    //       {
    //           return false;
    //       }
    //
    //       // If parameter cannot be cast to Point return false.
    //       extendedMaze p = obj as extendedMaze;
    //       if ((System.Object)p == null)
    //       {
    //           return false;
    //       }
    //
    //       // Return true if the fields match:
    //       return this.m_Name == p.m_Name;
    //   }

        /// <summary>
        /// overload the operator '==' for tow given extendMaze
        /// </summary>
        /// <param name="a">exteandMaze a</param>
        /// <param name="b">extendMaze b</param>
        /// <returns></returns>
  //    public static bool operator ==(extendedMaze a, extendedMaze b)
  //    {
  //        // If both are null, or both are same instance, return true.
  //        if (System.Object.ReferenceEquals(a.m_Name, b.m_Name))
  //        {
  //            return true;
  //        }
  //
  //        // If one is null, but not both, return false.
  //        if (((object)a == null) || ((object)b == null))
  //        {
  //            return false;
  //        }
  //
  //        // Return true if the fields match:
  //        return a.m_Name == b.m_Name;
  //    }
     //public static bool operator !=(extendedMaze a, extendedMaze b)
     //{
     //    if (b == null)
     //        return true;
     //
     //    return !(a.m_Name == b.m_Name);
     //}

        public void SaveGuiParams(string Cell)
        {
            m_CellSize = Convert.ToInt32(Cell);
        }


    }
}

