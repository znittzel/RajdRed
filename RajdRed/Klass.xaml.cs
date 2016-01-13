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
    /// Interaction logic for Klass.xaml
    /// </summary>
    public partial class Klass : UserControl
    {
        private MainWindow _mainWindow;
        private Canvas _canvas;
        private bool _onField = false;
        private bool _isSelected = false;
        private Point _posOfMouseOnHit;
        private Point _posOfShapeOnHit;

        public List<Nod> _noder = new List<Nod>(); 

        public Klass(MainWindow w, Point pt)
        {
            InitializeComponent();

            _mainWindow = w;
            _canvas = w.getCanvas();

			setKlassColors();

			Canvas.SetLeft(this, pt.X - 50);
			Canvas.SetTop(this, pt.Y - 10);

            MouseMove += Klass_MouseMove;
            MouseUp += Klass_MouseUp;

            _canvas.Children.Add(this);
        }
		public MainWindow MainWindow()
		{
			return _mainWindow;
		}

        /// <summary>
        /// Händer när användaren klickar ned på Klassen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Klass_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.SizeAll;
            if (e.ClickCount == 2)
            {
                if (_onField)
                {
                    openSettingsMenu(e);
                }
            }
            else
            {
                CaptureMouse();
                Point pt = e.GetPosition(_canvas);

                _isSelected = true;
                _canvas.Children.Remove(this);
                _canvas.Children.Add(this);
                _posOfMouseOnHit = pt;
                _posOfShapeOnHit.X = Canvas.GetLeft(this);
                _posOfShapeOnHit.Y = Canvas.GetTop(this);
            }
        }

        /// <summary>
        /// Öppnar egenskapsfönster
        /// </summary>
        /// <param name="e"></param>
        private void openSettingsMenu(MouseEventArgs e)
        {
            Grid g = new Grid() { Width = _canvas.ActualWidth, Height = _canvas.ActualHeight, Background = Brushes.Black, Opacity = 0.2 };
            Canvas.SetLeft(g, 0);
            Canvas.SetTop(g, 0);

            ClassSettings cs = new ClassSettings(this, g);
            Point posOnCanvas = e.GetPosition(_canvas) - _posOfMouseOnHit + _posOfShapeOnHit;
            double x = (posOnCanvas.X + ActualWidth / 2) - cs.Width / 2;
            double y = (posOnCanvas.Y + ActualHeight / 2) - cs.Height / 2;

            if (cs.Width + x > _mainWindow.ActualWidth)
                Canvas.SetLeft(cs, x - (x + cs.Width - _mainWindow.ActualWidth));
            else if (x < 0)
                Canvas.SetLeft(cs, x - x);
            else
                Canvas.SetLeft(cs, x);

            if (cs.Height + y > _mainWindow.ActualHeight)
                Canvas.SetTop(cs, y - (y + cs.Height - _mainWindow.ActualHeight));
            else if (y < 0)
                Canvas.SetTop(cs, y - y);
            else
                Canvas.SetTop(cs, y);

            _canvas.Children.Add(g);
            _canvas.Children.Add(cs);
        }

        /// <summary>
        /// Händer när en Klass flyttas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Klass_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseCaptured && _isSelected != false)
            {
                Point pt = e.GetPosition(_canvas);
                Canvas.SetLeft(this, (pt.X - _posOfMouseOnHit.X) + _posOfShapeOnHit.X);
                Canvas.SetTop(this, (pt.Y - _posOfMouseOnHit.Y) + _posOfShapeOnHit.Y);

                Point posOnCanvas = pt - _posOfMouseOnHit + _posOfShapeOnHit;

                if (_onField && posOnCanvas.Y <= 100)
                    Canvas.SetTop(this, 100.1);

                if (posOnCanvas.X <= 0)
                    Canvas.SetLeft(this, 0.1);

            }
        }

        /// <summary>
        /// Händer när användaren släpper musnedtryckning på Klass
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Klass_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            if (_isSelected == true)
            {
                Point pt = e.GetPosition(_canvas);
                Point posOnCanvas = pt - _posOfMouseOnHit + _posOfShapeOnHit;

                if (posOnCanvas.Y <= 100 && !_onField)
                {
                    Delete();
                }
                else
                {
                    _onField = true;
                }
            }

            _isSelected = false;
        }

        /// <summary>
        /// Tar bort Klass
        /// </summary>
        public void Delete()
        {
            _mainWindow.DeleteKlass(this);
        }

        /// <summary>
        /// Stänger egenskapsfönstret
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="g"></param>
        public void CloseSettings(ClassSettings cs, Grid g)
        {
            _canvas.Children.Remove(cs);
            _canvas.Children.Remove(g);
        }

        /// <summary>
        /// Sparar egenskaper
        /// </summary>
        /// <param name="cs"></param>
        public void Save(ClassSettings cs)
        {
            ClassName.Content = cs.ClassName.Text;
            Attributes.Text = cs.Attributes.Text;
            Methods.Text = cs.Methods.Text;
        }

        /// <summary>
        /// VISAR yttre område på Klass från var användaren drar en linje ifrån
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InnerGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            OuterBorder.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// GÖMMER yttre område på Klass från var användaren drar en linje ifrån
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OuterGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            OuterBorder.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Ändrar muspekaren till ett plus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OuterBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Cross;
        }

        /// <summary>
        /// Hämtar alla noder kopplade till Klassen
        /// </summary>
        /// <returns></returns>
        public List<Nod> GetNods()
        {
            return _noder;
        }

        /// <summary>
        /// MouseDown-Event för OuterBorder. Händer när användaren försöker dra en linje från klassen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OuterBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {   
            Point p = e.GetPosition(this);
            OnSide os = getSideByPoint(p);
            if (os != OnSide.Corner)
            {
                Nod node = new Nod(this, os, new Point(p.X - 5, p.Y - 5));
                _noder.Add(node);
                this.OuterGrid.Children.Add(node);
            }
        }

        /// <summary>
        /// Returnerar vilken sida ett musklick befinner sig om Klassen. Musklicket måste ske på yttre området
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private OnSide getSideByPoint(Point p)
        {
            if (p.X <= InnerBorder.Margin.Left 
                && (p.Y >= InnerBorder.Margin.Left && p.Y < this.ActualHeight-InnerBorder.Margin.Left))
            {
                return OnSide.Left;
            }
            else if (p.X >= this.ActualWidth-InnerBorder.Margin.Left 
                && (p.Y >= InnerBorder.Margin.Left && p.Y < this.ActualHeight-InnerBorder.Margin.Left)) {
                    return OnSide.Right;
                }
            else if (p.Y >= this.ActualHeight - InnerBorder.Margin.Left
              && (p.X >= InnerBorder.Margin.Left && p.X < this.ActualWidth - InnerBorder.Margin.Left)
                )
            {
                return OnSide.Bottom;
            }
            else if (p.Y <= InnerBorder.Margin.Left
              && (p.X >= InnerBorder.Margin.Left && p.X < this.ActualWidth - InnerBorder.Margin.Left))
            {
                return OnSide.Top;
            }

            return OnSide.Corner;
        }

		public void setKlassColors()
		{
			var bc = new BrushConverter();

			bgTopRow.SetCurrentValue(Control.BackgroundProperty, (Brush)bc.ConvertFrom(MainWindow().Colors.KlassNameBg));
			bgMidRow.SetCurrentValue(Control.BackgroundProperty, (Brush)bc.ConvertFrom(MainWindow().Colors.KlassAttributesBg));
			bgBotRow.SetCurrentValue(Control.BackgroundProperty, (Brush)bc.ConvertFrom(MainWindow().Colors.KlassMethodsBg));
		}
    }
}
