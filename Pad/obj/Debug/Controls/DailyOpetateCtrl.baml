   M S B A M L   `   `   `  ����>  :Pad, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Pad.ControlsPad.Controls  5      #    Pad.Controls.DailyOpetateCtrl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bei9http://schemas.microsoft.com/expression/2010/interactions  5      &localclr-namespace:Pad.Controls  5      -   5          >�Name$  uc��5	       $
��1620��5
      $
��1000��6    � 	Resources 5         IsReady �� TargetProperty �� 
TargetName R� KeyTime  System.Windows.Visibility   Visible R� Value  NotReady   Hidden  
SendSource9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime	 O� Value
   EasingFunction.  (System.Windows.Media.Animation.PowerEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode   	Collapsed��%�  &	        &	 �     &	 *    �� 5   
   .��5      T� $ (UIElement.Visibility)�6,   $ border��6_   ..�5      i� $ 0:0:0.0��6)   # Z 6;   5      5      �� 5   
   .��5      T� $ (UIElement.Visibility)�6,   $ border��6_   ..�5      i� $ 0:0:0.0��6)   # Z 6;   5      5      �� 5   
   .��5      U� $ (Canvas.Top)�6,   $ 
drageImage��6U   .\�5        $ 0:0:0.2��6'   $
	 -500Q�69   
 5        5      $ EaseIn=�6$   5      5      T� 5      $ (UIElement.Visibility)�6,   $ 
drageImage��6_   ..�5       i� $ 0:0:0.3��6)   # Z 6;   5!      5"      5#      .  (System.Windows.Interactivity.Interaction   Triggers 5%      9  3Microsoft.Expression.Interactivity.Core.DataTrigger  5&   
      Value$ 1��6L   D  >Microsoft.Expression.Interactivity.Core.PropertyChangedTrigger   Binding 6   ��  �� Path$ 
DragStatus� �� ElementName$ uc��.  (System.Windows.Interactivity.TriggerBase   Actions. 5'      F	  @Microsoft.Expression.Interactivity.Media.ControlStoryboardAction	  ?
  9Microsoft.Expression.Interactivity.Media.StoryboardAction 
  
Storyboard# [  6)   5(        5)   
   $ 0��6L    6   �� $ 
DragStatus�$ uc��. 5*      	  # [ 6)   5+        5,   
   $ 2��6L    6   �� $ 
DragStatus�$ uc��. 5-      	  # [ 6)   5.      .��51      �� -   5         ��Name$ canvas��51      .+�52   
   � ��6   �� $ uc��$ ActualHeight���6C   �� $ uc��$ ActualWidth� � RowDefinitions 53      �� 54      $
��auto �6    �� 55      $��* �6    .+�58      � $��110,185,110,90q�6   $
��1400��6+   $	��350��68   $	��Top=�6E     BoardBackground#��[ 59       � ShowGridLines .   5:      �� 5;      $��* �6$   �� 5<      $��* �6$   �� 5=      $��* �6$   �� 5>      $��* �6$   �� 5?      $��* �6$   �� 5@      $��* �6$   �� 5A      $��* �6$   �� 5B      $��* �6$   �� 5C      $��* �6$   �� 5D      $��* �6$    � ColumnDefinitions 5F      �� 5G      $��* �6'   �� 5H      $��* �6'   �� 5I      $��* �6'   �� 5J      $��* �6'   �� 5K      $��* �6'   �� 5L      $��* �6'   �� 5M      $��* �6'   �� 5N      $��* �6'   �� 5O      $��* �6'   �� 5P      $��* �6'   .+�5R      � -   5         �Name$ 
DailyBoard��5R      $��10��6+   $��10��6@   5S      5T      � 5V      $��1��6   $	��175��6     5W      �� 5X      $��* �6'   �� 5Y      $
��auto �6'   �� 5Z      $��* �6'   .+�5\      �� -   5         ��Name$ btn_DailyLast��5\      $��	上一页��60   $��0��6?   $��Bottom=�5]      $��Right=�64   $��0,0,20,0q�5^      $	��110��5_      $��52��6%   6O   �� 5`      -   5        $ btn_DailyNext��5`      $��	下一页��60   $��2��6>   $��Bottom=�5a      $
��Left=�64   $��20,0,0,0q�5b      $	��110��5c      $��52��6%   5d   8   �� 5e      -   5         ��Name$ lb_DailySourcesImg��5e      $
��1135��5f      $	��175��6#   $	��Top=�5g      
������ $��Disabled=�5h      $��Hidden=�5i      ! �� IsDeferredScrollingEnabled .  ��.  $��1��5k   <   ��5l      �� 5m      .R�5n      ��  �� Orientation$ 
Horizontal=�6)    K� IsItemsHost . 6B   5o      ��5q      �� 5r      .R�5|      �� -   5        $��0q�5|   %   ��5}   "   �� 5~   &   .R�5   *   � $	��300��6/   $	��150��6;   
�������.+�5�   .   ��  ��Name$ 
img_Source��64   $
��Fill=�6H   
 � Tag$ listboxitem��6W   ��5�   4   �� *	TargetImg+6S   �� 5�   .   $��75��65   $��Stretch=�6A   $��Center=�6_   ���#2A2A2A._�5�   2   �� 
�������$��42(�6l   $��Center=�5�   <     �� TextAlignment$  Center=�5�   <   $��WordEllipsis=�6S   $��NoWrap=�6o   ��5�   <   �� *	KeyName+5�   �   5�   /   5�   +   5�   '   5�      5�      5�      5�      5�      � 5�   
   -   5        $ 
drageImage��5�      $	��300��6$   $	��150��60   
�������$y�	Collapsed=�6T   ! J� Opacity$	! 0.4Q�5�      .+�5�      �� $
��Fill=�6   ��6$   �� *	TargetImg+6C   �� 5�      -	   5        " ��Name$" border��5�      # �� CornerRadius$# 20��6%   $��4q�67   
�������$y�Hidden=�6`   ._�5�      �� -
   5        $ ��Name$$ 	rectangle��5�      
�������$	! 0.5Q�6C   5�      5�      5�      5�      5�      