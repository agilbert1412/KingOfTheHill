using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using KingOfTheHill.Spleef.SpleefBoard;
using KingOfTheHill.Spleef.SpleefPlayers;

namespace KingOfTheHill.Spleef
{
    public class SpleefGame : Game
    {
        private static Random rand = new Random();

        public SpleefBoard.SpleefBoard Board { get; set; }
        public List<SpleefPlayer> Players { get; set; }
        
        private bool _isStarted = false;

        public SpleefGame(List<SpleefPlayer> players)
        {
            Board = new SpleefBoard.SpleefBoard(players.Count * 2, players.Count * 2);
            Players = players;
        }

        private void ShufflePlayers()
        {
            var oldLst = new List<SpleefPlayer>(Players);
            var newLst = new List<SpleefPlayer>();

            while (oldLst.Any())
            {
                var chosen = rand.Next(0, oldLst.Count);
                newLst.Add(oldLst[chosen]);
                oldLst.RemoveAt(chosen);
            }

            Players = newLst;
        }

        public List<SpleefPlayer> GetAlivePlayers()
        {
            return Players.Where(IsAlive).ToList();
        }

        private bool IsAlive(Player p)
        {
            return IsAlive(p.GetInfo());
        }

        private bool IsAlive(PlayerInfo p)
        {
            return IsAlive((SpleefPlayerInfo)p);
        }

        private bool IsAlive(SpleefPlayerInfo p)
        {
            return Board[p.CurrentLocation.X, p.CurrentLocation.Y].IsSolid;
        }

        public override void Paint(Graphics gfx, Rectangle bounds)
        {
            /*var startRow = 0;
            var startColumn = 0;
            var endRow = Board.Width - 1;
            var endColumn = Board.Height - 1;

            while (!RowHasInterestingSquares(startRow))
            {
                startRow++;
            }
            while (!RowHasInterestingSquares(endRow))
            {
                endRow--;
            }
            while (!ColumnHasInterestingSquares(startColumn))
            {
                startColumn++;
            }
            while (!ColumnHasInterestingSquares(endColumn))
            {
                endColumn--;
            }

            var expectedWidth = Board.Width;
            var expectedHeight = Board.Height;

            var realWidth = endColumn - startColumn + 1;
            var realHeight = endRow - startRow + 1;

            var sizeDiffX = expectedWidth / (float)realWidth;
            var sizeDiffY = expectedHeight / (float)realHeight;

            var sizeX = bounds.Width / (float)realWidth;
            var sizeY = bounds.Height / (float)realHeight;

            bounds.X -= (int)Math.Round(sizeX * startColumn);
            bounds.Y -= (int)Math.Round(sizeY * startRow);

            bounds.Width = (int)Math.Round(bounds.Width * sizeDiffX);
            bounds.Height = (int)Math.Round(bounds.Height * sizeDiffY);*/

            Board.Paint(gfx, bounds, !GetAlivePlayers().Any());

            foreach (var p in Players)
            {
                p.Paint(gfx, Board, bounds);
            }
        }

        public bool RowHasInterestingSquares(int rowIndex)
        {
            for (var i = 0; i < Board.Width; i++)
            {
                if (IsInterestingSquare(Board[i, rowIndex]))
                    return true;
            }

            return false;
        }

        public bool ColumnHasInterestingSquares(int columnIndex)
        {
            for (var i = 0; i < Board.Height; i++)
            {
                if (IsInterestingSquare(Board[columnIndex, i]))
                    return true;
            }

            return false;
        }

        private bool IsInterestingSquare(SpleefGridSquare spleefSquare)
        {
            return spleefSquare.IsSolid;
        }

