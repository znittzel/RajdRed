﻿using RajdRed.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RajdRed.Models
{
    public class NodCanvasModel : NodModelBase
    {
        public NodCanvasModel(){}

        public NodCanvasModel(Point p)
        {
            PositionLeft = p.X;
            PositionTop = p.Y;
            Path = NodTypesModel.Association;
        }

        void SetLinje(LinjeModel lm)
        {
            LinjeModel = lm;
        }

        public static NodCanvasModel CopyNod(NodKlassModel n)
        {
            NodCanvasModel ncm = new NodCanvasModel()
            {
                Height = n.Height,
                Width = n.Width,
                IsSelected = n.IsSelected,
                LinjeModel = n.LinjeModel,
                OnField = n.OnField,
                Path = n.Path,
                NodTypesModel = n.NodTypesModel,
                PositionLeft = n.PositionLeft,
                PositionTop = n.PositionTop
            };

            if (n.LinjeModel.Nod1 == n)
                n.LinjeModel.Nod1 = ncm;
            else
                n.LinjeModel.Nod2 = ncm;

            n.LinjeModel.SetOnPropertyChanged();

            ncm.Converted = true;

            return ncm;
        }
    }
}
