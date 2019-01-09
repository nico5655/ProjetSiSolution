using static System.Math;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Objects3D
{
    public abstract class TBaseModel : UIElement3D
    {
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

        public Rect3D BoiteEnglobante { get; set; }

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
