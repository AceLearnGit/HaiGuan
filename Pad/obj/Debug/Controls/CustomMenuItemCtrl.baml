   M S B A M L   `   `   `  ����>  :Pad, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Pad.ControlsPad.Controls  5      %    Pad.Controls.CustomMenuItemCtrl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:Pad.Controls     5      -   5          	�Name$
  self��5	         RadioButtonStyle1#���   5
       � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType ��Name J� SnapsToDevicePixels � ColumnDefinitions	 ��Name
 �� ElementName �� Path ��Name ��Name J� Opacity �� Triggers P� Property P� Value �� 
TargetName��%,  '��       &	  �     �� 5   
   " ��6   .�5      ��  � �� �#dedede6=   �� 5       � ��$ 22(�6)   66   �� 5       � ��$
 Bold"�6+   6:   �� 5       � ��$ Arial)�6+   6;   �� 5       � ��$ Center=�62   6C   5      �� 5   
   " 	�6*   .�5      ��  � ��$ 设置数据��6(   67   �� 5       � ��$ 22(�6)   66   �� 5       � �� �#2423236+   �� 5       � �� �#dedede6+   �� 5       � ��$ 1q�60   �� 5       � |� .  6:   �� 5       � �� 5      �� 5      " 	�6&   .R�5      �� $ templateRoot��6!   #��z��67   ._�5      �  .  5   "   �� 5    &   $
��Auto �67   �� 5!   &   $
��Auto �67   �� 5"   &   $��* �67   .+�5$   "   �� $	 itemIcon��6(   $��18��6k   $��UniformToFill=�6w   $��	50,0,10,0q�6�   $��Center=�6�   ��6:   �� $

 self��$ IconUrl�6�   �� 5%   "   $ contentPresenter��63   $��1��6M   |�.  $��Stretch=�6o   #��z��6�   ��. # z 6�   $��Center=�6  �� 5&   "   $ 	itemArrow��6,   $��2��6?   $��>��6O   $��10,0,0,0q�6X    6j   �� $

 self��$ ArrowOpacity�6�   5'      5(       5)      P� 5*       � �� . . �5+   "   ��  � ��$ 4,-1,0,0q�6<   5,      P� 5-       �@d�	IsChecked$
 trueY�6;   . �5.   "   ��  � ��$	 300��6:   6H   �� 5/   "    � �� �#e3b9496Q   �� 50   "   $ templateRoot��6)    �@��CornerRadius$ 	0,10,10,0��6[   6o   �� 51   "   $ 	itemArrow��6)    � ��$ Right=�6_   6o   52      P� 53       �@d�	IsChecked$ falseY�6;   . �54   "   ��  � ��$	 280��6:   6H   55      P� 56       6;   Z�  �@d�	IsChecked. �57   "   ��  � ��$	 280��6:   6H   58      5:      5<      5=      5>      5@      