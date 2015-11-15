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
            facebook.MessageSent += facebook_MessageSent;
            facebook.MessageReceived += facebook_MessageReceived;
            facebook.FileMessageReceived += facebook_FileMessageReceived;
            facebook.PhotoMessageReceived += facebook_PhotoMessageReceived;
            facebook.StickerMessageReceived += facebook_StickerMessageReceived;
            facebook.AnimatedImageMessageReceived += facebook_AnimatedImageMessageReceived;
            facebook.SearchUserCompleted += Facebook_SearchUserCompleted;
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
                facebook.SearchForUser("zzyun1120@hotmail.com");
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

        private void facebook_MessageSent(object sender, Facebook.MessageSentEventArgs e)
        {
            MessageBox.Show(e.MessageID);
        }

        private void facebook_MessageReceived(object sender, Facebook.MessageReceivedEventArgs e)
        {
            Console.WriteLine("RECEIVED Text message !");
            Console.WriteLine(e.Message.Body);
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

        private void Facebook_SearchUserCompleted(object sender, Facebook.SearchUserCompletedEventArgs e)
        {
            Console.WriteLine(e.Message);
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
    }
}
