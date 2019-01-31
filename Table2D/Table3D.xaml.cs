using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ProjetSI.Ballistique;

namespace ProjetSI
{
    /// <summary>
    /// Logique d'interaction pour Table3D.xaml
    /// </summary>
    public partial class Table3D : UserControl
    {
        public Table3D()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Position of the ball in the 3D space.
        /// </summary>
        public Point3D BallPos
        {
            get { return (Point3D)GetValue(BallPosProperty); }
            set { SetValue(BallPosProperty, value); }
        }

        /// <summary>
        /// Position of the ball in the 3D space.
        /// </summary>
        public static readonly DependencyProperty BallPosProperty =
            DependencyProperty.Register("BallPos", typeof(Point3D), typeof(Table3D), new PropertyMetadata(new Point3D(), (d, e) =>
            {
                Table3D m = d as Table3D;
                Point3D ballPos = (Point3D)e.NewValue;
                m.balle.AppliquerTransformation(new Vector3D(ballPos.X, ballPos.Y, ballPos.Z), m.MagnusRotation);
            }));

        /// <summary>
        /// Rotation angle of the ball.
        /// </summary>
        public Vector3D MagnusRotation
        {
            get { return (Vector3D)GetValue(MagnusRotationProperty); }
            set { SetValue(MagnusRotationProperty, value); }
        }

        /// <summary>
        /// Rotation angle of the ball.
        /// </summary>
        public static readonly DependencyProperty MagnusRotationProperty =
            DependencyProperty.Register("MagnusRotation", typeof(Vector3D), typeof(Table3D), new PropertyMetadata(new Vector3D(), (d, e) =>
             {
                 Table3D m = d as Table3D;
                 Vector3D omega = (Vector3D)e.NewValue;
                 m.balle.AppliquerTransformation(new Vector3D(m.BallPos.X, m.BallPos.Y, m.BallPos.Z), omega);
             }));

        /// <summary>
        /// Camera of the Viewport3D.
        /// </summary>
        public Camera Camera
        {
            get { return viewport.Camera; }
            set { viewport.Camera = value; viewport2.Camera = value; }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MultiBinding binding = new MultiBinding()
            {
                Converter = new BaseMultiConverter((objs, t, c, p) => GetPoint3Ds(
                    (double)objs[0], (double)objs[1], (double)objs[2], new Vector3D((double)objs[3], (double)objs[4], (double)objs[5]))),
            };
            binding.Bindings.Add(new Binding("BallSpeed") { Source = DataContext });
            binding.Bindings.Add(new Binding("BallisticAngle") { Source = DataContext });
            binding.Bindings.Add(new Binding("Angle") { Source = DataContext });
            binding.Bindings.Add(new Binding("XRotation") { Source = DataContext });//bindings have to be set manually for 3Ds objects
            binding.Bindings.Add(new Binding("YRotation") { Source = DataContext });
            binding.Bindings.Add(new Binding("ZRotation") { Source = DataContext });
            BindingOperations.SetBinding(courbe, CourbeBallistique3D.PointsProperty, binding);
            //BindingOperations.SetBinding(courbe2, CourbeBallistique3D.PointsProperty, binding);
            BindingOperations.SetBinding(courbe, UIElement3D.VisibilityProperty, new Binding("T")
            {
                Source = DataContext,
                Converter = new BaseConverter((value, targetType, parameter, culture) => (int)value == 0 ? Visibility.Visible : Visibility.Collapsed),
            });
            /*BindingOperations.SetBinding(courbe2, UIElement3D.VisibilityProperty, new Binding("T")
            {
                Source = DataContext,
                Converter = new BaseConverter((value, targetType, parameter, culture) => (int)value == 0 ? Visibility.Visible : Visibility.Collapsed),
            });*/
            courbe.InvalidateModel();
            //courbe2.InvalidateModel();
        }

        public Visibility FiletVisibility { get => filet.Visibility; set => filet.Visibility = value; }
        public Rect ClipperRect { get => clipper.Rect; set => clipper.Rect = value; }
        public double CourbeRayon
        {
            get => courbe.Rayon;
            set
            {
                courbe.Rayon = value;
            }
        }

        public Visibility MaquetteVisibility
        {
            get => maquette.Visibility; set
            {
                maquette.Visibility = value;
                if (value == Visibility.Collapsed)
                    greatMaster.Children.Remove(maquette);
            }
        }
    }
}
