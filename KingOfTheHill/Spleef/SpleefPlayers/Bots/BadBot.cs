using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using KingOfTheHill.Players;

namespace KingOfTheHill.Spleef.SpleefPlayers
{
    public class Polygon
    {
        public Polygon(List<Point> points)
        {
            _points = points.Select(p => new PointF(p.X, p.Y)).ToList();
        }

        private readonly List<PointF> _points;

        public PointF FindCentroid()
        {
            int numberOfPoints = _points.Count;
            var pts = new PointF[numberOfPoints + 1];
            _points.CopyTo(pts, 0);
            pts[numberOfPoints] = _points[0];

            double x = 0;
            double y = 0;
            for (var i = 0; i < numberOfPoints; i++)
            {
                double secondFactor = pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y;
                x += (pts[i].X + pts[i + 1].X) * secondFactor;
                y += (pts[i].Y + pts[i + 1].Y) * secondFactor;
            }

            double polygonArea = Math.Max(PolygonArea(), 1);
            x /= (6 * polygonArea);
            y /= (6 * polygonArea);

            if (!(x < 0))
            {
                return new PointF((float) x, (float) y);
            }

            x = -x;
            y = -y;

            return new PointF((float) x, (float) y);
        }

        private double PolygonArea()
        {
            return Math.Abs(SignedPolygonArea());
        }

        private double SignedPolygonArea()
        {
            int numberOfPoints = _points.Count;
            var pts = new PointF[numberOfPoints + 1];
            _points.CopyTo(pts, 0);
            pts[numberOfPoints] = _points[0];

            double area = 0;
            for (var i = 0; i < numberOfPoints; i++)
            {
                area += (pts[i + 1].X - pts[i].X) * (pts[i + 1].Y + pts[i].Y) / 2;
            }

            return area;
        }
    }

    public class BadBot : SpleefPlayer
    {
        private double _centerOfMassSolidWeight = 1.2;
        private double _centerOfMassEnemiesWeight = 1;

        public override void StartAll(List<SpleefPlayerInfo> allPlayers)
        {
            // Do nothing for now.
        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {
        }

        public override SpleefDecision PlayTurn(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board)
        {
            var bestHidingPosition = GetBestPointToHide(allPlayers, board);
            var currentPosition = GetMyLocation();

            var alivePlayers = GetAlivePlayers(allPlayers, board);
            var aliveEnemies = alivePlayers.Where(x => x.ID != this.GetInfo().ID);

            if (aliveEnemies.Any(x => FindShortestPathAStar(board, x.CurrentLocation, bestHidingPosition) != null))
            {
                return new SpleefDecision(SpleefAction.Move,
                    new Point(bestHidingPosition.X - currentPosition.X, bestHidingPosition.Y - currentPosition.Y));
            }

            if (board[currentPosition.X, currentPosition.Y].HealthRemaining >= 2)
            {
                return new SpleefDecision(SpleefAction.Wait, Point.Empty);
            }

            return new SpleefDecision(SpleefAction.Move,
                new Point(bestHidingPosition.X - currentPosition.X, bestHidingPosition.Y - currentPosition.Y));
        }

        private Point GetBestPointToHide(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board)
        {
            var allValidMoves = GetValidMoves(allPlayers, board, this.GetInfo() as SpleefPlayerInfo);

            var playersPositions = GetAlivePlayers(allPlayers, board)
                .Where(x => x.ID != this.GetInfo().ID)
                .Select(p => p.CurrentLocation);
            var list = playersPositions.ToList();
            var enemyCentroid = new Polygon(list).FindCentroid();
            var enemyCentroidPoint = new Point((int) enemyCentroid.X, (int) enemyCentroid.Y);

            var solidSquares = GetSolidSquares(allPlayers, board);
            var solidSquaresCentroid = new Polygon(solidSquares.ToList()).FindCentroid();
            var solidSquaresCentroidPoint = new Point((int) solidSquaresCentroid.X, (int) solidSquaresCentroid.Y);

            Point move = Point.Empty;

            var bestMovesByDescending = allValidMoves.Select(x => new
                {
                    Move = x,
                    Value = (GetDistance(solidSquaresCentroidPoint, x) * _centerOfMassSolidWeight) -
                            (GetDistance(enemyCentroidPoint, x) * _centerOfMassEnemiesWeight) + 100
                })
                .ToList();

            if (bestMovesByDescending.Any())
            {
                move = bestMovesByDescending.First().Move;
            }

            return move;
        }

        public IEnumerable<Point> GetSolidSquares(List<SpleefPlayerInfo> players, SpleefBoard.SpleefBoard board)
        {
            for (var i = 0; i < board.Width; i++)
            {
                for (var j = 0; j < board.Height; j++)
                {
                    if (board[i, j].IsSolid && FindShortestPathAStar(board, GetMyLocation(), new Point(i, j)) != null)
                    {
                        yield return new Point(i, j);
                    }
                }
            }
        }

        public IEnumerable<Point> GetValidMoves(List<SpleefPlayerInfo> players, SpleefBoard.SpleefBoard board,
            SpleefPlayerInfo currentPlayer)
        {
            var alivePlayers = GetAlivePlayers(players, board);
            var otherAlivePlayers = alivePlayers.Where(x => x.ID != currentPlayer.ID).ToList();

            foreach (var point in GetAllMoves(board, currentPlayer.CurrentLocation))
            {
                if (board[point.X, point.Y].IsSolid && otherAlivePlayers.All(x => x.CurrentLocation != point))
                    yield return point;
            }
        }

        public IEnumerable<Point> GetAllMoves(SpleefBoard.SpleefBoard board, Point currentPosition)
        {
            var baseValidMoves = new List<Point>()
            {
                new Point(currentPosition.X - 1, currentPosition.Y - 1),
                new Point(currentPosition.X, currentPosition.Y - 1),
                new Point(currentPosition.X + 1, currentPosition.Y - 1),
                new Point(currentPosition.X - 1, currentPosition.Y),
                new Point(currentPosition.X + 1, currentPosition.Y),
                new Point(currentPosition.X - 1, currentPosition.Y + 1),
                new Point(currentPosition.X, currentPosition.Y + 1),
                new Point(currentPosition.X + 1, currentPosition.Y + 1),
            };

            var validMoves = baseValidMoves.Where(m => m.X >= 0 && m.Y >= 0 && m.X < board.Width && m.Y < board.Height);

            return validMoves;
        }
    }
}
