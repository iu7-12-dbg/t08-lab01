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

		CellData[,] data;
		Coordinates goalCoord;
		LinkedList<Coordinates> plannedNodes;

		//эвристическая оценка вершины
		float Heuristic(ICell c)
		{
			return EuclidianDistance( c.Coordinates, goalCoord );
		}

		//точная оценка вершины
		float Score(ICell c)
		{
			/*if ( to.Weight > 1 )
				return 999999.0f;
			else*/
				return (float)c.Weight;
		}

		void InitData(ICell[,] grid, Coordinates startCoord, Coordinates goalCoord)
		{
			this.goalCoord = goalCoord;

			int xsize = grid.GetLength( 0 ), ysize = grid.GetLength( 1 );
			data = new CellData[xsize, ysize];
			for ( int x = 0; x < xsize; ++x )
				for ( int y = 0; y < ysize; ++y )
					data[x, y] = new CellData();

			data.At( startCoord ).scoreCalc = 0;
			data.At( startCoord ).scoreHeur = Heuristic( grid.At( startCoord ) );
		
			plannedNodes = new LinkedList<Coordinates>();
		}

		//поиск в очереди запланированных вершины с наименьшей эвристической оценкой
		Coordinates TakeMinimalHeuristic()
		{
			//находим в запланированных вершину с наименьшей эвристической стоимостью
			Coordinates cur = plannedNodes.First.Value;
			float minHeur = data.At( cur ).scoreHeur;
			foreach ( Coordinates e in plannedNodes )
			{
				if ( data.At( e ).scoreHeur < minHeur )
				{
					cur = e;
					minHeur = data.At( e ).scoreHeur;
				}
			}
			plannedNodes.Remove( cur );
			return cur;
		}


		public IEnumerable<ICell> Process(ICell[,] grid, Coordinates startCoord, Coordinates goalCoord)
		{
			InitData( grid, startCoord, goalCoord );

			plannedNodes.AddLast( startCoord );
			while ( plannedNodes.Count > 0 )
			{
				var cur = TakeMinimalHeuristic();

				//если найденная - пункт назначения, то заканчиваем поиск и начинаем сборку пути
				if ( cur == goalCoord )
					break;


				CellData curData = data.At( cur );
				curData.processed = true;

				//рассматриваем все с ней смежные, необработанные ранее
				var adjList = GetAdjacent( grid, cur );
				foreach ( Coordinates adj in adjList.Where( c => !data.At( c ).processed ) )
				{
					//определяем стоимость смежной вершины на основе уже пройденного пути
					float score = curData.scoreCalc + Score( grid.At(adj) );

					//если вершина не была посещена, или в неё можно прийти более коротким путём - обновляем информацию
					bool notFound = !plannedNodes.Contains( adj );
					CellData adjData = data.At( adj );
					if ( notFound || score < adjData.scoreCalc )
					{
						adjData.cameFrom = cur;
						adjData.scoreCalc = score;
						adjData.scoreHeur = score + Heuristic( grid.At(adj) );

						//если вершина ещё не была посещена - запланируем посещение
						if ( notFound )
							plannedNodes.AddLast( adj );
					}
				}
			}

			var res = new LinkedList<ICell>();
			Coordinates pos = goalCoord;
			while (pos != nilCoord)
			{
				res.AddFirst( grid.At(pos) );
				pos = data.At(pos).cameFrom;
			}
			return res;
		}

		//////////////////////////////////////////////////////////////////////////


		static List<Coordinates> GetAdjacent(ICell[,] grid, Coordinates at)
		{
			var res = new List<Coordinates>( 4 );

			int xsize = grid.GetLength( 0 ), ysize = grid.GetLength( 1 );
			if ( at.Y > 0 ) res.Add( grid[at.X,   at.Y-1].Coordinates );
			if ( at.X > 0 ) res.Add( grid[at.X-1, at.Y].Coordinates );
			if ( at.Y < ysize-1 ) res.Add( grid[at.X,   at.Y+1].Coordinates );
			if ( at.X < xsize-1 ) res.Add( grid[at.X+1, at.Y].Coordinates );

			return res;
		}

		static float EuclidianDistance(Coordinates from, Coordinates to)
		{
			return (float)Math.Sqrt( Math.Pow( from.X - to.X, 2 )
				+ Math.Pow( from.Y - to.Y, 2 ) );
		}
	}
}
