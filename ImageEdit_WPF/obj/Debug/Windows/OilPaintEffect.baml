   M S B A M L   `   `   `  ����H  DImageEdit_WPF, Version=0.9.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ImageEdit_WPF.HelperClassesImageEdit_WPF.HelperClasses  5   	   *    $ImageEdit_WPF.Windows.OilPaintEffect   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ;local)clr-namespace:ImageEdit_WPF.HelperClasses     5   	     � Title$  Oil paint effect��5   	   $	��200��6"   $	��250��6/   
������� � 
ResizeMode$ NoResize=�6S   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5       $
��Auto �6   6,   �� 5!      $
��Auto �6   6,   .+�5#   
   � $��0��6   $��10q�6!   .+�5$      �� Kernel size:6&   �� 5%      -   5        $��cmbKernelSize��5%   (   $��5,0,0,0q�6   .@�5&      �� .��6   36"   �� 5'      .��6   56"   �� 5(      .��6   96"   �� 5)      .��6   116#   �� 5*      .��6   156#   5+      5,      � 5-   
   $��1��6   $��10q�6!   .+�5.      �� Intensity levels:6,   �� 5/       �� Padding$ 5,0,0,0q�6   $��40��6*   $��25��65   ��50      �� 51       �� ElementName$ sldIntensityLevels��6    �� Path$ Value�6?    �� 	Converter 52      6   0ImageEdit_WPF.HelperClasses.SliderValueConverter  53      55      57      58      �� 59   
   -   5        $��sldIntensityLevels��5;      $��2��59      $��5q�5:       � Maximum$	 200Q�5<       � Minimum$ 0Q�5=      	 � Value$	 0Q�5>      
 �� IsSnapToTickEnabled
 .  �� TickFrequency$ 1Q�5@      �� 5A   
   $��3��6   $��10q�6"   $��Center=�6.   .+�5B      �� -   5        $��ok��5B      $��	30,2,30,2q�6   .��6F   OK6J   5C      5D      5E      