/*
Basic image processing software
<https://github.com/nlabiris/ImageEdit_WPF>

Copyright (C) 2015  Nikos Labiris

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/



using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageEdit_WPF
{
    public class ZoomBorder : Border
    {
        private UIElement _child = null;
        private bool _isStillDownLeft = false;
        private bool _isStillDownMiddle = false;
        private Point _origin;
        private Point _start;

        private static TranslateTransform GetTranslateTransform(UIElement element)
        {
            return (TranslateTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is TranslateTransform);
        }

        private static ScaleTransform GetScaleTransform(UIElement element)
        {
            return (ScaleTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is ScaleTransform);
        }

        public override UIElement Child
        {
            get
            {
                return base.Child;
            }
            set
            {
                if (value != null && value != this.Child)
                {
                    this.Initialize(value);
                }
                base.Child = value;
            }
        }

        public void Initialize(UIElement element)
        {
            this._child = element;
            if (_child != null)
            {
                TransformGroup group = new TransformGroup();
                ScaleTransform st = new ScaleTransform();
                group.Children.Add(st);
                TranslateTransform tt = new TranslateTransform();
                group.Children.Add(tt);
                _child.RenderTransform = group;
                _child.RenderTransformOrigin = new Point(0.0, 0.0);
                this.MouseDown += child_MouseDown;
                this.MouseUp += child_MouseUp;
                this.MouseWheel += child_MouseWheel;
                this.MouseMove += child_MouseMove;
                this.PreviewMouseRightButtonDown += new MouseButtonEventHandler(child_PreviewMouseRightButtonDown);
            }
        }

        private void child_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isStillDownLeft = false;
            _isStillDownMiddle = false;

            if (_child != null)
            {
                if (e.ChangedButton == MouseButton.Middle)
                {
                    _child.ReleaseMouseCapture();
                    this.Cursor = Cursors.Cross;
                }
                if (e.ChangedButton == MouseButton.Left)
                {
                    this.Cursor = Cursors.Cross;
                }
            }
        }

        private void child_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_child != null)
            {
                if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
                {
                    TranslateTransform tt = GetTranslateTransform(_child);
                    _start = e.GetPosition(this);
                    _origin = new Point(tt.X, tt.Y);
                    this.Cursor = Cursors.SizeAll;
                    _child.CaptureMouse();
                    _isStillDownMiddle = true;
                }
                else if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
                {
                    //TranslateTransform tt = GetTranslateTransform(child);
                    _start = e.GetPosition(this);
                    //origin = new Point(tt.X, tt.Y);
                    this.Cursor = Cursors.Cross;
                    
                    _isStillDownLeft = true;
                }
            }
        }

        public void Reset()
        {
            if (_child != null)
            {
                // reset zoom
                var st = GetScaleTransform(_child);
                st.ScaleX = 1.0;
                st.ScaleY = 1.0;

                // reset pan
                var tt = GetTranslateTransform(_child);
                tt.X = 0.0;
                tt.Y = 0.0;
            }
        }

        private void child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_child != null)
            {
                ScaleTransform st = GetScaleTransform(_child);
                TranslateTransform tt = GetTranslateTransform(_child);

                double zoom = e.Delta > 0 ? .2 : -.2;
                if (!(e.Delta > 0) && (st.ScaleX < .4 || st.ScaleY < .4))
                    return;

                Point relative = e.GetPosition(_child);
                double abosuluteX;
                double abosuluteY;

                abosuluteX = relative.X * st.ScaleX + tt.X;
                abosuluteY = relative.Y * st.ScaleY + tt.Y;

                st.ScaleX += zoom;
                st.ScaleY += zoom;

                tt.X = abosuluteX - relative.X * st.ScaleX;
                tt.Y = abosuluteY - relative.Y * st.ScaleY;
            }
        }

        void child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Reset();
        }

        private void child_MouseMove(object sender, MouseEventArgs e)
        {
            if (_child != null)
            {
                if (_isStillDownMiddle)
                {
                    TranslateTransform tt = GetTranslateTransform(_child);
                    Vector v = _start - e.GetPosition(this);
                    tt.X = _origin.X - v.X;
                    tt.Y = _origin.Y - v.Y;
                    //rect.Width = (int)tt.X;
                    //rect.Height = (int)tt.Y;
                }
                else if (_isStillDownLeft)
                {
                    Point pos = e.GetPosition(this);

                    double x = Math.Min(pos.X, _start.X);
                    double y = Math.Min(pos.Y, _start.Y);

                    double w = Math.Max(pos.X, _start.X) - x;
                    double h = Math.Max(pos.Y, _start.Y) - y;

                    //rect.Width = w;
                    //rect.Height = h;
                }
            }
        }
    }
}
