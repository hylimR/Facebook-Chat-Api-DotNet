using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using EdgeJs;

namespace FacebookChatApi
{
    public class Facebook
    {
        #region "Variable"
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsLoggedIn { get; private set; }
        public string CurrentLoginUserID { get; private set; }

        private bool listenMessage;
        public bool ListenMessage
        {
            get { return listenMessage; }
            set
            {
                //If original value same as argument , no need to do anything.
                if (listenMessage == value) return;
                listenMessage = value;
                OnMessageListenerToggled(new EventArgs());
            }
        }
        #endregion

        #region "Event Delegate Definition"
        public delegate void LoggedInEventHandler(object sender, EventArgs e);
        public delegate void FailedLoginEventHandler(object sender, EventArgs e);
        public delegate void MessageSentEventHandler(object sender, MessageSentEventArgs e);
        public delegate void MessageListenerToggleEventHandler(object sender, EventArgs e);
        public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void StickerMessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void PhotoMessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void FileMessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void ShareMessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void AnimatedImageMessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void SearchUserCompletedEventHandler(object sender, SearchUserCompletedEventArgs e);
        #endregion

        #region "Event List"
        /// <summary>
        /// Event fired when login successful
        /// </summary>
        public event LoggedInEventHandler LoggedIn;
        /// <summary>
        /// Event fired when login failed
        /// </summary>
        public event FailedLoginEventHandler FailedLogin;
        /// <summary>
        /// Event fired when message is successfully sent
        /// </summary>
        public event MessageSentEventHandler MessageSent;
        /// <summary>
        /// Event fired when message listener is toggled.
        /// </summary>
        public event MessageListenerToggleEventHandler MessageListenerToggle;
        /// <summary>
        /// Event fired when a text message is received
        /// </summary>
        public event MessageReceivedEventHandler MessageReceived;
        /// <summary>
        /// Event fired when a sticker message is received
        /// </summary>
        public event StickerMessageReceivedEventHandler StickerMessageReceived;
        /// <summary>
        /// Event fired when a photo message is received
        /// </summary>
        public event PhotoMessageReceivedEventHandler PhotoMessageReceived;
        /// <summary>
        /// Event fired when a file message is received
        /// </summary>
        public event FileMessageReceivedEventHandler FileMessageReceived;
        /// <summary>
        /// Event fired when a share message is received
        /// </summary>
        public event ShareMessageReceivedEventHandler ShareMessageReceived;
        /// <summary>
        /// Event fired when a animated image(GIF) is received
        /// </summary>
        public event AnimatedImageMessageReceivedEventHandler AnimatedImageMessageReceived;
        /// <summary>
        /// Event fired when user search result is returned
        /// </summary>
        public event SearchUserCompletedEventHandler SearchUserCompleted;
        #endregion

        #region "Constructor"
        //Empty Constructor
        public Facebook()
        {
            this.listenMessage = false;
        }

        public Facebook(string email, string password)
        {
            this.Email = email;
            this.Password = password;
            this.listenMessage = false;
        }
        #endregion

        #region "LOGIN/LOGOUT"
        ///<summary>
        ///Login into facebook, email and password must be set
        ///</summary>
        public async Task Login()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(this.Password))
                {
                    throw new Exception("Please set email and password before login");
                }

                var login = Edge.Func(NodeJSScriptConstant.METHOD_LOGIN);
                dynamic result = await login(new { email = this.Email, password = this.Password });

                if ((string) result.status == "200")
                {
                    IsLoggedIn = true;
                    CurrentLoginUserID = (string)result.currentUserID;
                    OnLoggedIn(new EventArgs());
                    return;
                }

