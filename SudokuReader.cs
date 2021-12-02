using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class SudokuReader
    {
        private const int SUDOKU_NUM = 5;
        public Sudoku[] sudokus = new Sudoku[SUDOKU_NUM];
        public void read(string filename)
        {
            List<List<int>> cells = new List<List<int>>();
            var lines = System.IO.File.ReadAllLines(filename);
            foreach(var line in lines)
            {
                List<int> row = new List<int>();
                foreach(var c in line)
                {
                    if(c >= '1' && c <= '9')
                    {
                        row.Add((int)Char.GetNumericValue(c));
                    }
                    else if(Char.IsWhiteSpace(c))
                    {
                        continue;
                    }
                    else
                    {
                        row.Add(0);
                    }
                }
                cells.Add(row);
            }
            if(cells.Count != 21)
            {
                throw new Exception("Dosya satır sayısı 21 olmalılı");
            }
            for(int i = 0; i < SUDOKU_NUM; i++)
            {
                sudokus[i] = new Sudoku(i);
            }

            for (int i = 0; i < 6; i++) {
                if(cells[i].Count != 18)
                {
                    throw new Exception("İlk 6 satırın uzunluğu 18 olmalı");
                }
                sudokus[0].addRow(i, cells[i].GetRange(0, 9).ToArray());
                sudokus[1].addRow(i, cells[i].GetRange(9, 9).ToArray());
            }

            for(int i = 6; i < 9; i++)
            {
                if(cells[i].Count != 21)
                {
                    throw new Exception("6-9 satırların uzunluğu 21 olmalı");
                }
                sudokus[0].addRow(i, cells[i].GetRange(0, 9).ToArray());
                sudokus[1].addRow(i, cells[i].GetRange(12, 9).ToArray());
                sudokus[2].addRow(i - 6, cells[i].GetRange(6, 9).ToArray());
            }

            for (int i = 9; i < 12; i++) {
                if(cells[i].Count != 9)
                {
                    throw new Exception("9-12 satırların uzunluğu 9 olmalı");
                }
                sudokus[2].addRow(i - 6, cells[i].ToArray());
            }

            for (int i = 12; i < 15; i++) { 
                if(cells[i].Count != 21)
                {
                    throw new Exception("12-15 satırların uzunluğu 21 olmalı");
                }
                sudokus[3].addRow(i - 12, cells[i].GetRange(0, 9).ToArray());
                sudokus[4].addRow(i - 12, cells[i].GetRange(12, 9).ToArray());
                sudokus[2].addRow(i - 6, cells[i].GetRange(6, 9).ToArray());
            }

            for(int i = 15; i < 21; i++)
            {
                if(cells[i].Count != 18)
                {
                    throw new Exception("15-21 satırın uzunluğu 18 olmalı");
                }
                sudokus[3].addRow(i - 12, cells[i].GetRange(0, 9).ToArray());
                sudokus[4].addRow(i - 12, cells[i].GetRange(9, 9).ToArray());
            }
        }
    }
}
