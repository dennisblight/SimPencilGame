   M S B A M L   `   `   `  ���� M  IDeanor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f1d50610b66f83771clr-namespace:Deanor.ControlsDeanor.Controls  +clr-namespace:Deanor.SceneDeanor.Scene  +clr-namespace:Deanor.PagesDeanor.Pages      Deanor.Pages.GamePage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .ctrlclr-namespace:Deanor.Controls     ,sceneclr-namespace:Deanor.Scene     (pclr-namespace:Deanor.Pages        SuedeTextureBrush#��[  z�T�   � 	Resources   �� 
TargetType �� Property �� Value  	PartStyle  GlyphPartStyle �� BasedOn  FontAwesome  TextPartStyle  PlayerState   Deanor.Pages.GamePage   HexWidth   	HexHeight   
HexFigures  FillEllipse �� ScaleX	 �� ScaleY��%�  '��       &	 /     &	 �     7  7  &	 �     7  &	 $    &	 �    �� " ��.���  � ��$ Segoe UI Black)��� " ��.���  � ��$
 10 0q���  � ��$ Center=���  � ��$ Center=���  � ��$
 10 0q��� " ��8   .���  � ��8  ��  � ��$ 20(��� " ��8   .���  � ��$ Segoe UI Black)���  � ��$ 12(��� " �.���  � ��$ Center=���  � ��$ Center=���  � ��$ 0 0 0 10q���  � ��# Z ��  � ��# Z ��  �@J�Clip B� #��Z �� " A�.���  � ��$ Center=���  � ��$ Center=���  �@J�RenderTransformOrigin$ 0.5 0.5&���  �@��Stretch$ UniformToFill=���  � z� �� $ 0Q�$	 0Q�.��� -   
 �Name$
 mainContainer�� � RowDefinitions �� $
��auto ��� �� $
��auto �.+���  �� Orientation$ 
Horizontal=�$
��Left=� K� ZIndex$ 10��.+��� -    ��Name$ 
backButton��  GhostButton#��[ $��10 7 0 7q�  MagentaBrush#��[ ����  	 Color07 �� Color# [	 .���� $��Center=�$��3q�.+��� $
��Back��#��[ �� $	����#��[ �� -   $ restartButton��#��[ $��10 7 0 7q� 
 ColorBrush02#��[
 ����   Color02# [ .���� $��Center=�$��3q�.+��� $��Replay��#��[ �� $	����#��[ �� $ 
Horizontal=�$��Right=�$ 10��.+��� -   $ surrenderButton��#��[ $��10q�  GrayBrush01#��[ ����  
 Gray01# [ .���� $��Center=�$��3q�.+��� $��	Surrender��#��[ �� $	����#��[ �� $��2��$��20 10q�$ 
Horizontal=�$��Right=�.+��� $ Vertical=�$��	10 5 10 0q�.+��� -    ��Name$ p1Name��$��Center=�$��0 0 0 10q�  GrayBrush03#��[ 
Player 1� -   $
 p1StateGrid��#��[ .+�A� -    A�Name$ p1StateFill��#��[ � -    �Name$ p1StateOutline��#��[ $��4��   	HexPoints � Points# Z �� -	   $ p1StateGlyph��#��[ 
���   �$��Center=�$��Center=��� -
   $ p1GameState��  SGlyphTrophy#��[ 
��� ���$y�Hidden=� �� TextAlignment$ Center=�#��[ �� $ Vertical=�$��	10 5 10 0q�.+��� -   $ p2Name��$��Center=�$��0 0 0 10q�#��[ 
Player 2� -   $
 p2StateGrid��#��[ .+�A� -   $ p2StateFill��#��[ � -   $ p2StateOutline��#��[ $��4��# Z �� -   $ p2StateGlyph��#��[ 
���   �$��Center=�$��Center=��� -   $ p2GameState��  
SGlyphFlag#��[ 
�������$y�Hidden=�$ Center=�#��[ 