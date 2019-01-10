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
        #endregion

        #region commands
        private ICommand fireBall;
        private ICommand setSpeed;
        private ICommand setAim;
        private ICommand dropBall;
        private ICommand startStop;
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
        public ICommand UpdatePos
        {
            get
            {
                if (updatePos == null)
                    updatePos = new BaseCommand<Point>((p) => { BallPos = p - new Vector(ballSize, ballSize); }, (p) => Zone.Contains(p) && TargetSelection);
                return updatePos;
            }
        }

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
        public Rect Zone
        {
            get
            {
                Point pt1 = ToAbsolute(new Point(TableWidth / 2, TableHeight / 2));
                Point pt2 = ToAbsolute(new Point(TableWidth, -TableHeight / 2));
                return new Rect(pt1, pt2);
            }
        }

        public Rect RelativeZone
        {
            get
            {
                Point pt1 = new Point(TableWidth / 2, TableHeight / 2);
                Point pt2 = new Point(TableWidth, -TableHeight / 2);
                return new Rect(pt1, pt2);
            }
        }

        public Point Center
        {
            get
            {
                return new Point(0, TableHeight / 2);
            }
        }

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

        public Point3D Ball2DPos
        {
            get => Points[T];
        }

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

        public bool TargetSelection
        {
            get => targetSelection; set
            {
                targetSelection = value;
                Notify();
            }
        }

        public double BallisticAngle
        {
            get => ballisticAngle; set
            {
                if (ballisticAngle != value)
                {
                    ballisticAngle = value;
                    Notify();
                    Notify("BallPos");
                }
            }
        }

        private void LoadDatas()
        {
            object[] datas = GetDatas(BallSpeed, BallisticAngle, Angle, new Vector3D(XRotation, YRotation, ZRotation));
            Points = (List<Point3D>)datas[0];
            Speeds = (List<Vector3D>)datas[1];
            Omegas = (List<Vector3D>)datas[2];
        }

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

        public List<Point3D> Points
        {
            get => points; set
            {
                points = value;
                Notify();
            }
        }

        public double XRotation
        {
            get => xRotation; set
            {
                xRotation = value;
                Notify();
                Notify("BallPos");
            }
        }

        public double YRotation
        {
            get => yRotation; set
            {
                yRotation = value;
                Notify();
                Notify("BallPos");
            }
        }

        public double ZRotation
        {
            get => zRotation; set
            {
                zRotation = value;
                Notify();
                Notify("BallPos");
            }
        }

        public ObservableCollection<string> Ports
        {
            get => ports; set
            {
                ports = value;
                Notify();
            }
        }

        public Robot Robot
        {
            get => robot; set
            {
                robot = value;
                Notify();
            }
        }

        public double AnimationSpeed
        {
            get => animationSpeed;
            set
            {
                animationSpeed = value;
                Notify();
            }
        }

        public List<Vector3D> Speeds
        {
            get => speeds;
            set
            {
                speeds = value;
                Notify();
            }
        }

        public List<Vector3D> Omegas
        {
            get => omegas;
            set
            {
                omegas = value;
                Notify();
            }
        }
        #endregion

        #region methods
        public void Start()
        {
            LoadDatas();
            stopwatch.Reset();
            stopwatch.Start();
            (FireBall as BaseCommand).OnChanged();
        }
        Stopwatch stopwatch = new Stopwatch();

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (stopwatch.IsRunning)
                if ((T + 1) >= points.Count)
                {
                    MainWindow.DipThread.Invoke(stopwatch.Reset, DispatcherPriority.Send);
                    T = 0;
                }
                else
                {
                    try
                    {
                        MainWindow.DipThread.Invoke(() =>
                    {
                        int pt = T;
                        T = (int)(stopwatch.ElapsedMilliseconds * AnimationSpeed);
                        try
                        {
                            double y = 0;
                            Point3D pt1 = new Point3D();
                            Rect rect = new Rect(new Point(0, -TableHeight / 2), new Point(TableWidth, TableHeight / 2));
                            if (T < points.Count && (y = points.GetRange(pt, T - pt).Min(p => p.Y)) <= 0 &&
                            rect.Contains(new Point((pt1 = points.Last(p => p.Y == y)).X, pt1.Y)))
                                player.Play();
                        }
                        catch (InvalidOperationException) { }
                    }, DispatcherPriority.Send);
                    }
                    catch { }
                }
        }

        SoundPlayer player;
        private bool _continue = true;

        private void CalculateParameters(Vector vector)
        {
            double length = vector.Length;
            if (GetLength(BallSpeed, BallisticAngle, new Vector3D(XRotation, YRotation, ZRotation)) != length)
            {
                double len = GetLength(BallSpeed, maxAngle, new Vector3D(XRotation, YRotation, ZRotation));
                if (len == length)
                {
                    BallisticAngle = maxAngle;
                }
                else if (len < length)
                {
                    BallisticAngle = maxAngle;
                    BallSpeed = GetSpeed(length, maxAngle, new Vector3D(XRotation, YRotation, ZRotation));
                }
                else if ((len = GetLength(BallSpeed, minAngle, new Vector3D(XRotation, YRotation, ZRotation))) == length)
                {
                    BallisticAngle = minAngle;
                }
                else if (len > length)
                {
                    BallisticAngle = minAngle;
                    BallSpeed = GetSpeed(length, minAngle, new Vector3D(XRotation, YRotation, ZRotation));
                }
                else
                {
                    BallisticAngle = GetAngle(length, BallSpeed, new Vector3D(XRotation, YRotation, ZRotation));
                }
            }
            if (GetPosition(BallSpeed, BallisticAngle, Angle, new Vector3D(XRotation, YRotation, ZRotation)).Y != vector.Y)
            {
                Angle = GetLowAngle(BallisticAngle, BallSpeed, new Vector3D(XRotation, y: YRotation, z: ZRotation), vector);
            }
        }

        private Point ToAbsolute(Point p)
        {
            Vector v = p - Center;
            return new Point(p.X + Center.X, Center.Y - p.Y);
        }

        private Point ToRelative(Point p)
        {
            return new Point(p.X - Center.X, Center.Y - p.Y);
        }
        #endregion
    }
}
