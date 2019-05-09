using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using KingOfTheHill.Board;
using KingOfTheHill.Players;

namespace KingOfTheHill.Spleef.SpleefPlayers
{
    public abstract class SpleefPlayer : Player
    {
        public Stopwatch swPlays = new Stopwatch();
        public int nbPlays = 0;

        protected Random RandomGen = new Random();

        public SpleefPlayer()
        {
            Info = new SpleefPlayerInfo();
        }

        public SpleefPlayer(string name, int id)
        {
            Info = new SpleefPlayerInfo(name, id);
        }

        public void Paint(Graphics gfx, GridBoard board, Rectangle bounds)
        {
            var stepX = bounds.Width / board.Width;
            var stepY = bounds.Height / board.Height;

            var ellipse = new Rectangle(((SpleefPlayerInfo)Info).CurrentLocation.X * stepX, ((SpleefPlayerInfo)Info).CurrentLocation.Y * stepY, stepX, stepY);

            gfx.FillEllipse(new SolidBrush(((SpleefPlayerInfo)Info).PlayerColor), ellipse);
            gfx.DrawEllipse(Pens.Black, ellipse);

            var words = Info.Name.Split(' ');
            var stringToDraw = string.Join("\r\n", words);

            var longestWordLength = words.OrderByDescending(x => x.Length).First().Length;

            if (longestWordLength > 7)
            {
                longestWordLength--;
            }
            if (longestWordLength > 9)
            {
                longestWordLength--;
            }

            var drawFont = new Font("Arial", stepX / longestWordLength, FontStyle.Bold);
            var drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            gfx.DrawString(stringToDraw, drawFont, Brushes.Black, ellipse, drawFormat);
        }

        public override PlayerInfo GetInfo()
        {
            return (SpleefPlayerInfo)Info;
        }

        public abstract void StartAll(List<SpleefPlayerInfo> allPlayers);

        public abstract void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores);

        public abstract SpleefDecision PlayTurn(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board);

        public List<SpleefPlayerInfo> GetAlivePlayers(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board)
        {
            return allPlayers.Where(x => board[x.CurrentLocation.X, x.CurrentLocation.Y].IsSolid).ToList();
        }

        #region Useful Methods

        /// <summary>
        /// Gets the Manhattan Distance between two points
        /// </summary>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <returns>The distance as a number of squares</returns>
        protected int GetDistance(Point point1, Point point2)
        {
            return GetDistance(point1.X, point1.Y, point2.X, point2.Y);
        }

        /// <summary>
        /// Gets the Manhattan Distance between two points
        /// </summary>
        /// <param name="x1">The X of the first point</param>
        /// <param name="y1">The Y of the first point</param>
        /// <param name="x2">The X of the second point</param>
        /// <param name="y2">The Y of the second point</param>
        /// <returns>The distance as a number of squares</returns>
        protected int GetDistance(int x1, int y1, int x2, int y2)
        {
            return (Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
        }

        /// <summary>
        /// Gets the location of the bot calling it
        /// </summary>
        /// <returns>The location as a point</returns>
        protected Point GetMyLocation()
        {
            return GetPlayerLocation(Info);
        }

        /// <summary>
        /// Gets the location of a player
        /// </summary>
        /// <param name="info">The info to identify the player</param>
        /// <returns>The location as a point</returns>
        protected Point GetPlayerLocation(PlayerInfo info)
        {
            return ((SpleefPlayerInfo) info).CurrentLocation;
        }

        /// <summary>
        /// Gets the square targeted by a bot if they are in a specific location and target a specific offset
        /// </summary>
        /// <param name="myLocation">The origin location</param>
        /// <param name="movement">The offset</param>
        /// <returns>The destination location</returns>
        protected Point GetTargetSquare(Point myLocation, Point movement)
        {
            return GetTargetSquare(myLocation, movement.X, movement.Y);
        }

        /// <summary>
        /// Gets the square targeted by a bot if they are in a specific location and target a specific offset
        /// </summary>
        /// <param name="myLocation">The origin location</param>
        /// <param name="movement">The offset</param>
        /// <returns>The destination location</returns>
        protected Point GetTargetSquare(Point myLocation, int x, int y)
        {
            return new Point(myLocation.X + x, myLocation.Y + y);
        }

        /// <summary>
        /// Tests if a bot is still alive on a specific board
        /// </summary>
        /// <param name="info">The bot to check</param>
        /// <param name="board">The board</param>
        /// <returns>true if it is alive</returns>
        protected bool IsAlive(PlayerInfo info, SpleefBoard.SpleefBoard board)
        {
            return IsAlive(GetPlayerLocation(info), board);
        }
        
        /// <summary>
        /// Tests if a bot on a given square is still alive on a specific board
        /// </summary>
        /// <param name="info">The square to check</param>
        /// <param name="board">The board</param>
        /// <returns>true if a bot that would be on that square would be alive</returns>
        protected bool IsAlive(Point location, SpleefBoard.SpleefBoard board)
        {
            return board[location.X, location.Y].IsSolid;
        }

        /// <summary>
        /// Gets the list of all alive players from a list of players and a board they are on
        /// </summary>
        /// <param name="players">The players to check</param>
        /// <param name="board">The board they are on</param>
        /// <returns>The list of the players who are actually alive</returns>
        protected List<SpleefPlayerInfo> GetAlivePlayers(List<PlayerInfo> players, SpleefBoard.SpleefBoard board)
        {
            return players.Where(x => IsAlive(x, board)).OfType<SpleefPlayerInfo>().ToList();
        }

        /// <summary>
        /// Helper function for A* pathfinding. No use in calling it by itself
        /// </summary>
        /// <param name="cameFrom"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        private List<Point> RebuildPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            var totalPath = new List<Point>();
            totalPath.Add(current);
	
            var currentInCameFrom = false;
	
            foreach (var c in cameFrom) {
                if (c.Key == current) {
                    currentInCameFrom = true;
                }
            }
	
            while(currentInCameFrom)
            {
                current = cameFrom[current];
                totalPath.Add(current);
                currentInCameFrom = false;
                foreach (var c in cameFrom)
                if (c.Key == current)
                    currentInCameFrom = true;
            }

            totalPath.Reverse();
            return totalPath;
        }

