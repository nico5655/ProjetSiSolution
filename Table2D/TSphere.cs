using System.Windows;
using System.Windows.Media.Media3D;

namespace Objects3D
{
    /// <summary>
    /// Represents a sphere in the 3D space.
    /// </summary>
    public class TSphere : TBaseModel
    {
        /// <summary>
        /// Sphere ray.
        /// </summary>
        public static readonly DependencyProperty PropertyRayon =
            DependencyProperty.Register("P_Rayon", typeof(double), typeof(TSphere), new PropertyMetadata(1d, DefaultPm));
        public static readonly DependencyProperty PropertyNbFacettesHori =
            DependencyProperty.Register("P_NbFacettesHori", typeof(int), typeof(TSphere), new PropertyMetadata(40, DefaultPm));
        public static readonly DependencyProperty PropertyNbFacettesVert =
            DependencyProperty.Register("P_NbFacettesVert", typeof(int), typeof(TSphere), new PropertyMetadata(40, DefaultPm));
        /// <summary>
        /// Ball position in the 3D space.
        /// </summary>
        public static readonly DependencyProperty PropertyPointDeReference =
            DependencyProperty.Register("P_PointDeReference", typeof(Point3D), typeof(TSphere), new PropertyMetadata(new Point3D(0, 0, 0), DefaultPm));

        /// <summary>
        /// Sphere ray.
        /// </summary>
        public double P_Rayon
        {
            get
            {
                return (double)GetValue(PropertyRayon);
            }
            set
            {
                SetValue(PropertyRayon, value);
            }
        }

        public int P_NbFacettesHori
        {
            get
            {
                return (int)GetValue(PropertyNbFacettesHori);
            }
            set
            {
                SetValue(PropertyNbFacettesHori, value);
            }
        }

        public int P_NbFacettesVert
        {
            get
            {
                return (int)GetValue(PropertyNbFacettesHori);
            }
            set
            {
                SetValue(PropertyNbFacettesHori, value);
            }
        }

        /// <summary>
        /// Ball position in the 3D space.
        /// </summary>
        public Point3D P_PointDeReference
        {
            get
            {
                return (Point3D)GetValue(PropertyPointDeReference);
            }
            set
            {
                SetValue(PropertyPointDeReference, value);
            }
        }

        /// <summary>
        /// Ball's center of gravity.
        /// </summary>
        public Point3D P_PointDeGravite { get; set; }

        /// <summary>
        /// Update the 3D model.
        /// </summary>
        protected override void OnUpdateModel()
        {
            P_Model = new GeometryModel3D(GenererSphere(P_PointDeReference, P_Rayon,
                P_Rayon, P_NbFacettesHori, P_NbFacettesVert), GetMaterial());
            base.OnUpdateModel();
            P_PointDeGravite = GenererPointGravite(BoiteEnglobante);
        }

        /// <summary>
        /// Apply 3D translation and 3D rotation to the ball.
        /// </summary>
        /// <param name="trans">3D translation to apply</param>
        /// <param name="omega">3D rotation to apply</param>
        public void AppliquerTransformation(Vector3D trans, Vector3D omega)
        {
            Transform3DGroup trs = new Transform3DGroup();
            trs.Children.Add(new TranslateTransform3D(trans));
            //if needed
            trs.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 155), trs.Transform(P_PointDeReference)));
            //base transform to see the text
            double value = omega.Length;//rotation value
            trs.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(omega, value), trs.Transform(P_PointDeReference)));//rotation axis and value
            Transform = trs;
        }

        /// <summary>
        /// Resets the ball position.
        /// </summary>
        public void ResetPosition()
        {
            Transform = new TranslateTransform3D(0d, 0d, 0d);
        }
    }
}