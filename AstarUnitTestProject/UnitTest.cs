using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Search;
using ProjectSrc.Model.Search.Domains.Maze.Algorithms;
using ProjectSrc.Model.Search.Heuristic;
using Model.Search.Algorithms;
using ProjectSrc.Model.Search.Domains.Maze;
using ProjectSrc.Model.Search.Domains.Maze.Algorithms.Dfs;

namespace AstarUnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void AstarTest()
        {

        }

        #region AstarConstructor()
        /// <summary>
        ///A test for ASearchingAlgorithm Constructor
        ///</summary>
        [TestMethod()]
        public void ASearchingAlgorithm_ConstructorTest()
        {
            IHeuristic AirDistance = new MazeAirDistance();
            ASearchingAlgorithm astarAir = new Astar(AirDistance);
            Assert.IsNotNull(astarAir);
            Assert.IsNotNull(AirDistance);
            Assert.IsInstanceOfType(astarAir, typeof(ASearchingAlgorithm));

        }
        #endregion  AstarConstructor



        #region Equals_Obj_Test_Matching_Same_Solotion

        /// <summary>
        ///A test for Equals(Object inObject) with different last name
        ///</summary>
        [TestMethod()]
        public void Equals_Obj_Test_Matching_Same_Solotion()
        {
            IHeuristic AirDistance = new MazeAirDistance();
            ASearchingAlgorithm astarAir = new Astar(AirDistance);
            MazeGenerator DfsMazeGenerator = new DfsMazeGenerator();
            maze Dfs = DfsMazeGenerator.generatMaze(20, 20);
            ISearchable SearchableMaze = new SearchableMaze(Dfs, false);
            Solution target = astarAir.Solve(SearchableMaze);

            Solution inObject = astarAir.Solve(SearchableMaze);
            bool expected = true;
            bool actual = target.GetSolutionPath().Capacity.Equals(inObject.GetSolutionPath().Capacity);
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Equals(Other Solotion)
        /// <summary>
        ///A test for Equals(String inString) where inString is null
        ///</summary>
        [TestMethod()]
        public void Equals_Sol_Test_null()
        {
            IHeuristic AirDistance = new MazeAirDistance();
            ASearchingAlgorithm astarAir = new Astar(AirDistance);
            MazeGenerator DfsMazeGenerator = new DfsMazeGenerator();
            maze Dfs = DfsMazeGenerator.generatMaze(20, 20);
            ISearchable SearchableMazeA = new SearchableMaze(Dfs, false);
            ISearchable SearchableMazeB = new SearchableMaze(Dfs, true);

            Solution target = astarAir.Solve(SearchableMazeA);
            Solution inObject = astarAir.Solve(SearchableMazeB);
            bool expected = false;
            bool actual = target.Equals(inObject);
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Run100
        /// <summary>
        ///A test for Equals(Object inObject) with different last name
        ///</summary>
        [TestMethod()]
        public void Run100()
        {
            IHeuristic AirDistance = new MazeAirDistance();
            ASearchingAlgorithm astarAir = new Astar(AirDistance);
            MazeGenerator DfsMazeGenerator = new DfsMazeGenerator();
            maze Dfs = DfsMazeGenerator.generatMaze(20, 20);
            ISearchable SearchableMaze = new SearchableMaze(Dfs, true);
            Solution inObject = astarAir.Solve(SearchableMaze);
            Solution target;

            bool expected = true;
            for (int i = 0; i < 100; i++)
            {
                target = astarAir.Solve(SearchableMaze);
                bool actual = target.GetSolutionPath().Capacity.Equals(inObject.GetSolutionPath().Capacity);
                Assert.AreEqual(expected, actual);
            }


        }
        #endregion





    }
}
