   M S B A M L   `   `   `  ����H  DImageEdit_WPF, Version=0.9.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ImageEdit_WPF.HelperClassesImageEdit_WPF.HelperClasses  5   	   /    )ImageEdit_WPF.Windows.ContrastEnhancement   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ;local)clr-namespace:ImageEdit_WPF.HelperClasses     5   	     � Title$  Contrast Enhancement��5   	   $	��240��6&   $	��210��63   
������� � 
ResizeMode$ NoResize=�6W   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5       $
��Auto �6   6,   �� 5!      $
��Auto �6   6,   �� 5"      $
��Auto �6   6,   .+�5$   
   � $��0��6   $��10q�6!   .+�5%      �� Brightness:6%   �� 5&       �� Padding$ 5,0,0,0q�6   $��40��6*   $��25��65   ��5'      �� 5(       �� ElementName$ sldBrightness��6    �� Path$ Value�6:    �� 	Converter 5)      6   0ImageEdit_WPF.HelperClasses.SliderValueConverter  5*      5,      5.      5/      �� 50   
   -   5        $��sldBrightness��52      $��1��50      $��5q�51       � Maximum$	 255Q�53       � Minimum$ 0Q�54      	 � Value$	 0Q�55      
 �� IsSnapToTickEnabled
 .  �� TickFrequency$ 1Q�57      � 58   
   $��2��6   $��10q�6!   .+�59      �� 	Contrast:6#   �� 5:      $ 5,0,0,0q�6   $��40��6*   $��25��65   ��5;      �� 5<      $ sldContrast��6   $ Value�68    5=      D   >ImageEdit_WPF.HelperClasses.SliderValueConverter_GradientBased  5>      5@      5B      5C      �� 5D   
   -   5        $��sldContrast��5F      $��3��5D      $��5q�5E      $	 255Q�5G      $ 0Q�5H      $	 0Q�5I      �� 5J   
   $��4��6   $��10q�6"   $��Center=�6.   .+�5K      �� -   5        $��ok��5K      $��	30,2,30,2q�6   .��6D   OK6H   5L      5M      5N      