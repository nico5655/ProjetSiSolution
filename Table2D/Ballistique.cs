using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;
using static System.Math;

namespace ProjetSI
{
    /// <summary>
    /// Fournit les méthodes de calcul autour du tir, notamment de sa trajectoire.
    /// </summary>
    public static class Ballistique
    {
        #region constantes
        /// <summary>
        /// minimum ballistic angle (°).
        /// </summary>
        public const double minAngle = 15;
        /// <summary>
        /// maximum ballistic angle (°).
        /// </summary>
        public const double maxAngle = 35;
        /// <summary>
        /// minimum ball speed (m/s).
        /// </summary>
        public const double minSpeed = 450;
        /// <summary>
        /// maximum ball speed (m/s).
        /// </summary>
        public const double maxSpeed = 650;
        /// <summary>
        /// delta t for trajectory (s).
        /// </summary>
        public const double dt = 1e-3;
        /// <summary>
        /// Gravitation constant (m/s²).
        /// </summary>
        const double G = 980.6;
        /// <summary>
        /// Reduction coefficient due to friction during ball bounce.
        /// </summary>
        const double Cr = 0.786;
        /// <summary>
        /// D length on the model (cm).
        /// </summary>
        const double D = 21.4;//11.154;
        /// <summary>
        /// B length on the model (cm).
        /// </summary>
        const double B = 22.85;//12.11;
        /// <summary>
        /// K length on the model (cm).
        /// </summary>
        const double K = 11.6;
        /// <summary>
        /// Drag coefficient (S.I).
        /// </summary>
        const double p = 1.34e-3;
        /// <summary>
        /// Magnus coefficient (S.I).
        /// </summary>
        const double a = 6.666e-3;
        /// <summary>
        /// minimum lowAngle (°).
        /// </summary>
        public const double minLowAngle = 45;
        /// <summary>
        /// maximum lowAngle (°).
        /// </summary>
        public const double maxLowAngle = 135;
        /// <summary>
        /// rayon de la balle (cm).
        /// </summary>
        const double r = 2;
        #endregion

        #region other
        /// <summary>
        /// Calculate model "tige filetée" length (cm) from shout angle.
        /// </summary>
        /// <param name="shoutAngle">Ballistic shout angle (°).</param>
        /// <returns>tige length (mm)</returns>
        public static double GetTigeLength(double shoutAngle)
        {
            double x = Sqrt(Pow(D, 2) + 2 * Sin(shoutAngle * PI / 180) * D * K + Pow(K, 2));
            return x;//tige len formula, calculated using Al-Kachi theorem.
        }

        /// <summary>
        /// Calculates model's beta angle.
        /// </summary>
        /// <param name="alpha">Model shout angle (°).</param>
        /// <returns></returns>
        public static double GetBeta(double alpha)
        {
            double x = GetTigeLength(alpha);
            double cos = (x * x - D * D + Pow(B - K, 2)) / (2 * (B - K) * x);//model's beta formula
            return Acos(cos) * 180 / PI;
        }

        /// <summary>
        /// Calculate model height (cm) from shout angle.
        /// </summary>
        /// <param name="angle">Ballistic shout angle</param>
        /// <returns>Start y for shoot (cm).</returns>
        public static double Z0(double angle)
        {
            return D * Sin(angle * PI / 180) + B;//z0 formula
        }

        /// <summary>
        /// Get angle from vector direction.
        /// </summary>
        /// <param name="v">Vector to calculate the angle from.</param>
        /// <returns></returns>
        public static double ToAngle(Vector v)
        {
            v.Normalize();//normalizing th vector (reducing his length to 1)
            double angle = Acos(v.X) * 180 / PI;//there are two angles matching this x
            if (v.Y < 0)//we use the y to know which one to return.
                angle = 90 - angle;
            else
                angle += 90;
            //don't know why we were adding 90
            //maybe to compensate for the error between Asin and Acos above without knowing it?
            //but not sure
            return angle;
        }

        /// <summary>
        /// Create vector with direction calculated from angle and length.
        /// </summary>
        /// <param name="angle">Angle to calculate the vector direction from.</param>
        /// <param name="length">Vector length.</param>
        /// <returns></returns>
        public static Vector ToVector(double angle, double length)
        {
            Vector v = new Vector(length * Sin(angle * PI / 180), -length * Cos(angle * PI / 180));
            return v;//vectorial projection
        }

