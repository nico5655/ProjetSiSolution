using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Objects3D
{
    public class TCone : UIElement3D
    {
        //la propriete Modele pour le cube
        private static readonly DependencyProperty ProprieteModele =
            DependencyProperty.Register("P_Modele",
                                        typeof(Model3D),
                                        typeof(TCone),
                                        new PropertyMetadata(PM_Modele));
        private static void PM_Modele(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCone cylindre = (TCone)d;
            cylindre.Visual3DModel = cylindre.P_Modele;
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
                                       typeof(TCone),
                                       new PropertyMetadata(new Point3D(0, 0, 0), PM_PointReference));
        private static void PM_PointReference(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCone cylindre = (TCone)d;
            cylindre.InvalidateModel();
        }
        public Point3D P_PointReference
        {
            get { return (Point3D)GetValue(ProprietePointReference); }
            set { SetValue(ProprietePointReference, value); }
        }
        //la propriete Hauteur (selon Y)
        private static readonly DependencyProperty ProprieteHauteur =
           DependencyProperty.Register("P_Hauteur",
                                       typeof(double),
                                       typeof(TCone),
                                       new PropertyMetadata(1.0, PM_Hauteur));
        private static void PM_Hauteur(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCone cylindre = (TCone)d;
            cylindre.InvalidateModel();
        }
        public double P_Hauteur
        {
            get { return (double)GetValue(ProprieteHauteur); }
            set { SetValue(ProprieteHauteur, value); }
        }
        //la propriete RayonExterne
        private static readonly DependencyProperty ProprieteRayonExterne =
           DependencyProperty.Register("P_RayonExterne",
                                       typeof(double),
                                       typeof(TCone),
                                       new PropertyMetadata(1.0, PM_RayonExterne));
        private static void PM_RayonExterne(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCone cylindre = (TCone)d;
            cylindre.InvalidateModel();
        }
        public double P_RayonExterne
        {
            get { return (double)GetValue(ProprieteRayonExterne); }
            set { SetValue(ProprieteRayonExterne, value); }
        }
        //la propriete NombreDivision
        private static readonly DependencyProperty ProprieteNombreDivision =
           DependencyProperty.Register("P_NombreDivision",
                                       typeof(int),
                                       typeof(TCone),
                                       new PropertyMetadata(40, PM_NombreDivision));
        private static void PM_NombreDivision(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCone cylindre = (TCone)d;
            cylindre.InvalidateModel();
        }
        public int P_NombreDivision
        {
            get { return (int)GetValue(ProprieteNombreDivision); }
            set { SetValue(ProprieteNombreDivision, value); }
        }
        //la propriete CouleurDegradeFaceExterne
        private static readonly DependencyProperty ProprieteCouleurDegradeFaceExterne =
           DependencyProperty.Register("P_CouleurDegradeFaceExterne",
                                       typeof(LinearGradientBrush),
                                       typeof(TCone),
                                       new PropertyMetadata(new LinearGradientBrush(Colors.Blue, Colors.LightYellow, 0), PM_CouleurDegradeFaceExterne));
        private static void PM_CouleurDegradeFaceExterne(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCone cylindre = (TCone)d;
            cylindre.InvalidateModel();
        }
        public LinearGradientBrush P_CouleurDegradeFaceExterne
        {
            get { return (LinearGradientBrush)GetValue(ProprieteCouleurDegradeFaceExterne); }
            set { SetValue(ProprieteCouleurDegradeFaceExterne, value); }
        }
        //la propriete CouleurUnieFaceDessous
        private static readonly DependencyProperty ProprieteCouleurUnieFaceDessous =
           DependencyProperty.Register("P_CouleurUnieFaceDessous",
                                       typeof(Color),
                                       typeof(TCone),
                                       new PropertyMetadata(Colors.Red, PM_CouleurUnieFaceDessous));
        private static void PM_CouleurUnieFaceDessous(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCone cylindre = (TCone)d;
            cylindre.InvalidateModel();
        }
        public Color P_CouleurUnieFaceDessous
        {
            get { return (Color)GetValue(ProprieteCouleurUnieFaceDessous); }
            set { SetValue(ProprieteCouleurUnieFaceDessous, value); }
        }
        //constructeur
        public TCone()
        {
        }
        //la propriete TextureFaceExterne
        private static readonly DependencyProperty ProprieteTextureFaceExterne =
           DependencyProperty.Register("P_TextureFaceExterne",
                                       typeof(Material),
                                       typeof(TCone),
                                       new PropertyMetadata(null, PM_TextureFaceExterne));
        private static void PM_TextureFaceExterne(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCone cylindre = (TCone)d;
            cylindre.InvalidateModel();
        }
        public Material P_TextureFaceExterne
        {
            get { return (Material)GetValue(ProprieteTextureFaceExterne); }
            set { SetValue(ProprieteTextureFaceExterne, value); }
        }
        //la propriete TextureFaceDessous
        private static readonly DependencyProperty ProprieteTextureFaceDessous =
           DependencyProperty.Register("P_TextureFaceDessous",
                                       typeof(Material),
                                       typeof(TCone),
                                       new PropertyMetadata(null, PM_TextureFaceDessous));
        private static void PM_TextureFaceDessous(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCone cylindre = (TCone)d;
            cylindre.InvalidateModel();
        }
        public Material P_TextureFaceDessous
        {
            get { return (Material)GetValue(ProprieteTextureFaceDessous); }
            set { SetValue(ProprieteTextureFaceDessous, value); }
        }
        //redefinition de la methode de rendu
        protected override void OnUpdateModel()
        {
            Model3DGroup gp_geo_cone = new Model3DGroup();
            //face exterieur
            GeometryModel3D geo_cone_exterieur = new GeometryModel3D();
            geo_cone_exterieur.Geometry = MaillageConeFaceExterne(this.P_PointReference, this.P_Hauteur, this.P_RayonExterne, this.P_NombreDivision);
            if (this.P_TextureFaceExterne != null)
            {
                geo_cone_exterieur.Material = this.P_TextureFaceExterne;
            }
            else
            {
                geo_cone_exterieur.Material = new DiffuseMaterial(this.P_CouleurDegradeFaceExterne);
            }
            geo_cone_exterieur.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            gp_geo_cone.Children.Add(geo_cone_exterieur);
            //face dessous
            GeometryModel3D geo_cone_dessous = new GeometryModel3D();
            geo_cone_dessous.Geometry = MaillageConeFaceDessous(this.P_PointReference, this.P_RayonExterne, this.P_NombreDivision);
            if (this.P_TextureFaceDessous != null)
            {
                geo_cone_dessous.Material = this.P_TextureFaceDessous;
            }
            else
            {
                geo_cone_dessous.Material = new DiffuseMaterial(new SolidColorBrush(this.P_CouleurUnieFaceDessous));
            }
            geo_cone_dessous.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            gp_geo_cone.Children.Add(geo_cone_dessous);
            //
            P_Modele = gp_geo_cone;
        }
        //modeliser la geometrie de la face externe
        private Geometry3D MaillageConeFaceExterne(Point3D pt_ref, double hauteur, double ray_ext, int nb_div)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            double pas_angle = 360d / nb_div;
            int cpt_facette = 0;
            for (double angle = 0; angle < 360; angle += pas_angle)
            {
                Point3D pt_a = new Point3D();
                pt_a.X = pt_ref.X;
                pt_a.Y = pt_ref.Y + hauteur;
                pt_a.Z = pt_ref.Z;
                Point3D pt_b = new Point3D();
                pt_b.X = pt_ref.X + ray_ext * Math.Cos(angle * Math.PI / 180);
                pt_b.Y = pt_ref.Y + 0;
                pt_b.Z = pt_ref.Z - ray_ext * Math.Sin(angle * Math.PI / 180);
                Point3D pt_c = new Point3D();
                pt_c.X = pt_ref.X + ray_ext * Math.Cos((angle + pas_angle) * Math.PI / 180);
                pt_c.Y = pt_ref.Y + 0;
                pt_c.Z = pt_ref.Z - ray_ext * Math.Sin((angle + pas_angle) * Math.PI / 180);
                Point3D pt_d = new Point3D();
                pt_d.X = pt_ref.X;
                pt_d.Y = pt_ref.Y + hauteur;
                pt_d.Z = pt_ref.Z;
                maillage.Positions.Add(pt_a);
                maillage.Positions.Add(pt_b);
                maillage.Positions.Add(pt_c);
                maillage.Positions.Add(pt_d);
                Point pt_hg = new Point((double)cpt_facette / (double)nb_div, 0);
                Point pt_hd = new Point((double)(cpt_facette + 1) / (double)nb_div, 0);
                Point pt_bg = new Point((double)cpt_facette / (double)nb_div, 1);
                Point pt_bd = new Point((double)(cpt_facette + 1) / (double)nb_div, 1);
                maillage.TextureCoordinates.Add(pt_hg);
                maillage.TextureCoordinates.Add(pt_bg);
                maillage.TextureCoordinates.Add(pt_bd);
                maillage.TextureCoordinates.Add(pt_hd);
                cpt_facette++;
            }
            //
            GenererTriangleIndice(maillage);
            return maillage;
        }
        //modeliser la geometrie de la face du dessous
        private Geometry3D MaillageConeFaceDessous(Point3D pt_ref, double ray_ext, int nb_div)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            double pas_angle = 360d / nb_div;
            int cpt_facette = 0;
            for (double angle = 0; angle < 360; angle += pas_angle)
            {
                Point3D pt_b = new Point3D();
                pt_b.X = pt_ref.X + ray_ext * Math.Cos(angle * Math.PI / 180);
                pt_b.Y = pt_ref.Y + 0;
                pt_b.Z = pt_ref.Z - ray_ext * Math.Sin(angle * Math.PI / 180);
                Point3D pt_g = new Point3D();
                pt_g.X = pt_ref.X;
                pt_g.Y = pt_ref.Y + 0;
                pt_g.Z = pt_ref.Z;
                Point3D pt_f = new Point3D();
                pt_f.X = pt_ref.X;
                pt_f.Y = pt_ref.Y + 0;
                pt_f.Z = pt_ref.Z;
                Point3D pt_c = new Point3D();
                pt_c.X = pt_ref.X + ray_ext * Math.Cos((angle + pas_angle) * Math.PI / 180);
                pt_c.Y = pt_ref.Y + 0;
                pt_c.Z = pt_ref.Z - ray_ext * Math.Sin((angle + pas_angle) * Math.PI / 180);
                maillage.Positions.Add(pt_b);
                maillage.Positions.Add(pt_g);
                maillage.Positions.Add(pt_f);
                maillage.Positions.Add(pt_c);
                Point pt_hg = new Point((double)cpt_facette / (double)nb_div, 0);
                Point pt_hd = new Point((double)(cpt_facette + 1) / (double)nb_div, 0);
                Point pt_bg = new Point((double)cpt_facette / (double)nb_div, 1);
                Point pt_bd = new Point((double)(cpt_facette + 1) / (double)nb_div, 1);
                maillage.TextureCoordinates.Add(pt_hg);
                maillage.TextureCoordinates.Add(pt_bg);
                maillage.TextureCoordinates.Add(pt_bd);
                maillage.TextureCoordinates.Add(pt_hd);
                cpt_facette++;
            }
            //
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