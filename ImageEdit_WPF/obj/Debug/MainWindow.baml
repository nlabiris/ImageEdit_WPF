   M S B A M L   `   `   `  ����H  DImageEdit_WPF, Version=0.9.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ImageEdit_WPF.CommandsImageEdit_WPF.Commands  5   	   I)clr-namespace:ImageEdit_WPF.HelperClassesImageEdit_WPF.HelperClasses  5   	       ImageEdit_WPF.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   9commands$clr-namespace:ImageEdit_WPF.Commands     5   	   ChelperClasses)clr-namespace:ImageEdit_WPF.HelperClasses     5   	   -   5          � Title$  	ImageEdit��5   	   $	��350��6   $	��525��6(   
���   � J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5    
   -   5           Command$ ApplicationCommands.Open��5       6U     5!   
   -   5        $ ApplicationCommands.Save��5!      6U     5"   
   -   5        $- 'commands:InformationCommand.Information��5"      6�     5#   
   -   5        $ commands:HelpCommand.Help��5#      6u     5$   
   -   5        $ commands:UndoCommand.Undo��5$      6u     5%   
   -   5        $ commands:RedoCommand.Redo��5%      6u   .��5)      �  � RowDefinitions 5*   
   �� 5+      $
��Auto �6   6,   �� 5,      6   �� 5-      $
��Auto �6   6,   .+�51   
   o� -   5        $��menuBar��51      $��0��6   $��0��6+   .@�52      m� $
��File��6    m� StaysOpenOnClick . .@�53      m� -	   5        $
��open��53      $��Open...��6'    m� Command$ ApplicationCommands.Open��68   6]   m� 54      -
   5        $��reopen��54      $��Reopen last image��6)   6[   �� 55      6   m� 56      -   5        $
��save��56      $
��Save��6'   $ ApplicationCommands.Save��65   6Z   m� 57      -   5        $��saveAs��57      $��
Save as...��6)   6T   58      m� 59      $
��Edit��6    . .@�5:      m� -   5         m�Name$
 undo��5:      $
��Undo��6)   $ commands:UndoCommand.Undo��67   6v   m� 5;      -   5        $
 redo��5;      $
��Redo��6)   $ commands:RedoCommand.Redo��67   6v   5<      m� 5=      $��Image��6    . .@�5>      m� -   5        $��information��5>      $��Information��6.   $- 'commands:InformationCommand.Information��6C   6w   �� 5?      6   m� 5@      -   5        $��	histogram��5@      $��	Histogram��6,   6Y   m� 5A      -   5        $��	shiftBits��5A      $��
Shift bits��6,   6Z   m� 5B      -   5        $��	threshold��5B      $��	Threshold��6,   6Y   m� 5C      -   5        $��autoThreshold��5C      $��Auto Threshold��60   6f   m� 5D      -   5        $��contrastEnhancement��5D      $��Contrast Enhancement��66   6x   m� 5E      -   5        $��
brightness��5E      $��
Brightness��6-   6\   m� 5F      -   5        $��contrast��5F      $��Contrast��6+   6V   5G      m� 5H      $��Effect��6    . .@�5I      m� -   5        $��negative��5I      $��Negative��6+   6V   m� 5J      -   5        $��
squareRoot��5J      $'��!Square root (increase brightness)��6-   6s   m� 5K      -   5        $��	grayscale��5K      $��	Grayscale��6,   6Y   m� 5L      -   5        $��sepia��5L      $��Sepia��6(   6O   m� 5M      -   5        $��cartoonEffect��5M      $��Cartoon��60   6a   m� 5N      -   5        $��oilPaintEffect��5N      $��Oil Painting��61   6h   m� 5O      -   5        $��histogramEqualization��5O      $��Histogram Equalization��68   .@�5P      m� -   5        $��histogramEqualizationRGB��5P      $	��RGB��6?   6u   m� 5Q      -   5        $��histogramEqualizationHSV��5Q      $	��HSV��6?   6u   m� 5R      -    5        $��histogramEqualizationYUV��5R      $	��YUV��6?   6u   5S      5T      m� 5U      $��Filters��6    . .@�5V      m� -!   5        $��sobel��5V      $��Edge detection (Sobel)��6(   6^   m� 5W      -"   5        $��gradientBased��5W      $%��Edge detection (Gradient-based)��60   6y   m� 5X      -#   5        $��gaussianBlur��5X      $��Gaussian Blur��6/   6c   m� 5Y      -$   5        $��sharpen��5Y      $��Sharpen��6*   6S   m� 5Z      -%   5        $��
noiseColor��5Z      $#��Salt-and-Pepper noise (Color)��6-   6o   m� 5[      -&   5        $��noiseBW��5[      $)��#Salt-and-Pepper noise (Black/White)��6*   6o   m� 5\      -'   5        $��noiseReductionMean��5\      $��Noise reduction (Mean)��65   6x   m� 5]      -(   5        $��noiseReductionMedian��5]      $��Noise reduction (Median)��67   6~   5^      m� 5_      $��Options��6    . .@�5`      m� -)   5        $��preferences��5`      $��Preferences��6.   6E   �� 5a      6   m� 5b      -*   5        $��statusBarShowHide��5b      $��Show/Hide status bar��64   6t   5c      m� 5d      $
��Help��6    . .@�5e      m� -+   5        $
��help��5e      $��Help/Documentation��6'   $ commands:HelpCommand.Help��6C   6i   m� 5f      -,   5        $��about��5f      $��About...��6(   6P   5g      5h      ,   &ImageEdit_WPF.HelperClasses.ZoomBorder  5k   
   --   5          Name$ border��5k   #   $��1��63   $��0��6@   }�. 
���   �   Child. 5l      �� -.   5        	 ��Name$	 	mainImage��5l      ��6@   �� 
 �� Path$
 M_bitmapBind� �� UpdateSourceTrigger$ PropertyChanged=�6�   5m      �� 5p   
   -/   5        $��	statusBar��5p      $��2��6%   $��0��62   $y�Visible=�6B   ��5q      �� 5r      .R�5s      �  � ColumnDefinitions 5t      �� 5u      $
��Auto �6/   6>   �� 5v      $
��Auto �6/   6>   �� 5w      $��* �6/   6;   5y      5z      .@�5|      �� .��5}      �� -0   5        $��imageResolution��5}      65   5~      �� 5      -1   5        $��separatorFirst��5   (   $��1��6   $y�	Collapsed=�6>    J� Opacity$	 0.5Q�6U   �� 5�      $��2��6   .��5�      �� -2   5        $��	imageSize��5�      6/   5�      5�      5�      5�      