        #endregion

        #region 2D
        /// <summary>
        /// Does the ball is the net?
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="bAngle"></param>
        /// <param name="speed"></param>
        /// <param name="tableWidth"></param>
        /// <returns>False if it hits the net and true if it doesn't.</returns>
        public static bool Filet(double angle, double bAngle, double speed, double tableWidth)
        {
            //in this old version without effects, checking if the ball hits the net is much simpler
            Vector v = ToVector(angle, 1);//creating a vector with direction matching the angle
            //in order to get the direction of the ball in the table plan (only defined by low angle in this version)
            v *= tableWidth / 2 / v.X;//if the ball make and 90° angle, v.X value would be 1
            //we created the vector from the ball startpoint to the point where the ball hit (or doesn't hit) the net
            double distanceFilet = v.Length;//we get his length
            double y = GetYAtLength(speed, bAngle, distanceFilet);//then we get the height of the ball at this length
            return y >= 15.25;//if it is below the net height, then the ball hits the net
        }

        /// <summary>
        /// Calculate Y value (cm) at this length from speed and shoot angle.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Ballistic shoot angle (°).</param>
        /// <param name="length">Shoot length (cm).</param>
        /// <returns>Ball y value (cm) at this length.</returns>
        public static double GetYAtLength(double speed, double angle, double length) =>
    GetYAtLength(speed, angle, length, Z0(angle));

        /// <summary>
        /// Calculate Y value at a specific length from speed, shoot angle and start height.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <param name="length">Shoot length (cm).</param>
        /// <param name="z0">Z0 is start height (cm).</param>
        /// <returns>Ball y value (cm) at this length.</returns>
        private static double GetYAtLength(double speed, double angle, double length, double z0)
        {
            List<Point> pts = GetPoints(speed, angle, (x, y1) => x < length, z0);
            double y = pts.LastOrDefault().Y;//y of the point at this length
            return -y;
        }

        /// <summary>
        /// Calculate shoot trajectory 2D points from speed and angle with drag.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Shout angle (°).</param>
        /// <returns></returns>
        public static List<Point> GetPoints(double speed, double angle) =>
            GetPoints(speed, angle, firstImpact, Z0(angle));

        /// <summary>
        /// Calculate shoot trajectory 2D points from speed, angle and stop condition with drag.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Shout angle (°).</param>
        /// <param name="condition">Condition to end trajectory.</param>
        /// <returns></returns>
        public static List<Point> GetPoints(double speed, double angle, Func<double, double, bool> condition) =>
            GetPoints(speed, angle, condition, Z0(angle));

        /// <summary>
        /// Calculate shoot trajectory points from speed, angle, stop condition and z0.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Shout angle (°).</param>
        /// <param name="condition">Condition to end trajectory.</param>
        /// <param name="z0">Start height (cm).</param>
        /// <returns>Points of trajectory.</returns>
        internal static List<Point> GetPoints(double speed, double angle, Func<double, double, bool> condition, double z0)
        {
            //returns points in the 2D space
            //include drag, but not magnus effect
            List<Point> points = new List<Point>();
            double x = 0;
            double y = z0;//start position
            double vx = speed * Cos(angle * PI / 180);//start speed
            double vy = speed * Sin(angle * PI / 180);
            int cpt = 1;
            points.Add(new Point(x, -y));//first point
            do
            {
                x += vx * dt;//adding speed to position
                y += vy * dt;
                double v = Sqrt(Pow(vx, 2) + Pow(vy, 2));//speed value
                vx -= p * v * vx * dt;//drag formula
                vy -= (G + p * v * vy) * dt;//drag and gravitation
                cpt++;
                points.Add(new Point(x, -y));//adding the calculated position
                //y is negated because on the 2d figure, y coordinates go from top to bottom
                //ball bouncing (in this less evolved version, without effects, the ball with always be on the table)
                if (y < 0)
                {
                    vy *= -Cr;//only changing speeds
                    vx *= Cr;//bouncing is more simple without effects
                    y = 0;
                }//this version does not implement net bouncing
            } while (condition(x, y) && cpt < 3e3);//stopping after 3 seconds or when the condition is false
            return points;
        }

