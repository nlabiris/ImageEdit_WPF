﻿#pragma checksum "..\..\..\Windows\Histogram.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B5B52117531B545F987A0144F2D7E76E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ImageEdit_WPF.Windows {
    
    
    /// <summary>
    /// Histogram
    /// </summary>
    public partial class Histogram : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 38 "..\..\..\Windows\Histogram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton gray;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Windows\Histogram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton red;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Windows\Histogram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton green;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Windows\Histogram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton blue;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Windows\Histogram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox groupBox;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Windows\Histogram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Polygon polygon;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\Windows\Histogram.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock meanValue;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ImageEdit_WPF;component/windows/histogram.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\Histogram.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.gray = ((System.Windows.Controls.RadioButton)(target));
            
            #line 38 "..\..\..\Windows\Histogram.xaml"
            this.gray.Checked += new System.Windows.RoutedEventHandler(this.gray_Checked);
            
            #line default
            #line hidden
            return;
            case 2:
            this.red = ((System.Windows.Controls.RadioButton)(target));
            
            #line 39 "..\..\..\Windows\Histogram.xaml"
            this.red.Checked += new System.Windows.RoutedEventHandler(this.red_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.green = ((System.Windows.Controls.RadioButton)(target));
            
            #line 40 "..\..\..\Windows\Histogram.xaml"
            this.green.Checked += new System.Windows.RoutedEventHandler(this.green_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.blue = ((System.Windows.Controls.RadioButton)(target));
            
            #line 41 "..\..\..\Windows\Histogram.xaml"
            this.blue.Checked += new System.Windows.RoutedEventHandler(this.blue_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.groupBox = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 6:
            this.polygon = ((System.Windows.Shapes.Polygon)(target));
            return;
            case 7:
            this.meanValue = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

