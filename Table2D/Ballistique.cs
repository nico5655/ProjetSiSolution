using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;
using static System.Math;

namespace ProjetSI
{
    public static class Ballistique
    {
        public const double minAngle = 15;
        public const double maxAngle = 35;
        public const double minSpeed = 450;
        public const double maxSpeed = 650;
        public const double dt = 1e-3;
        const double G = 980.6;
        const double Cr = 0.786;
        const double D = 10.6;//11.154;
        const double B = 12.11;
        const double K = 6.5;
        const double p = 1.34e-3;
        public const double minLowAngle = 45;
        public const double maxLowAngle = 135;
        const double r = 2;

        public static Vector GetPosition(double speed, double angle, double lowAngle, Vector3D rotation)
        {
            List<Point3D> point3Ds = GetPoint3Ds(speed, angle, lowAngle, (x, y) => y > 0, rotation);
            Point3D p = point3Ds.LastOrDefault();
            return new Vector(p.X, p.Z);
        }

        public static double GetLowAngle(double angle, double speed, Vector3D rotation, Vector position)
        {
            double a = minLowAngle;
            double b = maxLowAngle;
            int cpt = 0;
            double accuracy = 1e-6;
            do
            {
                if (position.Y > GetPosition(speed, angle, (a + b) / 2, rotation).Y)
                    a = (a + b) / 2;
                else
                    b = (a + b) / 2;
                cpt++;
            } while (Abs(b - a) > accuracy);
            return (a + b) / 2;
        }

        public static Vector3D GetRotation(int t, Vector3D speed)
        {
            double ts = t * dt;
            Vector3D dSpeed = speed * 180 / PI;
            dSpeed *= ts;
            return new Vector3D(dSpeed.X % 360, dSpeed.Y % 360, -dSpeed.Z % 360);
        }

        public static double ToAngle(Vector v)
        {
            v.Normalize();
            double angle = Acos(v.X) * 180 / PI;
            if (v.Y < 0)
                angle = 90 - angle;
            else
                angle += 90;
            return angle;
        }

        public static bool Filet(double angle, double bAngle, double speed, double tableWidth)
        {
            Vector v = ToVector(angle, 1);
            v *= tableWidth / 2 / v.X;
            double distanceFilet = v.Length;
            double y = GetYAtLength(speed, bAngle, distanceFilet);
            return y >= 15.25;
        }

        public static double GetTigeLength(double shoutAngle)
        {
            double x = Sqrt(Pow(D, 2) + 2 * Sin(shoutAngle * PI / 180) * D * K + Pow(K, 2));
            return x;
        }

        public static double GetSpeed(double length, double angle, Vector3D rotation)
        {
            double a = minSpeed;
            double b = maxSpeed;
            int cpt = 0;
            double accuracy = 1e-6;
            do
            {
                if (length > GetLength((a + b) / 2, angle, rotation))
                    a = (a + b) / 2;
                else
                    b = (a + b) / 2;
                cpt++;
            } while (Abs(b - a) > accuracy);
            return (a + b) / 2;
        }

        public static double GetAngle(double length, double speed, Vector3D rotation)
        {
            double a = minAngle;
            double b = maxAngle;
            double accuracy = 1e-6;
            do
            {
                if (length > GetLength(speed, (a + b) / 2, rotation))
                    a = (a + b) / 2;
                else
                    b = (a + b) / 2;
            } while (Abs(a - b) > accuracy);
            return (a + b) / 2;
        }

        public static Vector ToVector(double angle, double length)
        {
            Vector v = new Vector(length * Sin(angle * PI / 180), -length * Cos(angle * PI / 180));
            return v;
        }

        public static double GetYAtLength(double speed, double angle, double length) =>
            GetYAtLength(speed, angle, length, Z0(angle));

        private static double GetYAtLength(double speed, double angle, double length, double z0)
        {
            List<Point> pts = GetPoints(speed, angle, (x, y1) => x < length, z0);
            double y = pts.LastOrDefault().Y;
            return -y;
        }

        public static double GetLength(double speed, double angle, Vector3D rotation)
            => GetLength(speed, angle, rotation, (x, y) => y > 0);


        private static double GetLength(double speed, double angle, Vector3D rotation, Func<double, double, bool> condition)
        {
            List<Point3D> pts = GetPoint3Ds(speed, angle, 90, condition, rotation);
            Point3D pt = pts.LastOrDefault();
            Vector vector = new Vector(pt.X, pt.Z);
            return vector.Length;
        }

        public static List<Point> GetPoints(double speed, double angle) =>
            GetPoints(speed, angle, (x, y) => y > 0, Z0(angle));

        public static List<Point> GetPoints(double speed, double angle, Func<double, double, bool> condition) =>
            GetPoints(speed, angle, condition, Z0(angle));

        internal static List<Point> GetPoints(double speed, double angle, Func<double, double, bool> condition, double z0)
        {
            List<Point> points = new List<Point>();
            double x = 0;
            double y = z0;
            double vx = speed * Cos(angle * PI / 180);
            double vy = speed * Sin(angle * PI / 180);
            int cpt = 1;
            points.Add(new Point(x, -y));
            do
            {
                x += vx * dt;
                y += vy * dt;
                double v = Sqrt(Pow(vx, 2) + Pow(vy, 2));
                vx -= p * v * vx * dt;
                vy -= (G + p * v * vy) * dt;
                cpt++;
                points.Add(new Point(x, -y));
                if (y < 0)
                {
                    vy *= -Cr;
                    vx *= Cr;
                    y = 0;
                }
            } while (condition(x, y) && cpt < 5e3);
            return points;
        }

