using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Objects3D
{
    public class TRepere : UIElement3D
    {
        //la propriete Modele pour le repere
        private static readonly DependencyProperty ProprieteModele =
            DependencyProperty.Register("P_Modele",
                                        typeof(Model3D),
                                        typeof(TRepere),
                                        new PropertyMetadata(PM_Modele));
        private static void PM_Modele(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TRepere le_repere = (TRepere)d;
            le_repere.Visual3DModel = le_repere.P_Modele;
        }
        private Model3D P_Modele
        {
            get { return (Model3D)GetValue(ProprieteModele); }
            set { SetValue(ProprieteModele, value); }
        }
        //la propriete LongueurDemiAxe
        private static readonly DependencyProperty ProprieteLongueurDemiAxe =
           DependencyProperty.Register("P_LongueurDemiAxe",
                                       typeof(double),
                                       typeof(TRepere),
                                       new PropertyMetadata(1.0, PM_LongueurDemiAxe));
        private static void PM_LongueurDemiAxe(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TRepere le_repere = (TRepere)d;
            le_repere.InvalidateModel();
        }
        public double P_LongueurDemiAxe
        {
            get { return (double)GetValue(ProprieteLongueurDemiAxe); }
            set { SetValue(ProprieteLongueurDemiAxe, value); }
        }
        //la propriete EpaisseurAxe
        private static readonly DependencyProperty ProprieteEpaisseurAxe =
           DependencyProperty.Register("P_EpaisseurAxe",
                                       typeof(double),
                                       typeof(TRepere),
                                       new PropertyMetadata(0.01, PM_EpaisseurAxe));
        private static void PM_EpaisseurAxe(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TRepere le_repere = (TRepere)d;
            le_repere.InvalidateModel();
        }
        public double P_EpaisseurAxe
        {
            get { return (double)GetValue(ProprieteEpaisseurAxe); }
            set { SetValue(ProprieteEpaisseurAxe, value); }
        }
        //la propriete CouleurAxeX
        private static readonly DependencyProperty ProprieteCouleurAxeX =
           DependencyProperty.Register("P_CouleurAxeX",
                                       typeof(Color),
                                       typeof(TRepere),
                                       new PropertyMetadata(Colors.Red, PM_CouleurAxeX));
        private static void PM_CouleurAxeX(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TRepere le_repere = (TRepere)d;
            le_repere.InvalidateModel();
        }
        public Color P_CouleurAxeX
        {
            get { return (Color)GetValue(ProprieteCouleurAxeX); }
            set { SetValue(ProprieteCouleurAxeX, value); }
        }
        //la propriete CouleurAxeY
        private static readonly DependencyProperty ProprieteCouleurAxeY =
           DependencyProperty.Register("P_CouleurAxeY",
                                       typeof(Color),
                                       typeof(TRepere),
                                       new PropertyMetadata(Colors.Green, PM_CouleurAxeY));
        private static void PM_CouleurAxeY(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TRepere le_repere = (TRepere)d;
            le_repere.InvalidateModel();
        }
        public Color P_CouleurAxeY
        {
            get { return (Color)GetValue(ProprieteCouleurAxeY); }
            set { SetValue(ProprieteCouleurAxeY, value); }
        }
        //la propriete CouleurAxeZ
        private static readonly DependencyProperty ProprieteCouleurAxeZ =
           DependencyProperty.Register("P_CouleurAxeZ",
                                       typeof(Color),
                                       typeof(TRepere),
                                       new PropertyMetadata(Colors.Blue, PM_CouleurAxeZ));
        private static void PM_CouleurAxeZ(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TRepere le_repere = (TRepere)d;
            le_repere.InvalidateModel();
        }
        public Color P_CouleurAxeZ
        {
            get { return (Color)GetValue(ProprieteCouleurAxeZ); }
            set { SetValue(ProprieteCouleurAxeZ, value); }
        }
        //redefinition de la methode de rendu
        protected override void OnUpdateModel()
        {
            Model3DGroup groupe_axe = new Model3DGroup();
            groupe_axe.Children.Add(GenererMaillage(P_LongueurDemiAxe, "x"));
            groupe_axe.Children.Add(GenererMaillage(P_LongueurDemiAxe, "y"));
            groupe_axe.Children.Add(GenererMaillage(P_LongueurDemiAxe, "z"));
            P_Modele = groupe_axe;
        }
        //generer la geometrie du maillage
        private Model3DGroup GenererMaillage(double long_demi_axe, string type_axe)
        {
            double dist_x = 0;
            double dist_y = 0;
            double dist_z = 0;
            double prof = this.P_EpaisseurAxe;//0.01;
            Color couleur = Colors.Black;
            Model3DGroup un_axe = new Model3DGroup();
            switch (type_axe)
            {
                case "x":
                    dist_x = long_demi_axe;
                    dist_y = prof;
                    dist_z = prof;
                    couleur = this.P_CouleurAxeX;
                    break;
                case "y":
                    dist_x = prof;
                    dist_y = long_demi_axe;
                    dist_z = prof;
                    couleur = this.P_CouleurAxeY;
                    break;
                case "z":
                    dist_x = prof;
                    dist_y = prof;
                    dist_z = long_demi_axe;
                    couleur = this.P_CouleurAxeZ;
                    break;
            }
            Point3D pt1 = new Point3D(+dist_x, -dist_y, +dist_z); //+x -y +z
            Point3D pt2 = new Point3D(+dist_x, +dist_y, +dist_z); //+x +y +z
            Point3D pt3 = new Point3D(-dist_x, +dist_y, +dist_z); //-x +y +z
            Point3D pt4 = new Point3D(-dist_x, -dist_y, +dist_z); //-x -y +z
            Point3D pt5 = new Point3D(+dist_x, -dist_y, -dist_z); //+x -y -z
            Point3D pt6 = new Point3D(+dist_x, +dist_y, -dist_z); //+x +y -z
            Point3D pt7 = new Point3D(-dist_x, +dist_y, -dist_z); //-x +y -z
            Point3D pt8 = new Point3D(-dist_x, -dist_y, -dist_z); //-x -y -z
                                                                  //1234 avant
            GeometryModel3D face_avant = ModeliserFaceAxe(pt1, pt2, pt3, pt4, couleur);
            un_axe.Children.Add(face_avant);
            //5621 droite
            GeometryModel3D face_droite = ModeliserFaceAxe(pt5, pt6, pt2, pt1, couleur);
            un_axe.Children.Add(face_droite);
            //8765 arriere
            GeometryModel3D face_arriere = ModeliserFaceAxe(pt8, pt7, pt6, pt5, couleur);
            un_axe.Children.Add(face_arriere);
            //4378 gauche
            GeometryModel3D face_gauche = ModeliserFaceAxe(pt4, pt3, pt7, pt8, couleur);
            un_axe.Children.Add(face_gauche);
            //2673 dessus
            GeometryModel3D face_dessus = ModeliserFaceAxe(pt2, pt6, pt7, pt3, couleur);
            un_axe.Children.Add(face_dessus);
            //5148 dessous
            GeometryModel3D face_dessous = ModeliserFaceAxe(pt5, pt1, pt4, pt8, couleur);
            un_axe.Children.Add(face_dessous);
            return un_axe;
        }
        //face d'un axe
        private GeometryModel3D ModeliserFaceAxe(Point3D pt1, Point3D pt2, Point3D pt3, Point3D pt4, Color couleur)
        {
            GeometryModel3D face = new GeometryModel3D();
            MeshGeometry3D maillage = new MeshGeometry3D();
            maillage.Positions.Add(pt1);
            maillage.Positions.Add(pt2);
            maillage.Positions.Add(pt3);
            maillage.Positions.Add(pt4);
            maillage.TriangleIndices.Add(0);
            maillage.TriangleIndices.Add(1);
            maillage.TriangleIndices.Add(2);
            maillage.TriangleIndices.Add(2);
            maillage.TriangleIndices.Add(3);
            maillage.TriangleIndices.Add(0);
            maillage.TextureCoordinates.Add(new Point(1, 1));
            maillage.TextureCoordinates.Add(new Point(1, 0));
            maillage.TextureCoordinates.Add(new Point(0, 0));
            maillage.TextureCoordinates.Add(new Point(0, 1));
            maillage.Freeze();
            face.Geometry = maillage;
            face.Material = new DiffuseMaterial(new SolidColorBrush(couleur));
            face.BackMaterial = new DiffuseMaterial(new SolidColorBrush(couleur));
            return face;
        }


    }//end class
}
