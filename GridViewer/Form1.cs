using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PathfindingAlgorithms.Algorithms;
using PathfindingAlgorithms.Cells;

namespace GridViewer
{
	public partial class Form1 : Form
	{
		int allwidth, allheight, cellW, cellH;
		int gridWcount = 8, gridHcount = 5;

		class Cell : UserControl, ICell
		{
			static Color colorMatter = Color.Green, colorVoid = Color.DarkGray;

			public bool Selected { get { return _selected; } set { _selected = value; Invalidate(); } }
			bool _selected = false;

			Label lbl;

			public Cell(Form1 form, int x, int y, double w)
				: base()
			{
				Coordinates = new Coordinates( x, y );
				Weight = w;

				double r = colorMatter.R + w * (colorVoid.R - colorMatter.R);
				double g = colorMatter.G + w * (colorVoid.G - colorMatter.G);
				double b = colorMatter.B + w * (colorVoid.B - colorMatter.B);

				this.BackColor = Color.FromArgb( (int)r, (int)g, (int)b );

				x = form.cellW * x;
				y = form.cellH * y;
				this.Location = new Point( x, y );
				this.Size = new Size( form.cellW, form.cellH );
				this.BorderStyle = BorderStyle.FixedSingle;

				lbl = new Label();
				lbl.Location = new Point( 10,10 );
				lbl.AutoSize = true;
				lbl.Text = String.Format( "{0}", Weight );
				this.Controls.Add( lbl );
				
				Paint += Cell_Paint;
				Click += Cell_Click;
			}

			public double Weight { get; set; }

			public Coordinates Coordinates { get; private set; }

			public override string ToString()
			{
				return String.Format( "[{0}, {1}]; {2}", Coordinates.X, Coordinates.Y, Weight );
			}

			void Cell_Paint(object _, PaintEventArgs e)
			{
				if ( Selected )
				{
					int thickness = 4;
					int halfThickness = thickness/2;
					using ( Pen p = new Pen( Color.Red, thickness ) )
					{
						e.Graphics.DrawRectangle( p, new Rectangle( halfThickness,
																  halfThickness,
																  ClientSize.Width-thickness,
																  ClientSize.Height-thickness ) );
					}
				}
			}
			void Cell_Click(object _, EventArgs __)
			{
				Selected = !Selected;
			}
		}

		class PathNode : UserControl
		{
			static int sizeX = 10, sizeY = 10;
			public PathNode(Form1 form, int x, int y)
				: base()
			{
				BackColor = Color.Red;
				Location = new Point( form.cellW*x + (form.cellW - sizeX)/2, form.cellH*y + (form.cellH - sizeY)/2 );
				this.Size = new Size( sizeX, sizeY );
			}
		}

		public Form1()
		{
			InitializeComponent();
		}

		void InitGridData()
		{
			allwidth = this.ClientRectangle.Width;
			allheight = this.ClientRectangle.Height;

			cellW = allwidth / gridWcount;
			cellH = allheight / gridHcount;
			
			cells = new ICell[gridWcount, gridHcount];
		}

		Coordinates start, goal;
		Cell startSelected = null;
		ICell[,] cells;
		Astar astar = new Astar();

		IEnumerable<ICell> pathCells = null;
		List<PathNode> path = new List<PathNode>();

		private void Form1_Load(object sender, EventArgs e)
		{
			InitGridData();

			Random r = new Random();
			for ( int x = 0; x < gridWcount; ++x )
			{
				for ( int y = 0; y < gridHcount; ++y )
				{
					int wInt = r.Next() % 6;
					var c = new Cell( this, x, y, wInt / 5.0 );
					c.Click +=
						(_, __) => {
							if ( startSelected == null)
							{
								start = c.Coordinates;
								startSelected = c;
								ClearPath();
							}
							else
							{
								goal = c.Coordinates;
								pathCells = astar.Process( cells, start, goal );
								DrawPath();

								c.Selected = false;
								startSelected.Selected = false;
								startSelected = null;
							}
						};
					cells[x,y] = c;
					this.Controls.Add( c );
				}
			}
		}
		void ClearPath()
		{
			pathCells = null;
			foreach ( var c in path )
			{
				this.Controls.Remove( c );
			}
			path.Clear();
		}
		void DrawPath()
		{
			foreach ( var c in pathCells )
			{
				var node = new PathNode( this, c.Coordinates.X, c.Coordinates.Y );
				path.Add( node );
				this.Controls.Add( node );
				node.BringToFront();
			}
			this.Invalidate();
			foreach ( var c in path )
			{
				c.Invalidate();
			}
		}
	}
}
