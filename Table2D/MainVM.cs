using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.IO.Ports;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using static ProjetSI.Ballistique;

namespace ProjetSI
{
    public class MainVM : INotifyPropertyChanged
    {
        public MainVM()
        {
            Angle = 90;
            YRotation = 0;
            LoadDatas();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            try
            {
                player = new SoundPlayer("rebond.wav");
                player.LoadAsync();
            }
            catch { }
            Ports = new ObservableCollection<string>(SerialPort.GetPortNames());
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            Robot = new Robot();
            Robot.Ready += Robot_Ready;
            if (!InDesignMode())
            {
                Robot.PortNumber = Ports.LastOrDefault();
                Loop();
            }
        }

        private async void Loop()
        {
            await Task.Run(() =>
            {
                while (_continue)
                {
                    try
                    {
                        using (NamedPipeServerStream pipe = new NamedPipeServerStream("ping/tir763", PipeDirection.InOut))
                        {
                            pipe.WaitForConnection();
                            using (StreamReader sr = new StreamReader(pipe))
                            {
                                string line = sr.ReadLine();
                                if (line != null)
                                {
                                    string[] tab = line.Split(';');
                                    Point pt = new Point();
                                    bool change = false;
                                    bool fireBall = false;
                                    foreach (string item in tab)
                                    {
                                        string[] tab1 = item.Split('=');
                                        if (tab1[1] != "None")
                                        {
                                            double value = double.Parse(tab1[1].Replace('.', ','));
                                            switch (tab1[0])
                                            {
                                                case "speed":
                                                    BallSpeed = value;
                                                    break;
                                                case "bAngle":
                                                    BallisticAngle = value;
                                                    break;
                                                case "dAngle":
                                                    Angle = value;
                                                    break;
                                                case "rotationX":
                                                    XRotation = value;
                                                    break;
                                                case "rotationY":
                                                    YRotation = value;
                                                    break;
                                                case "rotationZ":
                                                    ZRotation = value;
                                                    break;
                                                case "x":
                                                    pt.X = value;
                                                    change = true;
                                                    break;
                                                case "y":
                                                    pt.Y = -value;
                                                    change = true;
                                                    break;
                                                case "fire":
                                                    if (value == 1)
                                                        fireBall = true;
                                                    break;
                                            }
                                        }
                                    }
                                    if (change)
                                        MainWindow.DipThread?.Invoke(() => BallPos = pt + (Center - new Point()) - new Vector(ballSize, ballSize));
                                    if (fireBall)
                                        MainWindow.DipThread?.Invoke(() => FireBall?.Execute(null), System.Windows.Threading.DispatcherPriority.Send);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            });
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MainWindow.DipThread.Invoke(() =>
            Ports = new ObservableCollection<string>(SerialPort.GetPortNames()));
        }

        public static bool InDesignMode()
        {
            return !(Application.Current is App);
        }

        private void Robot_Ready(object arg1, EventArgs arg2)
        {
            (SetSpeed as BaseCommand)?.OnCanExecuteChanged();
            (SetBallAngle as BaseCommand)?.OnCanExecuteChanged();
            (SetDownAngle as BaseCommand)?.OnCanExecuteChanged();
            (DropBall as BaseCommand)?.OnCanExecuteChanged();
            (StartStop as BaseCommand)?.OnCanExecuteChanged();
        }

        private Robot robot;
        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify([CallerMemberName] string str = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }

        #region fields
        private System.Timers.Timer timer = new System.Timers.Timer(5000);
        private double tableHeight = 152.5;
        private double tableWidth = 274;
        private double ballisticAngle = maxAngle;
        private double ballSpeed = 550;
        private bool targetSelection = true;
        private double angle = 0;
        public Point MousePos { get; set; }
        internal const double ballSize = 2.5;//rayon de la cible rouge
        private int t = 0;
        private List<Point3D> points = new List<Point3D>();
        private List<Vector3D> speeds = new List<Vector3D>();
        private List<Vector3D> omegas = new List<Vector3D>();
        private double xRotation;
        private double yRotation;
        private double zRotation;
        private ObservableCollection<string> ports;
        private double animationSpeed = 0.5;
        private double fps;
        #endregion

        #region commands
        private ICommand fireBall;
        private ICommand setSpeed;
        private ICommand setAim;
        private ICommand dropBall;
        private ICommand startStop;
        /// <summary>
        /// Fire the ball.
        /// </summary>
        public ICommand FireBall
        {
            get
            {
                if (fireBall == null)
                    fireBall = new BaseCommand(Start, () => T == 0);
                return fireBall;
            }
        }
        private ICommand updatePos;
        /// <summary>
        /// Update ball position on click.
        /// </summary>
        public ICommand UpdatePos
        {
            get
            {
                if (updatePos == null)
                    updatePos = new BaseCommand<Point>((p) => { BallPos = p - new Vector(ballSize, ballSize); }, (p) => Zone.Contains(p) && TargetSelection);
                return updatePos;
            }
        }

        /// <summary>
        /// Set the robot speed to "BallSpeed" value.
        /// </summary>
        public ICommand SetSpeed
        {
            get
            {
                if (setSpeed == null)
                    setSpeed = new BaseCommand(() =>
                    {
                        Robot.Done = !Robot.SetSpeed(BallSpeed);
                        (SetSpeed as BaseCommand).OnCanExecuteChanged();
                        Thread.Sleep(10);
                    }, () => Robot.Done);
                return setSpeed;
            }
        }

        /// <summary>
        /// Set the robots down angle to "Angle" value.
        /// </summary>
        public ICommand SetDownAngle
        {
            get
            {
                if (setAim == null)
                    setAim = new BaseCommand(() =>
                    {
                        Robot.Done = !Robot.SetDownAngle(Angle);
                        (SetDownAngle as BaseCommand).OnCanExecuteChanged();
                        Thread.Sleep(10);
                    }, () => Robot.Done);
                return setAim;
            }
        }

        private ICommand setBallAngle;
        /// <summary>
        /// Set the robots shoot angle to "BallisticAngle" value.
        /// </summary>
        public ICommand SetBallAngle
        {
            get
            {
                if (setBallAngle == null)
                    setBallAngle = new BaseCommand(() =>
                    {
                        Robot.Done = !Robot.SetShootAngle(BallisticAngle);
                        (SetBallAngle as BaseCommand).OnCanExecuteChanged();
                        Thread.Sleep(10);
                    }, () => Robot.Done);
                return setBallAngle;
            }
        }

        /// <summary>
        /// Drops the ball.
        /// </summary>
        public ICommand DropBall
        {
            get
            {
                if (dropBall == null)
                    dropBall = new BaseCommand(() =>
                    {
                        Robot.Done = !Robot.DropBall();
                        (DropBall as BaseCommand).OnCanExecuteChanged();
                        Thread.Sleep(10);
                    }, () => Robot.Done);
                return dropBall;
            }
        }

        /// <summary>
        /// Start or stop the speeds motor.
        /// </summary>
        public ICommand StartStop
        {
            get
            {
                if (startStop == null)
                    startStop = new BaseCommand(() =>
                    {
                        if (Robot.Started)
                            Robot.Stop();
                        else
                            Robot.Start();
                        (StartStop as BaseCommand).OnCanExecuteChanged();
                    }, () => Robot.Done);
                return startStop;
            }
        }
        #endregion

        #region properties
        /// <summary>
        /// Area after the net where the ball can land.
        /// </summary>
        public Rect Zone
        {
            get
            {
                Point pt1 = ToAbsolute(new Point(TableWidth / 2, TableHeight / 2));
                Point pt2 = ToAbsolute(new Point(TableWidth, -TableHeight / 2));
                return new Rect(pt1, pt2);
            }
        }

        /// <summary>
        /// Area in relative coordinates.
        /// </summary>
        public Rect RelativeZone
        {
            get
            {
                Point pt1 = new Point(TableWidth / 2, TableHeight / 2);
                Point pt2 = new Point(TableWidth, -TableHeight / 2);
                return new Rect(pt1, pt2);
            }
        }

        /// <summary>
        /// Model's position on the table.
        /// </summary>
        public Point Center
        {
            get
            {
                return new Point(0, TableHeight / 2);
            }
        }

        /// <summary>
        /// Height of the ping pong table.
        /// </summary>
        public double TableHeight
        {
            get => tableHeight;
            set
            {
                tableHeight = value;
                Notify();
                Notify("Center");
                Notify("Zone");
            }
        }

        /// <summary>
        /// Width of the ping pong table.
        /// </summary>
        public double TableWidth
        {
            get => tableWidth; set
            {
                tableWidth = value;
                Notify();
                Notify("Center");
                Notify("Zone");
            }
        }

        /// <summary>
        /// Ball 3D pos at this time of the animation.
        /// </summary>
        public Point3D Ball3DPos
        {
            get => Points[T];
        }

        /// <summary>
        /// Model orientation angle.
        /// </summary>
        public double Angle
        {
            get => angle;
            set
            {
                if (value != Angle)
                {
                    angle = value;
                    Notify();
                    Notify("BallPos");
                }
            }
        }

        /// <summary>
        /// Speed value for the ball at the time it is fired.
        /// </summary>
        public double BallSpeed
        {
            get => ballSpeed; set
            {
                if (BallSpeed != value)
                {
                    ballSpeed = value;
                    Notify();
                    Notify("BallPos");
                }
            }
        }

        /// <summary>
        /// Can we select the target by a click.
        /// </summary>
        public bool TargetSelection
        {
            get => targetSelection; set
            {
                targetSelection = value;
                Notify();
            }
        }

        /// <summary>
        /// Ball's ballistic shoot angle.
        /// </summary>
        public double BallisticAngle
        {
            get => ballisticAngle; set
            {
                if (ballisticAngle != value)
                {
                    ballisticAngle = value;
                    Notify();
                    Notify("BallPos");
                    LoadDatas();
                }
            }
        }

        /// <summary>
        /// Recalculate datas for the animations when the parameters change.
        /// </summary>
        private void LoadDatas()
        {
            object[] datas = GetDatas(BallSpeed, BallisticAngle, Angle, new Vector3D(XRotation, YRotation, ZRotation));
            Points = (List<Point3D>)datas[0];
            Speeds = (List<Vector3D>)datas[1];
            Omegas = (List<Vector3D>)datas[2];
        }

        /// <summary>
        /// First impact 2D position on the table.
        /// </summary>
        public Point BallPos
        {
            get
            {
                Vector v1 = GetPosition(BallSpeed, BallisticAngle, Angle, new Vector3D(XRotation, YRotation, ZRotation));
                Vector v2 = new Vector(ballSize, ballSize);
                Point bpos = Center + v1 - v2;
                return bpos;
            }
            set
            {
                Vector v = value + new Vector(ballSize, ballSize) - Center;
                Angle = ToAngle(v);
                CalculateParameters(v);
                Notify();
            }
        }

        /// <summary>
        /// Time frame value for the animation.
        /// </summary>
        public int T
        {
            get => t; set
            {
                if (t != value)
                {
                    t = value;
                    Notify();
                    MainWindow.DipThread.Invoke((FireBall as BaseCommand).OnChanged);
                }
            }
        }

        /// <summary>
        /// Trajectory's points.
        /// </summary>
        public List<Point3D> Points
        {
            get => points; set
            {
                points = value;
                Notify();
            }
        }

        /// <summary>
        /// X component of the ball's 3D rotation vector.
        /// </summary>
        public double XRotation
        {
            get => xRotation; set
            {
                xRotation = value;
                Notify();
                Notify("BallPos");
            }
        }

        /// <summary>
        /// Y component of the ball's 3D rotation vector.
        /// </summary>
        public double YRotation
        {
            get => yRotation; set
            {
                yRotation = value;
                Notify();
                Notify("BallPos");
            }
        }

        /// <summary>
        /// Z component of the ball's 3D rotation vector.
        /// </summary>
        public double ZRotation
        {
            get => zRotation; set
            {
                zRotation = value;
                Notify();
                Notify("BallPos");
            }
        }

        /// <summary>
        /// All ports, select one for the arduino.
        /// </summary>
        public ObservableCollection<string> Ports
        {
            get => ports; set
            {
                ports = value;
                Notify();
            }
        }

        /// <summary>
        /// The arduino, shoot parameters are sent and the model shoots.
        /// </summary>
        public Robot Robot
        {
            get => robot; set
            {
                robot = value;
                Notify();
            }
        }

        /// <summary>
        /// Animation speed, 0 is paused and one is real speed.
        /// </summary>
        public double AnimationSpeed
        {
            get => animationSpeed;
            set
            {
                if (animationSpeed != value)
                {
                    if (stopwatch.IsRunning)
                    {//changing start time to keep the ball at the same position
                        stopwatch.Stop();
                        if (animationSpeed == 0)
                            startElapsed = (int)(startElapsed / value - stopwatch.ElapsedMilliseconds);
                        else if (value == 0)
                            startElapsed = (int)((startElapsed + stopwatch.ElapsedMilliseconds) * animationSpeed);
                        else
                            startElapsed = (int)((startElapsed + stopwatch.ElapsedMilliseconds) * (animationSpeed / value) - stopwatch.ElapsedMilliseconds);
                        animationSpeed = value;
                        stopwatch.Start();
                    }
                    else
                        animationSpeed = value;
                    Notify();
                }
            }
        }

        /// <summary>
        /// All speeds for animation.
        /// </summary>
        public List<Vector3D> Speeds
        {
            get => speeds;
            set
            {
                speeds = value;
                Notify();
            }
        }

        /// <summary>
        /// All rotations for animation.
        /// </summary>
        public List<Vector3D> Omegas
        {
            get => omegas;
            set
            {
                omegas = value;
                Notify();
            }
        }

        /// <summary>
        /// Frames Per Second
        /// </summary>
        public double FPS
        {
            get => fps;
            set
            {
                fps = value;
                Notify();
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Start Ball animation.
        /// </summary>
        public void Start()
        {
            LoadDatas();
            startElapsed = 0;
            stopwatch.Reset();
            stopwatch.Start();
            (FireBall as BaseCommand).OnChanged();
        }
        Stopwatch stopwatch = new Stopwatch();
        int startElapsed = 0;
        int calls = 0;

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (stopwatch.IsRunning)//if there is an ongoing animation
            {
                calls++;
                FPS = calls / stopwatch.Elapsed.TotalSeconds;
                if ((T + 1) >= points.Count)//if the animation is finished
                {
                    startElapsed = 0;
                    MainWindow.DipThread.Invoke(stopwatch.Reset, DispatcherPriority.Normal);//we reset everything
                    T = 0;
                    FPS = 0;
                    calls = 0;
                }
                else
                {
                    try
                    {
                        MainWindow.DipThread.Invoke(() =>
                    {
                        if (AnimationSpeed != 0)//If the animation is not paused.
                        {
                            int pt = T;
                            T = (int)((startElapsed + stopwatch.ElapsedMilliseconds) * AnimationSpeed);
                            if (T < pt)
                            {
                                stopwatch.Stop();//for debug
                                stopwatch.Start();
                            }
                            if (pt < T)
                                try
                                {
                                    double y = 0;
                                    Point3D pt1 = new Point3D();
                                    Rect rect = new Rect(new Point(0, -TableHeight / 2), new Point(TableWidth, TableHeight / 2));
                                    if (T < points.Count && (y = points.GetRange(pt, T - pt).Min(p => p.Y)) <= 0 &&
                                    rect.Contains(new Point((pt1 = points.Last(p => p.Y == y)).X, pt1.Y)))//if the ball is bouncing
                                        player.Play();//we play a sound
                                }
                                catch (InvalidOperationException) { }
                        }
                        else if (t != startElapsed)
                            T = startElapsed;//resynchronizing if paused
                    }, DispatcherPriority.Input);
                    }
                    catch { }
                }
            }
        }

        SoundPlayer player;
        private bool _continue = true;

        /// <summary>
        /// Calculate all ballistic parameter to match the new Ball direction.
        /// </summary>
        /// <param name="vector">Direction from start point to first impact point.</param>
        private void CalculateParameters(Vector vector)
        {
            double length = vector.Length;//we first calculate the parameters for the shoot length
            if (GetLength(BallSpeed, BallisticAngle, new Vector3D(XRotation, YRotation, ZRotation)) != length)
            {//if we don't already have the right parameters
                //the purpose of this algorithm is to use the angle to get the right length and change the speed only if necessary
                double len = GetLength(BallSpeed, maxAngle, new Vector3D(XRotation, YRotation, ZRotation));//maximum length with this speed
                if (len == length)//if it matches exactly
                {
                    BallisticAngle = maxAngle;
                }
                else if (len < length)//if max angle isn't enough
                {//we have to change the speed
                    BallisticAngle = maxAngle;
                    BallSpeed = GetSpeed(length, maxAngle, new Vector3D(XRotation, YRotation, ZRotation));
                }
                else if ((len = GetLength(BallSpeed, minAngle, new Vector3D(XRotation, YRotation, ZRotation))) == length)
                {//if it matches the minimum angle
                    BallisticAngle = minAngle;
                }
                else if (len > length)
                {//if it is below the minimum, we also have to change the speed
                    BallisticAngle = minAngle;
                    BallSpeed = GetSpeed(length, minAngle, new Vector3D(XRotation, YRotation, ZRotation));
                }
                else
                {//if it is none of those, it means the target angle is in the interval and we can find it by dichotomie
                    BallisticAngle = GetAngle(length, BallSpeed, new Vector3D(XRotation, YRotation, ZRotation));
                }
            }//then we see if a "Y" effect has modified the trajectory and we need to change the angle
            if (GetPosition(BallSpeed, BallisticAngle, Angle, new Vector3D(XRotation, YRotation, ZRotation)).Y != vector.Y)
            {
                Angle = GetLowAngle(BallisticAngle, BallSpeed, new Vector3D(XRotation, y: YRotation, z: ZRotation), vector);
            }
        }

        /// <summary>
        /// Convert point in relative coordinates to a point in absolute coordinates.
        /// </summary>
        /// <param name="p">Point in relative coordinates.</param>
        /// <returns></returns>
        private Point ToAbsolute(Point p)
        {
            Vector v = p - Center;
            return new Point(p.X + Center.X, Center.Y - p.Y);
        }

        /// <summary>
        /// Convert a point in absolute coordinates to a point in absolute coordinates.
        /// </summary>
        /// <param name="p">Point in absolute coordinates.</param>
        /// <returns></returns>
        private Point ToRelative(Point p)
        {
            return new Point(p.X - Center.X, Center.Y - p.Y);
        }
        #endregion
    }
}
