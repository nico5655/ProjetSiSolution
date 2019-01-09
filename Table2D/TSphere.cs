using System.Windows;
using System.Windows.Media.Media3D;

namespace Objects3D
{
    public class TSphere : TBaseModel
    {
        public static readonly DependencyProperty PropertyRayon =
            DependencyProperty.Register("P_Rayon", typeof(double), typeof(TSphere), new PropertyMetadata(1d, DefaultPm));
        public static readonly DependencyProperty PropertyNbFacettesHori =
            DependencyProperty.Register("P_NbFacettesHori", typeof(int), typeof(TSphere), new PropertyMetadata(40, DefaultPm));
        public static readonly DependencyProperty PropertyNbFacettesVert =
            DependencyProperty.Register("P_NbFacettesVert", typeof(int), typeof(TSphere), new PropertyMetadata(40, DefaultPm));
        public static readonly DependencyProperty PropertyPointDeReference =
            DependencyProperty.Register("P_PointDeReference", typeof(Point3D), typeof(TSphere), new PropertyMetadata(new Point3D(0, 0, 0), DefaultPm));

        
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
        public Point3D P_PointDeGravite { get; set; }

        protected override void OnUpdateModel()
        {
            P_Model = new GeometryModel3D(GenererSphere(P_PointDeReference, P_Rayon,
                P_Rayon, P_NbFacettesHori, P_NbFacettesVert), GetMaterial());
            base.OnUpdateModel();
            P_PointDeGravite = GenererPointGravite(BoiteEnglobante);
        }
        public void AppliquerTransformation(Vector3D trans, Vector3D omega)
        {
            Transform3DGroup trs = new Transform3DGroup();
            trs.Children.Add(new TranslateTransform3D(trans));
            //if needed
            trs.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), omega.X), trs.Transform(P_PointDeReference)));
            trs.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), omega.Y + 155), trs.Transform(P_PointDeReference)));
            trs.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), omega.Z), trs.Transform(P_PointDeReference)));
            Transform = trs;
        }

        public void ResetPosition()
        {
            Transform = new TranslateTransform3D(0d, 0d, 0d);
        }
    }
}