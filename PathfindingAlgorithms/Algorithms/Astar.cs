using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathfindingAlgorithms.Cells;

namespace PathfindingAlgorithms.Algorithms
{
	using UGrid = IList<IList<ICell>>;
	class Grid
	{
		public List<ICell> cells;
		public int m,n,len;
		public Grid(UGrid cellsGrid)
		{
			m = cellsGrid.Count;
			n = cellsGrid[0].Count;
			len = m*n;
			cells = new List<ICell>( len );

			for ( int i = 0; i < m; ++i )
				for ( int j = 0; j < n; ++j )
					cells.Add( cellsGrid[i][j] );
		}
		public ICell GetCell(int x, int y)
		{
			if ( x < 0 || x >= n || y < 0 || y >= m )
				return null;
			return cells[GetIndex(x,y)];
		}
		public ICell GetCell(Coordinates c)
		{
			return GetCell( c.X, c.Y );
		}

		public int GetIndex(int x, int y)
		{
			if ( x < 0 || x >= n || y < 0 || y >= m )
				return -1;
			return x + n*y;
		}
		public int GetIndex(Coordinates c)
		{
			return GetIndex( c.X, c.Y );
		}

		public Coordinates GetCoordinates(int index)
		{
			if ( index < 0 || index >= len )
				return new Coordinates(-1,-1);

			Coordinates c = new Coordinates( index / n, index % n );
			return c;
		}

		public List<ICell> GetAdjacents(int x, int y)
		{
			var res = new List<ICell>( 4 );
			ICell c;
			if ( (c = GetCell( x-1, y )) != null ) res.Add( c );
			if ( (c = GetCell( x+1, y )) != null ) res.Add( c );
			if ( (c = GetCell( x, y-1 )) != null ) res.Add( c );
			if ( (c = GetCell( x, y+1 )) != null ) res.Add( c );
			return res;
		}
		public List<ICell> GetAdjacents(int index)
		{
			Coordinates c = GetCoordinates( index );
			return GetAdjacents( c.X, c.Y );
		}
	}

	class Astar : IPathFindingAlgorithm
	{
		List<float> scoreCalc;	//рассчитанная стоимость до каждой вершины от старта
		List<float> scoreHeur;	//эвристическая оценка стоимости до каждой вершины от конца

		List<bool> processed;	//обработана ли вершина
		List<int> cameFrom;		//из какой вершины пришли в данную

		Grid grid;

		ICell start, goal;
		int start_index, goal_index;

		void Init(UGrid cellsGrid, Coordinates startCoord, Coordinates goalCoord)
		{
			int len = grid.m * grid.n;

			scoreCalc = FillList<float>( len, -1 );
			scoreHeur = FillList<float>( len, -1 );
			processed = FillList<bool>( len, false );
			cameFrom = FillList<int>( len, -1 );

			start = grid.GetCell(startCoord);
			start_index = grid.GetIndex( startCoord );
			goal = grid.GetCell( goalCoord );
			goal_index = grid.GetIndex( goalCoord );
		}

		float Heuristic(ICell c)
		{
			return CalcDistance( c.Coordinates, goal.Coordinates );
		}

		public IEnumerable<ICell> Process(UGrid cellsGrid, Coordinates startCoord, Coordinates goalCoord)
		{
			Init( cellsGrid, startCoord, goalCoord );

			scoreCalc[start_index] = 0;
			scoreHeur[goal_index] = Heuristic( start );

			var q = new LinkedList<int>();
			q.AddLast( start_index );

			while ( q.Count > 0 )
			{
				//находим вершину с наименьшей эвристической стоимостью
				int cur = q.First();
				float minHeur = scoreHeur[cur];
				foreach ( var e in q )
				{
					if ( scoreHeur[e] < minHeur )
					{
						cur = e;
						minHeur = scoreHeur[e];
					}
				}

				//если найденная - пункт назначения, то заканчиваем поиск и начинаем сборку пути
				if ( cur == goal_index )
					break;

				q.Remove( cur );
				processed[cur] = true;

				//рассматриваем все с ней смежные, необработанные ранее
				var adjList = grid.GetAdjacents( cur );
				foreach (var adj in adjList)
				{
					int adjIndex = grid.GetIndex( adj.Coordinates );
					if ( processed[adjIndex] )
						continue;

					//определяем стоимость смежной вершины на основе уже пройденного пути
					float score = scoreCalc[cur] + CalcDistance( grid.cells[cur].Coordinates, adj.Coordinates );

					//если вершина не была посещена, или в неё можно прийти более коротким путём - обновляем информацию
					bool notFound = !q.Contains( adjIndex );
					if ( notFound || score < scoreCalc[adjIndex] )
					{
						cameFrom[adjIndex] = cur;
						scoreCalc[adjIndex] = score;
						scoreHeur[adjIndex] = score + Heuristic(adj);

						//если вершина ещё не была посещена - запланируем посещение
						if ( notFound )
							q.AddLast( adjIndex );
					}
				}
			}

			var res = new LinkedList<ICell>();
			int pos = goal_index;
			while ( pos != -1 )
			{
				res.AddFirst( grid.cells[pos] );
				pos = cameFrom[pos];
			}
			return res;
		}

		//////////////////////////////////////////////////////////////////////////

		static List<T> FillList<T>(int count, T val)
		{
			var l = new List<T>( count );
			for ( int i = 0; i < count; ++i )
				l[i] = val;
			return l;
		}

		static float CalcDistance(Coordinates from, Coordinates to)
		{
			return (float)Math.Sqrt( Math.Pow( from.X - to.X, 2 )
				+ Math.Pow( from.Y - to.Y, 2 ) );
		}
	}
}
