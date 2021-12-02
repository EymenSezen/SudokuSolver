using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class GraphViewModel
    {
        public GraphViewModel()
        {
            SudokuDBContext db = new SudokuDBContext();
            var cellSolutions = db.CellSolutions.ToList();
            var thread5Solutions = cellSolutions.Where(e => e.IsThread5).ToList();
            var thread10Solutions = cellSolutions.Where(e => !e.IsThread5).ToList();


            PlotModel pm = new PlotModel();
            pm.Title = "İlişki Grafiği (kırmızı: 10 thread, mavi: 5 thread)";
            this.Model = pm;
            MakeSeries(thread5Solutions, OxyColor.FromRgb(0, 0, 255));
            MakeSeries(thread10Solutions, OxyColor.FromRgb(255, 0, 0));
        }

        private void MakeSeries(IEnumerable<CellSolution> cellSolutions, OxyColor color)
        {
            var minTime = cellSolutions.Min(e => e.Date);
            var maxTime = (cellSolutions.Max(e => e.Date) - minTime).TotalSeconds + 1;

            var s1 = new LineSeries();
            s1.Color = color;
            for (int i = 0; i < maxTime; i++)
            {
                int y = cellSolutions.Count(e => (e.Date - minTime).TotalSeconds <= i);
                s1.Points.Add(new DataPoint(i, y));
            }
            Model.Series.Add(s1);
        }

        public PlotModel Model { get; private set; }
    }
}