        public static List<Point3D> GetPoint3Ds(double speed, double angle, double lowAngle, Vector3D rotation)
            => GetPoint3Ds(speed, angle, lowAngle, (x, y) => x < 400 && x > -75, rotation);

        public static bool IsWorking(double speed, double angle, double lowAngle, Vector3D rotation)
        {
            List<Point3D> point3Ds = GetPoint3Ds(speed, angle, lowAngle, (x, y) => y > 0, rotation);
            Point3D pt = point3Ds.LastOrDefault();
            Point point = new Point(pt.X, pt.Z);
            if (!(Application.Current.MainWindow.DataContext is MainVM vM))
                return false;
            Rect rect = vM.RelativeZone;
            bool result = rect.Contains(point);
            return result;
        }

        public static object[] GetDatas(double speed, double angle, double lowAngle,
          Vector3D omega) => GetDatas(speed, angle, lowAngle, (x, y) => x < 400 && x > -75, omega);

        public static object[] GetDatas(double speed, double angle, double lowAngle, Func<double, double, bool> condition, Vector3D omega)
        {
            List<Point3D> points = new List<Point3D>();
            List<Vector3D> speeds = new List<Vector3D>();
            List<Vector3D> rotations = new List<Vector3D>();
            try
            {
                MainVM vM = Application.Current.MainWindow.DataContext as MainVM;
                double tableWidth = (vM != null) ? vM.TableWidth : 274;
                double tableHeight = (vM != null) ? vM.TableHeight : 152.5;
                Rect zone = new Rect(new Point(0, -tableHeight / 2), new Point(tableWidth, tableHeight / 2));
                double a = 1.8e-5 / 2.7e-3;
                Vector3D v = new Vector3D(Cos(angle * PI / 180) * Sin(lowAngle * PI / 180),
                    Sin(angle * PI / 180), -Cos(angle * PI / 180) * Cos(lowAngle * PI / 180));
                v *= speed;
                Point3D position = new Point3D(0, Z0(angle), 0);
                Vector3D rotation = new Vector3D();
                points.Add(position);
                speeds.Add(v);
                rotations.Add(rotation);
                int cpt = 1;
                do
                {
                    Vector3D g = new Vector3D(0, -G, 0);
                    Vector3D ft = -p * v.Length * v;
                    Vector3D magnus = a * Vector3D.CrossProduct(omega, v);
                    Vector3D ac = g + ft + magnus;
                    v += ac * dt;
                    position += v * dt;
                    rotation += omega * 180 / PI * dt;
                    cpt++;
                    if (position.Y <= 0 && zone.Contains(new Point(position.X, position.Z)))
                    {
                        v.Y *= -Cr;
                        v.X *= Cr;
                        v.Z *= Cr;
                        //Vector3D rotation2 = new Vector3D(0, omega.Y * 5, 0);
                        //magnus = Vector3D.CrossProduct(rotation2, v);
                        v += magnus * dt;
                        v.X += 2 / Sqrt(3) * omega.Z * r * Cr;
                        omega.Z *= 1 - Cr;
                        //omega.Y *= Cr;
                        position.Y = 0;
                    }
                    if (((points.Last().X < tableWidth / 2 && position.X > tableWidth / 2) || (points.Last().X > tableWidth / 2 && position.X < tableWidth / 2)) && zone.Contains(new Point(position.X, position.Z)) && position.Y < 15.25)
                    {
                        v.Y *= Cr;
                        v.X *= -Cr;
                        v.Z *= Cr;
                        position.X = tableWidth / 2;
                    }
                    points.Add(position);
                    speeds.Add(v);
                    rotations.Add(rotation);
                } while (condition(position.X, position.Y) && cpt < 3e3);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new object[] { points, speeds, rotations };
        }


        public static List<Point3D> GetPoint3Ds(double speed, double angle, double lowAngle, Func<double, double, bool> condition, Vector3D omega)
        {
            object[] datas = GetDatas(speed, angle, lowAngle, condition, omega);
            return (List<Point3D>)datas[0];
        }

        public static double GetNLength(double speed, double angle) => GetNLength(speed, angle, Z0(angle));
        private static double GetNLength(double speed, double angle, double z0)
        {
            double a = Pow(speed, 2) * Sin(2 * angle * PI / 180) / (2 * G) +
                Sqrt(Pow(speed * speed * Sin(2 * angle * PI / 180) / (2 * G), 2) +
                2 * z0 * Pow(speed * Cos(angle * PI / 180), 2) / G);
            return a;
        }

        public static double Z0(double angle)
        {
            return D * Sin(angle * PI / 180) + B;
        }

        public static List<Point> GetNPoints(double speed, double angle) => GetNPoints(speed, angle, Z0(angle));

        private static List<Point> GetNPoints(double speed, double angle, double z0)
        {
            List<Point> points = new List<Point>();
            double dist = GetNLength(speed, angle, z0);
            for (double x = 0; x <= dist + dist / 2000; x += dist / 20)
            {
                double a = 2 * Pow(speed * Cos(angle * PI / 180), 2);
                points.Add(new Point(x, -(-Pow(x, 2) * (G / a) + x * Tan(angle * PI / 180) + z0)));
            }
            return points;
        }
    }
}
