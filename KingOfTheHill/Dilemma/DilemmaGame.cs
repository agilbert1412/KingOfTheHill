using KingOfTheHill.Dilemma.DilemmaPlayers;
using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace KingOfTheHill.Dilemma
{
    public class DilemmaGame : Game
    {
        private static Random rand = new Random();
        
        private DilemmaPlayer[] Players { get; set; }

        public Dictionary<DilemmaPlayerInfo, DilemmaDecision> PlayersDecisions { get; private set; }
        
        private bool _hasHappened = false;

        public DilemmaGame(DilemmaPlayer[] players)
        {
            if (players.Length != 2)
            {
                throw new ArgumentException("A round of Prisoner's Dilemma contains exactly two players");
            }
            Players = players;
            PlayersDecisions = new Dictionary<DilemmaPlayerInfo, DilemmaDecision>();
        }

        public DilemmaGame(Dictionary<DilemmaPlayerInfo, DilemmaDecision> decisions)
        {
            PlayersDecisions = decisions;
        }

        private void ShufflePlayers()
        {
            if (rand.Next(2) < 1)
            {
                var tmp = Players[0];
                Players[0] = Players[1];
                Players[1] = tmp;
            }
        }

        public override void Paint(Graphics gfx, Rectangle bounds)
        {
            var width = bounds.Width / 3;
            var rectLeft = new Rectangle(bounds.X, bounds.Y, width, bounds.Height);
            var rectRight = new Rectangle(bounds.X + (width * 2), bounds.Y, width, bounds.Height);

            Players[0].Paint(gfx, rectLeft);
            Players[1].Paint(gfx, rectRight);

            var half = width / 2;

            var rectAction1 = new Rectangle(bounds.X + width, bounds.Y + (half/2), half, bounds.Height / 2);
            var rectAction2 = new Rectangle(bounds.X + width + half, bounds.Y + (half / 2), half, bounds.Height / 2);

            if (_hasHappened)
            {
                var dec1 = PlayersDecisions[Players[0].GetInfo() as DilemmaPlayerInfo].Action;
                var dec2 = PlayersDecisions[Players[1].GetInfo() as DilemmaPlayerInfo].Action;

                var img1 = KingOfTheHill.Properties.Resources.question_mark;
                if (dec1 == DilemmaAction.Cover)
                {
                    img1 = KingOfTheHill.Properties.Resources.Peace;
                }
                else if (dec1 == DilemmaAction.Betray)
                {
                    img1 = KingOfTheHill.Properties.Resources.Betray;
                }

                var img2 = KingOfTheHill.Properties.Resources.question_mark;
                if (dec2 == DilemmaAction.Cover)
                {
                    img2 = KingOfTheHill.Properties.Resources.Peace;
                }
                else if (dec2 == DilemmaAction.Betray)
                {
                    img2 = KingOfTheHill.Properties.Resources.Betray;
                }

                gfx.DrawImage(img1, rectAction1);
                gfx.DrawImage(img2, rectAction2);
            }
            else
            {
                gfx.DrawImage(KingOfTheHill.Properties.Resources.question_mark, rectAction1);
                gfx.DrawImage(KingOfTheHill.Properties.Resources.question_mark, rectAction2);
            }
        }

        public override bool PlayStep(Dictionary<PlayerInfo, int> playersScores, object completeHistory)
        {
            foreach (var player in Players)
            {
                player.swPlays.Start();

                var decision = DilemmaDecision.DefaultDecision;

                try
                {
                    decision = player.PlayTurn(playersScores, completeHistory as List<DilemmaGame>);
                }
                catch (Exception e)
                {
                    decision = DilemmaDecision.DefaultDecision;
                }

                PlayersDecisions.Add(player.Info as DilemmaPlayerInfo, decision);

                player.nbPlays++;
                player.swPlays.Stop();
            }

            _hasHappened = true;

            return true;
        }

        private DilemmaPlayerInfo GetDilemmaInfo(Player p)
        {
            return p.GetInfo() as DilemmaPlayerInfo;
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

        public DilemmaGame Clone()
        {
            var clone = new DilemmaGame(PlayersDecisions);

            return clone;
        }
    }
}
