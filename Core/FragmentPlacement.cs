﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core;

namespace VideoEditor.Core
{
    public class FragmentPlacement
    {
        public Fragment Fragment { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public TimeSpan Position { get; set; } // Start time in milliseconds
        public TimeSpan EndPosition => Position + Fragment.Duration; // End time in milliseconds
        public FragmentPlacement(Fragment fragment, TimeSpan position, double x, double y)
        {
            Fragment = fragment;
            Position = position;
            X = x;
            Y = y;
        }
        public FragmentPlacement(Fragment fragment, TimeSpan position): this(fragment, position, 0, 0) { }
        public FragmentPlacement(Fragment fragment) : this(fragment, TimeSpan.Zero) { }

    }
}