                IsLoggedIn = false;
                OnFailedLogin(new EventArgs());
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at Login : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }

        ///<summary>
        ///<para>Will automatically approve of any recent logins and continue with the login process.</para>
        ///</summary>
        public async Task ForceLogin()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(this.Password))
                {
                    throw new Exception("Please set email and password before login");
                }

                var login = Edge.Func(NodeJSScriptConstant.METHOD_LOGIN);
                var result = await login(new { email = this.Email, password = this.Password, forceLogin = true });

                if ((string)result == "200")
                {
                    IsLoggedIn = true;
                    OnLoggedIn(new EventArgs());
                    return;
                }

                IsLoggedIn = false;
                OnFailedLogin(new EventArgs());
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at Login : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }
        #endregion

        #region "SEND MESSAGE"
        ///<summary>
        ///Normal message consist of only string
        ///</summary>
        public async void SendTextMessage(string message, string receiverID)
        {
            try
            {
                //Pass a delegate to handle Node JS event
                var onMessageSent = (Func<object, Task<object>>)(async (messageInfo) =>
                {
                    JObject jObj = JObject.Parse((string)messageInfo);
                    MessageSentEventArgs e = new MessageSentEventArgs(
                        jObj.GetValue("threadID").ToString(),
                        jObj.GetValue("messageID").ToString(),
                        jObj.GetValue("timestamp").ToString());

                    //Raise Message sent event
                    OnMessageSent(e);

                    //Return any object back to Node JS if required
                    return "";
                });

                var sendMessage = Edge.Func(NodeJSScriptConstant.METHOD_SEND_MESSAGE);
                var result = await sendMessage(new
                {
                    type = "text",
                    message = message,
                    receiver = receiverID,
                    onMessageSent = onMessageSent
                });

            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at SendTextMessage : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }

        ///<summary>
        ///Any type of readable/facebook acceptable file (include images/text etc)
        ///</summary>
        public async void SendFileMessage(string filePath, string receiverID)
        {
            try
            {
                //Pass a delegate to handle Node JS event
                var onMessageSent = (Func<object, Task<object>>)(async (messageInfo) =>
                {
                    JObject jObj = JObject.Parse((string)messageInfo);
                    MessageSentEventArgs e = new MessageSentEventArgs(
                        jObj.GetValue("threadID").ToString(),
                        jObj.GetValue("messageID").ToString(),
                        jObj.GetValue("timestamp").ToString());

                    //Raise Message sent event
                    OnMessageSent(e);

                    //Return any object back to Node JS if required
                    return "";
                });

                var sendMessage = Edge.Func(NodeJSScriptConstant.METHOD_SEND_MESSAGE);
                var result = await sendMessage(new
                {
                    type = "file",
                    attachment = filePath,
                    receiver = receiverID,
                    onMessageSent = onMessageSent
                });
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at SendImageMessage : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }

        ///<summary>
        ///Send a sticker image by supplying sticker ID
        ///</summary>

        /* TODO -> Need a way to obtain sticker ID , every account have different sticker pack
        public async void SendStickerMessage(string filePath, string receiverID)
        {
            try
            {
                var onMessageSent = (Func<object, Task<object>>)(async (messageInfo) =>
                {
                    JObject jObj = JObject.Parse((string)messageInfo);
                    MessageSentEventArgs e = new MessageSentEventArgs(
                        jObj.GetValue("threadID").ToString(),
                        jObj.GetValue("messageID").ToString(),
                        jObj.GetValue("timestamp").ToString());

                    //Raise Message sent event
                    OnMessageSent(e);

                    //Return any object back to Node JS if required
                    return "";
                });

                var sendMessage = Edge.Func(NodeJSScriptConstant.METHOD_SEND_MESSAGE);
                var result = await sendMessage(new
                {
                    type = "file",
                    attachment = filePath,
                    receiver = receiverID,
                    onMessageSent = onMessageSent
                });
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at SendImageMessage : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }
        */

        ///<summary>
        ///Send a url message (can be website url, image url, file attachment url etc.)
        ///</summary>
        public async void SendLinkMessage(string link, string receiverID)
        {
            try
            {
                //Pass a delegate to handle Node JS event
                var onMessageSent = (Func<object, Task<object>>)(async (messageInfo) =>
                {
                    JObject jObj = JObject.Parse((string)messageInfo);
                    MessageSentEventArgs e = new MessageSentEventArgs(
                        jObj.GetValue("threadID").ToString(),
                        jObj.GetValue("messageID").ToString(),
                        jObj.GetValue("timestamp").ToString());

                    //Raise Message sent event
                    OnMessageSent(e);

                    //Return any object back to Node JS if required
                    return "";
                });

                var sendMessage = Edge.Func(NodeJSScriptConstant.METHOD_SEND_MESSAGE);
                var result = await sendMessage(new
                {
                    type = "url",
                    url = link,
                    receiver = receiverID,
                    onMessageSent = onMessageSent
                });
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at SendLinkMessage : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }
        #endregion

        #region "RECEIVED MESSAGE"
        private async Task ListeningToMessage()
        {
            var onMessageReceived = (Func<object, Task<object>>)(async (messageJson) =>
            {
                Message message = Message.Parse((string)messageJson);
                MessageReceivedEventArgs e = new MessageReceivedEventArgs(message);
                //Raise Message sent event
                if (!message.ContainAttachments())
                {
                    OnMessageReceived(e);
                }
                else
                {
                    if (message.ContainFileAttachments())
                    {
                        OnFileMessageReceived(e);
                    }

                    if (message.ContainPhotoAttachments())
                    {
                        OnPhotoMessageReceived(e);
                    }

                    if (message.ContainShareAttachments())
                    {
                        OnShareMessageReceived(e);
                    }

                    if (message.ContainStickerAttachments())
                    {
                        OnStickerMessageReceived(e);
                    }

                    if (message.ContainAnimatedImageAttachments())
                    {
                        OnAnimatedImageMessageReceived(e);
                    }
                }

                //Return any object back to Node JS if required
                return "";
            });

            var toggleMessageListening = Edge.Func(NodeJSScriptConstant.METHOD_MESSAGE_LISTENER);
            await toggleMessageListening(new
            {
                listenMessage = this.listenMessage,
                onMessageReceived = onMessageReceived
            });
        }
        #endregion

        #region "Utilities"
        /// <summary>
        /// Search user based on email OR username
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async void SearchForUser(string keyword)
        {
            var searchForUser = Edge.Func(NodeJSScriptConstant.METHOD_SEARCH_USER);
            var result = await searchForUser(new { keyword = keyword });

            List<UserSearchResult> userList = UserSearchResult.Parse((string)result);
            string message = "Search Completed ! ";
            if(userList.Count == 0)
            {
                message += " No results found !";
            }
            else
            {
                message += userList.Count + " Results Found ~";
            }

            SearchUserCompletedEventArgs args = new SearchUserCompletedEventArgs(userList, message);
            OnSearchUserCompleted(args);
        }
        #endregion

        #region "Event"
        protected void OnLoggedIn(EventArgs e)
        {
            if (LoggedIn != null)
                LoggedIn(this, e);
        }

        protected void OnFailedLogin(EventArgs e)
        {
            if (FailedLogin != null)
                FailedLogin(this, e);
        }

        protected async void OnMessageListenerToggled(EventArgs e)
        {
            await ListeningToMessage();
            if (MessageListenerToggle != null)
                MessageListenerToggle(this, e);
        }

        protected void OnMessageSent(MessageSentEventArgs e)
        {
            if (MessageSent != null)
                MessageSent(this, e);
        }

        protected void OnMessageReceived(MessageReceivedEventArgs e)
        {
            if (MessageReceived != null)
                MessageReceived(this, e);
        }

        protected void OnFileMessageReceived(MessageReceivedEventArgs e)
        {
            if (FileMessageReceived != null)
                FileMessageReceived(this, e);
        }

        protected void OnPhotoMessageReceived(MessageReceivedEventArgs e)
        {
            if (PhotoMessageReceived != null)
                PhotoMessageReceived(this, e);
        }

        protected void OnShareMessageReceived(MessageReceivedEventArgs e)
        {
            if (ShareMessageReceived != null)
                ShareMessageReceived(this, e);
        }

        protected void OnStickerMessageReceived(MessageReceivedEventArgs e)
        {
            if (StickerMessageReceived != null)
                StickerMessageReceived(this, e);
        }

        protected void OnAnimatedImageMessageReceived(MessageReceivedEventArgs e)
        {
            if (AnimatedImageMessageReceived != null)
                AnimatedImageMessageReceived(this, e);
        }

        protected void OnSearchUserCompleted(SearchUserCompletedEventArgs e)
        {
            if (SearchUserCompleted != null)
                SearchUserCompleted(this, e);
        }
        #endregion

        #region "Event Argument"
        public class MessageSentEventArgs : EventArgs
        {
            public string ThreadID { get; private set; }
            public string MessageID { get; private set; }
            public DateTime Timestamp { get; private set; }

            internal MessageSentEventArgs(string threadID, string messageID, long timeStamp)
            {
                this.ThreadID = threadID;
                this.MessageID = messageID;
                this.Timestamp = new DateTime(timeStamp);
            }

            internal MessageSentEventArgs(string threadID, string messageID, string timeStamp)
            {
                long timeStampInTick;
                long.TryParse(timeStamp, out timeStampInTick);

                this.ThreadID = threadID;
                this.MessageID = messageID;
                this.Timestamp = new DateTime(timeStampInTick);
            }
        }

        public class MessageReceivedEventArgs : EventArgs
        {
            public Message Message { get; private set; }

            internal MessageReceivedEventArgs(Message message)
            {
                Message = message;
            }
        }

        public class SearchUserCompletedEventArgs : EventArgs
        {
            public List<UserSearchResult> UserList { get; private set; }
            public string Message { get; private set; }

            internal SearchUserCompletedEventArgs(List<UserSearchResult> userList, string message)
            {
                this.UserList = userList;
                this.Message = message;
            }
        }
        #endregion
    }
}
