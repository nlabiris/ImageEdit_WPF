   M S B A M L   `   `   `  ���� H  DImageEdit_WPF, Version=0.9.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ImageEdit_WPF.HelperClassesImageEdit_WPF.HelperClasses  )    #ImageEdit_WPF.Windows.GradientBased   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ;local)clr-namespace:ImageEdit_WPF.HelperClasses       � Title$%  Edge detection (Gradient based)��$	��440��$	��300��
������� � 
ResizeMode$ NoResize=�.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��5q�$��5q�$��Filter type��.��� .+��� 	Filter:�� -   $��
cmbFilters��$��10,0,0,0q��� $��1��$��5q�$��5q�$��Derivative level��.���� .+�	� -   $��rdbFirstDerivative��.��First derivative	� -   $��rdbSecondDerivative��.��Second derivative� $��2��$��10q�.+��� $��Center=�
Threshold:��  �� Padding$ 5,4,0,0q�$��40��$��25������  �� ElementName$ sldThreshold�� �� Path$ Value� �� 	Converter 6   0ImageEdit_WPF.HelperClasses.SliderValueConverter  �� -   $��sldThreshold��$��5q� � Maximum$	 255Q� � Minimum$ 0Q�	 � Value$	 0Q�$	��165��
 �� IsSnapToTickEnabled
 .  �� TickFrequency$ 1Q��� $��3��$��5q�$��5q�$��Colour Factors��.���  �� $
��Auto ��� $
��Auto ��� $
��Auto � � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $��* �.+��� $��0��$��0��$��Center=�Red:�� $��0��$��1��$��40��$��25��$ 7,4,0,0q����� $ sldRedFactor��$ Value� D   >ImageEdit_WPF.HelperClasses.SliderValueConverter_GradientBased  �� -   $��sldRedFactor��$��0��$��2��$��5q�$ 3Q�$ 0Q�$	 0Q��� $��1��$��0��$��Center=�Green:�� $��1��$��1��$��40��$��25��$ 7,4,0,0q����� $ sldGreenFactor��$ Value�   �� -   $��sldGreenFactor��$��1��$��2��$��5q�$ 3Q�$ 0Q�$	 0Q��� $��2��$��0��$��Center=�Blue:�� $��2��$��1��$��40��$��25��$ 7,4,0,0q����� $ sldBlueFactor��$ Value�   �� -   $��sldBlueFactor��$��2��$��2��$��5q�$ 3Q�$ 0Q�$	 0Q��� -   $��ok��$��4��$��100,10,100,10q�.��OK