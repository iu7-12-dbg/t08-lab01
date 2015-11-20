using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathfindingAlgorithms.Cells;

namespace PathfindingAlgorithms.Algorithms
{
	public class Astar : IPathFindingAlgorithm
	{
		public static readonly Coordinates nilCoord = new Coordinates( -1, -1 );

		class CellData
		{
			public float scoreCalc = -1, scoreHeur = -1;
			public bool processed  = false;
			public Coordinates cameFrom = nilCoord;
		}

		Coordinates goalCoord;

		float Heuristic(ICell c)
		{
			return CalcDistance( c.Coordinates, goalCoord );
		}
		float Score(ICell from, ICell to)
		{
			/*if ( to.Weight > 1 )
				return 999999.0f;
			else*/
				return (float)to.Weight;
		}

		public IEnumerable<ICell> Process(ICell[,] grid, Coordinates startCoord, Coordinates goalCoord)
		{
			this.goalCoord = goalCoord;

            var data = new CellData[grid.GetLength(0), grid.GetLength(1)];
			data.At( startCoord ).scoreCalc = 0;
			data.At( startCoord ).scoreHeur = Heuristic( grid.At(startCoord) );

			var q = new LinkedList<Coordinates>();
			q.AddLast( startCoord );
			while ( q.Count > 0 )
			{
				//находим в запланированных вершину с наименьшей эвристической стоимостью
				Coordinates cur = q.First();
				float minHeur = data.At(cur).scoreHeur;
				foreach ( Coordinates e in q )
				{
					if ( data.At(e).scoreHeur < minHeur )
					{
						cur = e;
						minHeur = data.At( e ).scoreHeur;
					}
				}

				//если найденная - пункт назначения, то заканчиваем поиск и начинаем сборку пути
				if ( cur == goalCoord )
					break;

				q.Remove( cur );

				CellData curData = data.At( cur );
				curData.processed = true;

				//рассматриваем все с ней смежные, необработанные ранее
				var adjList = GetAdjacents( grid, cur );
				foreach ( Coordinates adj in adjList.Where( c => !data.At( c ).processed ) )
				{
					//определяем стоимость смежной вершины на основе уже пройденного пути
					float score = curData.scoreCalc + Score( grid.At(cur), grid.At(adj) );

					//если вершина не была посещена, или в неё можно прийти более коротким путём - обновляем информацию
					bool notFound = !q.Contains( adj );
					CellData adjData = data.At( adj );
					if ( notFound || score < adjData.scoreCalc )
					{
						adjData.cameFrom = cur;
						adjData.scoreCalc = score;
						adjData.scoreHeur = score + Heuristic( grid.At(adj) );

						//если вершина ещё не была посещена - запланируем посещение
						if ( notFound )
							q.AddLast( adj );
					}
				}
			}

			var res = new LinkedList<ICell>();
			Coordinates pos = goalCoord;
			while (pos != nilCoord)
			{
				res.AddFirst( grid[pos.X][pos.Y] );
				pos = data.At(pos).cameFrom;
			}
			return res;
		}

		//////////////////////////////////////////////////////////////////////////


		static List<Coordinates> GetAdjacent(ICell[,] grid, Coordinates at)
		{
			var res = new List<Coordinates>( 4 );
			if ( at.Y > 0 ) res.Add( grid[at.X][at.Y-1].Coordinates );
			if ( at.X > 0 ) res.Add( grid[at.X-1][at.Y].Coordinates );
			if ( at.Y < grid[0].Count-1 ) res.Add( grid[at.X][at.Y+1].Coordinates );
			if ( at.X < grid.Count-1 ) res.Add( grid[at.X+1][at.Y].Coordinates );
			return res;
		}

		static float CalcDistance(Coordinates from, Coordinates to)
		{
			return (float)Math.Sqrt( Math.Pow( from.X - to.X, 2 )
				+ Math.Pow( from.Y - to.Y, 2 ) );
		}
    }
}
