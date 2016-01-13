using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathfindingAlgorithms.Cells;
using System.Diagnostics.Contracts;

namespace PathfindingAlgorithms.Algorithms.Astar
{
	public sealed class Astar : IPathFindingAlgorithm
    { 
		class CellData
		{
            public double scoreCalc = -1, scoreHeur = -1;
			public bool processed = false;
			public Coordinates? cameFrom = null; // may need to return nilCoord
		}

		private readonly IHeuristic heuristic;
        private readonly IAdjacement adjacement;

        ICell[,] grid;
        Dictionary<Coordinates, CellData> data;
        Coordinates startCoord, goalCoord;
        List<Coordinates> plannedNodes;

        public Astar(IHeuristic heuristic, IAdjacement adjacement)
        {
            if (heuristic == null)
                throw new ArgumentNullException("heuristic");
            if (adjacement == null)
                throw new ArgumentNullException("adjacement");

            this.heuristic = heuristic;
            this.adjacement = adjacement;
        }

        public IEnumerable<ICell> Process(ICell[,] cells, Coordinates from, Coordinates to)
        {
            try
            {
                Contract.Requires(cells != null);
                Contract.Requires(from.Inside(new Coordinates(cells.GetLength(0), cells.GetLength(1))));
                Contract.Requires(to.Inside(new Coordinates(cells.GetLength(0), cells.GetLength(1))));

                InitData(cells, from, to);

                plannedNodes.Add(startCoord);
                while (plannedNodes.Count > 0)
                {
                    //находим в запланированных вершину с наименьшей эвристической стоимостью
                    var cur = TakeMinimalHeuristic();

                    //если найденная - пункт назначения, то заканчиваем поиск и начинаем сборку пути
                    if (cur == goalCoord)
                        break;

                    CellData curData = data[cur];
                    curData.processed = true;

                    //рассматриваем все с ней смежные, необработанные ранее
                    var adjList = adjacement.Adjacement(grid, cur);
                    foreach (Coordinates adj in adjList.Where(c => !data[c].processed))
                    {
                        //определяем стоимость смежной вершины на основе уже пройденного пути
                        double score = curData.scoreCalc + Score(grid.At(adj));

                        //если вершина не была посещена, или в неё можно прийти более коротким путём - обновляем информацию
                        bool notFound = !plannedNodes.Contains(adj);
                        CellData adjData = data[adj];
                        if (notFound || score < adjData.scoreCalc)
                        {
                            adjData.cameFrom = cur;
                            adjData.scoreCalc = score;
                            adjData.scoreHeur = score + Heuristic(grid.At(adj));

                            //если вершина ещё не была посещена - запланируем посещение
                            if (notFound)
                                plannedNodes.Add(adj);
                        }
                    }
                }
                return AssemblePath();
            }
            catch (KeyNotFoundException)
            {
                throw new IndexOutOfRangeException();
            }
        }

        private void InitData(ICell[,] grid, Coordinates startCoord, Coordinates goalCoord)
		{
			this.startCoord = startCoord;
			this.goalCoord = goalCoord;
			this.grid = grid;

			int xsize = grid.GetLength(0), ysize = grid.GetLength(1);
            data = new Dictionary<Coordinates, CellData>(xsize * ysize);
            for (int x = 0; x < xsize; ++x)
                for (int y = 0; y < ysize; ++y)
                    data.Add(grid[x, y].Coordinates, new CellData());

			data[startCoord].scoreCalc = 0;
			data[startCoord].scoreHeur = Heuristic(grid.At(startCoord));
			plannedNodes = new List<Coordinates>();
		}

		/// <summary>
        /// Поиск вершины с минимальной запланированной эвристической оценкой
        /// </summary>
		private Coordinates TakeMinimalHeuristic()
		{
			Coordinates cur = plannedNodes[0];
			double minHeur = data[cur].scoreHeur;
            int minIndex = 0;
            for (int i = 1; i < plannedNodes.Count; ++i)
                if (data[plannedNodes[i]].scoreHeur < minHeur)
                {
                    cur = plannedNodes[i];
                    minHeur = data[cur].scoreHeur;
                    minIndex = i;
                }
            plannedNodes.RemoveAt(minIndex);
			return cur;
		}

		//сборка пути на основе собранных данных
		IEnumerable<ICell> AssemblePath()
		{
			var res = new LinkedList<ICell>();

			var curCoord = goalCoord;
			var curData = data[goalCoord];
			while (curData.cameFrom.HasValue)
			{
				res.AddFirst(grid.At(curCoord));
				curCoord = curData.cameFrom.Value;
				curData = data[curCoord];
			}

			if (startCoord == goalCoord || res.Count > 0 )
				res.AddFirst(grid.At(startCoord));

			return res;
		}

        /// <summary>
        /// Эвристическая оценка вершинв
        /// </summary>
        private double Heuristic(ICell c)
        {
            return heuristic.Heuristic(c.Coordinates, goalCoord);
        }

        /// <summary>
        /// Точная оценка вершины
        /// </summary>
        private double Score(ICell c)
        {
            return c.Weight;
        }
    }
}
