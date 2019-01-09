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
        public Point3D P_PointDeGravite
        {
            get
            {
                return p_PointDeGravite;
            }

            set
            {
                p_PointDeGravite = value;
            }
        }
        private Point3D p_PointDeGravite;
        protected override void OnUpdateModel()
        {
            P_Model = new GeometryModel3D(GenererSphere(P_PointDeReference, P_Rayon,
                P_Rayon, P_NbFacettesHori, P_NbFacettesVert), GetMaterial());
            base.OnUpdateModel();
            P_PointDeGravite = GenererPointGravite(BoiteEnglobante);
        }
        public void AppliquerTranslation(double transx, double transy, double transz, Vector3D axe, double angle)
        {
            if (P_Model != null)
            {
                BoiteEnglobante = P_Model.Bounds;
                P_PointDeGravite = GenererPointGravite(BoiteEnglobante);
            }
            Transform3DGroup trs = new Transform3DGroup();
            trs.Children.Add(new TranslateTransform3D(transx, transy, transz));
            trs.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(axe, angle), P_PointDeReference));
            Transform = trs;
            if (P_Model != null)
            {
                Rect3D rect = BoiteEnglobante;
                Point3D point = rect.Location;
                point.Offset(transx, transy, transz);
                rect.Location = point;
                BoiteEnglobante = rect;
                P_PointDeGravite = GenererPointGravite(BoiteEnglobante);
            }
        }
        public bool TestCollision(IEnglobe other)
        {
            return other.BoiteEnglobante.IntersectsWith(BoiteEnglobante);
        }
        public void ResetPosition()
        {
            Transform = new TranslateTransform3D(0d, 0d, 0d);
        }
    }
}