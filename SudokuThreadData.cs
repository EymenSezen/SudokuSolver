namespace SudokuSolver
{
    public partial class SamuraiSolver
    {
        class SudokuThreadData
        {
            public Sudoku sudoku { get; set; }
            public int i { get; set; }
            public int threadPerSudoku { get; set; }
        }
    }
}
