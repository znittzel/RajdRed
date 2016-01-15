﻿using System;
using System.Collections.Generic;
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

namespace RajdRed
{
    /// <summary>
    /// Interaction logic for Nod.xaml
    /// </summary>
    /// 
    public enum OnSide
    {
        Left,
        Right,
        Top,
        Bottom,
        Corner
    }

    public partial class Nod : UserControl
    {
        private Klass _klass = null;
        private Linje _linje = null;
        private Shape _shape;
        private OnSide _onSide;
        private Point _nodPos;

        public Nod() 
        {
            InitializeComponent();
            OuterGrid.Children.Add(_shape);
        }

        public Nod(Linje l) {
            InitializeComponent();
            _linje = l;


        }

        /// <summary>
        /// Nodens konstruktor om den skapas bunden till en klass
        /// </summary>
        /// <param name="k"></param>
        /// <param name="os"></param>
        /// <param name="p"></param>
        public Nod(Klass k)
        {
            InitializeComponent();
            //_onSide = os;
            _klass = k;

            TurnToNode();
            this.OuterGrid.Children.Add(_shape);

            //PositionOfNod(p);
            //SetPositionWithMargin();
        }

        /// <summary>
        /// Position relativt Canvas:en
        /// </summary>
        /// <returns></returns>
        public Point Position()
        {
            return new Point(
                    Canvas.GetLeft(_klass.MainWindow().getCanvas()), 
                    Canvas.GetTop(_klass.MainWindow().getCanvas())
                );
        }

        //public void PositionOfNod(Point p)
        //{
        //    _nodPos.X = p.X / _klass.MinWidth;
        //    _nodPos.Y = p.Y / _klass.MinHeight;
        //}

        /// <summary>
        /// Sätter noden på rätt position runt en klass
        /// </summary>
        /// <param name="p"></param>
        //public void SetPositionWithMargin()
        //{
        //    switch (_onSide) {
        //        case OnSide.Left:
        //            this.Margin = new Thickness(0, _nodPos.Y * _klass.MinHeight, 0, 0);
        //            this.HorizontalAlignment = HorizontalAlignment.Left;
        //            this.VerticalAlignment = VerticalAlignment.Top;
        //            break;
        //        case OnSide.Right:
        //            this.Margin = new Thickness(0, _nodPos.Y * _klass.MinHeight, 0, 0);
        //            this.HorizontalAlignment = HorizontalAlignment.Right;
        //            this.VerticalAlignment = VerticalAlignment.Top;
        //            break;
        //        case OnSide.Top:
        //            this.Margin = new Thickness(_nodPos.X * _klass.MinWidth, 0, 0, 0);
        //            this.HorizontalAlignment = HorizontalAlignment.Left;
        //            this.VerticalAlignment = VerticalAlignment.Top;
        //            break;
        //        case OnSide.Bottom:
        //            this.Margin = new Thickness(_nodPos.X * _klass.MinWidth, 0, 0, 0);
        //            this.HorizontalAlignment = HorizontalAlignment.Left;
        //            this.VerticalAlignment = VerticalAlignment.Bottom;
        //            break;
        //    }
        //}

        /// <summary>
        /// Returnerar om noden är bunden till en klass
        /// </summary>
        /// <returns></returns>
        public bool IsBindToKlass()
        {
            return (_klass != null ? true : false);
        }

        /// <summary>
        /// Returnerar om noden är bunden till en extern linje
        /// </summary>
        /// <returns></returns>
        public bool IsBindToLinje()
        {
            //return (_linje != null ? true : false);
            return false;
        }

        /// <summary>
        /// Ändrar noden till en association
        /// </summary>
        public void TurnToAssociation()
        {
            _shape = new Ellipse() {Stroke = Brushes.Black, StrokeThickness = 1};

        }

        /// <summary>
        /// Ändrar noden till ett arv
        /// </summary>
        public void TurnToGeneralization()
        {
            if (_onSide == OnSide.Bottom)
            {
                _shape = new Polygon()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Points = new PointCollection() { 
                    new Point(this.Width/2, 0), 
                    new Point(this.Width, this.Height), 
                    new Point(0, this.Height) 
                    }
                };
            }
            
        }

        /// <summary>
        /// Ändrar noden till ett aggregat eller komposition (om fylld)
        /// </summary>
        /// <param name="filled"></param>
        public void TurnToAggregation(bool filled)
        {
            _shape = new Polygon() { 
                Stroke=Brushes.Black, 
                StrokeThickness = 1,
                Points = new PointCollection()
                {
                    new Point(this.Width/2, 0),
                    new Point(this.Width, this.Height/2),
                    new Point(this.Width/2, this.Height),
                    new Point(0, this.Height/2)
                }
            };

            if (filled)
            {
                _shape.Fill = Brushes.Black;
            }

        }

        public void TurnToNode()
        {
            _shape = new Polygon()
            {
                Points = new PointCollection() {
                    new Point(2,5),
                    new Point(8,5),
                    new Point(5,5),
                    new Point(5,2),
                    new Point(5,8)
                },
                StrokeThickness = 1,
                Stroke = Brushes.Gray
            };
        }

        /// <summary>
        /// Returnerar _onSide
        /// </summary>
        /// <returns></returns>
        public OnSide GetOnSide()
        {
            return _onSide;
        }

        private void OuterGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            OuterEllipse.Visibility = Visibility.Visible;
        }

        private void OuterGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            OuterEllipse.Visibility = Visibility.Hidden;
        }

        private void OuterGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _klass.SetNode(this);
            _klass.NodeGrid.Visibility = Visibility.Hidden;
            _linje = new Linje(this, e.GetPosition(_klass.MainWindow().getCanvas()));
        }

        public Klass GetKlass()
        {
            return _klass;
        }


    }
}
