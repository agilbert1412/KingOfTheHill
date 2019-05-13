using System;

namespace KingOfTheHill.Dilemma
{
    public class DilemmaDecision : Decision
    {
        public DilemmaAction Action { get; set; }

        public DilemmaDecision(DilemmaAction action)
        {
            Action = action;
        }

        public DilemmaDecision() : this(DilemmaAction.Nothing)
        {

        }

        public static DilemmaDecision DefaultDecision
        {
            get
            {
                return new DilemmaDecision();
            }
        }

        public static int[] GetPointsChanges(DilemmaAction action1, DilemmaAction action2)
        {
            var pointDiff = new int[2];

            if (action1 == DilemmaAction.Nothing && action2 == DilemmaAction.Nothing)
            {
                pointDiff[0] = -3;
                pointDiff[1] = -3;
            }
            else if (action1 == DilemmaAction.Nothing)
            {
                pointDiff[0] =  0;
                pointDiff[1] = 3;
            }
            else if (action2 == DilemmaAction.Nothing)
            {
                pointDiff[0] =  3;
                pointDiff[1] = 0;
            }
            else if (action1 == DilemmaAction.Betray && action2 == DilemmaAction.Betray)
            {
                pointDiff[0] =  1;
                pointDiff[1] = 1;
            }
            else if (action1 == DilemmaAction.Cover && action2 == DilemmaAction.Cover)
            {
                pointDiff[0] =  2;
                pointDiff[1] = 2;
            }
            else if (action1 == DilemmaAction.Betray && action2 == DilemmaAction.Cover)
            {
                pointDiff[0] =  3;
                pointDiff[1] = 0;
            }
            else if (action1 == DilemmaAction.Cover && action2 == DilemmaAction.Betray)
            {
                pointDiff[0] =  0;
                pointDiff[1] = 3;
            }

            return pointDiff;
        }
    }

    public enum DilemmaAction
    {
        Cover,
        Betray,
        Nothing
    }
}
