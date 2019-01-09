using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static ProjetSI.Ballistique;

namespace ProjetSI
{
    public class CourbeBallistique : Shape, INotifyPropertyChanged
    {
        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        public bool Trainee
        {
            get { return (bool)GetValue(TraineeProperty); }
            set { SetValue(TraineeProperty, value); }
        }

        public static readonly DependencyProperty TraineeProperty =
            DependencyProperty.Register("Trainee", typeof(bool), typeof(CourbeBallistique),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(double), typeof(CourbeBallistique),
                new FrameworkPropertyMetadata(20d, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(CourbeBallistique),
                new FrameworkPropertyMetadata(45d, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point), typeof(CourbeBallistique),
                new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender));

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify([CallerMemberName] string str = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }

        public double Longueur
        {
            get
            {
                if (Trainee)
                    return GetLength(Speed, Angle, new System.Windows.Media.Media3D.Vector3D());
                return GetNLength(Speed, Angle);
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                List<Point> points;
                if (Trainee)
                    points = GetPoints(Speed, Angle, (x, y) => x < 300);
                else
                    points = GetNPoints(Speed, Angle);
                PathGeometry geometry = new PathGeometry(new List<PathFigure>()
                {
                    new PathFigure(points.FirstOrDefault(), new List<PathSegment>()
                    {
                        new PolyBezierSegment(points, true),
                    }, false), })
                {
                    Transform = new TranslateTransform(StartPoint.X, StartPoint.Y)
                };
                Notify("Longueur");
                return geometry;
            }
        }
    }
}
