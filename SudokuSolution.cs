using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuSolution
    {
        public int[][] grid;
        public Dictionary<Point, CellSolution> cellSolutions;
        public SudokuSolution(int[][] grid, Dictionary<Point, CellSolution> cellSolutions)
        {
            int[][] copy = new int[grid.Length][];
            for (int i = 0; i < grid.Length; i++)
            {
                copy[i] = new int[grid[i].Length];
                for (int j = 0; j < grid[i].Length; j++)
                {
                    copy[i][j] = grid[i][j];
                }
            }
            this.grid = copy;
            this.cellSolutions = cellSolutions;
        }
    }
}
