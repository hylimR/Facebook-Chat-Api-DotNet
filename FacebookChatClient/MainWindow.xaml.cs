using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FacebookChatApi;

namespace FacebookChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Facebook facebook;

        public MainWindow()
        {
            InitializeComponent();
            facebook = new Facebook();
            facebook.LoggedIn += facebook_LoggedIn;
            facebook.FailedLogin += facebook_FailedLogin;
            facebook.LoggedOut += facebook_LoggedOut;
            facebook.MessageSent += facebook_MessageSent;
            facebook.MessageReceived += facebook_MessageReceived;
            facebook.FileMessageReceived += facebook_FileMessageReceived;
            facebook.PhotoMessageReceived += facebook_PhotoMessageReceived;
            facebook.StickerMessageReceived += facebook_StickerMessageReceived;
            facebook.AnimatedImageMessageReceived += facebook_AnimatedImageMessageReceived;
            facebook.ShareMessageReceived += facebook_ShareMessageReceived;
            facebook.SearchUserCompleted += facebook_SearchUserCompleted;
            facebook.GroupCreated += facebook_GroupCreated;
            facebook.ThreadGet += facebook_ThreadGet;
            facebook.UserAddedToGroup += facebook_UserAddedToGroup;
            facebook.UserRemovedFromGroup += facebook_UserRemovedFromGroup;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                facebook.Email = tbxEmail.Text;
                facebook.Password = tbxPassword.Password;
                facebook.Login();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void facebook_LoggedIn(object sender, EventArgs e)
        {
            try
            {
                facebook.ListenMessage = true;
                btnSearchUser.IsEnabled = true;
                btnSendMessage.IsEnabled = true;
                btnStopListening.IsEnabled = true;
                btnCreateGroup.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void facebook_FailedLogin(object sender, EventArgs e)
        {
            MessageBox.Show("Failed Login");
        }

        private void facebook_LoggedOut(object sender, EventArgs e)
        {
            facebook = new Facebook();

            btnSearchUser.IsEnabled = false;
            btnSendMessage.IsEnabled = false;
            btnStopListening.IsEnabled = false;
            btnCreateGroup.IsEnabled = false;
        }

        private void facebook_MessageSent(object sender, Facebook.MessageSentEventArgs e)
        {
            MessageBox.Show(e.MessageID);
        }

        private void facebook_MessageReceived(object sender, Facebook.MessageReceivedEventArgs e)
        {
            MessageBox.Show("RECEIVED Text message !\n Content : " + e.Message.Body);
        }

        private void facebook_AnimatedImageMessageReceived(object sender, Facebook.MessageReceivedEventArgs e)
        {
            Console.WriteLine("RECEIVED ANIMATED IMAGE !");
            Console.WriteLine(e.Message.ContainAnimatedImageAttachments().ToString());
        }

        private void facebook_StickerMessageReceived(object sender, Facebook.MessageReceivedEventArgs e)
        {
            Console.WriteLine("RECEIVED sticker !");
            Console.WriteLine(e.Message.ContainStickerAttachments().ToString());
            Console.WriteLine(e.Message.Attachments.Count);
        }

        private void facebook_PhotoMessageReceived(object sender, Facebook.MessageReceivedEventArgs e)
        {
            Console.WriteLine("RECEIVED photo message!");
            Console.WriteLine(e.Message.ContainPhotoAttachments().ToString());
        }

        private void facebook_FileMessageReceived(object sender, Facebook.MessageReceivedEventArgs e)
        {
            Console.WriteLine("RECEIVED file message !");
            Console.WriteLine(e.Message.ContainFileAttachments().ToString());
        }

        private void facebook_ShareMessageReceived(object sender, Facebook.MessageReceivedEventArgs e)
        {
            Console.WriteLine("RECEIVED SHARE message !");
            Console.WriteLine(e.Message.ContainShareAttachments().ToString());
        }

        private void facebook_GroupCreated(object sender, Facebook.MessageSentEventArgs e)
        {

            tbxThreadID.Text = e.ThreadID;

        }

        private void facebook_SearchUserCompleted(object sender, Facebook.SearchUserCompletedEventArgs e)
        {
            if (e.UserList.Count > 0)
                tbxSearchResult.Text = e.UserList[0].UserID;
        }

        private void facebook_ThreadGet(object sender, Facebook.ThreadGetEventArgs e)
        {
            Console.WriteLine(e.HasResult);
        }

        private void facebook_UserAddedToGroup(object sender, EventArgs e)
        {
            MessageBox.Show("USER ADDED");
        }

        private void facebook_UserRemovedFromGroup(object sender, EventArgs e)
        {
            MessageBox.Show("USER REMOVED");
        }

        private void btnStopListening_Click(object sender, RoutedEventArgs e)
        {
            if (facebook.ListenMessage == true)
            {
                facebook.ListenMessage = false;
            }
            else
            {
                facebook.ListenMessage = true;
            }
            Console.WriteLine(facebook.ListenMessage.ToString());
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            facebook.SendTextMessage(tbxSendMessage.Text, tbxSearchResult.Text);
        }

        private void btnSearchUser_Click(object sender, RoutedEventArgs e)
        {
            facebook.SearchForUser(tbxSearchTerm.Text);
        }

        private async void btnCreateGroup_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxGroupMembers.Text) && tbxGroupMembers.Text.Contains(','))
            {
                string[] groupMembers = tbxGroupMembers.Text.Split(',');
                string groupName = "TEST CREATE GROUP";
                await facebook.CreateNewGroup(groupName, groupMembers);
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            facebook.Logout();
        }

        private void btnRemoveUser_Click(object sender, RoutedEventArgs e)
        {
            facebook.RemoveUserFromGroup(tbxUserID.Text, tbxThreadID.Text);
        }
    }
}
