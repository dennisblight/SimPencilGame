   M S B A M L   `   `   `  ���� M  IDeanor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f1d50610b66f83771clr-namespace:Deanor.ControlsDeanor.Controls  !    Deanor.Pages.SettingSection   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .ctrlclr-namespace:Deanor.Controls        SnowTextureBrush#��[  z�T�   � 	Resources   �� 
TargetType �� Property �� Value  	PartStyle  GlyphPartStyle �� BasedOn  FontAwesome  TextPartStyle  GhostRadioButton �� 
TargetType �� CornerRadius'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Transitions%  System.Windows.VisualTransition	   From	
   To   GeneratedDuration   States   System.Windows.VisualState   
Storyboard �� 
TargetName �� TargetProperty	 W� To g� Duration W� EasingFunction.  (System.Windows.Media.Animation.PowerEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode	 �� To �� EasingFunction-  'System.Windows.Media.Animation.SineEase ��Name J� RenderTransformOrigin �� ScaleX �� ScaleY  CustomRadioButton   Name g� FillBehavior4  .System.Windows.Media.Animation.ExponentialEase  RedBrush K� ZIndex J� Opacity  NoobContent �� Orientation �� TextAlignment 	 NormalContent 
 
ProContent��%<  '��       &	 /     &	 �     7  7  &	 �     7  '�� $    &	 P    &	 M    7  &	 �    7  &		 s    7  &	
     7  �� " ��.���  � ��$ Segoe UI Black)��� " ��.���  � ��$
 10 0q���  � ��$ Center=���  � ��$ Center=���  � ��$
 10 0q��� " ��8   .���  � ��8  ��  � ��$ 20(��� " ��8   .���  � ��$ Segoe UI Black)���  � ��$ 12(��� " ��.���  �@�Cursor$
 Hand���� " 	�.���  � ��$ Segoe UI Black)���  � ��$ 14(���  � ��$ 	10 3 10 7q���  � ��$ 5q���  � �� �� " 	�.R��� $ 2��$��0q�      $	 Normal��$
 	MouseOver��$ 0:0:0.2D�  $	 	MouseOver��$
 Normal��$ 0:0:0.2D�  $	 Checked��$
 Normal��$ 0:0:0.2D�  $	 	MouseOver��$
 Pressed��$ 0:0:0.1D�  $	 Pressed��$
 	MouseOver��$ 0:0:0.1D�.   
Normal��  	MouseOver��. �� .��W� $ HoverRectangle��$ RenderTransform.ScaleX�$ 1X�$ 0:0:0.2D�   $ EaseOut=�W� $ HoverRectangle��$ RenderTransform.ScaleY�$	 0.3X�$ 0:0:0.2D�   $ EaseOut=�  Pressed��. �� .���� $6 0(RadioButton.Foreground).(SolidColorBrush.Color)�$ WhiteX�$ 0:0:0.2D�   $ EaseIn=�W� $ HoverRectangle��$ RenderTransform.ScaleY�$ 1X�$ 0:0:0.2D�   $ EaseOut=�W� $ HoverRectangle��$ RenderTransform.ScaleX�$ 1X�$ 0:0:0.2D�   $ EaseOut=�  Checked��. �� .���� $6 0(RadioButton.Foreground).(SolidColorBrush.Color)�$ WhiteX�$ 0:0:0.2D�   $ EaseIn=�W� $ HoverRectangle��$ RenderTransform.ScaleX�$ 1X�$ 0:0:0.2D�   $ EaseOut=�W� $ HoverRectangle��$ RenderTransform.ScaleY�$ 1X�$ 0:0:0.2D�   $ EaseOut=�._�� }�. ���	#01000000.+��� $ HoverRectangle�����#00a651$ 0.5 1&�z��� $ 0Q�$	 0.3Q�� #��z��$��Center=�$��Center=�.+��� $��Center=�$��Center=��� " 	�.���  � ��$ 50����  � ��$ 40����  � ��$ Segoe UI Black)���  � ��$ 12(���  � �� �� " 	�.R��� $��0.5 0q����	#77ee1d25   $ CommonStates��   $	 Normal��$
 	MouseOver��$ 0:0:0.2D�  $	 	MouseOver��$
 Normal��$ 0:0:0.2D�  $	 Checked��$
 Normal��$ 0:0:0.2D�  $	 	MouseOver��$
 Pressed��$ 0:0:0.1D�  $	 Pressed��$
 	MouseOver��$ 0:0:0.1D�.   
Normal��  	MouseOver��. �� .���� $6 0(RadioButton.Foreground).(SolidColorBrush.Color)�$ WhiteX�$ 0:0:0.2D�   $ EaseIn=�W� $ HoverRectangle��$ RenderTransform.ScaleY�$ 1X�$ 0:0:0.2D�   $ EaseOut=�  Pressed��. �� .���� $6 0(RadioButton.Foreground).(SolidColorBrush.Color)�$ WhiteX�$ 0:0:0D�W� $ HoverRectangle��$ RenderTransform.ScaleY�$ 1X�$ 0:0:0D�W� $ PressedRectangle��$ Opacity�$	 0.3X�$ 0:0:0.1D�$ HoldEnd=�   $ EaseOut=�  Checked��. �� .���� $6 0(RadioButton.Foreground).(SolidColorBrush.Color)�$ WhiteX�$ 0:0:0.2D�   $ EaseIn=�W� $ HoverRectangle��$ RenderTransform.ScaleY�$ 1X�$ 0:0:0.2D�   $ EaseOut=�._�� .+��� $ HoverRectangle��8��  $ 0��$ 0.5 1&�z��� $ 1Q�$ 0Q��� $ PressedRectangle��
���   �$ 2��$ 0Q��� $��Center=�$��Center=�#��z��$ 1���� .R��� $ Vertical=�$��Center=�.+��� $ Center=�$��15(�8��  $��0 0 0 5q��� $��Segoe UI Black)�$ Center=�$
��NOOB��$��9(��� .R��� $ Vertical=�$��Center=�.+��� $ Center=�$��15(�8��  $��0 0 0 5q�	 �� $��Segoe UI Black)�$ Center=�$��NORMAL��$��9(��� .R��� $ Vertical=�$��Center=�.+��� $ Center=�$	��7.5(�8��  $��0 0 0 5q��� 	 �� $��Segoe UI Black)�$ Center=�$	��PRO��$��9(�.���   � RowDefinitions  �� $
��auto ��� �� $
��auto �! � ColumnDefinitions! �� �� .+��� $��0��$��0��$ Vertical=�$��Center=�.+�!	   Deanor.Controls.ColorPicker	  -   " 	 Name$" firstColorPicker��$��Center=�$��Center=�# 	  Radius$# 25Q�$��60 80 60 60q��� -   $ ��Name$$ firstTextBlock��$��Player 1��$��Center=�$��20(����	#FF636363	� -   % 	�Name$% rbPlayer1First��& 	� 	GroupName$& g3��$��Center=�' d� 	IsChecked$
' TrueY�#��[ ���#636363.��
Play First�� $��0��$��1��$ Vertical=�$��Center=�.+�	  -   $" secondColorPicker��$��Center=�$��Center=�$# 25Q�$��60 80 60 60q��� -   $$ secondTextBlock��$��Player 2��$��Center=�$��20(����	#FF636363	� -   $% rbPlayer2First��$& g3��$��Center=�#��[ ���#636363.��
Play First�� -   ( ��Name$( verticeCountContainer��$ Vertical=�$��1��$��0��$��Center=�$��Center=�.+��� $ Center=����#ee1d25Number of Dots�� $��0 0 0 2q�#��[ $��0 5 0 0q�._��� -   $( verticeCountRBG��$ 
Horizontal=�  ��%%   '	�       7  �� 8   " 	�.�-	   .+�	� ���#ee1d25$& g1��.��4	� ���#ee1d25$& g1��.��5	� ���#ee1d25$& g1��.��6	� ���#ee1d25$& g1��.��7	� ���#ee1d25$& g1��.��8	� ���#ee1d25$& g1��.��9	� ���#ee1d25$& g1��.��10�� -
   $( aiLevelContainer��$ Vertical=�$��1��$��1��$��Center=�$��Center=�.+��� $ Center=����#ee1d25
AI Level�� $��0 0 0 2q�#��[ $��0 5 0 0q�._��� -   $( 
aiLevelRBG��$ 
Horizontal=�  ��%%   '	�       7  �� 8   " 	�.�-   .+�	� -   $% 
rbAiLevel0�����#ee1d25$& g2��#��[ 	� -   $% 
rbAiLevel1�����#ee1d25$& g2��#��[	 	� -   $% 
rbAiLevel2�����#ee1d25$& g2��#��[
 �� -   $��2��$��0��$��10q�$
��Left=�  GhostButton#��[   MagentaBrush#��[ ����   Color07) �� Color#) [ .���� $��Center=�$��3q�.+��� $
��Back��#��[ �� $	����#��[ �� -   $��2��$��1��$��10q�$��Right=�#��[   
GreenBrush#��[ ����   Color16#) [ .���� $��Center=�$��3q�.+��� $��Start��#��[ �� $	����#��[ 