using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.Players;
using KingOfTheHill.Spleef.SpleefBoard;

namespace KingOfTheHill.Spleef.SpleefPlayers.Bots
{
    public class ScaleBot : SpleefPlayer
    {
        int gameNumber = 0;
        int turnNumber = 0;
        Dictionary<int, PlayerPersonality> playerArray = new Dictionary<int, PlayerPersonality>();
        int totAggr = 1;
        int stayCpt = 3;

        double[] ratiosBalance = new Double[5]
        {
            5.5, 1.5, 2.75, 2.75, 1.0
        };

        public override void StartAll(List<SpleefPlayerInfo> allPlayers)
        {
            gameNumber = 0;
            turnNumber = 0;
            playerArray = new Dictionary<int, PlayerPersonality>();
            totAggr = 1;
            stayCpt = 3;
        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {
            gameNumber++;
            turnNumber = 0;
        }

        public override SpleefDecision PlayTurn(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board)
        {
            var choix = SpleefDecision.DefaultDecision;
            turnNumber++;
            if (gameNumber == 1 && turnNumber == 1)
            {
		        foreach (var p in allPlayers)
                {
                    playerArray.Add(p.ID, new PlayerPersonality(p.ID));
                }
            }

            var players = GetAlivePlayers(allPlayers, board);
            var me = (SpleefPlayerInfo)Info;

            // Need history
            /*if (roundNum !== 0 && me.History.length > 0)
            {
		        foreach (var p in players)
                {
                    if (players[p].History[players[p].History.length - 1].indexOf("HOLE") !== -1)
                    {
                        playerArray[players[p].ID].Aggr++;
                        totAggr++;
                        playerArray[players[p].ID].Echec == 0.5;
                    }
                    else if (players[p].History[players[p].History.length - 1].indexOf("MOVE") !== -1)
                    {
                        playerArray[players[p].ID].Echec == 0.5;
                    }
                    if (roundNum == 5)
                    {
                        playerArray[players[p].ID].Echec == 0.5;
                    }
                    else if (players[p].History[players[p].History.length - 1] == ("I suck at this game") && playerArray[players[p].ID].Echec != 0.5)
                    {
                        playerArray[players[p].ID].Echec == 2;
                    }
                }
            }*/

            //Moves bitches
            var canStay = true;
            if (stayCpt == 0)
            {
                canStay = false;
            }
            // ajouter et si bot agressif à proximité, ne pas stay, sen éloigner ou le bloquer

            var myBoard = board.Clone();
            var myBoard2 = board.Clone();

	        foreach (var p in players)
            {
                if (p.ID != me.ID)
                {
                    myBoard[p.CurrentLocation.X, p.CurrentLocation.Y].Status = SpleefSquareStatus.Hole;
                }
                myBoard2[p.CurrentLocation.X, p.CurrentLocation.Y].Status = SpleefSquareStatus.Hole;
            }


            var myLongArray = new List<List<Point>>();
            for (var i = 0; i < myBoard2.Width; i++)
            {
                for (var j = 0; j < myBoard2.Height; j++)
                {
                    if (myBoard2[i, j].IsSolid)
                    {
                        var arrP = new List<Point>();
                        arrP.Add(new Point(i,j ));
                        myLongArray.Add(arrP);
                    }
                }
            }

            var longeurArray = -1;
            while (longeurArray != myLongArray.Count)
            {
                longeurArray = myLongArray.Count;
                for (var m = 0; m < myLongArray.Count; m++)
                {
                    for (var n = m + 1; n < myLongArray.Count; n++)
                    {
                        var isAnyAdjacent = false;
                        for (var k = 0; k < myLongArray[n].Count; k++)
                        {
                            for (var l = 0; l < myLongArray[m].Count; l++)
                            {
                                var dis = GetDistance(myLongArray[n][k], myLongArray[m][l]);
                                if (dis == 1 || (dis == 2 && myLongArray[n][k].X != myLongArray[m][l].X && myLongArray[n][k].Y != myLongArray[m][l].Y))
                                {
                                    isAnyAdjacent = true;
                                }
                            }
                        }

                        if (isAnyAdjacent)
                        {

                            for (var l = 0; l < myLongArray[m].Count; l++)
                            {
                                myLongArray[n].Add(myLongArray[m][l]);
                            }

                            myLongArray.RemoveAt(m);

                            n = myLongArray.Count + 1;
                            m = myLongArray.Count + 1;
                        }
                    }
                }
            }


            //code temporaire
            var monArray8CaseCancer = GetCasesAround(myBoard, me.CurrentLocation);

            var array8CaseCancerScores = new Dictionary<Point, double>();

            if (monArray8CaseCancer.Count > 0)
            {
		        foreach (var cancer in monArray8CaseCancer)
                {
                    var arraySecondaire = GetCasesAround(myBoard, cancer);
                    var nombreCasesTourSuivant = arraySecondaire.Count;
                    var lesProfondeurs = 0;
                    if (nombreCasesTourSuivant > 0)
                    {
				        foreach (var sdf in arraySecondaire)
                        {
                            var arrayTertiere = GetCasesAround(myBoard, sdf);
                            lesProfondeurs += arrayTertiere.Count;
                        }
                    }

                    var distanceEnnemi = 0.0;
                    var multipli = 1.0;
			        foreach (var dude in players)
                    {
                        if (dude.ID != me.ID)
                        {
                            var pathEnemi = FindShortestPathAStar(board, cancer, dude.CurrentLocation);
                            if (pathEnemi != null)
                            {
                                multipli = ((1 + (playerArray[dude.ID].Aggr / totAggr)) * playerArray[dude.ID].Failure) * ratiosBalance[2];
                                distanceEnnemi += (Math.Sqrt(pathEnemi.Count)) * multipli / players.Count;
                            }
                        }
                    }
                    
                    var sizeZone = 0;
                    for (var m = 0; m < myLongArray.Count; m++)
                    {
                        for (var n = 0; n < myLongArray[m].Count; n++)
                        {
                            if (myLongArray[m][n].X == cancer.X && myLongArray[m][n].Y == cancer.Y)
                            {
                                sizeZone = myLongArray[m].Count;
                            }
                        }
                    }

                    array8CaseCancerScores.Add(cancer, (ratiosBalance[0] * nombreCasesTourSuivant)
                                                        + (ratiosBalance[1] * distanceEnnemi)
                                                        + (ratiosBalance[3] * lesProfondeurs)
                                                        + (ratiosBalance[4] * sizeZone / players.Count));
                }

                var bestMoves = new List<Point>();
                var bestMovesScore = double.MinValue;
		        foreach (var m in monArray8CaseCancer)
                {
                    if (array8CaseCancerScores[m] > bestMovesScore)
                    {
                        bestMovesScore = array8CaseCancerScores[m];
                        bestMoves = new List<Point>() {m};
                    }
                    else if (Math.Abs(array8CaseCancerScores[m] - bestMovesScore) < 0.01)
                    {
                        bestMoves.Add(m);
                    }
                }

                var chosenMove = bestMoves[RandomGen.Next(0, bestMoves.Count)];

                var isSafe = true;
		        foreach (var dude in players)
                {
                    if (dude.ID != me.ID)
                    {
                        if (FindShortestPathAStar(board, me.CurrentLocation, dude.CurrentLocation) != null)
                        {
                            isSafe = false;
                        }
                    }
                }

                if (players.Count < 4)
                {
			        foreach (var p in players)
                    {

                    }
                }

                if (isSafe && stayCpt > 0)
                {
                    choix = SpleefDecision.DefaultDecision;
                }
                else
                {
                    choix = new SpleefDecision(SpleefAction.Move, new Point(chosenMove.X, chosenMove.Y));
                }

            }
            
            if (choix.Action == SpleefAction.Wait)
            {
                var caseAttaque = GetCasesAround2Dis(myBoard, me.CurrentLocation);
                if (caseAttaque.Count > 0)
                {
                    var chosenAttack = caseAttaque[RandomGen.Next(0, caseAttaque.Count)];

                    choix = new SpleefDecision(SpleefAction.Hole, new Point(chosenAttack.X, chosenAttack.Y));
                }
                SpleefPlayerInfo closestOpponent = null;
                var closestOpponentDistance = double.MaxValue;
		        foreach (var p in players)
                {
                    if (p.ID != me.ID)
                    {
                        var dist = GetDistance(me.CurrentLocation, p.CurrentLocation);
                        if (dist < closestOpponentDistance && dist > 0)
                        {
                            closestOpponentDistance = dist;
                            closestOpponent = p;
                        }
                    }
                }

                if (closestOpponentDistance < 3)
                {
                    choix = new SpleefDecision(SpleefAction.Hole, new Point(closestOpponent.CurrentLocation.X, closestOpponent.CurrentLocation.Y));
                }
            }


            // Retours possibles
            // Si une action invalide est retournée, l'action "WAIT" sera effectuée à la place
            // Faire un trou sur un trou existant, faire un trou trop loin
            // Bouger sur une case trop loin, bouger sur sa propre case
            // faire un trou ou bouger en dehors du board
            // sont toutes des actions invalides

            // Actions quand on est au sol
            // WAIT					Ne rien faire pour ce tour
            // MOVE X Y				Bouger sur une case (et détruire la case d'ou vous venez) dans un range de 1 de toi incluant les diagonales. Ni X ni Y ne peuvent être à plus de 1 de ta coordonnée.
            // HOLE X Y				Exploser une case au choix dans un range de 1-2 (Distance de Manhattan)


            if (choix.Action == SpleefAction.Wait || choix.Action == SpleefAction.Hole)
            {
                stayCpt--;
            }
            else
            {
                stayCpt = 3;
            }
            if (choix.Action == SpleefAction.Wait)
            {
                choix = new SpleefDecision(SpleefAction.Hole, new Point(-9999, -9999));
            }
            choix.Target = new Point(choix.Target.X - me.CurrentLocation.X, choix.Target.Y - me.CurrentLocation.Y);
            return choix;
        }

        private List<Point> GetCasesAround(SpleefBoard.SpleefBoard board, Point pos)
        {
            var cases = new List<Point>();
            for (var x = -1; x < 2; x++)
            {
                for (var y = -1; y < 2; y++)
                {
                    var realCoordX = pos.X + x;

                    var realCoordY = pos.Y + y;
                    if ((x != 0 || y != 0) && (realCoordX > -1 && realCoordY > -1 && realCoordX < board.Width && realCoordY < board.Height
                                               && board[realCoordX, realCoordY].IsSolid))
                    {
                        cases.Add(new Point(realCoordX, realCoordY));
                    }
                }
            }
            return cases;
        }

        private List<Point> GetCasesAround2Dis(SpleefBoard.SpleefBoard board, Point pos)
        {
            var cases = new List<Point>();
            var casestempo = new List<Point>()
            {
                new Point(pos.X + 2, pos.Y),
                new Point(pos.X - 2, pos.Y),
                new Point(pos.X, pos.Y + 2),
                new Point(pos.X, pos.Y - 2)
            };

            foreach (var c in casestempo)
            {
                var laCase = c;
                if ((laCase.X > -1 && laCase.Y > -1 && laCase.X < board.Width && laCase.Y < board.Height
                     && board[laCase.X, laCase.Y].IsSolid) && (FindShortestPathAStar(board, pos, laCase) == null))
                {
                    cases.Add(laCase);
                }
            }
            return cases;
        }
    }

    internal class PlayerPersonality
    {
        public int ID { get; set; }
        public double Aggr { get; set; }
        public double Failure { get; set; }

        internal PlayerPersonality(int id)
        {
            ID = id;
            Aggr = 1;
            Failure = 1;
        }
    }
}
