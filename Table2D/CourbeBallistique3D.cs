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
    /// <summary>
    /// Represents a curve in the 3D space.
    /// </summary>
    public class CourbeBallistique3D : UIElement3D
    {
        /// <summary>
        /// Create a new CourbeBallistique3D object.
        /// </summary>
        public CourbeBallistique3D()
        {
            UpdateGeometry();
        }

        /// <summary>
        /// Curve start point in 3D space.
        /// </summary>
        public Point3D StartPoint
        {
            get { return (Point3D)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        /// <summary>
        /// Cylinder ray.
        /// </summary>
        public double Rayon
        {
            get { return (double)GetValue(RayonProperty); }
            set { SetValue(RayonProperty, value); }
        }

        /// <summary>
        /// Curve points in 3D space.
        /// </summary>
        public List<Point3D> Points
        {
            get { return (List<Point3D>)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        /// <summary>
        /// Curve points in 3D space.
        /// </summary>
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(List<Point3D>), typeof(CourbeBallistique3D), new PropertyMetadata(new List<Point3D>(
            GetPoint3Ds(550, 35, 90, new Vector3D())), PM));

        /// <summary>
        /// Cylinder ray.
        /// </summary>
        public static readonly DependencyProperty RayonProperty =
            DependencyProperty.Register("Rayon", typeof(double), typeof(CourbeBallistique3D), new PropertyMetadata(1d, PM));

        /// <summary>
        /// Curve start point in 3D space.
        /// </summary>
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point3D), typeof(CourbeBallistique3D),
                new PropertyMetadata(new Point3D(), PM_Transform));

        /// <summary>
        /// Property manager for model.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        static void PM(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as UIElement3D).InvalidateModel();
        }
        
        /// <summary>
        /// Property manager called when transform changes.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        static void PM_Transform(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Point3D pt = new Point3D();
            if (e.Property == StartPointProperty)
            {
                pt = (Point3D)e.NewValue;
            }
            Transform3DGroup group = new Transform3DGroup();
            TranslateTransform3D translate = new TranslateTransform3D(pt.X, pt.Y, pt.Z);//adding translation to transform
            group.Children.Add(translate);
            (d as CourbeBallistique3D).Transform = group;
        }

        protected override void OnUpdateModel()
        {
            base.OnUpdateModel();
            UpdateGeometry();
        }

        /// <summary>
        /// Update the curve geometry with the new trajectory. 
        /// </summary>
        internal void UpdateGeometry()
        {
            //creating 3D positions from trajectory positions
            //cylinder around the trajectory
            List<Point3D> positions = new List<Point3D>();//points of the mesh
            List<Point> textureCoordinates = new List<Point>();//2d coordinates to add a brush onto the mesh
            List<int> triangleIndices = new List<int>();//indices to create triangles
            int nb_div = 30;//30 faces
            double step = 360d / nb_div;
            for (int i = 0; i < Points.Count - 1; i++)//iterating in the trajectory points
            {
                Point3D pt1 = Points[i];//first and second point
                Point3D pt2 = Points[i + 1];
                int cpt_facette = 0;
                for (double angle = 0; angle < 360; angle += step)
                {//for each two points, creating a cylinder between the two points with the specified Rayon
                    //creating first point at every iteration
                    Point3D pt_a = new Point3D//first segment, second point
                    {
                        X = pt2.X + Rayon * Math.Cos(angle * Math.PI / 180),//2d x coordinate in trigonometric circle
                        Y = pt2.Y,
                        Z = pt2.Z - Rayon * Math.Sin(angle * Math.PI / 180)//2d y coordinate in trigonometric circle
                    };
                    Point3D pt_b = new Point3D//first segment, first point
                    {
                        X = pt1.X + Rayon * Math.Cos(angle * Math.PI / 180),
                        Y = pt1.Y + Rayon * Math.Sin(angle * Math.PI / 180),//adding ray to the y
                        Z = pt1.Z - Rayon * Math.Sin(angle * Math.PI / 180)
                    };
                    Point3D pt_c = new Point3D//second segment, first point
                    {
                        X = pt1.X + Rayon * Math.Cos((angle + step) * Math.PI / 180),
                        Y = pt1.Y + Rayon * Math.Sin((angle + step) * Math.PI / 180),
                        Z = pt1.Z - Rayon * Math.Sin((angle + step) * Math.PI / 180)
                    };
                    Point3D pt_d = new Point3D//second segment, second points
                    {
                        X = pt2.X + Rayon * Math.Cos((angle + step) * Math.PI / 180),
                        Y = pt2.Y,
                        Z = pt2.Z - Rayon * Math.Sin((angle + step) * Math.PI / 180)
                    };
                    positions.Add(pt_a);//adding calculated points to the mesh
                    positions.Add(pt_b);
                    positions.Add(pt_c);
                    positions.Add(pt_d);

                    Point pt_hg = new Point((double)cpt_facette / nb_div, 0);//calculating 2d textures coordinates matching 3d position
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
            for (int xx = 0; xx < positions.Count; xx += 4)
            {//creating triangles in the mesh by defining triangle indices
                triangleIndices.Add(xx);
                triangleIndices.Add(xx + 1);
                triangleIndices.Add(xx + 2);
                triangleIndices.Add(xx + 2);
                triangleIndices.Add(xx + 3);
                triangleIndices.Add(xx);
            }
            MaterialGroup group = new MaterialGroup();//curve materials
            group.Children.Add(new DiffuseMaterial(new SolidColorBrush(Colors.DarkCyan)));//brush colored in DarkBlue
            //group.Children.Add(new EmissiveMaterial(new SolidColorBrush(Colors.DarkBlue)));
            GeometryModel3D geom = new GeometryModel3D()//creating 3d model with calculated parameters
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
