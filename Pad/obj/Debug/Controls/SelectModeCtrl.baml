   M S B A M L   `   `   `  ����>  :Pad, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Pad.ControlsPad.Controls  5      C ?UICommon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG1clr-namespace:UICommon.Controls;assembly=UICommonUICommon.Controls 5      !    Pad.Controls.SelectModeCtrl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:Pad.Controls     5      FUICommon1clr-namespace:UICommon.Controls;assembly=UICommon     5      .��5
      � $��50,62q�6   .+�5   
   �� $	��Top=�6   .+�5      �� $!��请 设 定 展 示 区 域��6      TitleTextBlockStyle#��[  6-   6\   �� 5      $��2��6     ControlForeground#��[ 6#   $��0,5,0,0q�6M   6`   5      � 5   
     BoardBackground#��[ 6   $	��694��5      $	��489��6   $
��Left=�5      $	��Top=�6.     � ShowGridLines  . $��	10,86,0,0q�5   (    � RowDefinitions 5      �� 5      $��* �6    �� 5      $��* �6    �� 5      $��* �6    �� 5      $��* �6    �� 5      $��* �6    �� 5      $��* �6    �� 5      $��* �6    �� 5      $��* �6    �� 5      $��* �6    �� 5      $��* �6     � ColumnDefinitions 5      �� 5       $��* �6#   �� 5!      $��* �6#   �� 5"      $��* �6#   �� 5#      $��* �6#   �� 5$      $��* �6#   �� 5%      $��* �6#   �� 5&      $��* �6#   �� 5'      $��* �6#   �� 5(      $��* �6#   �� 5)      $��* �6#   .+�5+      � -   5         �Name$ Board��5+      $��10��6"   $��10��67    K� ZIndex$ 0��6I   ��5,      �� *Width+ �� Mode$ Default=� �� RelativeSource �� *FindAncestor+ �� AncestorType" ���5-      �� *Height+$ OneWay=� �� *FindAncestor+" �5/      �� 50      -   5         ��Name$ cv��50      |�. $��10��62   $��10��6G   
������ $ 1��6r   ��51      �� *Width+$ Default=� �� *FindAncestor+" ���52      �� *Height+$ OneWay=� �� *FindAncestor+" �.+�56      �� -   5        	 ��Name$
	 rect��56      
���  ��
������ $��2��6J   $y�	Collapsed=�6^   
 �� Left$	
 100��57      
 �� Top$	 100��6.   )  #UICommon.Controls.DragControlHelper  58      -   5          Name$ helper��58   -   &   UICommon.Controls.DragHelperBase   CornerWidth$ 5��6=   6M   59      5:      �� 5;   
    �� Orientation$ 
Horizontal=�6   $��40��6.   $	��Top=�6:   $
��Left=�6R   $��
785,86,0,0q�6m   .+�5<      �� $��	横坐标��6   $��0,0,30,0q�6#   67   �� 5=      -   5         ��Name$ 	locationX��5=      $��1��6)   $	��150��62   5>      �� 5?   
   $ 
Horizontal=�6   $��40��6.   $	��Top=�6:   $
��Left=�6R   $��1204,86,0,0q�6m   $	��306��6�   .+�5@      �� $��横向占用数��6   $��0,0,30,0q�6%   �� 5A      -   5        $ rowSpan��5A      $��1��6'   $	��150��60   5B      �� 5C   
   $ 
Horizontal=�6   $��40��6.   $	��Top=�6:   $
��Left=�6R   $��785,158,0,0q�6m   .+�5D      �� $��	纵坐标��6   $��0,0,30,0q�6#   67   �� 5E      -   5        $ 	locationY��5E      $��1��6)   $	��150��62   6@   5F      �� 5G   
   $ 
Horizontal=�6   $��40��6.   $	��Top=�6:   $
��Left=�6R   $��1204,158,0,0q�6m   $	��306��6�   .+�5H      �� $��纵向占用数��6   $��0,0,30,0q�6%   69   �� 5I      -   5        $ 
columnSpan��5I      $��1��6*   $	��150��63   6A   5J      �� 5K   
   $ 
Horizontal=�6   $��40��6.   $��785,248,10,668q�6:   .+�5L      �� $��	数据源��6   $��0,0,15,0q�6#   67   �� 5M      -	   5         ��Name$ 
cb_Sources��5M      $	��267��6+    �� Text$ 	选择源��67   $��18(�6B   $��Center=�5N      $��36��62     ComboBoxStyle#��[ 6>   $	��0,2q�6e   ��5O      �� 5P      .R�5Q      �� ��6$   �� *	KeyName+6?   5R      5T      �� 5U      -
   5         ��Name$ cb_IsWebKit��5U       d� 	IsChecked$
 TrueY�6,   $��是否使用WebKit内核��5V      $��22(�6.   #��[ 6<   $��Right=�5W      $��Center=�63   $��Center=�6N   $
��50,0q�6p   5X      �� 5Y   
   -   5         ��Name$ lb_SetedRegion��5Y      $��20(�5Z      #��[ 5[        ControlBackground#��[ 6B   $��785,373,0,381q�5\   E   ��5]      �� 5^      .R�5_      �� ��6    ��  �� StringFormat$ 	区域{0}��6F   5`      5b      �� 5c   
   -   5         ��Name$ btn_Show��5c      $��显示��6#   $��54��60   $��50,746,1295,156q�6<   6U   �� 5d   
   -   5        $ btn_SaveAsTemplate��5d      $��存为模板��6-   $��54��6<   $��694,746,651,156q�6H   6a   �� 5e   
   -   5        $ btn_Back��5e      $��返回��6#   $��54��60   $��1335,746,10,156q�6<   6U   �� 5f   
   -   5        $ btn_SaveOneScreen��5f      $��保存该区域��6,   $��54��6<   $��50,913,1295,-11q�6H   6a   �� 5g   
   -   5        $ btn_UpdateOneScreen��5g      $��更新该区域��6.   $��54��6>   $��1335,913,10,-11q�6J   6c   5h      5i      