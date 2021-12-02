using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuDBContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=sudoku;Trusted_Connection=true");

        }
        public DbSet<CellSolution> CellSolutions { get; set; }
        //public void Add(List<CellSolution> cellSolutions)
        //{
        //    foreach(var cellSolution in cellSolutions)
        //    {
        //        var addedEntity = this.Entry(cellSolution);
        //        addedEntity.State = EntityState.Added;
        //        this.SaveChanges();
        //    }
        //}

    }
}
