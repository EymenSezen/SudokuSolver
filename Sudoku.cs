using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SudokuSolver
{
    public class Sudoku
    {
        public int ID;
        public int[][] grid;
        public Sudoku(int ID)
        {
            this.ID = ID;
            this.grid = new int[9][];
        }

        public Sudoku(Sudoku other)
        {
            int[][] copy = new int[other.grid.Length][];
            for (int i = 0; i < other.grid.Length; i++)
            {
                copy[i] = new int[other.grid[i].Length];
                for (int j = 0; j < other.grid[i].Length; j++)
                {
                    copy[i][j] = other.grid[i][j];
                }
            }
            this.ID = other.ID;
            this.grid = copy;
        }
        
        public void addRow(int i, int[] row)
        {
            this.grid[i] = row;
        }

        private bool isPossible(int y, int x, int n)
        {
            for (int i = 0; i < 9; i++)
            {
                if(grid[y][i] == n)
                {
                    return false;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (grid[i][x] == n)
                {
                    return false;
                }
            }
            int x0 = x / 3 * 3;
            int y0 = y / 3 * 3;
            for (int i=0;i<3;i++)
            {
                for(int j=0;j<3;j++)
                {
                    if(grid[y0 + i][x0 + j] == n)
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public List<SudokuSolution> getSolutions(int threadPerSudoku, int threadId)
        {
            List<SudokuSolution> solutions = new List<SudokuSolution>();
            Dictionary<Point, CellSolution> cellSolutions = new Dictionary<Point, CellSolution>(new PointComparer());
            getSolutions(solutions, cellSolutions, threadPerSudoku, threadId);
            return solutions;
        }

        private bool getSolutions(List<SudokuSolution> solutions, Dictionary<Point, CellSolution> cellSolutions, int threadPerSudoku, int threadId)
        {
            // threadler için ayrı ayrı iki tane başlangıç noktası oluyor (threadlere göre)
            for (int y=0;y<9;y++)
            {
                for(int x=0;x<9;x++)
                {
                    if(grid[y][x]==0)
                    {
                        for(int n=1;n<10;n++)
                        {
                            if (isPossible(y, x, n))
                            {
                                grid[y][x] = n;
                                DateTime d = DateTime.Now;
                                var cellSolution = new CellSolution
                                {
                                    X = x,
                                    Y = y,
                                    Date = d,
                                    N = n,
                                    IsThread5 = threadPerSudoku >= 2 ? false : true,
                                    ThreadId = threadId,
                                };
                                var p = new Point
                                {
                                    y = y,
                                    x = x,
                                };
                                cellSolutions[p] = cellSolution;
                                getSolutions(solutions, cellSolutions, threadPerSudoku, threadId);
                                grid[y][x] = 0;
                            }
                        }
                        return false;
                    }
                }
            }

            var solution = new SudokuSolution(grid, cellSolutions);
            solutions.Add(solution);
            cellSolutions.Clear();
            return true;
        }

        override public string ToString()
        {
            string s = String.Format("------ Sudoku #{0} ------\n", this.ID);
            foreach(var row in this.grid)
            {
                foreach (var cell in row)
                {
                    s += String.Format("{0}, ", cell);
                }
                s += '\n';
            }

            return s;
        }
    }

}
