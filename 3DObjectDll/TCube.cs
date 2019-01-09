using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Objects3D
{
    public class TCube : TBaseModel
    {
        public static readonly DependencyProperty PropertyPointDeReference =
            DependencyProperty.Register("P_PointDeReference", typeof(Point3D), typeof(TCube), new PropertyMetadata(new Point3D(0, 0, 0), DefaultPm));
        public static readonly DependencyProperty PropertyLargeurX =
            DependencyProperty.Register("P_LargeurX", typeof(double), typeof(TCube), new PropertyMetadata(1d, DefaultPm));
        public static readonly DependencyProperty PropertyHauteurY =
            DependencyProperty.Register("P_HauteurY", typeof(double), typeof(TCube), new PropertyMetadata(1d, DefaultPm));
        public static readonly DependencyProperty PropertyProfondeurZ =
            DependencyProperty.Register("P_ProfondeurZ", typeof(double), typeof(TCube), new PropertyMetadata(1d, DefaultPm));
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
        public double P_LargeurX
        {
            get
            {
                return (double)GetValue(PropertyLargeurX);
            }
            set
            {
                SetValue(PropertyLargeurX, value);
            }
        }
        public double P_HauteurY
        {
            get
            {
                return (double)GetValue(PropertyHauteurY);
            }
            set
            {
                SetValue(PropertyHauteurY, value);
            }
        }
        public double P_ProfondeurZ
        {
            get
            {
                return (double)GetValue(PropertyProfondeurZ);
            }
            set
            {
                SetValue(PropertyProfondeurZ, value);
            }
        }
        public Point3D P_PointDeGravite { get; set; }
        protected override void OnUpdateModel()
        {
            Model3D model = new GeometryModel3D(Maillage(P_PointDeReference, P_LargeurX, P_HauteurY, P_ProfondeurZ), GetMaterial());
            P_Model = model;
            base.OnUpdateModel();
            P_PointDeGravite = GenererPointGravite(BoiteEnglobante);
        }
        public void AppliquerTranslation(double transx, double transy, double transz)
        {
            Transform = new TranslateTransform3D(transx, transy, transz);
            InvalidateModel();
            Rect3D rect = BoiteEnglobante;
            Point3D point = rect.Location;
            point.Offset(transx, transy, transz);
            rect.Location = point;
            BoiteEnglobante = rect;
            P_PointDeGravite = GenererPointGravite(BoiteEnglobante);
        }
        public void ResetPosition()
        {
            Transform = new TranslateTransform3D(0d, 0d, 0d);
        }
        private MeshGeometry3D Maillage(Point3D toto, double long_x, double long_y, double long_z)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            Point3D pt_a = new Point3D(toto.X, toto.Y, toto.Z);
            Point3D pt_b = new Point3D(toto.X + long_x, toto.Y, toto.Z);
            Point3D pt_c = new Point3D(toto.X + long_x, toto.Y + long_y, toto.Z);
            Point3D pt_d = new Point3D(toto.X, toto.Y + long_y, toto.Z);
            Point3D pt_e = new Point3D(toto.X, toto.Y, toto.Z - long_z);
            Point3D pt_f = new Point3D(toto.X + long_x, toto.Y, toto.Z - long_z);
            Point3D pt_g = new Point3D(toto.X + long_x, toto.Y + long_y, toto.Z - long_z);
            Point3D pt_h = new Point3D(toto.X, toto.Y + long_y, toto.Z - long_z);
            //face gauche 4037
            maillage.Positions.Add(pt_e);
            maillage.Positions.Add(pt_a);
            maillage.Positions.Add(pt_d);
            maillage.Positions.Add(pt_h);
            maillage.TextureCoordinates.Add(new Point(0d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(0d, 1d / 3d));
            //face avant 0123
            maillage.Positions.Add(pt_a);
            maillage.Positions.Add(pt_b);
            maillage.Positions.Add(pt_c);
            maillage.Positions.Add(pt_d);
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 1d / 3d));
            //face droite 1562
            maillage.Positions.Add(pt_b);
            maillage.Positions.Add(pt_f);
            maillage.Positions.Add(pt_g);
            maillage.Positions.Add(pt_c);
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(3d / 4d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(3d / 4d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 1d / 3d));
            //face arriere 5276
            maillage.Positions.Add(pt_f);
            maillage.Positions.Add(pt_c);
            maillage.Positions.Add(pt_h);
            maillage.Positions.Add(pt_g);
            maillage.TextureCoordinates.Add(new Point(3d / 4d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(3d / 4d, 1d / 3d));
            //face dessus 3267
            maillage.Positions.Add(pt_d);
            maillage.Positions.Add(pt_c);
            maillage.Positions.Add(pt_g);
            maillage.Positions.Add(pt_h);
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 0d));
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 0d));
            //face dessous 4510
            maillage.Positions.Add(pt_e);
            maillage.Positions.Add(pt_f);
            maillage.Positions.Add(pt_b);
            maillage.Positions.Add(pt_a);
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 2d / 3d));
            GenererTrianglesIndice(maillage);
            return maillage;
        }
    }
}