        /// <summary>
        /// Plays the next step of the game. Returns true if the game is over after this step
        /// </summary>
        /// <returns>True if the game is over, false if it can go on</returns>
        public override bool PlayStep(Dictionary<PlayerInfo, int> playersScores)
        {
            if (!_isStarted)
            {
                StartGame(playersScores);
            }

            var infos = Players.Select(x => GetSpleefInfo(x).Clone()).ToList();

            var playersAlive = GetAlivePlayers();

            var decisions = new Dictionary<SpleefPlayer, SpleefDecision>();

            foreach (var p in playersAlive)
            {
                p.swPlays.Start();
                SpleefDecision decision;
                try
                {
                    decision = p.PlayTurn(infos, Board.Clone());
                }
                catch (Exception ex)
                {
                    decision = SpleefDecision.DefaultDecision;
                }

                p.nbPlays++;
                p.swPlays.Stop();

                if (!decision.IsValid)
                {
                    decision = SpleefDecision.DefaultDecision;
                }

                decisions.Add(p, decision);
            }

            foreach (var action in decisions)
            {
                var decision = action.Value;
                var thisPlayerInfo = GetSpleefInfo(action.Key);

                if (decision.IsValid)
                {
                    var target = decision.Target;
                    switch (decision.Action)
                    {
                        case SpleefAction.Move:
                            if (target.X != 0 || target.Y != 0)
                            {
                                var destination = new Point(thisPlayerInfo.CurrentLocation.X + target.X, thisPlayerInfo.CurrentLocation.Y + target.Y);
                                if (destination.X >= 0 && destination.Y >= 0 && destination.X < Board.Width &&
                                    destination.Y < Board.Height)
                                {
                                    ((SpleefGridSquare) (Board.Squares[thisPlayerInfo.CurrentLocation.X, thisPlayerInfo.CurrentLocation.Y])).Destroy();
                                    thisPlayerInfo.CurrentLocation = destination;
                                }
                            }
                            break;
                        case SpleefAction.Hole:
                            var targettedSquare = new Point(thisPlayerInfo.CurrentLocation.X + target.X, thisPlayerInfo.CurrentLocation.Y + target.Y);
                            if (targettedSquare.X >= 0 && targettedSquare.Y >= 0 && targettedSquare.X < Board.Width &&
                                targettedSquare.Y < Board.Height)
                            {
                                ((SpleefGridSquare)(Board.Squares[targettedSquare.X, targettedSquare.Y])).Destroy();
                            }
                            break;
                        case SpleefAction.Wait:
                            break;
                    }
                }
                
                ((SpleefGridSquare)(Board.Squares[thisPlayerInfo.CurrentLocation.X, thisPlayerInfo.CurrentLocation.Y])).Damage();
            }

            return GetAlivePlayers().Count <= 1;
        }

        private SpleefPlayerInfo GetSpleefInfo(Player p)
        {
            return p.GetInfo() as SpleefPlayerInfo;
        }

        private void StartGame(Dictionary<PlayerInfo, int> playersAndScores)
        {
            foreach (var player in Players)
            {
                var thisPlayerInfo = GetSpleefInfo(player);
                thisPlayerInfo.CurrentLocation = new Point(rand.Next(0, Board.Width), rand.Next(0, Board.Height));
                while (Players.Any(x => x != player && GetSpleefInfo(x).CurrentLocation == thisPlayerInfo.CurrentLocation))
                {
                    thisPlayerInfo.CurrentLocation = new Point(rand.Next(0, Board.Width), rand.Next(0, Board.Height));
                }
                
                player.StartGame(playersAndScores);
            }

            ShufflePlayers();
            _isStarted = true;
        }

        private Color Lighten(Color color, int percent)
        {
            return Color.FromArgb(
                color.A,
                (int)Math.Min(color.R/* * (1 + (percent / 100f))*/ + 40, 255),
                (int)Math.Min(color.G/* * (1 + (percent / 100f))*/ + 40, 255),
                (int)Math.Min(color.B/* * (1 + (percent / 100f))*/ + 40, 255)
                );
        }

        public Dictionary<PlayerInfo, bool> GetStatus()
        {
            var status = new Dictionary<PlayerInfo, bool>();

            foreach(var p in Players)
            {
                status.Add(p.Info, IsAlive(p));
            }

            return status;
        }

        public long GetTime(PlayerInfo player)
        {
            foreach (var p in Players)
            {
                if (p.Info == player && p.nbPlays > 0)
                    return p.swPlays.ElapsedMilliseconds / p.nbPlays;
            }

            return 0;
        }
    }
}
