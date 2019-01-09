using static System.Math;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Objects3D
{
    public abstract class TBaseModel : UIElement3D, IEnglobe
    {
        private Rect3D boiteEnglobante;
        protected void GenererTrianglesIndice(MeshGeometry3D maillage)
        {
            int depart = 0;
            for (int xx = 0; xx < maillage.Positions.Count; xx += 4)
            {
                maillage.TriangleIndices.Add(depart);
                maillage.TriangleIndices.Add(depart + 1);
                maillage.TriangleIndices.Add(depart + 2);
                maillage.TriangleIndices.Add(depart + 2);
                maillage.TriangleIndices.Add(depart + 3);
                maillage.TriangleIndices.Add(depart);
                depart += 4;
            }
        }
        protected Geometry3D MaillageCube(double largeurx, double hauteury, double profondeurz, Point3D point_ref)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            Point3D pt_0 = new Point3D(point_ref.X, point_ref.Y, point_ref.Z);
            Point3D pt_1 = new Point3D(point_ref.X + largeurx, point_ref.Y, point_ref.Z);
            Point3D pt_2 = new Point3D(point_ref.X + largeurx, point_ref.Y + hauteury, point_ref.Z);
            Point3D pt_3 = new Point3D(point_ref.X, point_ref.Y + hauteury, point_ref.Z);
            Point3D pt_4 = new Point3D(point_ref.X, point_ref.Y, point_ref.Z - profondeurz);
            Point3D pt_5 = new Point3D(point_ref.X + largeurx, point_ref.Y, point_ref.Z - profondeurz);
            Point3D pt_6 = new Point3D(point_ref.X + largeurx, point_ref.Y + hauteury, point_ref.Z - profondeurz);
            Point3D pt_7 = new Point3D(point_ref.X, point_ref.Y + hauteury, point_ref.Z - profondeurz);
            maillage.Positions.Add(pt_4);
            maillage.Positions.Add(pt_0);
            maillage.Positions.Add(pt_3);
            maillage.Positions.Add(pt_7);
            maillage.TextureCoordinates.Add(new Point(0d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(0d, 1d / 3d));
            maillage.Positions.Add(pt_0);
            maillage.Positions.Add(pt_1);
            maillage.Positions.Add(pt_2);
            maillage.Positions.Add(pt_3);
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 1d / 3d));
            maillage.Positions.Add(pt_1);
            maillage.Positions.Add(pt_5);
            maillage.Positions.Add(pt_6);
            maillage.Positions.Add(pt_2);
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(3d / 4d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(3d / 4d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 1d / 3d));
            maillage.Positions.Add(pt_5);
            maillage.Positions.Add(pt_2);
            maillage.Positions.Add(pt_7);
            maillage.Positions.Add(pt_6);
            maillage.TextureCoordinates.Add(new Point(3d / 4d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(3d / 4d, 1d / 3d));
            maillage.Positions.Add(pt_3);
            maillage.Positions.Add(pt_2);
            maillage.Positions.Add(pt_6);
            maillage.Positions.Add(pt_7);
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 1d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 0d));
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 0d));
            //face dessous 4510
            maillage.Positions.Add(pt_4);
            maillage.Positions.Add(pt_5);
            maillage.Positions.Add(pt_1);
            maillage.Positions.Add(pt_0);
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d / 2d, 2d / 3d));
            maillage.TextureCoordinates.Add(new Point(1d / 4d, 2d / 3d));
            GenererTrianglesIndice(maillage);
            return maillage;
        }
        protected Geometry3D MaillageRectangleVerticalPerp(double hauteur, double largeur, Point3D origine)
        {
            MeshGeometry3D result = new MeshGeometry3D();
            result.Positions.Add(new Point3D(origine.X, origine.Y + hauteur, origine.Z));
            result.Positions.Add(new Point3D(origine.X, origine.Y + hauteur, origine.Z + largeur));
            result.Positions.Add(new Point3D(origine.X, origine.Y, origine.Z + largeur));
            result.Positions.Add(origine);
            result.TextureCoordinates.Add(new Point(0, 1));
            result.TextureCoordinates.Add(new Point(1, 1));
            result.TextureCoordinates.Add(new Point(1, 0));
            result.TextureCoordinates.Add(new Point(0, 0));
            GenererTrianglesIndice(result);
            return result;
        }
        protected Geometry3D GenererRectangleHorizontal(double longueur, double largeur, Point3D origine)
        {
            MeshGeometry3D result = new MeshGeometry3D();
            result.Positions.Add(new Point3D(origine.X, origine.Y, origine.Z + longueur));
            result.Positions.Add(new Point3D(origine.X + largeur, origine.Y, origine.Z + longueur));
            result.Positions.Add(new Point3D(origine.X + largeur, origine.Y, origine.Z));
            result.Positions.Add(origine);
            result.TextureCoordinates.Add(new Point(0, 1));
            result.TextureCoordinates.Add(new Point(1, 1));
            result.TextureCoordinates.Add(new Point(1, 0));
            result.TextureCoordinates.Add(new Point(0, 0));
            GenererTrianglesIndice(result);
            return result;
        }
        protected virtual Material GetMaterial()
        {
            if (modeActuel == ModesAffichage.Uni)
            {
                return new DiffuseMaterial(new SolidColorBrush(P_CouleurUnie));
            }
            else
            {
                return P_Texture;
            }
        }
        protected Geometry3D MaillageRectangleVerticalParal(double hauteur, double largeur, Point3D origine)
        {
            MeshGeometry3D result = new MeshGeometry3D();
            result.Positions.Add(origine);
            result.Positions.Add(new Point3D(origine.X + largeur, origine.Y, origine.Z));
            result.Positions.Add(new Point3D(origine.X + largeur, origine.Y + hauteur, origine.Z));
            result.Positions.Add(new Point3D(origine.X, origine.Y + hauteur, origine.Z));
            result.TextureCoordinates.Add(new Point(0, 1));
            result.TextureCoordinates.Add(new Point(1, 1));
            result.TextureCoordinates.Add(new Point(1, 0));
            result.TextureCoordinates.Add(new Point(0, 0));
            GenererTrianglesIndice(result);
            return result;
        }
        protected Geometry3D MaillageCarreHorizontalSol(double longXpos, double longXneg, double longZpos, double longZneg, Point3D centre)
        {
            MeshGeometry3D result = new MeshGeometry3D();
            result.Positions.Add(new Point3D(centre.X + longXpos, centre.Y, centre.Z + longZpos));
            result.Positions.Add(new Point3D(centre.X + longXpos, centre.Y, centre.Z - longZneg));
            result.Positions.Add(new Point3D(centre.X - longXneg, centre.Y, centre.Z - longZneg));
            result.Positions.Add(new Point3D(centre.X - longXneg, centre.Y, centre.Z + longZpos));
            result.TextureCoordinates.Add(new Point(1d, 1d));
            result.TextureCoordinates.Add(new Point(1d, 0d));
            result.TextureCoordinates.Add(new Point(0d, 0d));
            result.TextureCoordinates.Add(new Point(0d, 1d));
            GenererTrianglesIndice(result);
            return result;
        }
        public TBaseModel() : base() { modeActuel = ModesAffichage.Uni; }
        public static DependencyProperty PropertyCouleurUnie =
            DependencyProperty.Register("P_CouleurUnie", typeof(Color), typeof(TBaseModel), new PropertyMetadata(PM_CouleurUnie));
        public static DependencyProperty PropertyTexture =
            DependencyProperty.Register("P_Texture", typeof(Material), typeof(TBaseModel), new PropertyMetadata(PM_Texture));
        public static readonly DependencyProperty PropertyModel =
            DependencyProperty.Register("P_Model", typeof(Model3D), typeof(TBaseModel), new PropertyMetadata(PM_Model));
        public Model3D P_Model
        {
            get
            {
                return (Model3D)GetValue(PropertyModel);
            }
            set
            {
                SetValue(PropertyModel, value);
            }
        }
        public Material P_Texture
        {
            get
            {
                return (Material)GetValue(PropertyTexture);
            }
            set
            {
                SetValue(PropertyTexture, value);
            }
        }
        public Color P_CouleurUnie
        {
            get
            {
                return (Color)GetValue(PropertyCouleurUnie);
            }
            set
            {
                SetValue(PropertyCouleurUnie, value);
            }
        }

        public Rect3D BoiteEnglobante
        {
            get
            {
                return boiteEnglobante;
            }

            set
            {
                boiteEnglobante = value;
            }
        }

        protected enum ModesAffichage { Texture, Uni }
        protected ModesAffichage modeActuel;
        private static void PM_Model(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TBaseModel).Visual3DModel = (d as TBaseModel).P_Model;
        }
        private static void PM_CouleurUnie(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TBaseModel m = d as TBaseModel;
            m.modeActuel = ModesAffichage.Uni;
            m.InvalidateModel();
        }
        private static void PM_Texture(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TBaseModel).Texturer((Material)e.NewValue);
        }
        public virtual void Texturer(Material texture)
        {
            modeActuel = ModesAffichage.Texture;
            P_Texture = texture;
            InvalidateModel();
        }
        protected override void OnUpdateModel()
        {
            BoiteEnglobante = P_Model.Bounds;
            base.OnUpdateModel();
        }
        public void Texturer(BitmapImage texture)
        {
            Texturer(new DiffuseMaterial(new ImageBrush(texture)));
        }
        protected static void DefaultPm(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TBaseModel)?.InvalidateModel();
        }
        protected Geometry3D GenererFaceLaterale(double hauteur, double rayon, int nbfacettes, double rayonHaut, Point3D centreBase)
        {
            MeshGeometry3D rtour = new MeshGeometry3D();
            double pasangle = 360d / nbfacettes;
            int facettes = 0;
            for (double angle = 0; angle < 360; angle += pasangle)
            {
                rtour.Positions.Add(new Point3D(centreBase.X + rayonHaut * Cos(angle * PI / 180), centreBase.Y + hauteur, centreBase.Z - rayonHaut * Sin(angle * PI / 180)));
                rtour.Positions.Add(new Point3D(centreBase.X + rayon * Cos(angle * PI / 180), centreBase.Y, centreBase.Z - rayon * Sin(angle * PI / 180)));
                rtour.Positions.Add(new Point3D(centreBase.X + rayon * Cos((angle + pasangle) * PI / 180), centreBase.Y, centreBase.Z - rayon * Sin((angle + pasangle) * PI / 180)));
                rtour.Positions.Add(new Point3D(centreBase.X + rayonHaut * Cos((angle + pasangle) * PI / 180), centreBase.Y + hauteur, centreBase.Z - rayonHaut * Sin((angle + pasangle) * PI / 180)));
                rtour.TextureCoordinates.Add(new Point((double)(facettes) / nbfacettes, 1));
                rtour.TextureCoordinates.Add(new Point((double)(facettes) / nbfacettes, 0));
                rtour.TextureCoordinates.Add(new Point((double)(facettes + 1) / nbfacettes, 0));
                rtour.TextureCoordinates.Add(new Point((double)(facettes + 1) / nbfacettes, 1));
                facettes++;
            }
            GenererTrianglesIndice(rtour);
            return rtour;
        }
        protected Geometry3D GenererSphere(Point3D centre, double rayon, double rayonvertical, int nbDeFacettesHor, int nbDeFacettesVert)
        {
            MeshGeometry3D result = new MeshGeometry3D();
            double pasAngleTeta = 360d / nbDeFacettesHor;
            double pasAngleBeta = 180d / nbDeFacettesVert;
            int v = 0;
            int h = 0;
            for (double angeTeta = 0; angeTeta < 360; angeTeta += pasAngleTeta)
            {
                for (double angleBera = 90; angleBera > -90; angleBera -= pasAngleBeta)
                {
                    result.Positions.Add(new Point3D(centre.X + rayon * Cos(angleBera * PI / 180) * Cos(angeTeta * PI / 180), centre.Y + rayonvertical * Sin(angleBera * PI / 180), centre.Z - rayon * Cos(angleBera * PI / 180) * Sin(angeTeta * PI / 180)));
                    result.Positions.Add(new Point3D(centre.X + rayon * Cos((angleBera - pasAngleBeta) * PI / 180) * Cos(angeTeta * PI / 180), centre.Y + rayonvertical * Sin((angleBera - pasAngleBeta) * PI / 180), centre.Z - rayon * Cos((angleBera - pasAngleBeta) * PI / 180) * Sin(angeTeta * PI / 180)));
                    result.Positions.Add(new Point3D(centre.X + rayon * Cos((angleBera - pasAngleBeta) * PI / 180) * Cos((angeTeta + pasAngleTeta) * PI / 180), centre.Y + rayonvertical * Sin((angleBera - pasAngleBeta) * PI / 180), centre.Z - rayon * Cos((angleBera - pasAngleBeta) * PI / 180) * Sin((angeTeta + pasAngleTeta) * PI / 180)));
                    result.Positions.Add(new Point3D(centre.X + rayon * Cos(angleBera * PI / 180) * Cos((angeTeta + pasAngleTeta) * PI / 180), centre.Y + rayonvertical * Sin(angleBera * PI / 180), centre.Z - rayon * Cos(angleBera * PI / 180) * Sin((angeTeta + pasAngleTeta) * PI / 180)));
                    result.TextureCoordinates.Add(new Point((double)h / nbDeFacettesHor, (double)v / nbDeFacettesVert));
                    result.TextureCoordinates.Add(new Point((double)h / nbDeFacettesHor, (double)(v + 1) / nbDeFacettesVert));
                    result.TextureCoordinates.Add(new Point((double)(h + 1) / nbDeFacettesHor, (double)(v + 1) / nbDeFacettesVert));
                    result.TextureCoordinates.Add(new Point((double)(h + 1) / nbDeFacettesHor, (double)v / nbDeFacettesVert));
                    v++;
                }
                v = 0;
                h++;
            }
            GenererTrianglesIndice(result);
            return result;
        }

        protected Geometry3D GenererFacePlane(double Gr, double Pr, int nbfacettes, Point3D centre)
        {
            MeshGeometry3D rtour = new MeshGeometry3D();
            double pasangle = 360d / nbfacettes;
            int facettes = 0;
            for (double angle = 0; angle < 360; angle += pasangle)
            {
                rtour.Positions.Add(new Point3D(centre.X + Pr * Cos(angle * PI / 180), centre.Y, centre.Z - Pr * Sin(angle * PI / 180)));
                rtour.Positions.Add(new Point3D(centre.X + Gr * Cos(angle * PI / 180), centre.Y, centre.Z - Gr * Sin(angle * PI / 180)));
                rtour.Positions.Add(new Point3D(centre.X + Gr * Cos((angle + pasangle) * PI / 180), centre.Y, centre.Z - Gr * Sin((angle + pasangle) * PI / 180)));
                rtour.Positions.Add(new Point3D(centre.X + Pr * Cos((angle + pasangle) * PI / 180), centre.Y, centre.Z - Pr * Sin((angle + pasangle) * PI / 180)));
                rtour.TextureCoordinates.Add(new Point((double)(facettes) / nbfacettes, 0));
                rtour.TextureCoordinates.Add(new Point((double)(facettes + 1) / nbfacettes, 0));
                rtour.TextureCoordinates.Add(new Point((double)(facettes) / nbfacettes, 1));
                rtour.TextureCoordinates.Add(new Point((double)(facettes + 1) / nbfacettes, 1));
                facettes++;
            }
            GenererTrianglesIndice(rtour);
            return rtour;
        }
        protected Point3D GenererPointGravite(Rect3D boiteEnglobante)
        {
            return new Point3D(
                boiteEnglobante.Location.X + (boiteEnglobante.SizeX / 2d),
                boiteEnglobante.Location.Y + (boiteEnglobante.SizeY / 2d),
                boiteEnglobante.Location.Z + (boiteEnglobante.SizeZ / 2d)
                );
        }
    }
}
