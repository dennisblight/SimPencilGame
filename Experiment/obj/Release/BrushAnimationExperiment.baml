   M S B A M L   `   `   `  ���� E  AExperiment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Experiment
Experiment  )    #Experiment.BrushAnimationExperiment   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:Experiment       � Title$  BrushAnimationExperiment��$	��300��$	��300��.���� $��10q�.+��� $	��200��$	��100��
���   �$��1������ .��� -   GradientStop1�� � Color$ 
MediumBlue�� � Offset$	 0.0Q�� -   GradientStop2��$ Purple��$	 0.5Q�� -   GradientStop3��$	 Red��$	 1.0Q� � Triggers :�  :� RoutedEvent$# Rectangle.MouseLeftButtonDown��.[��� .���� .��W�  �� 
TargetName$ GradientStop1�� �� TargetProperty$ Offset� W� From$	 0.0X�	 W� To$	 1.0X�	 g� Duration$	 0:0:1.5D�
 g� AutoReverse
 . �� $ GradientStop2��$ Color� �� From$ PurpleX�	 �� To$ YellowX�$	 0:0:1.5D�
 .  g� 	BeginTime$ 0:0:3X��� $ GradientStop3��$ Color�$	 0:0:1.5D�
 . $ 0:0:6X�	 �� By �� 
 �� ScA$ -1��
 �� ScR$ 0��
 �� ScB$ 0��
 �� ScG$ 0��