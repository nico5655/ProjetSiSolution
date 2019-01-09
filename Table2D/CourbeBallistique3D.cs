using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static ProjetSI.Ballistique;
using System.Windows.Media.Media3D;


namespace ProjetSI
{
    public class CourbeBallistique3D : UIElement3D
    {
        public CourbeBallistique3D()
        {
            UpdateGeometry();
        }

        public Point3D StartPoint
        {
            get { return (Point3D)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        public double Rayon
        {
            get { return (double)GetValue(RayonProperty); }
            set { SetValue(RayonProperty, value); }
        }

        public List<Point3D> Points
        {
            get { return (List<Point3D>)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(List<Point3D>), typeof(CourbeBallistique3D), new PropertyMetadata(new List<Point3D>(
            GetPoint3Ds(550, 35, 90, new Vector3D())), PM));

        public static readonly DependencyProperty RayonProperty =
            DependencyProperty.Register("Rayon", typeof(double), typeof(CourbeBallistique3D), new PropertyMetadata(1d, PM));

        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point3D), typeof(CourbeBallistique3D),
                new PropertyMetadata(new Point3D(), PM_Transform));

        static void PM(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as UIElement3D).InvalidateModel();
        }

        static void PM_Transform(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Point3D pt = new Point3D();
            if (e.Property == StartPointProperty)
            {
                pt = (Point3D)e.NewValue;
            }
            Transform3DGroup group = new Transform3DGroup();
            TranslateTransform3D translate = new TranslateTransform3D(pt.X, pt.Y, pt.Z);
            group.Children.Add(translate);
            (d as CourbeBallistique3D).Transform = group;
        }

        protected override void OnUpdateModel()
        {
            base.OnUpdateModel();
            UpdateGeometry();
        }

        internal void UpdateGeometry()
        {
            List<Point3D> points;
            points = Points;
            List<Point3D> positions = new List<Point3D>();
            List<Point> textureCoordinates = new List<Point>();
            List<int> triangleIndices = new List<int>();
            double step = 360d / 30;
            int nb_div = 30;
            for (int i = 0; i < points.Count - 1; i++)
            {
                Point3D pt1 = points[i];
                Point3D pt2 = points[i + 1];
                int cpt_facette = 0;
                double ray_ext = Rayon;
                for (double angle = 0; angle < 360; angle += step)
                {
                    Point3D pt_a = new Point3D
                    {
                        X = pt2.X + ray_ext * Math.Cos(angle * Math.PI / 180),
                        Y = pt2.Y,
                        Z = pt2.Z - ray_ext * Math.Sin(angle * Math.PI / 180)
                    };
                    Point3D pt_b = new Point3D
                    {
                        X = pt1.X + ray_ext * Math.Cos(angle * Math.PI / 180),
                        Y = pt1.Y + ray_ext * Math.Sin(angle * Math.PI / 180),
                        Z = pt1.Z - ray_ext * Math.Sin(angle * Math.PI / 180)
                    };
                    Point3D pt_c = new Point3D
                    {
                        X = pt1.X + ray_ext * Math.Cos((angle + step) * Math.PI / 180),
                        Y = pt1.Y + ray_ext * Math.Sin((angle + step) * Math.PI / 180),
                        Z = pt1.Z - ray_ext * Math.Sin((angle + step) * Math.PI / 180)
                    };
                    Point3D pt_d = new Point3D
                    {
                        X = pt2.X + ray_ext * Math.Cos((angle + step) * Math.PI / 180),
                        Y = pt2.Y,
                        Z = pt2.Z - ray_ext * Math.Sin((angle + step) * Math.PI / 180)
                    };
                    positions.Add(pt_a);
                    positions.Add(pt_b);
                    positions.Add(pt_c);
                    positions.Add(pt_d);

                    Point pt_hg = new Point((double)cpt_facette / nb_div, 0);
                    Point pt_hd = new Point((double)(cpt_facette + 1) / nb_div, 0);
                    Point pt_bg = new Point((double)cpt_facette / nb_div, 1);
                    Point pt_bd = new Point((double)(cpt_facette + 1) / nb_div, 1);
                    textureCoordinates.Add(pt_hg);
                    textureCoordinates.Add(pt_bg);
                    textureCoordinates.Add(pt_bd);
                    textureCoordinates.Add(pt_hd);
                    cpt_facette++;
                }
            }
            int depart = 0;
            for (int xx = 0; xx < positions.Count; xx += 4)
            {
                triangleIndices.Add(depart);
                triangleIndices.Add(depart + 1);
                triangleIndices.Add(depart + 2);
                triangleIndices.Add(depart + 2);
                triangleIndices.Add(depart + 3);
                triangleIndices.Add(depart);
                depart += 4;
            }
            MaterialGroup group = new MaterialGroup();
            group.Children.Add(new DiffuseMaterial(new SolidColorBrush(Colors.DarkCyan)));
            //group.Children.Add(new EmissiveMaterial(new SolidColorBrush(Colors.DarkBlue)));
            GeometryModel3D geom = new GeometryModel3D()
            {
                Geometry = new MeshGeometry3D()
                {
                    Positions = new Point3DCollection(positions),
                    TriangleIndices = new Int32Collection(triangleIndices),
                    TextureCoordinates = new PointCollection(textureCoordinates),
                },
                Material = group,
                BackMaterial = group,
            };
            Visual3DModel = geom;
        }
    }
}
