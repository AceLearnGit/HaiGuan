   M S B A M L   `   `   `  ����>  :Pad, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Pad.ControlsPad.Controls  5      "    Pad.Controls.MeetingRoomCtrl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bei9http://schemas.microsoft.com/expression/2010/interactions  5      &localclr-namespace:Pad.Controls  5      -   5          >�Name$  uc��5	      $
��1620��5
      $
��1000��6   
������  � 	Resources 5         IsReady �� TargetProperty �� 
TargetName R� KeyTime  System.Windows.Visibility   Visible R� Value  NotReady   Hidden  
SendSource9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime	 O� Value
   EasingFunction.  (System.Windows.Media.Animation.PowerEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode��%  &	        &	 �     &	 *    �� 5   
   .��5      T� $ (UIElement.Visibility)�6,   $ border��6_   ..�5      i� $ 0:0:0.0��6)   # Z 6;   5      5      �� 5   
   .��5      T� $ (UIElement.Visibility)�6,   $ border��6_   ..�5      i� $ 0:0:0.0��6)   # Z 6;   5      5      �� 5   
   .��5      U� $ (Canvas.Top)�6,   $ 
drageImage��6U   .\�5        $ 0:0:0.3��6'   $
	 -200Q�69   
 5        5      $ EaseIn=�6$   5      5      5       5!      .  (System.Windows.Interactivity.Interaction   Triggers 5#      9  3Microsoft.Expression.Interactivity.Core.DataTrigger  5$   
      Value$ 1��6L   D  >Microsoft.Expression.Interactivity.Core.PropertyChangedTrigger   Binding 6   ��  �� Path$ 
DragStatus� �� ElementName$ uc��.  (System.Windows.Interactivity.TriggerBase   Actions. 5%      F	  @Microsoft.Expression.Interactivity.Media.ControlStoryboardAction	  ?
  9Microsoft.Expression.Interactivity.Media.StoryboardAction 
  
Storyboard# [  6)   5&        5'   
   $ 0��6L    6   �� $ 
DragStatus�$ uc��. 5(      	  # [ 6)   5)        5*   
   $ 2��6L    6   �� $ 
DragStatus�$ uc��. 5+      	  # [ 6)   5,      .��5.      �� -   5         ��Name$ canvas��5.      .+�5/   
   � ��6   �� $ uc��$ ActualHeight���6C   �� $ uc��$ ActualWidth� � RowDefinitions 50      �� 51      $
��auto �6    60   �� 52      6"   .+�54      � -   5         �Name$ Board��54        BoardBackground#��[ 6"   $	��Top=�55      $��Center=�6'   $	��780��56      $	��400��6   $��	0,155,0,0q�57      58      �� 59       �� Orientation$ 
Horizontal=�6   $��1��62   $	��175��5:      $	��top=�5;      $��Center=�6-   $��0,80,0,0q�5<      .+�5=      �� -   5         ��Name$ btn_DailyLast��5=      $��	上一页��60   $��Center=�5>      $��Bottom=�62   $��0,0,30,0q�5?      $��57��6'   $	��120��63   5A      �� 5C      -   5         ��Name$ lb_DailySourcesImg��5C      $��Center=�5D      $
��1150��63   
������ $��Disabled=�5F      $��Hidden=�5G      ! �� IsDeferredScrollingEnabled .  ��.  ��5J      �� 5K      .R�5L      �� $ 
Horizontal=�6)    K� IsItemsHost . 6B   5M      ��5O      �� 5P      .R�5Q      �� -   5        $��0q�5Q   %   ��5R   "   �� 5S   &   .R�5T   *   � $	��260��6/   $	��145��6;   
�������.+�5U   .   ��  ��Name$ 
img_Source��64   $
��Fill=�6H   
 � Tag$ listboxitem��6W   ��5V   4   �� *	TargetImg+�� 5W   .   $��75��65   $��Stretch=�6A   $��Center=�6_   ���#2A2A2A._�5X   2   �� 
�������$��42(�6l   $��Center=�5Y   <    �� TextAlignment$ Center=�5Z   <   $��WordEllipsis=�6S   $��NoWrap=�6o   ��5X   <   �� *	KeyName+5Z   �   5[   /   5\   +   5]   '   5_      5a      5c      �� 5d      -   5        $ btn_DailyNext��5d      $��	下一页��60   $��2��6>   $��Bottom=�5e      $
��Left=�60   $��30,0,0,0q�5f      $��57��6'   $	��120��63   5h      5j      5k      � 5m   
   -   5        $ 
drageImage��5m      $	��300��6$   $	��150��60   
�������$y�	Collapsed=�6T    J� Opacity$	 0.4Q�5n      .+�5o      �� $
��Fill=�6   ��6$   �� *	TargetImg+6C   �� 5p      -	   5         ��Name$ border��5p        �� CornerRadius$  20��6%   $��4q�67   
�������$y�Hidden=�6`   ._�5q      �� -
   5        ! ��Name$! 	rectangle��5q      
�������$	 0.5Q�6C   5r      5s      5t      5u      5v      