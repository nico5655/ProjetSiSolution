using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static ProjetSI.Ballistique;

namespace ProjetSI
{
    [Serializable]
    public class Tir : IEquatable<Tir>
    {
        public Tir() : this((minAngle + maxAngle) / 2, (minSpeed + maxSpeed) / 2, 90, new Vector3D()) { }

        public Tir(double ballisticAngle,
            double ballSpeed, double lowAngle, 
            Vector3D rotation): this(ballSpeed + " m/s " + ballisticAngle + "°", ballisticAngle, ballSpeed, lowAngle, rotation) { }
        public Tir(string name, double ballisticAngle, double ballSpeed, double lowAngle, Vector3D rotation)
        {
            Name = name;
            BallisticAngle = ballisticAngle;
            Rotation = rotation;
            BallSpeed = ballSpeed;
            LowAngle = lowAngle;
        }

        public double BallisticAngle { get; set; }
        public Vector3D Rotation { get; set; }
        public double BallSpeed { get; set; }
        public double LowAngle { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public bool Equals(Tir other)
        {
            return other.Name == this.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is Tir)
                return Equals(obj as Tir);
            return base.Equals(obj);
        }
    }
}
