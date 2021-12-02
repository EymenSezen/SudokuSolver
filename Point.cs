using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Point
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class PointComparer : IEqualityComparer<Point>
    {
        public bool Equals(Point a, Point b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public int GetHashCode([DisallowNull] Point obj)
        {
            return obj.x ^ obj.y;
        }
    }
}
