using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SudokuSolver
{
    /// <summary>
    /// Interaction logic for SudokuCanvas.xaml
    /// </summary>
    public partial class SudokuCanvas : UserControl
    {
        double cellWidth = 25;
        double cellHeight = 25;
        SamuraiSolver solver5, solver10;
        Sudoku[] sudokus;
        public SudokuCanvas(Sudoku[] sudokus)
        {
            InitializeComponent();
            this.sudokus = sudokus;
            this.solver5 = new SamuraiSolver(sudokus);
            this.solver10 = new SamuraiSolver(sudokus);

            drawSudokus(sudokus);
        }

        private void drawSudokus(Sudoku[] sudokus, bool solved = false, TimeSpan? time = null)
        {
            double offsetX = cellWidth;
            double offsetY = cellHeight;

            for (int y = 0; y < 9; y++)
            {
                for(int x = 0; x < 9; x++)
                {
                    int n = sudokus[0].grid[y][x];
                    drawCell(n, offsetX + x * cellWidth, offsetY + y * cellHeight);
                }
            }

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    int n = sudokus[1].grid[y][x];
                    drawCell(n, offsetX + (x + 12) * cellWidth, offsetY + y * cellHeight);
                }
            }

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    int n = sudokus[2].grid[y][x];
                    drawCell(n, offsetX + (x + 6) * cellWidth, offsetY + (y + 6) * cellHeight);
                }
            }

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    int n = sudokus[3].grid[y][x];
                    drawCell(n, offsetX + x * cellWidth, offsetY + (y + 12) * cellHeight);
                }
            }

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    int n = sudokus[4].grid[y][x];
                    drawCell(n, offsetX + (x + 12) * cellWidth, offsetY + (y + 12) * cellHeight);
                }
            }

            if(solved)
            {
                var textBlock = new TextBlock();
                textBlock.Text = "Çözüldü";
                textBlock.FontSize = 30;
                textBlock.Foreground = Brushes.Red;
                Canvas.SetLeft(textBlock, canvas.Width / 2 - 75);
                Canvas.SetTop(textBlock, canvas.Height - 25);
                canvas.Children.Add(textBlock);
            }

            if(time != null)
            {
                var textBlock = new TextBlock();
                textBlock.Text = time.Value.TotalSeconds.ToString() + " s";
                textBlock.FontSize = 30;
                textBlock.Foreground = Brushes.Red;
                Canvas.SetLeft(textBlock, canvas.Width - 25);
                Canvas.SetTop(textBlock, canvas.Height - 25);
                canvas.Children.Add(textBlock);
            }
        }

        private void drawCell(int n, double x, double y)
        {
            var rect = new Rectangle();
            rect.Stroke = Brushes.Black;
            rect.Fill = Brushes.White;
            //myRect.HorizontalAlignment = HorizontalAlignment.Left;
            //myRect.VerticalAlignment = VerticalAlignment.Center;
            rect.Height = cellHeight;
            rect.Width = cellWidth;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            TextBlock textBlock = new TextBlock();
            textBlock.Text = n.ToString();
            Canvas.SetLeft(textBlock, x + 8);
            Canvas.SetTop(textBlock, y + 5);
            canvas.Children.Add(rect);
            canvas.Children.Add(textBlock);
        }

        private void solveSudoku(SamuraiSolver solver, int threadPerSudoku = 1)
        {
            DateTime t1 = DateTime.Now;
            Thread solverThread = new Thread(new ThreadStart(() =>
            {
                solver.solve(threadPerSudoku);
            }));
            solverThread.Start();

            Thread thread = new Thread(new ThreadStart(() =>
            {
                do
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        canvas.Children.Clear();
                        drawSudokus(solver.sudokus, false, DateTime.Now - t1);
                    });
                    Thread.Sleep(200);
                } while (!solver.solved);
                this.Dispatcher.Invoke(() =>
                {
                    canvas.Children.Clear();
                    drawSudokus(solver.sudokus, true, DateTime.Now - t1);
                });
            }
            ));
            thread.Start();

            //solverThread.Join();
            //drawSudokus(sudokus, true);
        }

        private void solve5ThreadButton_Click(object sender, RoutedEventArgs e)
        {
            if(solver5.solved)
            {
                var db = new SudokuDBContext();
                foreach (var row in db.CellSolutions.Where(e => e.IsThread5))
                {
                    db.CellSolutions.Remove(row);
                }
                db.SaveChanges();
                this.solver5 = new SamuraiSolver(this.sudokus);
            }
            solveSudoku(solver5, 1);
        }

        private void solve10ThreadButton_Click(object sender, RoutedEventArgs e)
        {
            if (solver10.solved)
            {
                var db = new SudokuDBContext();
                foreach (var row in db.CellSolutions.Where(e => !e.IsThread5))
                {
                    db.CellSolutions.Remove(row);
                }
                db.SaveChanges();
                this.solver10 = new SamuraiSolver(this.sudokus);
            }
            solveSudoku(solver10, 2);
        }

        private void showGraphButton_Click(object sender, RoutedEventArgs e)
        {
            if(solver5.solved && solver10.solved)
            {
                var graphWindow = new GraphWindow();
                graphWindow.Show();
            } else
            {
                MessageBox.Show("Solver threadleri bitmedi. İki çöz butonuna da tıklayın","Hata");
            }
        }

        private void resetSudokuButton_Click(object sender, RoutedEventArgs e)
        {
            this.solver5 = new SamuraiSolver(sudokus);
            this.solver10 = new SamuraiSolver(sudokus);
            var db = new SudokuDBContext();
            foreach (var row in db.CellSolutions)
            {
                db.CellSolutions.Remove(row);
            }
            db.SaveChanges();
            canvas.Children.Clear();
            drawSudokus(sudokus);
        }

        //private void solveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Thread solver5Thread = new Thread(new ThreadStart(() =>
        //    {
        //        solver5.solve(1);
        //    }));
        //    Thread solver10Thread = new Thread(new ThreadStart(() =>
        //    {
        //        solver10.solve(2);
        //    }));
        //    solver5Thread.Start();
        //    solver10Thread.Start();

        //    Thread t2 = new Thread(new ThreadStart(() =>
        //    {
        //        do
        //        {
        //            this.Dispatcher.Invoke(() =>
        //            {
        //                canvas.Children.Clear();
        //                drawSudokus(solver5.sudokus);
        //            });
        //            Thread.Sleep(200);
        //        } while (!solver5.solved);
        //        this.Dispatcher.Invoke(() => {
        //            canvas.Children.Clear();
        //            drawSudokus(solver5.sudokus, true);
        //            solver10Thread.Join();
        //            var graphWindow = new GraphWindow();
        //            graphWindow.ShowDialog();
        //            //var db = new SudokuDBContext();
        //            //foreach (var row in db.CellSolutions)
        //            //{
        //            //    db.CellSolutions.Remove(row);
        //            //}
        //            //db.SaveChanges();
        //        });
        //    }
        //    ));
        //    t2.Start();
            
        //}
    }
}
