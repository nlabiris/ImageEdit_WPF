﻿#pragma checksum "..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F1C16157AD55876CA13CC6D4D43E783F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ImageEdit_WPF.Commands;
using ImageEdit_WPF.HelperClasses;
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


namespace ImageEdit_WPF {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 49 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Menu menuBar;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem open;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem reopen;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem save;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem saveAs;
        
        #line default
        #line hidden
        
        /// <summary>
        /// undo Name Field
        /// </summary>
        
        #line 58 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public System.Windows.Controls.MenuItem undo;
        
        #line default
        #line hidden
        
        /// <summary>
        /// redo Name Field
        /// </summary>
        
        #line 59 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public System.Windows.Controls.MenuItem redo;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem information;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem histogram;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem shiftBits;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem threshold;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem autoThreshold;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem contrastEnhancement;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem brightness;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem contrast;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem negative;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem squareRoot;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem grayscale;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem sepia;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem cartoonEffect;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem oilPaintEffect;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem histogramEqualization;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem histogramEqualizationRGB;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem histogramEqualizationHSV;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem histogramEqualizationYUV;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem sobel;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem gradientBased;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem gaussianBlur;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem sharpen;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem noiseColor;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem noiseBW;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem noiseReductionMean;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem noiseReductionMedian;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem preferences;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem statusBarShowHide;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem help;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem about;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ImageEdit_WPF.HelperClasses.ZoomBorder border;
        
        #line default
        #line hidden
        
        /// <summary>
        /// mainImage Name Field
        /// </summary>
        
        #line 108 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public System.Windows.Controls.Image mainImage;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.StatusBar statusBar;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock imageResolution;
        
