using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.Spleef.SpleefPlayers.Bots.SmartKaitoBot
{
    public class SimpleKaitoBrain
    {
        private static Random BrainRandom = new Random();

        public double SurviveOptionsValue { get; set; }
        public double LeafMoveValue { get; set; }
        public double DefaultMoveValue { get; set; }
        public double MoveValue { get; set; }
        public double WaitValue { get; set; }
        public double EnemyBirdDistance { get; set; }
        public double EnemyBirdDistancePow2 { get; set; }
        public double CantReachValue { get; set; }
        public double PathDistValue { get; set; }
        public double PathDistValuePow2 { get; set; }
        public double DepthValue { get; set; }

        public SimpleKaitoBrain()
        {
            Load(new Dictionary<string, double>());
        }

        public SimpleKaitoBrain(Dictionary<string, double> coefficients)
        {
            Load(coefficients);
        }

        public void Load(Dictionary<string, double> values)
        {
            SurviveOptionsValue = 3.36816837967032;
            LeafMoveValue = 0.318440075432302;
            DefaultMoveValue = 13.0579052310348;
            MoveValue = -3.29777475254902;
            WaitValue = 14.464838917695;
            EnemyBirdDistance = -17.7971062938967;
            EnemyBirdDistancePow2 = 4.97663827703521;
            CantReachValue = 5.14252936892397;
            PathDistValue = -14.996747616875;
            PathDistValuePow2 = 10.3745370356358;
            DepthValue = 6.96288384282549;

            double val;
            if (values.TryGetValue("SurviveOptionsValue", out val))
            {
                SurviveOptionsValue = val;
            }
            if (values.TryGetValue("LeafMoveValue", out val))
            {
                LeafMoveValue = val;
            }
            if (values.TryGetValue("DefaultMoveValue", out val))
            {
                DefaultMoveValue = val;
            }
            if (values.TryGetValue("MoveValue", out val))
            {
                MoveValue = val;
            }
            if (values.TryGetValue("WaitValue", out val))
            {
                WaitValue = val;
            }
            if (values.TryGetValue("EnemyBirdDistance", out val))
            {
                EnemyBirdDistance = val;
            }
            if (values.TryGetValue("EnemyBirdDistancePow2", out val))
            {
                EnemyBirdDistancePow2 = val;
            }
            if (values.TryGetValue("CantReachValue", out val))
            {
                CantReachValue = val;
            }
            if (values.TryGetValue("PathDistValue", out val))
            {
                PathDistValue = val;
            }
            if (values.TryGetValue("PathDistValuePow2", out val))
            {
                PathDistValuePow2 = val;
            }
            if (values.TryGetValue("DepthValue", out val))
            {
                DepthValue = val;
            }
        }

        public Dictionary<string, double> GetValues()
        {
            var dic = new Dictionary<string, double>();

            dic.Add("SurviveOptionsValue", SurviveOptionsValue);
            dic.Add("LeafMoveValue", LeafMoveValue);
            dic.Add("DefaultMoveValue", DefaultMoveValue);
            dic.Add("MoveValue", MoveValue);
            dic.Add("WaitValue", WaitValue);
            dic.Add("EnemyBirdDistance", EnemyBirdDistance);
            dic.Add("EnemyBirdDistancePow2", EnemyBirdDistancePow2);
            dic.Add("CantReachValue", CantReachValue);
            dic.Add("PathDistValue", PathDistValue);
            dic.Add("PathDistValuePow2", PathDistValuePow2);
            dic.Add("DepthValue", DepthValue);

            return dic;
        }

        public void Mutate(double variance)
        {
            SurviveOptionsValue = GetGaussianNumber(SurviveOptionsValue, variance);
            LeafMoveValue = GetGaussianNumber(LeafMoveValue, variance);
            DefaultMoveValue = GetGaussianNumber(DefaultMoveValue, variance);
            MoveValue = GetGaussianNumber(MoveValue, variance);
            WaitValue = GetGaussianNumber(WaitValue, variance);
            EnemyBirdDistance = GetGaussianNumber(EnemyBirdDistance, variance);
            EnemyBirdDistancePow2 = GetGaussianNumber(EnemyBirdDistancePow2, variance);
            CantReachValue = GetGaussianNumber(CantReachValue, variance);
            PathDistValue = GetGaussianNumber(PathDistValue, variance);
            PathDistValuePow2 = GetGaussianNumber(PathDistValuePow2, variance);
            DepthValue = GetGaussianNumber(DepthValue, variance);
        }

        private double GetGaussianNumber(double center, double variance)
        {
            double u1 = 1.0 - BrainRandom.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - BrainRandom.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                center + variance * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;
        }
    }
}
