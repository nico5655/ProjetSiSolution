using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Objects3D
{
    public class TAfficheMurale : TBaseModel
    {
        public static readonly DependencyProperty PropertyHauteur =
            DependencyProperty.Register("P_Hauteur", typeof(double), typeof(TAfficheMurale), new PropertyMetadata(1d, DefaultPm));
        public static readonly DependencyProperty PropertyLargeur =
            DependencyProperty.Register("P_Largeur", typeof(double), typeof(TAfficheMurale), new PropertyMetadata(1d, DefaultPm));
        public static readonly DependencyProperty PropertyPointDeReference =
            DependencyProperty.Register("P_PointDeReference", typeof(Point3D), typeof(TAfficheMurale), new PropertyMetadata(new Point3D(0, 0, 0), DefaultPm));
        public double P_Hauteur
        {
            get
            {
                return (double)GetValue(PropertyHauteur);
            }
            set
            {
                SetValue(PropertyLargeur, value);
            }
        }
        public double P_Largeur
        {
            get
            {
                return (double)GetValue(PropertyLargeur);
            }
            set
            {
                SetValue(PropertyLargeur, value);
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
        protected override void OnUpdateModel()
        {
            GeometryModel3D model = new GeometryModel3D();
            model.Material = GetMaterial();
            model.BackMaterial = GetMaterial();
            model.Geometry = MaillageRectangleVerticalParal(P_Hauteur, P_Largeur, P_PointDeReference);
            P_Model = model;
            base.OnUpdateModel();
        }
    }
}