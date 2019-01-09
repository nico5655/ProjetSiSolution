using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Objects3D
{
    public class TCubeDetailsFace : UIElement3D, IEnglobe
    {
        //la propriete Modele pour le cube
        private static readonly DependencyProperty ProprieteModele =
            DependencyProperty.Register("P_Modele",
                                        typeof(Model3D),
                                        typeof(TCubeDetailsFace),
                                        new PropertyMetadata(PM_Modele));
        private static void PM_Modele(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace cube = (TCubeDetailsFace)d;
            cube.Visual3DModel = cube.P_Modele;
        }
        private Model3D P_Modele
        {
            get { return (Model3D)GetValue(ProprieteModele); }
            set { SetValue(ProprieteModele, value); }
        }
        //la propriete PointReference (point de reference 3d pour le calcul) 
        private static readonly DependencyProperty ProprietePointReference =
           DependencyProperty.Register("P_PointReference",
                                       typeof(Point3D),
                                       typeof(TCubeDetailsFace),
                                       new PropertyMetadata(new Point3D(0, 0, 0), PM_PointReference));
        private static void PM_PointReference(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace affiche = (TCubeDetailsFace)d;
            affiche.InvalidateModel();
        }
        public Point3D P_PointReference
        {
            get { return (Point3D)GetValue(ProprietePointReference); }
            set { SetValue(ProprietePointReference, value); }
        }
        //la propriete Largeur (selon x)
        private static readonly DependencyProperty ProprieteLargeurX =
           DependencyProperty.Register("P_LargeurX",
                                       typeof(double),
                                       typeof(TCubeDetailsFace),
                                       new PropertyMetadata(1.0, PM_LargeurX));
        private static void PM_LargeurX(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace cube = (TCubeDetailsFace)d;
            cube.InvalidateModel();
        }
        public double P_LargeurX
        {
            get { return (double)GetValue(ProprieteLargeurX); }
            set { SetValue(ProprieteLargeurX, value); }
        }
        //la propriete Hauteur (selon y)
        private static readonly DependencyProperty ProprieteHauteurY =
           DependencyProperty.Register("P_HauteurY",
                                       typeof(double),
                                       typeof(TCubeDetailsFace),
                                       new PropertyMetadata(1.0, PM_HauteurY));
        private static void PM_HauteurY(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace cube = (TCubeDetailsFace)d;
            cube.InvalidateModel();
        }
        public double P_HauteurY
        {
            get { return (double)GetValue(ProprieteHauteurY); }
            set { SetValue(ProprieteHauteurY, value); }
        }
        //la propriete Profondeur (selon z)
        private static readonly DependencyProperty ProprieteProfondeurZ =
           DependencyProperty.Register("P_ProfondeurZ",
                                       typeof(double),
                                       typeof(TCubeDetailsFace),
                                       new PropertyMetadata(1.0, PM_ProfondeurZ));
        private static void PM_ProfondeurZ(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace cube = (TCubeDetailsFace)d;
            cube.InvalidateModel();
        }
        public double P_ProfondeurZ
        {
            get { return (double)GetValue(ProprieteProfondeurZ); }
            set { SetValue(ProprieteProfondeurZ, value); }
        }
        //la propriete CouleurUnie
        private static readonly DependencyProperty ProprieteCouleurUnie =
           DependencyProperty.Register("P_CouleurUnie",
                                       typeof(Color),
                                       typeof(TCubeDetailsFace),
                                       new PropertyMetadata(Colors.Red, PM_CouleurUnie));
        private static void PM_CouleurUnie(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace cube = (TCubeDetailsFace)d;
            cube.InvalidateModel();
        }
        public Color P_CouleurUnie
        {
            get { return (Color)GetValue(ProprieteCouleurUnie); }
            set { SetValue(ProprieteCouleurUnie, value); }
        }
        //la propriete Texture face avant
        private static readonly DependencyProperty ProprieteTextureFaceAvant =
           DependencyProperty.Register("P_TextureFaceAvant",
                                       typeof(Material),
                                       typeof(TCubeDetailsFace),
                                       new PropertyMetadata(null, PM_TextureFaceAvant));
        private static void PM_TextureFaceAvant(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace cube = (TCubeDetailsFace)d;
            cube.InvalidateModel();
        }
        public Material P_TextureFaceAvant
        {
            get { return (Material)GetValue(ProprieteTextureFaceAvant); }
            set { SetValue(ProprieteTextureFaceAvant, value); }
        }
        //la propriete Texture face gauche
        private static readonly DependencyProperty ProprieteTextureFaceGauche =
           DependencyProperty.Register("P_TextureFaceGauche",
                                       typeof(Material),
                                       typeof(TCubeDetailsFace),
                                       new PropertyMetadata(null, PM_TextureFaceGauche));
        private static void PM_TextureFaceGauche(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace cube = (TCubeDetailsFace)d;
            cube.InvalidateModel();
        }
        public Material P_TextureFaceGauche
        {
            get { return (Material)GetValue(ProprieteTextureFaceGauche); }
            set { SetValue(ProprieteTextureFaceGauche, value); }
        }
        //la propriete Texture face droite
        private static readonly DependencyProperty ProprieteTextureFaceDroite =
           DependencyProperty.Register("P_TextureFaceDroite",
                                       typeof(Material),
                                       typeof(TCubeDetailsFace),
                                       new PropertyMetadata(null, PM_TextureFaceDroite));
        private static void PM_TextureFaceDroite(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace cube = (TCubeDetailsFace)d;
            cube.InvalidateModel();
        }
        public Material P_TextureFaceDroite
        {
            get { return (Material)GetValue(ProprieteTextureFaceDroite); }
            set { SetValue(ProprieteTextureFaceDroite, value); }
        }
        //la propriete Texture face arriere
        private static readonly DependencyProperty ProprieteTextureFaceArriere =
           DependencyProperty.Register("P_TextureFaceArriere",
                                       typeof(Material),
                                       typeof(TCubeDetailsFace),
                                       new PropertyMetadata(null, PM_TextureFaceArriere));
        private static void PM_TextureFaceArriere(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace cube = (TCubeDetailsFace)d;
            cube.InvalidateModel();
        }
        public Material P_TextureFaceArriere
        {
            get { return (Material)GetValue(ProprieteTextureFaceArriere); }
            set { SetValue(ProprieteTextureFaceArriere, value); }
        }
        //la propriete Texture face dessus
        private static readonly DependencyProperty ProprieteTextureFaceDessus =
           DependencyProperty.Register("P_TextureFaceDessus",
                                       typeof(Material),
                                       typeof(TCubeDetailsFace),
                                       new PropertyMetadata(null, PM_TextureFaceDessus));
        private static void PM_TextureFaceDessus(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace cube = (TCubeDetailsFace)d;
            cube.InvalidateModel();
        }
        public Material P_TextureFaceDessus
        {
            get { return (Material)GetValue(ProprieteTextureFaceDessus); }
            set { SetValue(ProprieteTextureFaceDessus, value); }
        }
        //la propriete Texture face dessous
        private static readonly DependencyProperty ProprieteTextureFaceDessous =
           DependencyProperty.Register("P_TextureFaceDessous",
                                       typeof(Material),
                                       typeof(TCubeDetailsFace),
                                       new PropertyMetadata(null, PM_TextureFaceDessous));
        private static void PM_TextureFaceDessous(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCubeDetailsFace cube = (TCubeDetailsFace)d;
            cube.InvalidateModel();
        }
        public Material P_TextureFaceDessous
        {
            get { return (Material)GetValue(ProprieteTextureFaceDessous); }
            set { SetValue(ProprieteTextureFaceDessous, value); }
        }
        //champs
        private Rect3D v_boite_englobante;
        //proprietes
        public Rect3D BoiteEnglobante
        {
            get { return v_boite_englobante; }
            set { v_boite_englobante = value; }
        }
        //constructeur
        public TCubeDetailsFace() { }
        //redefinition de la methode de rendu
        protected override void OnUpdateModel()
        {
            Model3DGroup geo = new Model3DGroup();
            //face gauche
            GeometryModel3D geo_face_gauche = new GeometryModel3D();
            geo_face_gauche.Geometry = MaillageFaceGauche(this.P_PointReference, this.P_LargeurX, this.P_HauteurY, this.P_ProfondeurZ);
            if (this.P_TextureFaceGauche != null)
            {
                geo_face_gauche.Material = this.P_TextureFaceGauche;
            }
            else
            {
                geo_face_gauche.Material = new DiffuseMaterial(new SolidColorBrush(this.P_CouleurUnie));
            }
            geo_face_gauche.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            //face avant
            GeometryModel3D geo_face_avant = new GeometryModel3D();
            geo_face_avant.Geometry = MaillageFaceAvant(this.P_PointReference, this.P_LargeurX, this.P_HauteurY, this.P_ProfondeurZ);
            if (this.P_TextureFaceAvant != null)
            {
                geo_face_avant.Material = this.P_TextureFaceAvant;
            }
            else
            {
                geo_face_avant.Material = new DiffuseMaterial(new SolidColorBrush(this.P_CouleurUnie));
            }
            geo_face_avant.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            //face droite
            GeometryModel3D geo_face_droite = new GeometryModel3D();
            geo_face_droite.Geometry = MaillageFaceDroite(this.P_PointReference, this.P_LargeurX, this.P_HauteurY, this.P_ProfondeurZ);
            if (this.P_TextureFaceDroite != null)
            {
                geo_face_droite.Material = this.P_TextureFaceDroite;
            }
            else
            {
                geo_face_droite.Material = new DiffuseMaterial(new SolidColorBrush(this.P_CouleurUnie));
            }
            geo_face_droite.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            //face arriere
            GeometryModel3D geo_face_arriere = new GeometryModel3D();
            geo_face_arriere.Geometry = MaillageFaceArriere(this.P_PointReference, this.P_LargeurX, this.P_HauteurY, this.P_ProfondeurZ);
            if (this.P_TextureFaceArriere != null)
            {
                geo_face_arriere.Material = this.P_TextureFaceArriere;
            }
            else
            {
                geo_face_arriere.Material = new DiffuseMaterial(new SolidColorBrush(this.P_CouleurUnie));
            }
            geo_face_arriere.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            //face dessus
            GeometryModel3D geo_face_dessus = new GeometryModel3D();
            geo_face_dessus.Geometry = MaillageFaceDessus(this.P_PointReference, this.P_LargeurX, this.P_HauteurY, this.P_ProfondeurZ);
            if (this.P_TextureFaceDessus != null)
            {
                geo_face_dessus.Material = this.P_TextureFaceDessus;
            }
            else
            {
                geo_face_dessus.Material = new DiffuseMaterial(new SolidColorBrush(this.P_CouleurUnie));
            }
            geo_face_dessus.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            //face dessous
            GeometryModel3D geo_face_dessous = new GeometryModel3D();
            geo_face_dessous.Geometry = MaillageFaceDessous(this.P_PointReference, this.P_LargeurX, this.P_HauteurY, this.P_ProfondeurZ);
            if (this.P_TextureFaceDessous != null)
            {
                geo_face_dessous.Material = this.P_TextureFaceDessous;
            }
            else
            {
                geo_face_dessous.Material = new DiffuseMaterial(new SolidColorBrush(this.P_CouleurUnie));
            }
            geo_face_dessous.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            //composition du modele
            geo.Children.Add(geo_face_gauche);
            geo.Children.Add(geo_face_avant);
            geo.Children.Add(geo_face_droite);
            geo.Children.Add(geo_face_arriere);
            geo.Children.Add(geo_face_dessus);
            geo.Children.Add(geo_face_dessous);
            P_Modele = geo;
            v_boite_englobante = P_Modele.Bounds;
        }
        //maillage face gauche
        private MeshGeometry3D MaillageFaceGauche(Point3D point_ref, double long_x, double long_y, double long_z)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            Point3D pt_0 = new Point3D(point_ref.X, point_ref.Y, point_ref.Z);
            Point3D pt_3 = new Point3D(point_ref.X, point_ref.Y + long_y, point_ref.Z);
            Point3D pt_4 = new Point3D(point_ref.X, point_ref.Y, point_ref.Z - long_z);
            Point3D pt_7 = new Point3D(point_ref.X, point_ref.Y + long_y, point_ref.Z - long_z);
            //face gauche 4037
            maillage.Positions.Add(pt_4);
            maillage.Positions.Add(pt_0);
            maillage.Positions.Add(pt_3);
            maillage.Positions.Add(pt_7);
            maillage.TextureCoordinates.Add(new Point(0d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 0d));
            maillage.TextureCoordinates.Add(new Point(0d, 0d));
            GenererTriangleIndice(maillage);
            return maillage;
        }
        //maillage face avant
        private MeshGeometry3D MaillageFaceAvant(Point3D point_ref, double long_x, double long_y, double long_z)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            Point3D pt_0 = new Point3D(point_ref.X, point_ref.Y, point_ref.Z);
            Point3D pt_1 = new Point3D(point_ref.X + long_x, point_ref.Y, point_ref.Z);
            Point3D pt_2 = new Point3D(point_ref.X + long_x, point_ref.Y + long_y, point_ref.Z);
            Point3D pt_3 = new Point3D(point_ref.X, point_ref.Y + long_y, point_ref.Z);
            //face avant 0123
            maillage.Positions.Add(pt_0);
            maillage.Positions.Add(pt_1);
            maillage.Positions.Add(pt_2);
            maillage.Positions.Add(pt_3);
            maillage.TextureCoordinates.Add(new Point(0d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 0d));
            maillage.TextureCoordinates.Add(new Point(0d, 0d));
            GenererTriangleIndice(maillage);
            return maillage;
        }
        //maillage face droite
        private MeshGeometry3D MaillageFaceDroite(Point3D point_ref, double long_x, double long_y, double long_z)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            Point3D pt_1 = new Point3D(point_ref.X + long_x, point_ref.Y, point_ref.Z);
            Point3D pt_2 = new Point3D(point_ref.X + long_x, point_ref.Y + long_y, point_ref.Z);
            Point3D pt_5 = new Point3D(point_ref.X + long_x, point_ref.Y, point_ref.Z - long_z);
            Point3D pt_6 = new Point3D(point_ref.X + long_x, point_ref.Y + long_y, point_ref.Z - long_z);
            //face droite 1562
            maillage.Positions.Add(pt_1);
            maillage.Positions.Add(pt_5);
            maillage.Positions.Add(pt_6);
            maillage.Positions.Add(pt_2);
            maillage.TextureCoordinates.Add(new Point(0d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 0d));
            maillage.TextureCoordinates.Add(new Point(0d, 0d));
            GenererTriangleIndice(maillage);
            return maillage;
        }
        //maillage face arriere
        private MeshGeometry3D MaillageFaceArriere(Point3D point_ref, double long_x, double long_y, double long_z)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            Point3D pt_2 = new Point3D(point_ref.X + long_x, point_ref.Y + long_y, point_ref.Z);
            Point3D pt_5 = new Point3D(point_ref.X + long_x, point_ref.Y, point_ref.Z - long_z);
            Point3D pt_6 = new Point3D(point_ref.X + long_x, point_ref.Y + long_y, point_ref.Z - long_z);
            Point3D pt_7 = new Point3D(point_ref.X, point_ref.Y + long_y, point_ref.Z - long_z);
            //face arriere 5276
            maillage.Positions.Add(pt_5);
            maillage.Positions.Add(pt_2);
            maillage.Positions.Add(pt_7);
            maillage.Positions.Add(pt_6);
            maillage.TextureCoordinates.Add(new Point(0d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 0d));
            maillage.TextureCoordinates.Add(new Point(0d, 0d));
            GenererTriangleIndice(maillage);
            return maillage;
        }
        //maillage face dessus
        private MeshGeometry3D MaillageFaceDessus(Point3D point_ref, double long_x, double long_y, double long_z)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            Point3D pt_2 = new Point3D(point_ref.X + long_x, point_ref.Y + long_y, point_ref.Z);
            Point3D pt_3 = new Point3D(point_ref.X, point_ref.Y + long_y, point_ref.Z);
            Point3D pt_6 = new Point3D(point_ref.X + long_x, point_ref.Y + long_y, point_ref.Z - long_z);
            Point3D pt_7 = new Point3D(point_ref.X, point_ref.Y + long_y, point_ref.Z - long_z);
            //face dessus 3267
            maillage.Positions.Add(pt_3);
            maillage.Positions.Add(pt_2);
            maillage.Positions.Add(pt_6);
            maillage.Positions.Add(pt_7);
            maillage.TextureCoordinates.Add(new Point(0d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 0d));
            maillage.TextureCoordinates.Add(new Point(0d, 0d));
            GenererTriangleIndice(maillage);
            return maillage;
        }
        //maillage face dessous
        private MeshGeometry3D MaillageFaceDessous(Point3D point_ref, double long_x, double long_y, double long_z)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            Point3D pt_0 = new Point3D(point_ref.X, point_ref.Y, point_ref.Z);
            Point3D pt_1 = new Point3D(point_ref.X + long_x, point_ref.Y, point_ref.Z);
            Point3D pt_4 = new Point3D(point_ref.X, point_ref.Y, point_ref.Z - long_z);
            Point3D pt_5 = new Point3D(point_ref.X + long_x, point_ref.Y, point_ref.Z - long_z);
            //face dessous 4510
            maillage.Positions.Add(pt_4);
            maillage.Positions.Add(pt_5);
            maillage.Positions.Add(pt_1);
            maillage.Positions.Add(pt_0);
            maillage.TextureCoordinates.Add(new Point(0d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 1d));
            maillage.TextureCoordinates.Add(new Point(1d, 0d));
            maillage.TextureCoordinates.Add(new Point(0d, 0d));
            GenererTriangleIndice(maillage);
            return maillage;
        }
        //generer les indices pour la suite de triangles
        private void GenererTriangleIndice(MeshGeometry3D maillage)
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
    }//end class
}