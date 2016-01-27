﻿using RajdRed.Models.Base;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RajdRed.Models
{
    public class LinjeModel : RajdElement
    {        
        private double _x1;
        public double X1
        {
            get { return _x1; }
            set { 
                _x1 = value + _nod1.Width/2; 
                OnPropertyChanged("X1");  
            }
        }

        private double _y1;
        public double Y1
        {
            get { return _y1; }
            set { 
                _y1 = value + _nod1.Height/2; 
                OnPropertyChanged("Y1"); 
            }
        }

        private double _x2;
        public double X2
        {
            get { return _x2; }
            set { 
                _x2 = value + _nod2.Width/2; 
                OnPropertyChanged("X2"); 
            }
        }

        private double _y2;
        public double Y2
        {
            get { return _y2; }
            set { 
                _y2 = value + _nod2.Height/2; 
                OnPropertyChanged("Y2"); 
            }
        }

        private NodModelBase _nod1;
        public NodModelBase Nod1
        {
            set 
            { 
                X1 = value.PositionLeft;
                Y1 = value.PositionTop;
                OnPropertyChanged("X1");
                OnPropertyChanged("Y1");
            }
        }

        private NodModelBase _nod2;
        public NodModelBase Nod2
        {
            set
            {
                X2 = value.PositionLeft;
                Y2 = value.PositionTop;
                OnPropertyChanged("X2");
                OnPropertyChanged("Y2");
            }
        }

        public LinjeModel(NodModelBase n1, NodModelBase n2)
        {
            _nod1 = n1;
            _nod2 = n2;

            _nod1.PropertyChanged += (sender, eArgs) => {
                X1 = _nod1.PositionLeft;
                Y1 = _nod1.PositionTop;
            };
            _nod2.PropertyChanged += (sender, eArgs) =>
            {
                X2 = _nod2.PositionLeft;
                Y2 = _nod2.PositionTop;
            };
        }
    }
}
