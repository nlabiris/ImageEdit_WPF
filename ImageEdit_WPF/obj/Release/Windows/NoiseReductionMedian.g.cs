﻿#pragma checksum "..\..\..\Windows\NoiseReductionMedian.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5DAB48D15E3DB29D5C6B58C1E7370DC7"
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
    /// NoiseReductionMedian
    /// </summary>
    public partial class NoiseReductionMedian : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 36 "..\..\..\Windows\NoiseReductionMedian.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton three;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Windows\NoiseReductionMedian.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton five;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Windows\NoiseReductionMedian.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton seven;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Windows\NoiseReductionMedian.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ok;
        
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
            System.Uri resourceLocater = new System.Uri("/ImageEdit_WPF;component/windows/noisereductionmedian.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\NoiseReductionMedian.xaml"
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
            this.three = ((System.Windows.Controls.RadioButton)(target));
            
            #line 36 "..\..\..\Windows\NoiseReductionMedian.xaml"
            this.three.Checked += new System.Windows.RoutedEventHandler(this.three_Checked);
            
            #line default
            #line hidden
            return;
            case 2:
            this.five = ((System.Windows.Controls.RadioButton)(target));
            
            #line 37 "..\..\..\Windows\NoiseReductionMedian.xaml"
            this.five.Checked += new System.Windows.RoutedEventHandler(this.five_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.seven = ((System.Windows.Controls.RadioButton)(target));
            
            #line 38 "..\..\..\Windows\NoiseReductionMedian.xaml"
            this.seven.Checked += new System.Windows.RoutedEventHandler(this.seven_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ok = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\Windows\NoiseReductionMedian.xaml"
            this.ok.Click += new System.Windows.RoutedEventHandler(this.ok_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