        /// <summary>
        /// Calculate shoot trajectory from speed and angle by analytic calculation without drag.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <returns></returns>
        public static List<Point> GetNPoints(double speed, double angle) => GetNPoints(speed, angle, Z0(angle));

        /// <summary>
        /// Calculate shoot trajectory from speed, angle and z0 by analytic calculation without drag.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <param name="z0">Start height (cm).</param>
        /// <returns></returns>
        private static List<Point> GetNPoints(double speed, double angle, double z0)
        {
            List<Point> points = new List<Point>();
            double dist = GetNLength(speed, angle, z0);
            for (double x = 0; x <= dist + dist / 2000; x += dist / 20)
            {
                double a = 2 * Pow(speed * Cos(angle * PI / 180), 2);//uses analytical trajectory formula y(x)
                points.Add(new Point(x, -(-Pow(x, 2) * (G / a) + x * Tan(angle * PI / 180) + z0)));
                //trajectory equation is possible since this version doesn't include drag
            }
            return points;
        }

        /// <summary>
        /// Calculate length from speed and angle by analytic calculations.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <returns></returns>
        public static double GetNLength(double speed, double angle) => GetNLength(speed, angle, Z0(angle));

        /// <summary>
        /// Calculate length from speed, angle and z0 by analytic calculations.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <param name="z0">Start height (cm).</param>
        /// <returns></returns>
        private static double GetNLength(double speed, double angle, double z0)
        {
            double a = Pow(speed, 2) * Sin(2 * angle * PI / 180) / (2 * G) +
                Sqrt(Pow(speed * speed * Sin(2 * angle * PI / 180) / (2 * G), 2) +
                2 * z0 * Pow(speed * Cos(angle * PI / 180), 2) / G);//an analytical formula
            return a;
        }
        #endregion

        #region 3D
        /// <summary>
        /// Get position vector (from start point to impact point in table plan) from ball speed, shoot angle, low angle and ball 3D rotation.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Ballistic shout angle (°).</param>
        /// <param name="lowAngle">Low angle (model orientation) (°).</param>
        /// <param name="rotation">Ball 3D rotation (rd/s).</param>
        /// <returns></returns>
        public static Vector GetPosition(double speed, double angle, double lowAngle, Vector3D rotation)
        {
            List<Point3D> point3Ds = GetPoint3Ds(speed, angle, lowAngle, firstImpact, rotation);
            //calculating trajectoy with first impact delegate
            Point3D p = point3Ds.LastOrDefault();//last point is first impact point
            return new Vector(p.X, p.Z);//same as get length, but it returns the vector and not only its length
        }

        /// <summary>
        /// Calculate model orientation angle from shoot angle, speed, 3d rotation and position vector by dichotomie. Only used in case of a Y rotation effect.
        /// </summary>
        /// <param name="angle">Shoot angle (°).</param>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="rotation">Ball 3D rotation (rd/s).</param>
        /// <param name="position">Position vector (from start point to impact point in 2D table plan).</param>
        /// <returns></returns>
        public static double GetLowAngle(double angle, double speed, Vector3D rotation, Vector position)
        {//dichotomie
            //vector length has already been calculated
            //we want the right direction
            //usually we can just do an acos, but the y effect modify the ball trajectory
            //so we must find the right angle to counterbalance the y effect
            double a = minLowAngle;
            double b = maxLowAngle;
            int cpt = 0;
            double accuracy = 1e-6;
            do
            {
                if (position.Y > GetPosition(speed, angle, (a + b) / 2, rotation).Y)//using the y of the position
                    a = (a + b) / 2;
                else
                    b = (a + b) / 2;
                cpt++;
            } while (Abs(b - a) > accuracy);
            return (a + b) / 2;
        }

        /// <summary>
        /// Calculate ball speed from length, angle and 3D rotation by dichotomie.
        /// </summary>
        /// <param name="length">Shoot length (cm).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <param name="rotation">Ball 3D rotation (rd/s).</param>
        /// <returns>Ball speed.</returns>
        public static double GetSpeed(double length, double angle, Vector3D rotation)
        {//dichotomie
            //speed matching length with angle and rotation
            //range increase when speed increases
            double a = minSpeed;
            double b = maxSpeed;
            int cpt = 0;
            double accuracy = 1e-6;
            do
            {
                if (length > GetLength((a + b) / 2, angle, rotation))//reduce the interval
                    a = (a + b) / 2;
                else
                    b = (a + b) / 2;
                cpt++;
            } while (Abs(b - a) > accuracy);
            return (a + b) / 2;
        }

