   M S B A M L   `   `   `  ����K  GMagicQuizDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:MagicQuizDesktop.View.PagesMagicQuizDesktop.View.Pages  5      I)clr-namespace:MagicQuizDesktop.ViewModelsMagicQuizDesktop.ViewModels  5      I)clr-namespace:MagicQuizDesktop.ConvertersMagicQuizDesktop.Converters  5      _ [MaterialDesignThemes.Wpf, Version=5.0.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aeHclr-namespace:MaterialDesignThemes.Wpf;assembly=MaterialDesignThemes.WpfMaterialDesignThemes.Wpf 5	      +    %MagicQuizDesktop.View.Pages.UsersPage   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:MagicQuizDesktop.View.Pages     5      8mw)clr-namespace:MagicQuizDesktop.ViewModels     5      ?	converter)clr-namespace:MagicQuizDesktop.Converters     5      cmaterialDesignHclr-namespace:MaterialDesignThemes.Wpf;assembly=MaterialDesignThemes.Wpf     5	        N� Title$  	UsersPage��5       � DataContext 5      0   *MagicQuizDesktop.ViewModels.UsersViewModel  5   
    � 	Resources 5      B   <MagicQuizDesktop.Converters.BooleanToActiveInactiveConverter $   BooleanToActiveInactiveConverter��%   &	          5   
   65   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      .+�5   
   �� ���#D7596D$��12(�5      $��Medium"�5      $��
Montserrat)�5       $��10q�5!      $
��Wrap=�5"      ��5      �� *ErrorMessage+5"      �� 5#   
   
������ $��0q�5%      $��1��5&      ��5#      �� *Users+��5'   
   �� 5(      .R�5)      �  � Orientation$ 
Horizontal=�6   ��65   �� *ActualWidth+ �� RelativeSource ��  �� AncestorType ListView5*      ��5,   
   �� 5-      .R�5.      �� 
������ $��0q�51      ��5/      �� *#!DataContext.ShowUserWindowCommand+ �� " ����50      �� .��53      #  MaterialDesignThemes.Wpf.Card  $��5q�6*   $��16q�54   "   $	��380��55   "   $	��200��56   "   
�����_�.��58      �  � ColumnDefinitions 59      �� 5:   "   �� 5;   "   $
��auto �63    5=      �� 5>   "   �� 5?   "   �� 5@   "   $
��auto �60   .+�5D      �� $	��100��6%   $	��100��5E      
�������$��Center=�5G      $��Center=�5H       �� CornerRadius$ 50��5I      
���   �$��0��5K      $��2��5L      $��2q�5M      ._�5O   "   �� $	��100��5P   %   $	��100��5Q   %   $��Uniform=�5R   %   ��5O   )   �� *Avatar+5R   %   5S      �� 5U      $
��Bold"�6<   $
��Left=�6N   $��0��6i   $��20(�6v   ��6(   �� *Id+6v   �� 5V      $��2��6?   $��Center=�6T   $
��Bold"�6q   $��2��6�   $��20(�6�   $
��Wrap=�6�   
���������6(   �� *Name+6�   5W      5[      5\      5]      ��5_   
   �� 5`      .R�5a      �� $��Hidden=�6   .��5b      �� 5c      5d      5f      5g      5j      