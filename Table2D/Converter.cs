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

    public class FiletConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(targetType == typeof(double))
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

    public class TextConverter : IMultiValueConverter
    {

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Point p = (Point)value[0];
                Point center = (Point)value[1];
                Point pos = new Point(p.X - center.X, center.Y - p.Y) - new Vector(MainVM.ballSize, MainVM.ballSize);
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

    public class AngleTConverter : IMultiValueConverter
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
            return GetRotation(t, speed);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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
}
