using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Threading;

namespace SudokuSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Sudoku[] sudokus;

        public MainWindow()
        {
            InitializeComponent();

            var sudokuReader = new SudokuReader();
            sudokuReader.read("examples/sudoku.txt");
            sudokus = sudokuReader.sudokus;
            var sudokuCanvas = new SudokuCanvas(sudokus);
            grid.Children.Add(sudokuCanvas);
        }

        //private void thread5Button_Click(object sender, RoutedEventArgs e)
        //{
        //    sudokus1 = new Sudoku[sudokus.Length];
        //    for (int i = 0; i < sudokus.Length; i++)
        //    {
        //        sudokus1[i] = new Sudoku(sudokus[i]);
        //    }
        //    var solver = new SamuraiSolver(sudokus1);
        //    solver.solve(1);
        //    foreach (var sudoku in sudokus1)
        //    {
        //        MessageBox.Show(sudoku.ToString());
        //    }
        //}

        //private void thread10Button_Click(object sender, RoutedEventArgs e)
        //{
        //    sudokus2 = new Sudoku[sudokus.Length];
        //    for (int i = 0; i < sudokus.Length; i++)
        //    {
        //        sudokus2[i] = new Sudoku(sudokus[i]);
        //    }
        //    var solver = new SamuraiSolver(sudokus2);
        //    solver.solve(2);
        //    foreach (var sudoku in sudokus2)
        //    {
        //        MessageBox.Show(sudoku.ToString());
        //    }
        //}
    }
}
