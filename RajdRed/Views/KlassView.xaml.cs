﻿using RajdRed.Models;
using RajdRed.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RajdRed.Views
{
    /// <summary>
    /// Interaction logic for KlassView.xaml
    /// </summary>
    public partial class KlassView : UserControl
    {
        public KlassViewModel KlassViewModel { get { return DataContext as KlassViewModel; } }
		private Point _posOnUserControlOnHit;
        
		public KlassView()
        {
            InitializeComponent();
            Loaded += (sender, eArgs) => {
                if (!KlassViewModel.KlassModel.OnField)
                {
                    CaptureMouse();
                    KlassViewModel.SetKlassView(this);
					_posOnUserControlOnHit = new Point(ActualWidth / 2, ActualHeight / 2);
					KlassViewModel.KlassModel.PositionLeft = KlassViewModel.KlassModel.PositionLeft - (ActualWidth / 2);
					KlassViewModel.KlassModel.PositionTop = KlassViewModel.KlassModel.PositionTop - (ActualHeight / 2);

                    KlassViewModel.SetAdornerLayer();
                }
            };
        }

        public void Innerborder_MouseDown(object sender, MouseButtonEventArgs e)
        {
			CaptureMouse();
			_posOnUserControlOnHit = Mouse.GetPosition(this);
			KlassViewModel.KlassModel.IsSelected = true;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseCaptured && KlassViewModel.KlassModel.IsSelected)
            {
                Point p = e.GetPosition(Application.Current.MainWindow);
				SetValue(Canvas.LeftProperty, p.X - _posOnUserControlOnHit.X);
                SetValue(Canvas.TopProperty, p.Y - _posOnUserControlOnHit.Y);

				if (KlassViewModel.KlassModel.OnField && ((p.Y - _posOnUserControlOnHit.Y) <= 100.5))
					Canvas.SetTop(this, 100.6);

				if ((p.X - _posOnUserControlOnHit.X) <= 0.5)
					Canvas.SetLeft(this, 0.6);
            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

			Point p = e.GetPosition(Application.Current.MainWindow);
			Point posOfUserControlOnCanvas = new Point();
			posOfUserControlOnCanvas.X = p.X - _posOnUserControlOnHit.X;
			posOfUserControlOnCanvas.Y = p.Y - _posOnUserControlOnHit.Y;

			if (posOfUserControlOnCanvas.Y <= 100 && !KlassViewModel.KlassModel.OnField)
				KlassViewModel.Delete();
			else
				KlassViewModel.KlassModel.OnField = true;

			KlassViewModel.KlassModel.IsSelected = false;
        }
    }
}
