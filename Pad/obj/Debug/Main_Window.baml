   M S B A M L   `   `   `  ����>  :Pad, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PadPad  5   	   +clr-namespace:Pad.ControlsPad.Controls  5   	       Pad.Main_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:Pad     5   	   /controlsclr-namespace:Pad.Controls     5   	     � Title$  Main_Window��5	   	   $
��1080��5
   	   $
��1920��6    � WindowState$ 	Maximized=�5   	    � WindowStyle$
 None=�6!    � 
ResizeMode$ NoResize=�5   	    � Cursor$ Arrow��6    � 	Resources 5       �� 
TargetType �� Property   ControlForeground �� Value��%�   '��       7   �� 5   
   " ��6   .�5      ��  � ��8   6X   �� 5       � ��$ 22(�6)   66   �� 5       � ��$
 Bold"�6+   6:   �� 5       � ��$ Center=�62   6C   5      5      .��5      � -   5        	 �Name$	 
LayoutRoot��5      ���#232224
 � ColumnDefinitions
 5   
   �� 5      $
��300* �6   �� 5      $��1620* �6    � RowDefinitions 5   
   �� 5      $��1000* �6   �� 5      $	��80* �6   '  !System.Windows.VisualStateManager   VisualStateGroups 5    
   %  System.Windows.VisualStateGroup  5!      -   5        VisualStateGroup��   States. 5"         System.Windows.VisualState  -   5        SetDataState��   
Storyboard. 5#      �� .��5$      T�  �� TargetProperty$ (UIElement.Visibility)�68    �� 
TargetName$ SetDataGrid��6k   ..�5%      i�  R� KeyTime$ 0��65     System.Windows.Visibility   Visible R� Value# Z 6A   5&      T� 5'      $ (UIElement.Visibility)�68   $ DailyOperateGrid��6k   ..�5(      i� $ 0��65      	Collapsed# Z 6A   5)      5*      5+        5,      -   5        DailyOperateState��. 5-      �� .��5.      T� $ (UIElement.Visibility)�68   $ SetDataGrid��6k   ..�5/      i� $ 0��65   # Z 6A   50      T� 51      $ (UIElement.Visibility)�68   $ DailyOperateGrid��6k   ..�52      i� $ 0��65   # Z 6A   53      54      55      56        57      -   5        VisualStateGroup2��. 58        -   5        ScreenModeState��. 59      �� .��5:      T� $ (UIElement.Visibility)�68   $ ScreenModeGrid��6k   ..�5;      i� $ 0��65   # Z 6A   5<      T� 5=      $ (UIElement.Visibility)�68   $ MeetIngRoomModeGrid��6k   ..�5>      i� $ 0��65   # Z 6A   5?      T� 5@      $ (UIElement.Visibility)�68   $ tabMenu��6k   ..�5A      i� $ 0��65   # Z 6A   5B      5C      5D        5E      -   5        MeetingRoomState��. 5F      �� .��5G      T� $ (UIElement.Visibility)�68   $ ScreenModeGrid��6k   ..�5H      i� $ 0��65   # Z 6A   5I      T� 5J      $ (UIElement.Visibility)�68   $ MeetIngRoomModeGrid��6k   ..�5K      i� $ 0��65   # Z 6A   5L      T� 5M      $ (UIElement.Visibility)�68   $ tabMenu��6k   ..�5N      i� $ 0��65   # Z 6A   5O      5P      5Q      5R      .+�5T   
   � $��2��6   
 5U      �� 5V      $
��280* �6#   �� 5W      $	��20* �6#   .+�5Y      ]� -   5         ]�Name$ 	MenuPanel��5Y      $��0��6+   ���#2e2d30.+�5Z      � ���#2e2d30$	��200��6,   $	��280��69   $
��Left=�6E   $	��Top=�6`   .+�5[      �� -	   5         ��Name$ menuItemLogo��5[      $��Images/Icons/logo2.png��62   $	��120��6R   $	��120��6_   $��80,35,80,45q�6k   6�   5\      �� 5^      -
   5         ��Name$ tabMenu��5^      $	��Top=�6.   .+�5_      %   Pad.Controls.CustomMenuItemCtrl  -   5          Name$ 
rb_SetData��5_   2   %   Pad.Controls.CustomMenuItemCtrl   Checked$ rb_SetData_Checked��5d   *   $��选择模式��5`   *      IconUrl$! /Images/Icons/icon-mode.png��6:   
 � Tag$ 1��5a   *    	� 	GroupName$ X��62   $
��Left=�5b   *   $��0,1,0,0q�6E   $��55��5c   *   $��1��66   $��2��5d   G     5l      -   5        $ rb_DailyOperare��5l   2   $ rb_DailyOperare_Checked��5q   *   $��日常操作��5m   *   $#  /Images/Icons/icon-daily.png��69   $ 2��5n   *   $ X��62   $
��Left=�5o   *   $��0,1,0,0q�6E   $��55��5p   *   5q   *   5r        5s      -   5        $ rb_Exit��5s   .   $ rb_Exit_Checked��5w   *   $��Bottom=�5s   ?   $��0,0,0,60q�6Z   $��退        出��5t   *   $"  /Images/Icons/icon-back.png��6?   $ 0��5u   *   $ X��62   $��55��5v   *   5w   *   5x      5y      � 5z   
   -   5        $	 ScreenModeGrid��5z      $��1��6'   .+�5{      � -   5        $	 SetDataGrid��5{      $y�Visible=�6)   .+�5|         Pad.Controls.SetDataCtrl  -   5          Name$ setDataCtrl��5|   '   6>   5~      � 5      -   5        $	 SelectModeGrid��5      $y�	Collapsed=�6,   .+�5�      !   Pad.Controls.SelectModeCtrl  -   5          Name$ selectModeCtrl��5�   *   6D   5�      � 5�      -   5        $	 DailyOperateGrid��5�      $y�	Collapsed=�6-   .+�5�      #	   Pad.Controls.DailyOpetateCtrl	  -   5         	 Name$ dailyOpetateCtrl��5�   ,   6H   5�      5�      � 5�   
   -   5        $	 MeetIngRoomModeGrid��5�      $��1��6,   
������ $y�Hidden=�6U   .+�5�      "
   Pad.Controls.MeetingRoomCtrl
  6)   5�      � 5�   
   $��2��6   $��1��6$   .+�5�      �� $
��1640��6   $��Right=�6&     �� Orientation$  
Horizontal=�6B   .+�5�      #   Pad.Controls.PageSwitchButton  -   5        !  Name$! ScreenRadioButton��5�   ,   #   Pad.Controls.PageSwitchButton"   Checked$" ToggleButton_OnChecked��6�   $ MeetingOrScreen��6G   $��30(�6c   $	��820��6q   $��80��6}   
�������$��微软雅黑)�6�   $��Black"�6�   .��6�   大屏6�     5�      $" ToggleButton_OnChecked��6�   $ MeetingOrScreen��6,   $��30(�6H   $	��820��6V   $��80��6b   
�������$��微软雅黑)�6�   $��Black"�6�   .��6�   	会议室6�   5�      5�      �� 5�   
   -   5        $ rightBottomLogo��5�      $��Images/Icons/text.png��6)   $��Right=�6H   $��Bottom=�6d   $��1��6   $	��214��6�   $��20q�6�   6�   5�      5�      