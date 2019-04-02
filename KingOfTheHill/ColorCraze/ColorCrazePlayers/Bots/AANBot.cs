using KingOfTheHill.ColorCraze.Players;
using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.ColorCraze.Players
{
    class AANBot : ColorCrazePlayer
    {
        private int nbrPlayers;

        private List<Point> pointToContest;

        public override TurnAction PlayTurn(List<ColorCrazePlayerInfo> allPlayers, Board board)
        {
            //We dont went to go on these points ...
            List<Point> badPoints = new List<Point>();

            Point actualPosition = GetInfo().CurrentLocation;

            //For all the player, we do something...
            foreach(ColorCrazePlayerInfo player in allPlayers)
            {
                badPoints.Add(player.CurrentLocation);    
            }

            var colorBoard = (ColorCrazeBoard)board;
            var playerColor = GetInfo().PlayerColor;

            //Right
            Point right = new Point(actualPosition.X + 1, actualPosition.Y);
            if (!badPoints.Contains(right) && this.pointToContest.Contains(right) && (colorBoard.Squares[right.X,right.Y] as ColorCrazeGridSquare).Owner != GetInfo().ID )
            {
                return new TurnAction(this.Info, new ColorCrazeDecision(ColorCrazeDirection.Right));
            }

            //Up
            Point up = new Point(actualPosition.X, actualPosition.Y - 1);
            if (!badPoints.Contains(up) && this.pointToContest.Contains(up) && (colorBoard.Squares[up.X, up.Y] as ColorCrazeGridSquare).Owner != GetInfo().ID )
            {
                return new TurnAction(this.Info, new ColorCrazeDecision(ColorCrazeDirection.Up));
            }

            //Left
            Point left = new Point(actualPosition.X - 1, actualPosition.Y);
            if (!badPoints.Contains(left) && this.pointToContest.Contains(left) && (colorBoard.Squares[left.X, left.Y] as ColorCrazeGridSquare).Owner != GetInfo().ID )
            {
                return new TurnAction(this.Info, new ColorCrazeDecision(ColorCrazeDirection.Left));
            }

            //Bottom
            Point down = new Point(actualPosition.X, actualPosition.Y + 1);
            if (!badPoints.Contains(down) && this.pointToContest.Contains(down) && (colorBoard.Squares[down.X, down.Y] as ColorCrazeGridSquare).Owner != GetInfo().ID )
            {
                return new TurnAction(this.Info, new ColorCrazeDecision(ColorCrazeDirection.Down));
            }

            //Default
            var directions = Enum.GetValues(typeof(ColorCrazeDirection));
            var randomDir = (ColorCrazeDirection)directions.GetValue(new Random().Next(directions.Length));
            var action = new TurnAction(Info, new ColorCrazeDecision(randomDir));
            return action;

        }

        public override void StartAll(List<PlayerInfo> allPlayers)
        {
            this.nbrPlayers = allPlayers.Count();
        }
        
        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {
            //List of who contains the quarter of the point (which we try to contest)
            this.pointToContest = new List<Point>();

            this.pointToContest.Add(this.GetInfo().CurrentLocation);

            Point current = this.GetInfo().CurrentLocation;

            //Beatiful code...
            while (this.pointToContest.Count < Math.Pow(this.nbrPlayers,2)/4)
            {

                //Right
                bool continueInThatDirection = true;
                while (continueInThatDirection)
                {
                    if (current.X + 1 < this.nbrPlayers && (!this.pointToContest.Contains(new Point(current.X + 1, current.Y))))
                    {
                        current = new Point(current.X + 1, current.Y);
                        this.pointToContest.Add(current);
                        continueInThatDirection = this.pointToContest.Contains(new Point(current.X, current.Y - 1));
                    }
                    else
                    {
                        if(current.X + 1 < this.nbrPlayers)
                        {
                            current = new Point(current.X + 1, current.Y);
                        }
                        else
                        {
                            continueInThatDirection = false;
                        }
                    }
                }

                //Up
                continueInThatDirection = true;
                while (continueInThatDirection)
                {
                    if (current.Y - 1 >= 0 && (!this.pointToContest.Contains(new Point(current.X, current.Y - 1))))
                    {
                        current = new Point(current.X, current.Y-1);
                        this.pointToContest.Add(current);
                        continueInThatDirection = this.pointToContest.Contains(new Point(current.X-1, current.Y));
                    }
                    else
                    {
                        if(current.Y - 1 >= 0)
                        {
                            current = new Point(current.X, current.Y - 1);
                        }
                        else
                        {
                            continueInThatDirection = false;
                        }
                    }
                }

                //Left
                continueInThatDirection = true;
                while (continueInThatDirection)
                {
                    if (current.X -1 >= 0 && (!this.pointToContest.Contains(new Point(current.X - 1, current.Y))))
                    {
                        current = new Point(current.X -1, current.Y);
                        this.pointToContest.Add(current);
                        continueInThatDirection = this.pointToContest.Contains(new Point(current.X, current.Y + 1));
                    }
                    else
                    {
                        if(current.X - 1 >= 0)
                        {
                            current = new Point(current.X - 1, current.Y);
                        }
                        else
                        {
                            continueInThatDirection = false;
                        }
                    }
                }


                //Bottom
                continueInThatDirection = true;
                while (continueInThatDirection)
                {
                    if (current.Y + 1 < this.nbrPlayers && (!this.pointToContest.Contains(new Point(current.X, current.Y + 1))))
                    {
                        current = new Point(current.X, current.Y + 1);
                        this.pointToContest.Add(current);
                        continueInThatDirection = this.pointToContest.Contains(new Point(current.X + 1, current.Y));
                    }
                    else
                    {
                        if(current.Y + 1 < this.nbrPlayers)
                        {
                            current = new Point(current.X, current.Y + 1);
                        }
                        else
                        {
                            continueInThatDirection = false;
                        }
                    }
                }
            }
        }
    }
}
