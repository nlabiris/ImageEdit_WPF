   M S B A M L   `   `   `  ���� H  DImageEdit_WPF, Version=0.9.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ImageEdit_WPF.CommandsImageEdit_WPF.Commands  I)clr-namespace:ImageEdit_WPF.HelperClassesImageEdit_WPF.HelperClasses      ImageEdit_WPF.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     9commands$clr-namespace:ImageEdit_WPF.Commands     ChelperClasses)clr-namespace:ImageEdit_WPF.HelperClasses     -     � Title$  	ImageEdit��$	��350��$	��525��
���   � J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$ ApplicationCommands.Open��  -   $ ApplicationCommands.Save��  -   $- 'commands:InformationCommand.Information��  -   $ commands:HelpCommand.Help��  -   $ commands:UndoCommand.Undo��  -   $ commands:RedoCommand.Redo��.���  � RowDefinitions �� $
��Auto ��� �� $
��Auto �.+�o� -   $��menuBar��$��0��$��0��.@�m� $
��File�� m� StaysOpenOnClick . .@�m� -	   $
��open��$��Open...�� m� Command$ ApplicationCommands.Open��m� -
   $��reopen��$��Reopen last image���� m� -   $
��save��$
��Save��$ ApplicationCommands.Save��m� -   $��saveAs��$��
Save as...��m� $
��Edit�� . .@�m� -    m�Name$
 undo��$
��Undo��$ commands:UndoCommand.Undo��m� -   $
 redo��$
��Redo��$ commands:RedoCommand.Redo��m� $��Image�� . .@�m� -   $��information��$��Information��$- 'commands:InformationCommand.Information���� m� -   $��	histogram��$��	Histogram��m� -   $��	shiftBits��$��
Shift bits��m� -   $��	threshold��$��	Threshold��m� -   $��autoThreshold��$��Auto Threshold��m� -   $��contrastEnhancement��$��Contrast Enhancement��m� -   $��
brightness��$��
Brightness��m� -   $��contrast��$��Contrast��m� $��Effect�� . .@�m� -   $��negative��$��Negative��m� -   $��
squareRoot��$'��!Square root (increase brightness)��m� -   $��	grayscale��$��	Grayscale��m� -   $��sepia��$��Sepia��m� -   $��cartoonEffect��$��Cartoon��m� -   $��oilPaintEffect��$��Oil Painting��m� -   $��histogramEqualization��$��Histogram Equalization��.@�m� -   $��histogramEqualizationRGB��$	��RGB��m� -   $��histogramEqualizationHSV��$	��HSV��m� -    $��histogramEqualizationYUV��$	��YUV��m� $��Filters�� . .@�m� -!   $��sobel��$��Edge detection (Sobel)��m� -"   $��gradientBased��$%��Edge detection (Gradient-based)��m� -#   $��gaussianBlur��$��Gaussian Blur��m� -$   $��sharpen��$��Sharpen��m� -%   $��
noiseColor��$#��Salt-and-Pepper noise (Color)��m� -&   $��noiseBW��$)��#Salt-and-Pepper noise (Black/White)��m� -'   $��noiseReductionMean��$��Noise reduction (Mean)��m� -(   $��noiseReductionMedian��$��Noise reduction (Median)��m� $��Options�� . .@�m� -)   $��preferences��$��Preferences���� m� -*   $��statusBarShowHide��$��Show/Hide status bar��m� $
��Help�� . .@�m� -+   $
��help��$��Help/Documentation��$ commands:HelpCommand.Help��m� -,   $��about��$��About...��,   &ImageEdit_WPF.HelperClasses.ZoomBorder  --     Name$ border��$��1��$��0��}�. 
���   �   Child. �� -.   	 ��Name$	 	mainImage������ 
 �� Path$
 M_bitmapBind� �� UpdateSourceTrigger$ PropertyChanged=��� -/   $��	statusBar��$��2��$��0��$y�Visible=����� .R��  � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $��* �.@��� .���� -0   $��imageResolution���� -1   $��separatorFirst��$��1��$y�	Collapsed=� J� Opacity$	 0.5Q��� $��2��.���� -2   $��	imageSize��