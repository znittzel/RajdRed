﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RajdRed.Models.Base
{
    public abstract class RajdElement : BaseModel
    {
        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("Selected"); } }
        public bool OnField { get; set; }

        public Visibility Selected
        {
            get 
            { 
                if (_isSelected) 
                    return Visibility.Visible;  
                else 
                    return Visibility.Hidden; 
            }
        }
    }
}
