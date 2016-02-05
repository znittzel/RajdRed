﻿using RajdRed.Models;
using RajdRed.Repositories;
using RajdRed.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RajdRed.ViewModels
{
    public class TextBoxViewModel
    {
        public TextBoxModel TextBoxModel { get; set; }
        public TextBoxRepository TextBoxRepository { get; set; }
        public TextBoxView TextBoxView { get; set; }
        public TextBoxViewModel(Point p, TextBoxRepository tbr)
        {
            TextBoxModel = new TextBoxModel(p, this)
            {
                Text = "New text *"
            };
            TextBoxRepository = tbr;
        }

        public void SetView(TextBoxView t)
        {
            TextBoxView = t;
        }
    }
}