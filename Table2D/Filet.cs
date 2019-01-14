using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace ProjetSI
{
    /// <summary>
    /// Represent the net in a 2D trajectory graph.
    /// </summary>
    public class Filet : Shape
    {
        /// <summary>
        /// Shape's start point.
        /// </summary>
        public Point SPoint
        {
            get { return (Point)GetValue(SPointProperty); }
            set { SetValue(SPointProperty, value); }
        }

        /// <summary>
        /// Distance between the net and the edge of the table.
        /// </summary>
        public double DistanceFilet
        {
            get { return (double)GetValue(DistanceFiletProperty); }
            set { SetValue(DistanceFiletProperty, value); }
        }

        /// <summary>
        /// Distance between the net and the edge of the table.
        /// </summary>
        public static readonly DependencyProperty DistanceFiletProperty =
            DependencyProperty.Register("DistanceFilet", typeof(double), typeof(Filet),
                new FrameworkPropertyMetadata(137d, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Shape's start point.
        /// </summary>
        public static readonly DependencyProperty SPointProperty =
            DependencyProperty.Register("SPoint", typeof(Point), typeof(Filet),
                new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender));

        protected override Geometry DefiningGeometry
        {
            get
            {
                PathGeometry geometry = new PathGeometry(new List<PathFigure>()
                {
                    new PathFigure(new Point(DistanceFilet, 0), new List<PathSegment>(){//create segments to represent the table and the net
                        new LineSegment(new Point(DistanceFilet, -15.25), true) }, false),
                    new PathFigure(new Point(), new List<PathSegment>(){new LineSegment(new Point(DistanceFilet * 2, 0), true),
                }, false) })
                {
                    Transform = new TranslateTransform(SPoint.X, SPoint.Y)
                };
                return geometry;
            }
        }
    }
}
