using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static ProjetSI.Ballistique;
using static System.Math;

namespace ProjetSI
{
    /// <summary>
    /// Converter for the net (get the distance to the net or the validity of the shoot).
    /// </summary>
    public class FiletConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(double))
            {
                try
                {
                    double angle = (double)values[0];
                    double tableWidth = (double)values[1];
                    Vector v = ToVector(angle, 1);
                    v *= tableWidth / 2 / v.X;
                    double distanceFilet = v.Length;
                    return distanceFilet;
                }
                catch
                {
                    return 137;
                }
            }
            try
            {
                double angle = (double)values[0];
                double speed = (double)values[1];
                double bangle = (double)values[2];
                Vector3D rotation = new Vector3D((double)values[3], (double)values[4], (double)values[5]);
                if (IsWorking(speed, bangle, angle, rotation))
                    return Colors.LightGreen;
                return Colors.DarkRed;
            }
            catch
            {
                return Colors.Fuchsia;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The point is visible if it is in the area.
    /// </summary>
    public class TargetVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Point pos = (Point)values[0];
                Rect zone = (Rect)values[1];
                return zone.Contains(pos) ? Visibility.Visible : Visibility.Hidden;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convert speed and zRotation to the max or min distance.
    /// </summary>
    public class DistanceConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double speed = (double)value[0];
                double zRotation = (double)value[1];
                double l = 0;
                if ((string)parameter == "max")
                    l = GetLength(speed, maxAngle, new Vector3D(0, 0, zRotation));
                else
                    l = GetLength(speed, minAngle, new Vector3D(0, 0, zRotation));
                if (targetType == typeof(Vector))
                    return new Vector(0, -l);
                return l;
            }
            catch
            {
                return 0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convert point to its text representation.
    /// </summary>
    public class TextConverter : IMultiValueConverter
    {

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Point p = (Point)value[0];
                Point center = (Point)value[1];
                Point pos = new Point(p.X - center.X, p.Y - center.Y) - new Vector(MainVM.ballSize, MainVM.ballSize);
                return $"x={Round(pos.X, 1)}, y={Round(pos.Y, 1)}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Get Position in animation from points and time.
    /// </summary>
    public class Pos2DConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                List<Point3D> points = values[0] as List<Point3D>;
                int t = (int)values[1];
                Point3D pt = points[t];
                if (parameter is string)
                {
                    if (parameter.ToString().Contains("m"))
                        pt = new Point3D(pt.X / 100, pt.Y / 100, pt.Z / 100);
                }
                return pt;
            }
            catch { return new Point3D(); }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convert rotations and time into a rotation angle.
    /// </summary>
    public class AngleTConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int t = 0;
            Vector3D omega = new Vector3D();
            try
            {

                t = (int)values[0];
                List<Vector3D> speeds = (List<Vector3D>)values[1];
                omega = speeds[t];
            }
            catch { }
            return omega;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SpeedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int t = 0;
            Vector3D speed = new Vector3D();
            try
            {

                t = (int)values[0];
                List<Vector3D> speeds = (List<Vector3D>)values[1];
                speed = speeds[t];
            }
            catch { }
            if (parameter.ToString() == "x")
                return speed.X;
            if (parameter.ToString() == "y")
                return speed.Y;
            if (parameter.ToString() == "z")
                return speed.Z;
            return speed;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Base converter, is created with methods.
    /// </summary>
    public class BaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return convert(value, targetType, parameter, culture);
        }

        private Func<object, Type, object, CultureInfo, object> convert;

        public BaseConverter()
        {
            convert = (v, t, p, c) => v;
        }

        public BaseConverter(Func<object, Type, object, CultureInfo, object> convert)
        {
            this.convert = convert ?? throw new ArgumentNullException(nameof(convert));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Multi version of the base converter.
    /// </summary>
    public class BaseMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            return convert(value, targetType, parameter, culture);
        }

        private Func<object[], Type, object, CultureInfo, object> convert;

        public BaseMultiConverter()
        {
            convert = (v, t, p, c) => v;
        }

        public BaseMultiConverter(Func<object[], Type, object, CultureInfo, object> convert)
        {
            this.convert = convert ?? throw new ArgumentNullException(nameof(convert));
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convert bool in whatever is the value of True and False value.
    /// </summary>
    public class ButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return True;
            return False;
        }

        public object False { get; set; }
        public object True { get; set; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Negate a number.
    /// </summary>
    public class NegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -(double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -(double)value;
        }
    }

    public class ModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double angle = (double)value;
            double x = GetTigeLength(angle);

            double beta = GetBeta(angle);
            Vector3D d = (x - 27) * new Vector3D(0, Cos(55.2 * PI / 180), -Sin(55.2 * PI / 180));
            if (parameter?.ToString() == "z")
                return d.Z / 100;
            if (parameter?.ToString() == "y")
                return d.Y / 100;
            return beta;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