        /// <summary>
        /// Finds the shortest path containing no holes to go from one point to another
        /// </summary>
        /// <param name="board">The Spleef board, including holes</param>
        /// <param name="startPoint">The starting point</param>
        /// <param name="goal">The point to reach</param>
        /// <returns>The List of point from the start to the goal that the shortest path takes. Includes the start point and the goal.</returns>
        protected List<Point> FindShortestPathAStar(SpleefBoard.SpleefBoard board, Point startPoint, Point goal)
        {
	
            // The set of nodes already evaluated
            var closedSet = new List<Point>();

            // The set of currently discovered nodes that are not evaluated yet.
            // Initially, only the start node is known.
            var openSet = new List<Point>() {startPoint};

            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            var cameFrom = new Dictionary<Point, Point>();

            // For each node, the cost of getting from the start node to that node.
            var gScore = new Dictionary<Point, int>();

            // The cost of going from start to start is zero.
            gScore.Add(startPoint, 0);

            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            var fScore = new Dictionary<Point, int>();

            // For the first node, that value is completely heuristic.
            fScore.Add(startPoint, GetDistance(startPoint, goal));

            while (openSet.Any()) {		
                var current = Point.Empty;
		        var lowestfScore = int.MaxValue;
		        foreach (var os in openSet) {
			        if (fScore.ContainsKey(os)) {
				        if (fScore[os] < lowestfScore) {
					        lowestfScore = fScore[os];
					        current = os;
				        }
			        }
		        }

                if (current == goal)
                    return RebuildPath(cameFrom, current);

                openSet.RemoveAt(openSet.IndexOf(current));
                closedSet.Add(current);
		        
		        var neighbors = new List<Point>();
		        for (var i=-1; i < 2; i++) {
			        for (var j=-1; j < 2; j++) {
				        var neighX = current.X+i;
				        var neighY = current.Y+j;
				        if ((i != 0 || j != 0) && (neighX > -1 && neighY > -1 && neighX < board.Width && neighY < board.Height)) {
					        if (board[neighX, neighY].IsSolid) {
						        neighbors.Add(new Point(neighX, neighY));
					        }
				        }
			        }
		        }
		        
                foreach (var n in neighbors) {
                    if (!closedSet.Contains(n)) {
				        // The distance from start to a neighbor
				        var tentativeGScore = gScore[current] + 1;

				        if (!openSet.Contains(n))	// Discover a new node
					        openSet.Add(n);

				        if (!(gScore.ContainsKey(n)) || tentativeGScore < gScore[n]) {
					        // This path is the best until now. Record it!
					        cameFrom[n] = current;
					        gScore[n] = tentativeGScore;
					        var ds = GetDistance(n, goal);
					        fScore[n] = gScore[n] + ds;
				        }
			        }
		        }
	        }
	        return null;
        }

        #endregion Useful Methods
    }
}
