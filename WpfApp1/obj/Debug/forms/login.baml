   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:WpfApp1.formsWpfApp1.forms  5   	       WpfApp1.forms.login   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:WpfApp1.forms     5   	   -   5          � Title$  Sistema POS��5   	    � SizeToContent$ Manual=�6   $��618.75��64   $��721.603��6D    � WindowStartupLocation$ CenterScreen=�6T    � 	Resources 5	      -  'WpfApp1.forms.HeightToFontSizeConverter   HeightToFontSizeConverter��%   &	          5
   
   6*   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $��0��6   $	��250��6   ��5      �� 5       �� ImageSource$( "/WpfApp1;component/image/logo3.png��6   5      �� 5   
   $��1��6   $��50q�6"   .+�5       G� -   5         G�Name$ psswd��5        � ToolTip$ Contraseña��6*   $��40��6?   $	��250��6K   $��0,0,0,20q�6W   ��5!      �� 	 �� ElementName$	 psswd��
 �� Path$
 ActualHeight� �� 	Converter# [  �� 5#      -   5         ��Name$
 Exit��5#   %   $��Salir��6   $��18(�63   ���	#FF0C0A0A
�������$	��150��5$      $��40��6%   $��0,0,0,20q�61   ��5%      �� 5&       �� 
TargetType" ��6&   .R�5'      �� #��z��6!   #��z��5(      #��z��5)       �� CornerRadius$ 10��5*      ._�5+      �� $��5,0,5,0q�6/   $��Center=�5,   !   $��Center=�5-   !   ��. 5.   !   5/      50      52      53      �� 56   
   $��2��6   ���#EEEEEE._�57      �� $8��2© 2023 Todos los derechos reservados | Theriosoft��6   $��12(�6Q   $��Center=�58      $��Center=�65   $
��0,10q�6P   59      5:      5;      