using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Objects3D
{
    public class TCylindre : UIElement3D
    {
        //la propriete Modele pour le cylindre
        private static readonly DependencyProperty ProprieteModele =
            DependencyProperty.Register("P_Modele",
                                        typeof(Model3D),
                                        typeof(TCylindre),
                                        new PropertyMetadata(PM_Modele));
        private static void PM_Modele(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
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
                                       typeof(TCylindre),
                                       new PropertyMetadata(new Point3D(0, 0, 0), PM_PointReference));
        private static void PM_PointReference(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
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
                                       typeof(TCylindre),
                                       new PropertyMetadata(1.0, PM_Hauteur));
        private static void PM_Hauteur(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
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
                                       typeof(TCylindre),
                                       new PropertyMetadata(1.0, PM_RayonExterne));
        private static void PM_RayonExterne(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
            cylindre.InvalidateModel();
        }
        public double P_RayonExterne
        {
            get { return (double)GetValue(ProprieteRayonExterne); }
            set { SetValue(ProprieteRayonExterne, value); }
        }
        //la propriete RayonInterne
        private static readonly DependencyProperty ProprieteRayonInterne =
           DependencyProperty.Register("P_RayonInterne",
                                       typeof(double),
                                       typeof(TCylindre),
                                       new PropertyMetadata(0.5d, PM_RayonInterne));
        private static void PM_RayonInterne(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
            cylindre.InvalidateModel();
        }
        public double P_RayonInterne
        {
            get { return (double)GetValue(ProprieteRayonInterne); }
            set { SetValue(ProprieteRayonInterne, value); }
        }
        //la propriete NombreDivision
        private static readonly DependencyProperty ProprieteNombreDivision =
           DependencyProperty.Register("P_NombreDivision",
                                       typeof(int),
                                       typeof(TCylindre),
                                       new PropertyMetadata(40, PM_NombreDivision));
        private static void PM_NombreDivision(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
            cylindre.InvalidateModel();
        }
        public int P_NombreDivision
        {
            get { return (int)GetValue(ProprieteNombreDivision); }
            set { SetValue(ProprieteNombreDivision, value); }
        }
        //la propriete CouleurUnieFaceExterne
        private static readonly DependencyProperty ProprieteCouleurUnieFaceExterne =
           DependencyProperty.Register("P_CouleurUnieFaceExterne",
                                       typeof(Color),
                                       typeof(TCylindre),
                                       new PropertyMetadata(Colors.Blue, PM_CouleurUnieFaceExterne));
        private static void PM_CouleurUnieFaceExterne(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
            cylindre.InvalidateModel();
        }
        public Color P_CouleurUnieFaceExterne
        {
            get { return (Color)GetValue(ProprieteCouleurUnieFaceExterne); }
            set { SetValue(ProprieteCouleurUnieFaceExterne, value); }
        }
        //la propriete CouleurUnieFaceDessus
        private static readonly DependencyProperty ProprieteCouleurUnieFaceDessus =
           DependencyProperty.Register("P_CouleurUnieFaceDessus",
                                       typeof(Color),
                                       typeof(TCylindre),
                                       new PropertyMetadata(Colors.Red, PM_CouleurUnieFaceDessus));
        private static void PM_CouleurUnieFaceDessus(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
            cylindre.InvalidateModel();
        }
        public Color P_CouleurUnieFaceDessus
        {
            get { return (Color)GetValue(ProprieteCouleurUnieFaceDessus); }
            set { SetValue(ProprieteCouleurUnieFaceDessus, value); }
        }
        //la propriete CouleurUnieFaceDessous
        private static readonly DependencyProperty ProprieteCouleurUnieFaceDessous =
           DependencyProperty.Register("P_CouleurUnieFaceDessous",
                                       typeof(Color),
                                       typeof(TCylindre),
                                       new PropertyMetadata(Colors.Red, PM_CouleurUnieFaceDessous));
        private static void PM_CouleurUnieFaceDessous(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
            cylindre.InvalidateModel();
        }
        public Color P_CouleurUnieFaceDessous
        {
            get { return (Color)GetValue(ProprieteCouleurUnieFaceDessous); }
            set { SetValue(ProprieteCouleurUnieFaceDessous, value); }
        }
        //la propriete CouleurUnieFaceInterne
        private static readonly DependencyProperty ProprieteCouleurUnieFaceInterne =
           DependencyProperty.Register("P_CouleurUnieFaceInterne",
                                       typeof(Color),
                                       typeof(TCylindre),
                                       new PropertyMetadata(Colors.Orange, PM_CouleurUnieFaceInterne));
        private static void PM_CouleurUnieFaceInterne(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
            cylindre.InvalidateModel();
        }
        public Color P_CouleurUnieFaceInterne
        {
            get { return (Color)GetValue(ProprieteCouleurUnieFaceInterne); }
            set { SetValue(ProprieteCouleurUnieFaceInterne, value); }
        }

        //constructeur
        public TCylindre()
        {
        }
        //la propriete TextureFaceExterne
        private static readonly DependencyProperty ProprieteTextureFaceExterne =
           DependencyProperty.Register("P_TextureFaceExterne",
                                       typeof(Material),
                                       typeof(TCylindre),
                                       new PropertyMetadata(null, PM_TextureFaceExterne));
        private static void PM_TextureFaceExterne(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
            cylindre.InvalidateModel();
        }
        public Material P_TextureFaceExterne
        {
            get { return (Material)GetValue(ProprieteTextureFaceExterne); }
            set { SetValue(ProprieteTextureFaceExterne, value); }
        }
        //la propriete TextureFaceDessus
        private static readonly DependencyProperty ProprieteTextureFaceDessus =
           DependencyProperty.Register("P_TextureFaceDessus",
                                       typeof(Material),
                                       typeof(TCylindre),
                                       new PropertyMetadata(null, PM_TextureFaceDessus));
        private static void PM_TextureFaceDessus(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
            cylindre.InvalidateModel();
        }
        public Material P_TextureFaceDessus
        {
            get { return (Material)GetValue(ProprieteTextureFaceDessus); }
            set { SetValue(ProprieteTextureFaceDessus, value); }
        }
        //la propriete TextureFaceDessous
        private static readonly DependencyProperty ProprieteTextureFaceDessous =
           DependencyProperty.Register("P_TextureFaceDessous",
                                       typeof(Material),
                                       typeof(TCylindre),
                                       new PropertyMetadata(null, PM_TextureFaceDessous));
        private static void PM_TextureFaceDessous(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
            cylindre.InvalidateModel();
        }
        public Material P_TextureFaceDessous
        {
            get { return (Material)GetValue(ProprieteTextureFaceDessous); }
            set { SetValue(ProprieteTextureFaceDessous, value); }
        }
        //la propriete TextureFaceInterne
        private static readonly DependencyProperty ProprieteTextureFaceInterne =
           DependencyProperty.Register("P_TextureFaceInterne",
                                       typeof(Material),
                                       typeof(TCylindre),
                                       new PropertyMetadata(null, PM_TextureFaceInterne));
        private static void PM_TextureFaceInterne(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TCylindre cylindre = (TCylindre)d;
            cylindre.InvalidateModel();
        }
        public Material P_TextureFaceInterne
        {
            get { return (Material)GetValue(ProprieteTextureFaceInterne); }
            set { SetValue(ProprieteTextureFaceInterne, value); }
        }
        //redefinition de la methode de rendu
        protected override void OnUpdateModel()
        {
            Model3DGroup gp_geo_cyl = new Model3DGroup();
            //face exterieur
            GeometryModel3D geo_cyl_exterieur = new GeometryModel3D();
            geo_cyl_exterieur.Geometry = MaillageCylindreFaceExterne(this.P_PointReference, this.P_Hauteur, this.P_RayonExterne, this.P_NombreDivision);
            if (this.P_TextureFaceExterne != null)
            {
                geo_cyl_exterieur.Material = this.P_TextureFaceExterne;
            }
            else
            {
                geo_cyl_exterieur.Material = new DiffuseMaterial(new SolidColorBrush(this.P_CouleurUnieFaceExterne));
            }
            geo_cyl_exterieur.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            gp_geo_cyl.Children.Add(geo_cyl_exterieur);
            //face dessus
            GeometryModel3D geo_cyl_dessus = new GeometryModel3D();
            geo_cyl_dessus.Geometry = MaillageCylindreFaceDessus(this.P_PointReference, this.P_Hauteur, this.P_RayonExterne, this.P_RayonInterne, this.P_NombreDivision);
            if (this.P_TextureFaceDessus != null)
            {
                geo_cyl_dessus.Material = this.P_TextureFaceDessus;
            }
            else
            {
                geo_cyl_dessus.Material = new DiffuseMaterial(new SolidColorBrush(this.P_CouleurUnieFaceDessus));
            }
            geo_cyl_dessus.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            gp_geo_cyl.Children.Add(geo_cyl_dessus);
            //face dessous
            GeometryModel3D geo_cyl_dessous = new GeometryModel3D();
            geo_cyl_dessous.Geometry = MaillageCylindreFaceDessous(this.P_PointReference, this.P_RayonExterne, this.P_RayonInterne, this.P_NombreDivision);
            if (this.P_TextureFaceDessous != null)
            {
                geo_cyl_dessous.Material = this.P_TextureFaceDessous;
            }
            else
            {
                geo_cyl_dessous.Material = new DiffuseMaterial(new SolidColorBrush(this.P_CouleurUnieFaceDessous));
            }
            geo_cyl_dessous.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            gp_geo_cyl.Children.Add(geo_cyl_dessous);
            //face interne
            GeometryModel3D geo_cyl_interieur = new GeometryModel3D();
            geo_cyl_interieur.Geometry = MaillageCylindreFaceInterne(this.P_PointReference, this.P_Hauteur, this.P_RayonInterne, this.P_NombreDivision);
            if (this.P_TextureFaceDessous != null)
            {
                geo_cyl_interieur.Material = this.P_TextureFaceDessous;
            }
            else
            {
                geo_cyl_interieur.Material = new DiffuseMaterial(new SolidColorBrush(this.P_CouleurUnieFaceInterne));
            }
            geo_cyl_interieur.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
            gp_geo_cyl.Children.Add(geo_cyl_interieur);
            //
            P_Modele = gp_geo_cyl;
        }
        //modeliser la geometrie de la face externe
        private Geometry3D MaillageCylindreFaceExterne(Point3D pt_ref, double hauteur, double ray_ext, int nb_div)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            double pas_angle = 360d / nb_div;
            int cpt_facette = 0;
            for (double angle = 0; angle < 360; angle += pas_angle)
            {
                Point3D pt_a = new Point3D();
                pt_a.X = pt_ref.X + ray_ext * Math.Cos(angle * Math.PI / 180);
                pt_a.Y = pt_ref.Y + hauteur;
                pt_a.Z = pt_ref.Z - ray_ext * Math.Sin(angle * Math.PI / 180);
                Point3D pt_b = new Point3D();
                pt_b.X = pt_ref.X + ray_ext * Math.Cos(angle * Math.PI / 180);
                pt_b.Y = pt_ref.Y + 0;
                pt_b.Z = pt_ref.Z - ray_ext * Math.Sin(angle * Math.PI / 180);
                Point3D pt_c = new Point3D();
                pt_c.X = pt_ref.X + ray_ext * Math.Cos((angle + pas_angle) * Math.PI / 180);
                pt_c.Y = pt_ref.Y + 0;
                pt_c.Z = pt_ref.Z - ray_ext * Math.Sin((angle + pas_angle) * Math.PI / 180);
                Point3D pt_d = new Point3D();
                pt_d.X = pt_ref.X + ray_ext * Math.Cos((angle + pas_angle) * Math.PI / 180);
                pt_d.Y = pt_ref.Y + hauteur;
                pt_d.Z = pt_ref.Z - ray_ext * Math.Sin((angle + pas_angle) * Math.PI / 180);
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
        //modeliser la geometrie de la face du dessus
        private Geometry3D MaillageCylindreFaceDessus(Point3D pt_ref, double hauteur, double ray_ext, double ray_int, int nb_div)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            double pas_angle = 360d / nb_div;
            int cpt_facette = 0;
            for (double angle = 0; angle < 360; angle += pas_angle)
            {
                Point3D pt_h = new Point3D();
                pt_h.X = pt_ref.X + ray_int * Math.Cos(angle * Math.PI / 180);
                pt_h.Y = pt_ref.Y + hauteur;
                pt_h.Z = pt_ref.Z - ray_int * Math.Sin(angle * Math.PI / 180);
                Point3D pt_a = new Point3D();
                pt_a.X = pt_ref.X + ray_ext * Math.Cos(angle * Math.PI / 180);
                pt_a.Y = pt_ref.Y + hauteur;
                pt_a.Z = pt_ref.Z - ray_ext * Math.Sin(angle * Math.PI / 180);
                Point3D pt_d = new Point3D();
                pt_d.X = pt_ref.X + ray_ext * Math.Cos((angle + pas_angle) * Math.PI / 180);
                pt_d.Y = pt_ref.Y + hauteur;
                pt_d.Z = pt_ref.Z - ray_ext * Math.Sin((angle + pas_angle) * Math.PI / 180);
                Point3D pt_e = new Point3D();
                pt_e.X = pt_ref.X + ray_int * Math.Cos((angle + pas_angle) * Math.PI / 180);
                pt_e.Y = pt_ref.Y + hauteur;
                pt_e.Z = pt_ref.Z - ray_int * Math.Sin((angle + pas_angle) * Math.PI / 180);
                maillage.Positions.Add(pt_h);
                maillage.Positions.Add(pt_a);
                maillage.Positions.Add(pt_d);
                maillage.Positions.Add(pt_e);
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
            GenererTriangleIndice(maillage);
            return maillage;
        }
        //modeliser la geometrie de la face du dessous
        private Geometry3D MaillageCylindreFaceDessous(Point3D pt_ref, double ray_ext, double ray_int, int nb_div)
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
                pt_g.X = pt_ref.X + ray_int * Math.Cos(angle * Math.PI / 180);
                pt_g.Y = pt_ref.Y + 0;
                pt_g.Z = pt_ref.Z - ray_int * Math.Sin(angle * Math.PI / 180);
                Point3D pt_f = new Point3D();
                pt_f.X = pt_ref.X + ray_int * Math.Cos((angle + pas_angle) * Math.PI / 180);
                pt_f.Y = pt_ref.Y + 0;
                pt_f.Z = pt_ref.Z - ray_int * Math.Sin((angle + pas_angle) * Math.PI / 180);
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
        //modeliser la geometrie de la face interne
        private Geometry3D MaillageCylindreFaceInterne(Point3D pt_ref, double hauteur, double ray_int, int nb_div)
        {
            MeshGeometry3D maillage = new MeshGeometry3D();
            double pas_angle = 360d / nb_div;
            int cpt_facette = 0;
            for (double angle = 0; angle < 360; angle += pas_angle)
            {
                Point3D pt_e = new Point3D();
                pt_e.X = pt_ref.X + ray_int * Math.Cos((angle + pas_angle) * Math.PI / 180);
                pt_e.Y = pt_ref.Y + hauteur;
                pt_e.Z = pt_ref.Z - ray_int * Math.Sin((angle + pas_angle) * Math.PI / 180);
                Point3D pt_f = new Point3D();
                pt_f.X = pt_ref.X + ray_int * Math.Cos((angle + pas_angle) * Math.PI / 180);
                pt_f.Y = pt_ref.Y + 0;
                pt_f.Z = pt_ref.Z - ray_int * Math.Sin((angle + pas_angle) * Math.PI / 180);
                Point3D pt_g = new Point3D();
                pt_g.X = pt_ref.X + ray_int * Math.Cos(angle * Math.PI / 180);
                pt_g.Y = pt_ref.Y + 0;
                pt_g.Z = pt_ref.Z - ray_int * Math.Sin(angle * Math.PI / 180);
                Point3D pt_h = new Point3D();
                pt_h.X = pt_ref.X + ray_int * Math.Cos(angle * Math.PI / 180);
                pt_h.Y = pt_ref.Y + hauteur;
                pt_h.Z = pt_ref.Z - ray_int * Math.Sin(angle * Math.PI / 180);
                maillage.Positions.Add(pt_e);
                maillage.Positions.Add(pt_f);
                maillage.Positions.Add(pt_g);
                maillage.Positions.Add(pt_h);
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