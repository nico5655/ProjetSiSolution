using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static ProjetSI.Ballistique;

namespace ProjetSI
{
    public class Tir
    {
        public Tir() : this((minAngle + maxAngle) / 2, (minSpeed + maxSpeed) / 2, 90, new Vector3D()) { }

        public Tir(double ballisticAngle, double ballSpeed, double lowAngle, Vector3D rotation)
        {
            BallisticAngle = ballisticAngle;
            Rotation = rotation;
            BallSpeed = ballSpeed;
            LowAngle = lowAngle;
        }

        public double BallisticAngle { get; set; }
        public Vector3D Rotation { get; set; }
        public double BallSpeed { get; set; }
        public double LowAngle { get; set; }
    }
}
