using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Objects3D
{
    public class TSol : TBaseModel
    {
        public TSol() : base() { }
        protected override void OnUpdateModel()
        {
            Material materiau = GetMaterial();
            P_Model = new GeometryModel3D(MaillageCarreHorizontalSol(P_LargeurXPos,
                P_LargeurXNeg, P_ProfondeurZPos, P_ProfondeurZNeg, new Point3D(0, 0, 0)), materiau) { BackMaterial = materiau};
            base.OnUpdateModel();
        }
        public static readonly DependencyProperty PropertyLargeurXPos =
            DependencyProperty.Register("P_LargeurXPos", typeof(double), typeof(TSol), new PropertyMetadata(1d, DefaultPm));
        public static readonly DependencyProperty PropertyLargeurXNeg =
            DependencyProperty.Register("P_LargeurXNeg", typeof(double), typeof(TSol), new PropertyMetadata(1d, DefaultPm));
        public static readonly DependencyProperty PropertyProfondeurZPos =
            DependencyProperty.Register("P_ProfondeurZPos", typeof(double), typeof(TSol), new PropertyMetadata(1d, DefaultPm));
        public static readonly DependencyProperty PropertyProfondeurZNeg =
            DependencyProperty.Register("P_ProfondeurZNeg", typeof(double), typeof(TSol), new PropertyMetadata(1d, DefaultPm));
        public double P_LargeurXPos
        {
            get
            {
                return (double)GetValue(PropertyLargeurXPos);
            }
            set
            {
                SetValue(PropertyLargeurXPos, value);
            }
        }
        public double P_LargeurXNeg
        {
            get
            {
                return (double)GetValue(PropertyLargeurXNeg);
            }
            set
            {
                SetValue(PropertyLargeurXNeg, value);
            }
        }
        public double P_ProfondeurZPos
        {
            get
            {
                return (double)GetValue(PropertyProfondeurZPos);
            }
            set
            {
                SetValue(PropertyProfondeurZPos, value);
            }
        }
        public double P_ProfondeurZNeg
        {
            get
            {
                return (double)GetValue(PropertyProfondeurZNeg);
            }
            set
            {
                SetValue(PropertyProfondeurZNeg, value);
            }
        }
    }
}