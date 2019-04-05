using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.Spleef
{
    public class SpleefDecision : Decision
    {
        public SpleefAction Action { get; set; }

        public Point Target { get; set; }

        public bool IsValid
        {
            get
            {
                switch (Action)
                {
                    case SpleefAction.Wait:
                        return true;
                    case SpleefAction.Move:
                        return Math.Abs(Target.X) <= 1 && Math.Abs(Target.Y) <= 1;
                    case SpleefAction.Hole:
                        var totalDistance = Math.Abs(Target.X) + Math.Abs(Target.Y);
                        return totalDistance <= 2;
                    default:
                        return false;
                }
            }
        }

        public SpleefDecision(SpleefAction action, Point target)
        {
            Action = action;
            Target = target;
        }

        public SpleefDecision(Point target) : this(SpleefAction.Move,target)
        {

        }

        public SpleefDecision() : this(SpleefAction.Wait, Point.Empty)
        {

        }

        public static SpleefDecision DefaultDecision
        {
            get
            {
                return new SpleefDecision();
            }
        }
    }

    public enum SpleefAction
    {
        Wait,
        Move,
        Hole
    }
}
