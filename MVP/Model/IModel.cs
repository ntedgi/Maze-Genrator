using Model.Search;
using MVP.Model.Maze;
using ProjectSrc.Model;
using ProjectSrc.Model.Search.Domains.Maze;
using ProjectSrc.Model.Search.Domains.Maze.Algorithms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Model
{
    /// <summary>
    /// delagate for events
    /// </summary>
    public delegate void ModelFunc();
    public delegate void PrintF();

    /// IModel<summary>
    /// inteface for model
    /// </summary>
    public interface IModel
    {
        event PrintF PrintEvent;
        event PropertyChangedEventHandler PropertyChanged;
        void GenerateMaze(int H, int W, string Name,int cell);
        extendedMaze GetMaze(string Name);
        void SolveMaze(string N_maze);
        Solution GetSolution(string name);
        void Stop();
        void PrintAll();
        void Exit();
        void GetData();
        void StartProgram();
        string GetStatus();
        void Delete(string p);

        void SetParams(int _N, bool _IsDiagonal, string Alg, string Heuristic, string Genrator);


        void SaveFile(string p,string Name);

        void LoadMaze(string p);

        void move(string[] parameters);
    }
}
