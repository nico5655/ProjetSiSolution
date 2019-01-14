using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjetSI
{
    /// <summary>
    /// Graphic representation of an angle.
    /// </summary>
    public class Angle : Shape
    {
        /// <summary>
        /// Angle value (°).
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Center point of the angle.
        /// </summary>
        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        /// <summary>
        /// Default position of the second point (when value is 0°).
        /// </summary>
        public Vector StartVector
        {
            get { return (Vector)GetValue(StartVectorProperty); }
            set { SetValue(StartVectorProperty, value); }
        }

        public SweepDirection Direction
        {
            get { return (SweepDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(SweepDirection), typeof(Angle),
                new FrameworkPropertyMetadata(SweepDirection.Counterclockwise, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// Default position of the second point (when value is 0°).
        /// </summary>
        public static readonly DependencyProperty StartVectorProperty =
            DependencyProperty.Register("StartVector", typeof(Vector), typeof(Angle),
                new FrameworkPropertyMetadata(new Vector(0, -1), FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// Center point of the angle.
        /// </summary>
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Point), typeof(Angle),
                new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// Angle value (°).
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Angle),
                new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));


        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new StreamGeometry();
                double length = StartVector.Length;
                Point startPoint = Center + StartVector;
                Point destination = Center + Ballistique.ToVector((Direction == SweepDirection.Clockwise) ? Value : 180 - Value, length);
                using (StreamGeometryContext context = geometry.Open())
                {
                    context.BeginFigure(startPoint, false, false);
                    if (Value < 180)//Creating a circle arc to represent the angle
                        context.ArcTo(destination, new Size(length, length), Value, false, Direction, true, false);
                }
                return geometry;
            }
        }
    }
}