        #line default
        #line hidden
        
        
        #line 127 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Separator separatorFirst;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock imageSize;
        
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
            System.Uri resourceLocater = new System.Uri("/ImageEdit_WPF;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 28 "..\..\MainWindow.xaml"
            ((ImageEdit_WPF.MainWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 32 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.open_Executed);
            
            #line default
            #line hidden
            
            #line 32 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.open_CanExecute);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 33 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.save_Executed);
            
            #line default
            #line hidden
            
            #line 33 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.save_CanExecute);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 34 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.Information_Executed);
            
            #line default
            #line hidden
            
            #line 34 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Information_CanExecute);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 35 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.Help_Executed);
            
            #line default
            #line hidden
            
            #line 35 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Help_CanExecute);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 36 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.Undo_Executed);
            
            #line default
            #line hidden
            
            #line 36 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Undo_CanExecute);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 37 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.Redo_Executed);
            
            #line default
            #line hidden
            
            #line 37 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Redo_CanExecute);
            
            #line default
            #line hidden
            return;
            case 8:
            this.menuBar = ((System.Windows.Controls.Menu)(target));
            return;
            case 9:
            this.open = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 10:
            this.reopen = ((System.Windows.Controls.MenuItem)(target));
            
            #line 52 "..\..\MainWindow.xaml"
            this.reopen.Click += new System.Windows.RoutedEventHandler(this.reopen_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.save = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 12:
            this.saveAs = ((System.Windows.Controls.MenuItem)(target));
            
            #line 55 "..\..\MainWindow.xaml"
            this.saveAs.Click += new System.Windows.RoutedEventHandler(this.saveAs_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.undo = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 14:
            this.redo = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 15:
            this.information = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 16:
            this.histogram = ((System.Windows.Controls.MenuItem)(target));
            
            #line 64 "..\..\MainWindow.xaml"
            this.histogram.Click += new System.Windows.RoutedEventHandler(this.histogram_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            this.shiftBits = ((System.Windows.Controls.MenuItem)(target));
            
            #line 65 "..\..\MainWindow.xaml"
            this.shiftBits.Click += new System.Windows.RoutedEventHandler(this.shiftBits_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            this.threshold = ((System.Windows.Controls.MenuItem)(target));
            
            #line 66 "..\..\MainWindow.xaml"
            this.threshold.Click += new System.Windows.RoutedEventHandler(this.threshold_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            this.autoThreshold = ((System.Windows.Controls.MenuItem)(target));
            
            #line 67 "..\..\MainWindow.xaml"
            this.autoThreshold.Click += new System.Windows.RoutedEventHandler(this.autoThreshold_Click);
            
            #line default
            #line hidden
            return;
            case 20:
            this.contrastEnhancement = ((System.Windows.Controls.MenuItem)(target));
            
            #line 68 "..\..\MainWindow.xaml"
            this.contrastEnhancement.Click += new System.Windows.RoutedEventHandler(this.contrastEnhancement_Click);
            
            #line default
            #line hidden
            return;
            case 21:
            this.brightness = ((System.Windows.Controls.MenuItem)(target));
            
            #line 69 "..\..\MainWindow.xaml"
            this.brightness.Click += new System.Windows.RoutedEventHandler(this.brightness_Click);
            
            #line default
            #line hidden
            return;
            case 22:
            this.contrast = ((System.Windows.Controls.MenuItem)(target));
            
            #line 70 "..\..\MainWindow.xaml"
            this.contrast.Click += new System.Windows.RoutedEventHandler(this.contrast_Click);
            
            #line default
            #line hidden
            return;
            case 23:
            this.negative = ((System.Windows.Controls.MenuItem)(target));
            
            #line 73 "..\..\MainWindow.xaml"
            this.negative.Click += new System.Windows.RoutedEventHandler(this.negative_Click);
            
            #line default
            #line hidden
            return;
            case 24:
            this.squareRoot = ((System.Windows.Controls.MenuItem)(target));
            
            #line 74 "..\..\MainWindow.xaml"
            this.squareRoot.Click += new System.Windows.RoutedEventHandler(this.squareRoot_Click);
            
            #line default
            #line hidden
            return;
            case 25:
            this.grayscale = ((System.Windows.Controls.MenuItem)(target));
            
            #line 75 "..\..\MainWindow.xaml"
            this.grayscale.Click += new System.Windows.RoutedEventHandler(this.grayscale_Click);
            
            #line default
            #line hidden
            return;
            case 26:
            this.sepia = ((System.Windows.Controls.MenuItem)(target));
            
            #line 76 "..\..\MainWindow.xaml"
            this.sepia.Click += new System.Windows.RoutedEventHandler(this.Sepia_OnClick);
            
            #line default
            #line hidden
            return;
            case 27:
            this.cartoonEffect = ((System.Windows.Controls.MenuItem)(target));
            
            #line 77 "..\..\MainWindow.xaml"
            this.cartoonEffect.Click += new System.Windows.RoutedEventHandler(this.CartoonEffect_OnClick);
            
            #line default
            #line hidden
            return;
            case 28:
            this.oilPaintEffect = ((System.Windows.Controls.MenuItem)(target));
            
            #line 78 "..\..\MainWindow.xaml"
            this.oilPaintEffect.Click += new System.Windows.RoutedEventHandler(this.OilPaintEffect_OnClick);
            
            #line default
            #line hidden
            return;
            case 29:
            this.histogramEqualization = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 30:
            this.histogramEqualizationRGB = ((System.Windows.Controls.MenuItem)(target));
            
            #line 80 "..\..\MainWindow.xaml"
            this.histogramEqualizationRGB.Click += new System.Windows.RoutedEventHandler(this.histogramEqualizationRGB_Click);
            
            #line default
            #line hidden
            return;
            case 31:
            this.histogramEqualizationHSV = ((System.Windows.Controls.MenuItem)(target));
            
            #line 81 "..\..\MainWindow.xaml"
            this.histogramEqualizationHSV.Click += new System.Windows.RoutedEventHandler(this.histogramEqualizationHSV_Click);
            
            #line default
            #line hidden
            return;
            case 32:
            this.histogramEqualizationYUV = ((System.Windows.Controls.MenuItem)(target));
            
            #line 82 "..\..\MainWindow.xaml"
            this.histogramEqualizationYUV.Click += new System.Windows.RoutedEventHandler(this.histogramEqualizationYUV_Click);
            
            #line default
            #line hidden
            return;
            case 33:
            this.sobel = ((System.Windows.Controls.MenuItem)(target));
            
            #line 86 "..\..\MainWindow.xaml"
            this.sobel.Click += new System.Windows.RoutedEventHandler(this.sobel_Click);
            
            #line default
            #line hidden
            return;
            case 34:
            this.gradientBased = ((System.Windows.Controls.MenuItem)(target));
            
            #line 87 "..\..\MainWindow.xaml"
            this.gradientBased.Click += new System.Windows.RoutedEventHandler(this.GradientBased_OnClick);
            
            #line default
            #line hidden
            return;
            case 35:
            this.gaussianBlur = ((System.Windows.Controls.MenuItem)(target));
            
            #line 88 "..\..\MainWindow.xaml"
            this.gaussianBlur.Click += new System.Windows.RoutedEventHandler(this.gaussianBlur_Click);
            
            #line default
            #line hidden
            return;
            case 36:
            this.sharpen = ((System.Windows.Controls.MenuItem)(target));
            
            #line 89 "..\..\MainWindow.xaml"
            this.sharpen.Click += new System.Windows.RoutedEventHandler(this.sharpen_Click);
            
            #line default
            #line hidden
            return;
            case 37:
            this.noiseColor = ((System.Windows.Controls.MenuItem)(target));
            
            #line 90 "..\..\MainWindow.xaml"
            this.noiseColor.Click += new System.Windows.RoutedEventHandler(this.noiseColor_Click);
            
            #line default
            #line hidden
            return;
            case 38:
            this.noiseBW = ((System.Windows.Controls.MenuItem)(target));
            
            #line 91 "..\..\MainWindow.xaml"
            this.noiseBW.Click += new System.Windows.RoutedEventHandler(this.noiseBW_Click);
            
            #line default
            #line hidden
            return;
            case 39:
            this.noiseReductionMean = ((System.Windows.Controls.MenuItem)(target));
            
            #line 92 "..\..\MainWindow.xaml"
            this.noiseReductionMean.Click += new System.Windows.RoutedEventHandler(this.noiseReductionMean_Click);
            
            #line default
            #line hidden
            return;
            case 40:
            this.noiseReductionMedian = ((System.Windows.Controls.MenuItem)(target));
            
            #line 93 "..\..\MainWindow.xaml"
            this.noiseReductionMedian.Click += new System.Windows.RoutedEventHandler(this.noiseReductionMedian_Click);
            
            #line default
            #line hidden
            return;
            case 41:
            this.preferences = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 42:
            this.statusBarShowHide = ((System.Windows.Controls.MenuItem)(target));
            
            #line 98 "..\..\MainWindow.xaml"
            this.statusBarShowHide.Click += new System.Windows.RoutedEventHandler(this.statusBarShowHide_Click);
            
            #line default
            #line hidden
            return;
            case 43:
            this.help = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 44:
            this.about = ((System.Windows.Controls.MenuItem)(target));
            
            #line 102 "..\..\MainWindow.xaml"
            this.about.Click += new System.Windows.RoutedEventHandler(this.about_Click);
            
            #line default
            #line hidden
            return;
            case 45:
            this.border = ((ImageEdit_WPF.HelperClasses.ZoomBorder)(target));
            return;
            case 46:
            this.mainImage = ((System.Windows.Controls.Image)(target));
            return;
            case 47:
            this.statusBar = ((System.Windows.Controls.Primitives.StatusBar)(target));
            return;
            case 48:
            this.imageResolution = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 49:
            this.separatorFirst = ((System.Windows.Controls.Separator)(target));
            return;
            case 50:
            this.imageSize = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