        /// <summary>
        /// Calculate shoot angle from shoot length, ball speed and 3D rotation.
        /// </summary>
        /// <param name="length">Shoot length (cm).</param>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="rotation">Ball 3D rotation (rd/s).</param>
        /// <returns>Shoot angle matching these parameters.</returns>
        public static double GetAngle(double length, double speed, Vector3D rotation)
        {//dichotomie
            //goal is to find the angle matching the range, with the speed and the rotation as parameters
            //we know that the range increase when the angle increase
            double a = minAngle;//angle is between a and b
            double b = maxAngle;
            double accuracy = 1e-6;
            do
            {
                if (length > GetLength(speed, (a + b) / 2, rotation))//we reduce the interval
                    a = (a + b) / 2;
                else
                    b = (a + b) / 2;
            } while (Abs(a - b) > accuracy);//until it is small enough to get an accurate value
            return (a + b) / 2;
        }

        /// <summary>
        /// Calculate shoot length from speed, angle, rotation. Trajectoy last point is first impact point.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <param name="rotation">Ball 3D rotation (rd/s).</param>
        /// <param name="condition">Trajectory stop when condition is false.</param>
        /// <returns>Shoot length.</returns>
        public static double GetLength(double speed, double angle, Vector3D rotation)
            => GetLength(speed, angle, rotation, firstImpact);

        /// <summary>
        /// Calculate shoot length from speed, angle, rotation and condition. It uses the last point in the trajectory.
        /// </summary>
        /// <param name="speed">Ball speed (cm/s).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <param name="rotation">Ball 3D rotation (rd/s).</param>
        /// <param name="condition">Trajectory stop when condition is false.</param>
        /// <returns>Shoot length.</returns>
        private static double GetLength(double speed, double angle, Vector3D rotation, Func<double, double, bool> condition)
        {
            List<Point3D> pts = GetPoint3Ds(speed, angle, 90, condition, rotation);
            Point3D pt = pts.LastOrDefault();//last point of the trajectory
            Vector vector = new Vector(pt.X, pt.Z);//2D vector length
            return vector.Length;
        }

        /// <summary>
        /// Is the shoot valid? Use "MainVM.Zone".
        /// </summary>
        /// <param name="speed">Ball speed (m/s).</param>
        /// <param name="angle">Ball shoot angle (°).</param>
        /// <param name="lowAngle">Model orientation angle (°).</param>
        /// <param name="rotation">Ball 3D rotation (rd/s).</param>
        /// <returns>True if the first impact point is in the Area.</returns>
        public static bool IsWorking(double speed, double angle, double lowAngle, Vector3D rotation)
        {
            List<Point3D> point3Ds = GetPoint3Ds(speed, angle, lowAngle, firstImpact, rotation);
            Point3D pt = point3Ds.LastOrDefault();//last point of the trajectory is impact point
            Point point = new Point(pt.X, pt.Z);//Converted in his 2d position on the table
            if (!(Application.Current.MainWindow.DataContext is MainVM vM))
                return false;//in case the programm is not fully initialized
            Rect rect = vM.RelativeZone;//Area represents the table after the net
            bool result = rect.Contains(point);//if the ball first impact is here, it is valid
            return result;
        }

        /// <summary>
        /// Calculate shoot trajectory in the 3D space, including drag and magnus effect.
        /// </summary>
        /// <param name="speed">Ball speed (m/s).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <param name="lowAngle">Model orientation angle (°).</param>
        /// <param name="omega">Ball 3D rotation speed (rd/s).</param>
        /// <returns></returns>
        public static List<Point3D> GetPoint3Ds(double speed, double angle, double lowAngle, Vector3D rotation)
            => GetPoint3Ds(speed, angle, lowAngle, defaultDelegate, rotation);

        /// <summary>
        /// Calculate shoot trajectory in the 3D space, including drag and magnus effect.
        /// </summary>
        /// <param name="speed">Ball speed (m/s).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <param name="lowAngle">Model orientation angle (°).</param>
        /// <param name="condition">Ball keep moving while condition is true.</param>
        /// <param name="omega">Ball 3D rotation speed (rd/s).</param>
        /// <returns></returns>
        public static List<Point3D> GetPoint3Ds(double speed, double angle, double lowAngle, Func<double, double, bool> condition, Vector3D omega)
        {
            object[] datas = GetDatas(speed, angle, lowAngle, condition, omega);
            return (List<Point3D>)datas[0];//uses get datas and returns only the points
        }

