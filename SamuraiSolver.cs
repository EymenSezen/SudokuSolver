using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SudokuSolver
{
    public partial class SamuraiSolver
    {
        public Sudoku[] sudokus;
        ConcurrentDictionary<int, List<SudokuSolution>> grids = new ConcurrentDictionary<int, List<SudokuSolution>>();
        public bool solved = false;

        public SamuraiSolver(Sudoku[] sudokus)
        {
            this.sudokus = new Sudoku[sudokus.Length];
            for (int i = 0; i < sudokus.Length; i++)
            {
                this.sudokus[i] = new Sudoku(sudokus[i]);
            }
        }

        private void getSudokuSolutions(object sudokuThreadData)
        {
            var data = (SudokuThreadData)sudokuThreadData;
            var sudoku = data.sudoku;
            var threadPerSudoku = data.threadPerSudoku;
            var i = data.i;
            var g = sudoku.getSolutions(threadPerSudoku, 2 * sudoku.ID + i);
            List<SudokuSolution> gg;
            if (grids.TryGetValue(sudoku.ID, out gg))
            {
                gg.AddRange(g);
            }
            else
            {
                grids.TryAdd(sudoku.ID, g);
            }
        }
        public void solve(int threadPerSudoku = 1) //5 threD
        {
            List<Thread> threads = new List<Thread>();
            foreach (var sudoku in sudokus)
            {
                for(int i = 0; i < threadPerSudoku; i++)
                {
                    Thread thread = new Thread(getSudokuSolutions);
                    thread.Start(new SudokuThreadData { sudoku = sudoku, threadPerSudoku = threadPerSudoku, i = i});
                    threads.Add(thread);
                }
            }
            
            foreach (var thread in threads)
            {
                thread.Join();
            }

            foreach (var sol0 in grids[0])
            {
                foreach (var sol1 in grids[1])
                {
                    foreach (var sol2 in grids[2])
                    {
                        foreach (var sol3 in grids[3])
                        {
                            foreach (var sol4 in grids[4])
                            {
                                var sol0AndSol2 = compareSol0AndSol2(sol0.grid, sol2.grid);
                                var sol1AndSol2 = compareSol1AndSol2(sol1.grid, sol2.grid);
                                var sol3AndSol2 = compareSol3AndSol2(sol3.grid, sol2.grid);
                                var sol4AndSol2 = compareSol4AndSol2(sol4.grid, sol2.grid);

                                if (sol0AndSol2 && sol1AndSol2 && sol3AndSol2 && sol4AndSol2)
                                {
                                    lock(sudokus)
                                    {
                                        sudokus[0].grid = sol0.grid;
                                        sudokus[1].grid = sol1.grid;
                                        sudokus[2].grid = sol2.grid;
                                        sudokus[3].grid = sol3.grid;
                                        sudokus[4].grid = sol4.grid;
                                    }

                                    SudokuDBContext dbContext = new SudokuDBContext();
                                    dbContext.CellSolutions.AddRange(sol0.cellSolutions.Values.ToList());
                                    dbContext.CellSolutions.AddRange(sol1.cellSolutions.Values.ToList());
                                    dbContext.CellSolutions.AddRange(sol2.cellSolutions.Values.ToList());
                                    dbContext.CellSolutions.AddRange(sol3.cellSolutions.Values.ToList());
                                    dbContext.CellSolutions.AddRange(sol4.cellSolutions.Values.ToList());
                                    dbContext.SaveChanges();

                                    solved = true;

                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool compareSol0AndSol2(int[][] sol0, int[][] sol2)
        {
            int x, y;
            for (x = 6; x < 9; x++)
            {
                for (y = 6; y < 9; y++)
                {
                    if (sol0[y][x] != sol2[y - 6][x - 6])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool compareSol1AndSol2(int[][] sol1, int[][] sol2)
        {
            int x, y;
            for (x = 0; x < 3; x++)
            {
                for (y = 6; y < 9; y++)
                {
                    if (sol1[y][x] != sol2[y - 6][x + 6])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool compareSol3AndSol2(int[][] sol3, int[][] sol2)
        {
            int x, y;
            for (x = 6; x < 9; x++)
            {
                for (y = 0; y < 3; y++)
                {
                    if (sol3[y][x] != sol2[y + 6][x - 6])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool compareSol4AndSol2(int[][] sol4, int[][] sol2)
        {
            int x, y;
            for (x = 0; x < 3; x++)
            {
                for (y = 0; y < 3; y++)
                {
                    if (sol4[y][x] != sol2[y + 6][x + 6])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
