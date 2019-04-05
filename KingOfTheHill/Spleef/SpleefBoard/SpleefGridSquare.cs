using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using KingOfTheHill.Board;

namespace KingOfTheHill.Spleef.SpleefBoard
{
    public class SpleefGridSquare : GridSquare
    {
        public SpleefSquareStatus Status { get; set; }

        public int HealthRemaining { get; set; }

        public bool IsHole => Status == SpleefSquareStatus.Hole;

        public bool IsSolid => Status == SpleefSquareStatus.Solid;

        public override Color Color
        {
            get
            {
                if (IsSolid)
                {
                    return Color.White;
                }

                return Color.Gray;
            }
            set { base.Color = value; }
        }

        public SpleefGridSquare() : base()
        {
            Status = SpleefSquareStatus.Solid;
            HealthRemaining = 5;
        }

        public void Destroy()
        {
            Status = SpleefSquareStatus.Hole;
        }

        public void Damage()
        {
            HealthRemaining--;
            if (HealthRemaining < 1)
            {
                Destroy();
            }
        }
    }

    public enum SpleefSquareStatus
    {
        Solid,
        Hole
    }
}
