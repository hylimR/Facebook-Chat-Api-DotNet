   M S B A M L   `   `   `  ����M  IFacebookChatClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:FacebookChatClientFacebookChatClient  5   	   #    FacebookChatClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:FacebookChatClient     5   	     � Title$  
MainWindow��5   	   $	��540��6   $	��525��6)   .��5	      � $��	0,-60,0,0q�6   .+�5
   
   �� -   5         ��Name$ lblEmail��5
      $��Email : ��6"   $
��Left=�65   $��	31,87,0,0q�6P   $	��Top=�6c   �� 5   
   -   5        $ lblPassword��5      $��Password : ��6%   $
��Left=�6;   $��
10,113,0,0q�6V   $	��Top=�6j   �� 5   
   -   5         ��Name$ tbxEmail��5      $
��Left=�6$   $��	79,87,0,0q�6?    �� TextWrapping$
 Wrap=�6R   $��zzyun1120@hotmail.com��6f   $	��Top=�6�   $	��428��6�   G� 5   
   -   5         G�Name$ tbxPassword��5      $
��Left=�6+   $��
79,119,0,0q�6F   $	��Top=�6Z   $	��428��6r   �� 5   
   -   5         ��Name$ btnLogin��5      $��Login��6#   $
��Left=�63   $��174,144,0,0q�6N   $	��Top=�6c   $��98��6{    J� RenderTransformOrigin$ 0,-0.25&�6�   6�   �� 5   
   -   5        $ btnStopListening��5      {�.  $��StopListening��6=   $
��Left=�6U   $��174,288,0,0q�6p   $	��Top=�6�   $��98��6�   6�   �� 5   
   -   5        $��tbxSendMessage��5      $
��Left=�6)   $��23��6D   $��
10,187,0,0q�6P   $
 Wrap=�6d   $	��Top=�6x   $	��497��6�   �� 5   
   -   5        $��btnSendMessage��5      {�.  $��Send Message��6:   $
��Left=�6Q   $��174,213,0,0q�6l   $	��Top=�6�   $��98��6�   6�   �� 5   
   -	   5        $��tbxReceivedMessage��5      $
��Left=�6,   $��23��6G   $��
10,250,0,0q�6S   $
 Wrap=�6g   $	��Top=�6{   $	��497��6�   �� 5   
   -
   5        $��tbxSearchTerm��5      $
��Left=�6'   $��23��6B   $��
10,315,0,0q�6N   $
 Wrap=�6b   $	��Top=�6v   $	��497��6�   �� 5   
   -   5        $��tbxSearchResult��5      {�.  $
��Left=�6;   $��23��6V   $��
10,370,0,0q�6b   $
 Wrap=�6v   $	��Top=�6�   $	��497��6�   �� 5   
   -   5        $��btnSearchUser��5      {�.  $��Search User��68   $
��Left=�6N   $��174,343,0,0q�6i   $	��Top=�6~   $��98��6�   6�   �� 5   
   -   5        $��tbxGroupMembers��5      $
��Left=�6)   $��23��6D   $��
10,427,0,0q�6P   $
 Wrap=�6d   $	��Top=�6x   $	��497��6�   �� 5   
   -   5        $��btnCreateGroup��5      {�.  $��Create Group��69   $
��Left=�6P   $��174,455,0,0q�6k   $	��Top=�6�   $��98��6�   6�   �� 5   
   -   5        $��	btnLogout��5      $��Logout��6"   $
��Left=�63   $��277,144,0,0q�6N   $	��Top=�6c   $	��113��6{   6�   �� 5   
   -   5        $��	tbxUserID��5      $
��Left=�6#   $��23��6>   $��
10,490,0,0q�6J   $
 Wrap=�6^   $��TextBox��6r   $	��Top=�6�   $	��120��6�   �� 5   
   -   5        $��tbxThreadID��5      $
��Left=�6%   $��23��6@   $��
10,527,0,0q�6L   $
 Wrap=�6`   $��TextBox��6t   $	��Top=�6�   $	��120��6�   �� 5   
   -   5        $��btnRemoveUser��5      $��Remove User��6&   $
��Left=�6<   $��174,511,0,0q�6W   $	��Top=�6l   $��98��6�   6�   5      5      