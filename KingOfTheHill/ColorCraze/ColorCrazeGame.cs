using KingOfTheHill.ColorCraze.Players;
using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.ColorCraze
{
    public class ColorCrazeGame : Game
    {
        private static Random rand = new Random();

        public ColorCrazeBoard Board { get; set; }
        public List<ColorCrazePlayer> Players { get; set; }

        private int PlayerToPlay = 0;
        private int turnCounterAfterEnd = -1;

        public ColorCrazeGame(List<ColorCrazePlayer> players)
        {
            Board = new ColorCrazeBoard(players.Count, players.Count);
            Players = players;
        }

        private void ShufflePlayers()
        {
            var oldLst = new List<ColorCrazePlayer>(Players);
            var newLst = new List<ColorCrazePlayer>();

            while (oldLst.Any())
            {
                var chosen = rand.Next(0, oldLst.Count);
                newLst.Add(oldLst[chosen]);
                oldLst.RemoveAt(chosen);
            }

            Players = newLst;
        }

        public override void Paint(Graphics gfx, Rectangle bounds)
        {
            Board.Paint(gfx, bounds, turnCounterAfterEnd == -1);

            foreach (var p in Players)
            {
                p.Paint(gfx, Board, bounds);
            }
        }

        /// <summary>
        /// Plays the next step of the game. Returns true if the game is over after this step
        /// </summary>
        /// <returns>True if the game is over, false if it can go on</returns>
        public override bool PlayStep(Dictionary<PlayerInfo, int> playersScores)
        {
            if (turnCounterAfterEnd == -1)
            {
                StartGame(playersScores);
            }

            var infos = Players.Select(x => x.GetInfo()).ToList();
            
            Players[PlayerToPlay].swPlays.Start();
            TurnAction action;
            try
            {
                action = Players[PlayerToPlay].PlayTurn(infos, Board);
            }
            catch (Exception e)
            {
                action = new TurnAction(Players[PlayerToPlay].Info, new ColorCrazeDecision(new Point(0, 0)));
            }
            Players[PlayerToPlay].nbPlays++;
            Players[PlayerToPlay].swPlays.Stop();

            if (action.ActionTaken is ColorCrazeDecision)
            {
                var decision = (ColorCrazeDecision)action.ActionTaken;

                var absX = Math.Abs(decision.Movement.X);
                var absY = Math.Abs(decision.Movement.Y);
                var totalDistance = absX + absY;

                var movement = decision.Movement;

                if (totalDistance > 1 || absX > 1 || absY > 1)
                {
                    movement = new Point(0, 0);
                }

                var destination = new Point(Players[PlayerToPlay].GetInfo().CurrentLocation.X + movement.X, Players[PlayerToPlay].GetInfo().CurrentLocation.Y + movement.Y);
                if (destination.X >= 0 && destination.Y >= 0 && destination.X < Board.Width && destination.Y < Board.Height)
                {
                    if (!Players.Any(x => x.GetInfo().CurrentLocation == destination))
                    {
                        Players[PlayerToPlay].GetInfo().CurrentLocation = destination;
                        Board.Squares[destination.X, destination.Y].Color = Lighten(Players[PlayerToPlay].GetInfo().PlayerColor, 20);
                        ((ColorCrazeGridSquare)(Board.Squares[destination.X, destination.Y])).Owner = Players[PlayerToPlay].Info.ID;
                    }
                }
            }

            PlayerToPlay = (PlayerToPlay + 1) % Players.Count;

            if (Board.IsFull())
            {
                turnCounterAfterEnd++;
                return turnCounterAfterEnd > Board.Width * Board.Height;
            }

            return false;
        }

        private void StartGame(Dictionary<PlayerInfo, int> playersAndScores)
        {
            foreach (var player in Players)
            {
                player.GetInfo().CurrentLocation = new Point(rand.Next(0, Board.Width), rand.Next(0, Board.Height));
                while (Players.Any(x => x != player && x.GetInfo().CurrentLocation == player.GetInfo().CurrentLocation))
                {
                    player.GetInfo().CurrentLocation = new Point(rand.Next(0, Board.Width), rand.Next(0, Board.Height));
                }
                Board.Squares[player.GetInfo().CurrentLocation.X, player.GetInfo().CurrentLocation.Y].Color = Lighten(player.GetInfo().PlayerColor, 20);
                ((ColorCrazeGridSquare)(Board.Squares[player.GetInfo().CurrentLocation.X, player.GetInfo().CurrentLocation.Y])).Owner = player.Info.ID;
                
                player.StartGame(playersAndScores);
            }
            ShufflePlayers();
            turnCounterAfterEnd++;
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

        public override Dictionary<PlayerInfo, int> GetStatus()
        {
            var status = new Dictionary<PlayerInfo, int>();

            foreach(var p in Players)
            {
                status.Add(p.Info, Board.CountSquaresOfOwner(p.Info.ID));
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
