using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ProjetSI.Ballistique;

namespace ProjetSI
{
    /// <summary>
    /// Logique d'interaction pour Table2D.xaml
    /// </summary>
    public partial class Table2D : UserControl, INotifyPropertyChanged
    {
        public Table2D()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update the position in the textblock.
        /// </summary>
        public ICommand UpdatePos
        {
            get { return (ICommand)GetValue(UpdatePosProperty); }
            set { SetValue(UpdatePosProperty, value); }
        }

        public static readonly DependencyProperty UpdatePosProperty =
            DependencyProperty.Register("UpdatePos", typeof(ICommand), typeof(Table2D), new PropertyMetadata());

        private Point mousePos;

        /// <summary>
        /// Mouse 2D position.
        /// </summary>
        public Point MousePos
        {
            get => mousePos;
            set { mousePos = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MousePos")); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = MousePos;
            if(UpdatePos.CanExecute(p))
            {
                UpdatePos.Execute(p);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition((IInputElement)sender);
            MousePos = p;
        }
    }
}