        /// <summary>
        /// Calculate shoot trajectory in the 3D space, including drag and magnus effect. And also returns all speeds and rotation.
        /// </summary>
        /// <param name="speed">Ball speed (m/s).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <param name="lowAngle">Model orientation angle (°).</param>
        /// <param name="omega">Ball 3D rotation speed (rd/s).</param>
        /// <returns></returns>
        public static object[] GetDatas(double speed, double angle, double lowAngle,
          Vector3D omega) => GetDatas(speed, angle, lowAngle, defaultDelegate, omega);

        /// <summary>
        /// Calculate shoot trajectory in the 3D space, including drag and magnus effect. And also returns all speeds and rotation.
        /// </summary>
        /// <param name="speed">Ball speed (m/s).</param>
        /// <param name="angle">Shoot angle (°).</param>
        /// <param name="lowAngle">Model orientation angle (°).</param>
        /// <param name="condition">Ball keep moving while condition is true.</param>
        /// <param name="omega">Ball 3D rotation speed (rd/s).</param>
        /// <returns></returns>
        public static object[] GetDatas(double speed, double angle, double lowAngle, Func<double, double, bool> condition, Vector3D omega)
        {
            //get metadata matching these parameters
            Metadata metadata = metadatas.FirstOrDefault(d => d.Match(speed, angle, lowAngle, condition, omega));
            if (metadata != null)//if they are not null
                return metadata.Data;//no calculation needed
            List<Point3D> points = new List<Point3D>();//creating lists
            List<Vector3D> speeds = new List<Vector3D>();
            List<Vector3D> rotations = new List<Vector3D>();
            try
            {
                MainVM vM = Application.Current.MainWindow.DataContext as MainVM;//defining table area
                double tableWidth = (vM != null) ? vM.TableWidth : 274;//with main VM settings if possible
                double tableHeight = (vM != null) ? vM.TableHeight : 152.5;//else with default settings
                Rect zone = new Rect(new Point(0, -tableHeight / 2), new Point(tableWidth, tableHeight / 2));
                //creating speed vector, with directions matching angles.
                Vector3D v = new Vector3D(Cos(angle * PI / 180) * Sin(lowAngle * PI / 180),
                    Sin(angle * PI / 180), -Cos(angle * PI / 180) * Cos(lowAngle * PI / 180));
                v *= speed;//setting his length to speed
                Point3D position = new Point3D(0, Z0(angle), 0);//defining start position with z0.
                Vector3D rotation = new Vector3D();//rotation vector
                points.Add(position);//adding first position
                speeds.Add(v);//first speed
                rotations.Add(rotation);//and first rotation
                int cpt = 1;//starting counter to 1 (not 0 since we already added one position).
                do
                {
                    Vector3D g = new Vector3D(0, -G, 0);//gravitation (-gy)
                    Vector3D ft = -p * v.Length * v;//drag ((k/m).v²)
                    Vector3D magnus = a * Vector3D.CrossProduct(omega, v);//magnus effect ((q/m).omega^v)
                    Vector3D ac = g + ft + magnus;//acceleration
                    v += ac * dt;//adding acceleration to speed 
                    position += v * dt;//adding speed to position
                    rotation += omega * 180 / PI * dt;//adding omega (rotation speed in rd/s) to rotation
                    cpt++;//incrementing counter
                    if (position.Y <= 0 && zone.Contains(new Point(position.X, position.Z)))
                    {//is the ball bouncing on the table?
                        v.Y *= -Cr;//reducing and inverting y speed
                        v.X *= Cr;//reducing x and z speed
                        v.Z *= Cr;
                        //theoritically in a bounce, y rotation souldn't change ball speed
                        v.X += 2 / Sqrt(3) * omega.Y * (1 - Cr) * r * Cr * (v.Z / Abs(v.Z + 1e-10));//friction slightly move the rotation axis
                        v.Z -= 2 / Sqrt(3) * omega.Y * (1 - Cr) * r * Cr * (v.X / Abs(v.X + 1e-10));//which change the ball speed
                        v.X -= 2 / Sqrt(3) * omega.Z * r * Cr;//formula calculated with inertia momentum
                        v.Z += 2 / Sqrt(3) * omega.X * r * Cr;//for influence of rotation on speed during bounce
                        omega.X *= 1 - Cr;//ball loses rotation speed in x and z
                        omega.Z *= 1 - Cr;//what is lost in rotation speed is gain in speed
                        omega.Y *= Cr;//small reduction due to friction (same as speed)
                        position.Y = 0;//avoid ball getting stuck in the ground.
                    }
                    if (((points.Last().X < tableWidth / 2 && position.X > tableWidth / 2) ||
                        (points.Last().X > tableWidth / 2 && position.X < tableWidth / 2)) &&
                        zone.Contains(new Point(position.X, position.Z)) && position.Y < 15.25)
                    {//ball bouncing on the net
                        v.Y *= Cr;//we consider the net is made of the same materia as the table itself
                        v.X *= -Cr;//so same speed reduction
                        v.Z *= Cr;
                        position.X = tableWidth / 2;//making sure the ball doesn't get stuck.
                    }
                    points.Add(position);//adding calculated position
                    speeds.Add(v);//calculated speed
                    rotations.Add(rotation);//and calculated position
                } while (condition(position.X, position.Y) && cpt < 3e3);
                //ball stops after 3 seconds or when the condition is no longer true
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);//writing error
            }
            object[] datas = new object[] { points, speeds, rotations };
            metadatas.Add(new Metadata(datas, speed, angle, lowAngle, condition, omega));//adding metadatas
            return datas;
        }
        #endregion

