using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using KingOfTheHill.ColorCraze.ColorCrazeBoard;
using KingOfTheHill.Players;

namespace KingOfTheHill.ColorCraze.ColorCrazePlayers.Bots
{
    public class FillerBot : ColorCrazePlayer
    {
        Random r = new Random();

        public override void StartAll(List<PlayerInfo> allPlayers)
        {
            
        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {

        }

        public override ColorCrazeDecision PlayTurn(List<ColorCrazePlayerInfo> allPlayers, Board.Board board)
        {
            var colorBoard = (ColorCrazeBoard.ColorCrazeBoard)board;

            // Distance la plus courte vers une case vide trouvée (ici aucune)
            var minDistance = (colorBoard.Width * 2) + 1;
            // Position de la case vide la plus proche (pour l'instant aucune)
            var target = GetInfo().CurrentLocation;

            // On parcoure les deux dimensions du board
            for (var x = 0; x < colorBoard.Width; x++)
            {
                for (var y = 0; y < colorBoard.Height; y++)
                {
                    // Si la case est vide
                    if (((ColorCrazeGridSquare)(colorBoard.Squares[x,y])).Owner == -1)
                    {
                        // On teste la distance entre la case et nous-même
                        var distance = GetDistance(GetInfo().CurrentLocation.X, GetInfo().CurrentLocation.Y, x, y);

                        // Si elle est plus proche que la plus proche trouvée à date
                        if (distance < minDistance)
                        {
                            // On note sa distance comme la plus proche à date
                            minDistance = distance;
                            //On note la position de la case
                            target = new Point(x,y);
                        }
                    }
                }
            }

            var possibleDirections = new List<ColorCrazeDirection>();

            if (target.X > GetInfo().CurrentLocation.X)
            {
                possibleDirections.Add(ColorCrazeDirection.Right);
            }
            else if (target.X < GetInfo().CurrentLocation.X)
            {
                possibleDirections.Add(ColorCrazeDirection.Left);
            }
            if (target.Y > GetInfo().CurrentLocation.Y)
            {
                possibleDirections.Add(ColorCrazeDirection.Down);
            }
            else if (target.Y < GetInfo().CurrentLocation.Y)
            {
                possibleDirections.Add(ColorCrazeDirection.Up);
            }

            if (possibleDirections.Any())
            {
                return new ColorCrazeDecision(possibleDirections[r.Next(0, possibleDirections.Count)]);
            }
            else
            {
                var directions = Enum.GetValues(typeof(ColorCrazeDirection));
                var randomDir = (ColorCrazeDirection)directions.GetValue(r.Next(directions.Length));
                var action = new ColorCrazeDecision(randomDir);
                return action;
            }
        }
    }
}
