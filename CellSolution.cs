using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class CellSolution
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public DateTime Date { get; set; }
        public int N { get; set; }
        public bool IsThread5 { get; set; } = true;
        public int ThreadId { get; set; }
       
    }

}
