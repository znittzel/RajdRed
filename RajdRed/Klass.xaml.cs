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
        private Canvas canvas;
        private bool onField = false;
        public List<string> Colors = new List<string>() { "#222931", "#323a45" };
        private bool _isSelected = false;
        private Point _posOfMouseOnHit;
        private Point _posOfShapeOnHit;

        public List<Nod> Noder = new List<Nod>(); 

        public Klass(MainWindow w, string name)
        {
            InitializeComponent();

            _mainWindow = w;
            canvas = w.getCanvas();

            MouseDown += Klass_MouseDown;
            MouseMove += Klass_MouseMove;
            MouseUp += Klass_MouseUp;

            canvas.Children.Add(this);
        }

        public void Klass_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Grid g = new Grid() { Width = canvas.ActualWidth, Height = canvas.ActualHeight, Background = Brushes.Black, Opacity = 0.2 };
                Canvas.SetLeft(g, 0);
                Canvas.SetTop(g, 0);

                ClassSettings cs = new ClassSettings(this, g);
                Point posOnCanvas = e.GetPosition(canvas) - _posOfMouseOnHit + _posOfShapeOnHit;
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

                canvas.Children.Add(g);
                canvas.Children.Add(cs);
            }
            else
            {
                CaptureMouse();
                Point pt = e.GetPosition(canvas);

                _isSelected = true;
                canvas.Children.Remove(this);
                canvas.Children.Add(this);
                _posOfMouseOnHit = pt;
                _posOfShapeOnHit.X = Canvas.GetLeft(this);
                _posOfShapeOnHit.Y = Canvas.GetTop(this);
            }
        }

        public void Klass_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseCaptured && _isSelected != false)
            {
                Point pt = e.GetPosition(canvas);
                Canvas.SetLeft(this, (pt.X - _posOfMouseOnHit.X) + _posOfShapeOnHit.X);
                Canvas.SetTop(this, (pt.Y - _posOfMouseOnHit.Y) + _posOfShapeOnHit.Y);

                Point posOnCanvas = pt - _posOfMouseOnHit + _posOfShapeOnHit;

                if (onField && posOnCanvas.Y <= 100)
                    Canvas.SetTop(this, 100.1);

                if (posOnCanvas.X <= 0)
                    Canvas.SetLeft(this, 0.1);

            }
        }

        private void Klass_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            if (_isSelected == true)
            {
                Point pt = e.GetPosition(canvas);
                Point posOnCanvas = pt - _posOfMouseOnHit + _posOfShapeOnHit;

                if (posOnCanvas.Y <= 100 && !onField)
                {
                    Delete();
                }
                else
                {
                    onField = true;
                }
            }

            _isSelected = false;
        }

        public void Delete()
        {
            _mainWindow.DeleteKlass(this);
        }

        public void CloseSettings(ClassSettings cs, Grid g)
        {
            canvas.Children.Remove(cs);
            canvas.Children.Remove(g);
        }

        public void Save(ClassSettings cs)
        {
            ClassName.Content = cs.ClassName.Text;
            Attributes.Text = cs.Attributes.Text;
            Methods.Text = cs.Methods.Text;
        }
    }
}