        #region metadatas
        /// <summary>
        /// All metadas, used to never a trajectory calculation twice.
        /// </summary>
        private static List<Metadata> metadatas = new List<Metadata>();

        /// <summary>
        /// Store datas associated with all parameters.
        /// </summary>
        private class Metadata
        {
            /// <summary>
            /// Create metadata with specified data, speed, angle, lowAngle, condition and omega.
            /// </summary>
            /// <param name="data"></param>
            /// <param name="speed"></param>
            /// <param name="angle"></param>
            /// <param name="lowAngle"></param>
            /// <param name="condition"></param>
            /// <param name="omega"></param>
            public Metadata(object[] data, double speed, double angle, double lowAngle, Func<double, double, bool> condition, Vector3D omega)
            {
                Data = data;
                Speed = speed;
                Angle = angle;
                LowAngle = lowAngle;
                Condition = condition;
                Omega = omega;
            }

            /// <summary>
            /// Value stored in the metadata.
            /// </summary>
            public object[] Data { get; set; }

            /// <summary>
            /// Speed parameter used as identifier.
            /// </summary>
            public double Speed { get; set; }

            /// <summary>
            /// Angle parameter used as identifier.
            /// </summary>
            public double Angle { get; set; }

            /// <summary>
            /// LowAngle parameter used as identifier.
            /// </summary>
            public double LowAngle { get; set; }

            /// <summary>
            /// Condition parameter used as identifier.
            /// </summary>
            public Func<double, double, bool> Condition { get; set; }

            /// <summary>
            /// Omega parameter used as identifier.
            /// </summary>
            public Vector3D Omega { get; set; }

            /// <summary>
            /// Check if parameters match in order to get the data.
            /// </summary>
            /// <param name="speed"></param>
            /// <param name="angle"></param>
            /// <param name="lowAngle"></param>
            /// <param name="condition"></param>
            /// <param name="omega"></param>
            /// <returns>True if parameter match, else false.</returns>
            public bool Match(double speed, double angle, double lowAngle, Func<double, double, bool> condition, Vector3D omega)
            {
                bool result = Speed == speed && Angle == angle && LowAngle == lowAngle && Condition == condition && Omega == omega;
                return result;
            }
        }

        /// <summary>
        /// Default delegate in GetPoint3Ds, run the trajectory while the ball is in field of view.
        /// </summary>
        private static readonly Func<double, double, bool> defaultDelegate = (x, y) => x < 600 && x > -75 && y > -75;

        /// <summary>
        /// Default delegate in GetLength, stop the trajectory at the first impact.
        /// </summary>
        private static readonly Func<double, double, bool> firstImpact = (x, y) => y > 0;
        #endregion
    }
}
