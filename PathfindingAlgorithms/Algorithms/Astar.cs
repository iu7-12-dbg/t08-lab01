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

		ICell[,] grid;
		CellData[,] data;
		Coordinates startCoord, goalCoord;
		LinkedList<Coordinates> plannedNodes;

		//эвристическая оценка вершины
		float Heuristic(ICell c)
		{
			return EuclidianDistance( c.Coordinates, goalCoord );
		}

		//точная оценка вершины
		float Score(ICell c)
		{
				return (float)c.Weight;
		}

		void InitData(ICell[,] grid, Coordinates startCoord, Coordinates goalCoord)
		{
			this.startCoord = startCoord;
			this.goalCoord = goalCoord;
			this.grid = grid;

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

		//сборка пути на основе собранных данных
		IEnumerable<ICell> AssemblePath()
		{
			var res = new LinkedList<ICell>();

			var curCoord = goalCoord;
			var curData = data.At(goalCoord);
			while ( curData.cameFrom != nilCoord )
			{
				res.AddFirst( grid.At( curCoord ) );
				curCoord = curData.cameFrom;
				curData = data.At( curCoord );
			}

			if ( startCoord == goalCoord || res.Count > 0 )
				res.AddFirst( grid.At( startCoord ) );

			return res;
		}


		public IEnumerable<ICell> Process(ICell[,] cells, Coordinates from, Coordinates to)
		{
			InitData( cells, from, to );

			plannedNodes.AddLast( startCoord );
			while ( plannedNodes.Count > 0 )
			{
				//находим в запланированных вершину с наименьшей эвристической стоимостью
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

			return AssemblePath();
		}

		//////////////////////////////////////////////////////////////////////////


		static Coordinates? GetPassableNode(ICell[,] grid, int x, int y)
		{
			int xsize = grid.GetLength( 0 ), ysize = grid.GetLength( 1 );
			if ( x >= 0 && x < xsize && y >= 0 && y < ysize && grid[x,y].Weight >= 0 )
				return grid[x,y].Coordinates;
			return null;
		}
		static List<Coordinates> GetAdjacent(ICell[,] grid, Coordinates at)
		{
			var res = new List<Coordinates>( 4 );

			Coordinates? c;
			if ( (c = GetPassableNode( grid, at.X, at.Y-1 )).HasValue ) res.Add( c.Value );
			if ( (c = GetPassableNode( grid, at.X-1, at.Y )).HasValue ) res.Add( c.Value );
			if ( (c = GetPassableNode( grid, at.X, at.Y+1 )).HasValue ) res.Add( c.Value );
			if ( (c = GetPassableNode( grid, at.X+1, at.Y )).HasValue ) res.Add( c.Value );

			return res;
		}

		static float EuclidianDistance(Coordinates from, Coordinates to)
		{
			return (float)Math.Sqrt( Math.Pow( from.X - to.X, 2 )
				+ Math.Pow( from.Y - to.Y, 2 ) );
		}
	}
}
