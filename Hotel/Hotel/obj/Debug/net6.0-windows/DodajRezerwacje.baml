   M S B A M L   `   `   `  ����	  Hotel    Hotel.DodajRezerwacje   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Dodaj Rezerwację��5   	   $	��500��6"   $	��450��6/   ���#c77730 � 	Resources 5         ButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers :� RoutedEvent		 �� To
 �� TargetProperty g� FillBehavior g� Duration W� From	 W� To  
LabelStyle  ComboBoxStyle  DatePickerStyle(  "System.Windows.Controls.DatePicker��%�  &	        &	 �    &	 �    &	     �� 5   
   " ��6&   .�5      ��  � ��$ 40��6)   �� 5       � ��$	 200��6(   �� 5	       � �� �#F8FAE56-   �� 5
       � �� �#ab40196-   �� 5       � ��$ 14(�6+   �� 5       � ��$ 10q�6)   �� 5       � ��$ Trebuchet MS)�6+   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   ���#ab4019$��1q�6a   $ 6��6u   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5       5      :� 5      $ Button.MouseEnter��6   .[�5      �� .��5      �� .��5      �� 	 #d862436-   $1
 +(Button.Background).(SolidColorBrush.Color)�6:   $ HoldEnd=�6�   $ 0:0:0.2D�6�   W� 5      $
 (Button.Width)�6.    2006Y    2206d   $ HoldEnd=�6m   $ 0:0:.2D�6�   5      5      5       :� 5!      $ Button.MouseLeave��6   .[�5"      �� .��5#      �� .��5$      �� 	 #ab40196-   $1
 +(Button.Background).(SolidColorBrush.Color)�6;   $
 Stop=�6�   $ 0:0:0.2D�6�   W� 5%      $
 (Button.Width)�6.    2206Y    2006d   $
 Stop=�6m   $ 0:0:.2D�6�   5&      5'      5(      5*      �� 5,   
   " ��6%   .�5-      ��  � �� �#fefefe6+   �� 5.       � ��$ Trebuchet MS)�6+   �� 5/       � ��$ 0 6 0 0q�6'   �� 50       � ��$ 13(�6)   51      �� 53   
   " ��6(   .�54      ��  � �� �#ab40196+   �� 55       � ��$ Trebuchet MS)�6+   56      �� 58   
   "  6*   .�59      ��  � �� �#ab40196+   �� 5:       � ��$ Trebuchet MS)�6+   5;      5<      .��5>      �� $��10q�6   .+�5?   
   �� #��[ 6   .��64   	Gość:6;   �� 5@   
   -   5         ��Name$ cbGosc��5@      #��[ 6#   �� 5B   
   #��[ 6   .��64   Data rozpoczęcia:6G     5C   
   -   5          Name$ 
dpPoczatek��5C      #��[ 6)   �� 5E   
   #��[ 6   .��64   Data zakończenia:6G     5F   
   -   5        $ dpKoniec��5F      #��[ 6'   �� 5H   
   #��[ 6   .��64   86Pokój (jeżeli puste zostanie dobrany automatycznie):6j   �� 5I   
   -   5        $ cbPokoj��5I      #��[ 6$   �� 5K   
   #��[ 6   .��64   Zakładający rezerwację:6M   �� 5L   
   -   5        $ cbZakladajacy��5L      #��[ 6*   �� 5N   
   #��[ 6   .��64   Dodatkowi Goście:6G   �� 5O   
   -   5         ��Name$ lbDodatkowiGoscie��5O      $	��100��6-    �� SelectionMode$ Multiple=�6:   $��Trebuchet MS)�6S   ���#ab40196m   �� 5Q   
   -   5        $��Utwórz rezerwację��5Q      #��[  6L   5R      5S      