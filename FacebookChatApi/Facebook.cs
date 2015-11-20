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
        #region "CONSTANT"
        public const string FACEBOOK_STATUS_OK = "200";
        public const string FACEBOOK_STATUS_ERROR = "0";
        #endregion
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
        public delegate void LoggedOutEventHandler(object sender, EventArgs e);
        public delegate void FailedLogoutEventHandler(object sender, EventArgs e);
        public delegate void MessageSentEventHandler(object sender, MessageSentEventArgs e);
        public delegate void MessageSentFailedEventHandler(object sender, EventArgs e);
        public delegate void MessageListenerToggleEventHandler(object sender, EventArgs e);
        public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void StickerMessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void PhotoMessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void FileMessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void ShareMessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void AnimatedImageMessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public delegate void SearchUserCompletedEventHandler(object sender, SearchUserCompletedEventArgs e);
        public delegate void GroupCreatedEventHandler(object sender, MessageSentEventArgs e);
        public delegate void GroupCreationFailedEventHandler(object sender, EventArgs e);
        public delegate void GroupRenamedEventHandler(object sender, EventArgs e);
        public delegate void GroupRenamedFailedEventHandler(object sender, EventArgs e);
        public delegate void ThreadGetEventHandler(object sender, ThreadGetEventArgs e);
        public delegate void UserAddedToGroupEventHandler(object sender, EventArgs e);
        public delegate void UserRemovedFromGroupEventHandler(object sender, EventArgs e);
        public delegate void GroupChatDeletedEventHandler(object sender, EventArgs e);
        public delegate void GroupChatDeleteFailedEventHandler(object sender, EventArgs e);
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
        /// Event fired when logged out
        /// </summary>
        public event LoggedOutEventHandler LoggedOut;
        /// <summary>
        /// Event fired when logout failed
        /// </summary>
        public event FailedLogoutEventHandler FailedLogout;
        /// <summary>
        /// Event fired when message is successfully sent
        /// </summary>
        public event MessageSentEventHandler MessageSent;
        /// <summary>
        /// Event fired when message sent failed
        /// </summary>
        public event MessageSentFailedEventHandler MessageSentFailed;
        /// <summary>
        /// Event fired when message listener is toggled.
        /// </summary>
        public event MessageListenerToggleEventHandler MessageListenerToggle;
        /// <summary>
        /// Event fired when a text message is received (CROSS-THREAD event)
        /// </summary>
        public event MessageReceivedEventHandler MessageReceived;
        /// <summary>
        /// Event fired when a sticker message is received  (CROSS-THREAD event)
        /// </summary>
        public event StickerMessageReceivedEventHandler StickerMessageReceived;
        /// <summary>
        /// Event fired when a photo message is received    (CROSS-THREAD event)
        /// </summary>
        public event PhotoMessageReceivedEventHandler PhotoMessageReceived;
        /// <summary>
        /// Event fired when a file message is received (CROSS-THREAD event)
        /// </summary>
        public event FileMessageReceivedEventHandler FileMessageReceived;
        /// <summary>
        /// Event fired when a share message is received    (CROSS-THREAD event)
        /// </summary>
        public event ShareMessageReceivedEventHandler ShareMessageReceived;
        /// <summary>
        /// Event fired when a animated image(GIF) is received  (CROSS-THREAD event)
        /// </summary>
        public event AnimatedImageMessageReceivedEventHandler AnimatedImageMessageReceived;
        /// <summary>
        /// Event fired when user search result is returned
        /// </summary>
        public event SearchUserCompletedEventHandler SearchUserCompleted;
        /// <summary>
        /// Event fired when group creation success
        /// </summary>
        public event GroupCreatedEventHandler GroupCreated;
        /// <summary>
        /// Event fired when group creation failed
        /// </summary>
        public event GroupCreationFailedEventHandler GroupCreationFailed;
        /// <summary>
        /// Event fired when group renaming success
        /// </summary>
        public event GroupRenamedEventHandler GroupRenamed;
        /// <summary>
        /// Event fired when group renaming failed
        /// </summary>
        public event GroupRenamedFailedEventHandler GroupRenamedFailed;
        /// <summary>
        /// Event fired when thread list query result returned
        /// </summary>
        public event ThreadGetEventHandler ThreadGet;
        /// <summary>
        /// Event fired when user is added to a group
        /// </summary>
        public event UserAddedToGroupEventHandler UserAddedToGroup;
        /// <summary>
        /// Event fired when user is removed from a group
        /// </summary>
        public event UserRemovedFromGroupEventHandler UserRemovedFromGroup;
        /// <summary>
        /// Event fired when group chat is deleted
        /// </summary>
        public event GroupChatDeletedEventHandler GroupChatDeleted;
        /// <summary>
        /// Event fired when group chat deletion failed
        /// </summary>
        public event GroupChatDeleteFailedEventHandler GroupChatDeleteFailed;
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
        public async Task Login(bool forceLogin = false)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(this.Password))
                {
                    throw new Exception("Please set email and password before login");
                }

                //Start NodeJS script to start logging in
                var login = Edge.Func(NodeJSScriptConstant.METHOD_LOGIN);
                dynamic result = await login(new { email = this.Email, password = this.Password, forceLogin = forceLogin });

                //Raise approriate event depend on the outcome
                if ((string)result.status == FACEBOOK_STATUS_OK)
                {
                    IsLoggedIn = true;
                    CurrentLoginUserID = (string)result.currentUserID;
                    OnLoggedIn(EventArgs.Empty);
                    return;
                }

                IsLoggedIn = false;
                OnFailedLogin(EventArgs.Empty);
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at Login : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }

        /// <summary>
        /// Log user out of facebook
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            try
            {
                var logout = Edge.Func(NodeJSScriptConstant.METHOD_LOGOUT);
                var result = await logout(null);

                //Raise approriate event depend on the outcome
                if ((string)result == FACEBOOK_STATUS_OK)
                {
                    OnLoggedOut(new EventArgs());
                    return;
                }
                OnFailedLogout(new EventArgs());
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at Logout : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
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
                var sendMessage = Edge.Func(NodeJSScriptConstant.METHOD_SEND_MESSAGE);
                dynamic result = await sendMessage(new
                {
                    type = "text",
                    message = message,
                    receiver = receiverID,
                });

                if ((string)result.status == FACEBOOK_STATUS_OK)
                {
                    OnMessageSent(new MessageSentEventArgs(
                        (string)result.messageInfo.threadID,
                        (string)result.messageInfo.messageID,
                        (long)result.messageInfo.timestamp));
                }
                else if ((string)result.status == FACEBOOK_STATUS_ERROR)
                {
                    OnMessageSentFailed(new EventArgs());
                }
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
                var sendMessage = Edge.Func(NodeJSScriptConstant.METHOD_SEND_MESSAGE);
                dynamic result = await sendMessage(new
                {
                    type = "file",
                    attachment = filePath,
                    receiver = receiverID,
                });

                if ((string)result.status == FACEBOOK_STATUS_OK)
                {
                    OnMessageSent(new MessageSentEventArgs(
                        (string)result.messageInfo.threadID,
                        (string)result.messageInfo.messageID,
                        (long)result.messageInfo.timestamp));
                }
                else if ((string)result.status == FACEBOOK_STATUS_ERROR)
                {
                    OnMessageSentFailed(new EventArgs());
                }
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
                var sendMessage = Edge.Func(NodeJSScriptConstant.METHOD_SEND_MESSAGE);
                dynamic result = await sendMessage(new
                {
                    type = "file",
                    attachment = filePath,
                    receiver = receiverID,
                });
          
               if ((string)result.status == FACEBOOK_STATUS_OK)
                {
                    OnMessageSent(new MessageSentEventArgs(
                        (string)result.messageInfo.threadID,
                        (string)result.messageInfo.messageID,
                        (long)result.messageInfo.timestamp));
                }
                else if ((string)result.status == FACEBOOK_STATUS_ERROR)
                {
                    OnMessageSentFailed(new EventArgs());
                }
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at SendStickerMessage : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
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
                var sendMessage = Edge.Func(NodeJSScriptConstant.METHOD_SEND_MESSAGE);
                dynamic result = await sendMessage(new
                {
                    type = "url",
                    url = link,
                    receiver = receiverID,
                });

                if ((string)result.status == FACEBOOK_STATUS_OK)
                {
                    OnMessageSent(new MessageSentEventArgs(
                        (string)result.messageInfo.threadID,
                        (string)result.messageInfo.messageID,
                        (long)result.messageInfo.timestamp));
                }
                else if ((string)result.status == FACEBOOK_STATUS_ERROR)
                {
                    OnMessageSentFailed(new EventArgs());
                }
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
            try
            {
                //Various event depend on the type of message received
                var onMessageReceived = (Func<object, Task<object>>)(async (messageJson) =>
                {
                    Message message = Message.Parse((string)messageJson);
                    MessageReceivedEventArgs e = new MessageReceivedEventArgs(message);

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

                //Register event when received new message
                var toggleMessageListening = Edge.Func(NodeJSScriptConstant.METHOD_MESSAGE_LISTENER);
                await toggleMessageListening(new
                {
                    listenMessage = this.listenMessage,
                    onMessageReceived = onMessageReceived
                });
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at ListeningToMessage : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }
        #endregion

        #region "GROUP OPERATION"
        /// <summary>
        /// Create a new group
        /// </summary>
        /// <param name="groupName">Specify the group name</param>
        /// <param name="groupMembers">Specify the group members, at least two</param>
        /// <returns></returns>
        public async Task CreateNewGroup(string groupName, string[] groupMembers)
        {
            try
            {
                var createNewGroup = Edge.Func(NodeJSScriptConstant.METHOD_CREATE_GROUP);
                dynamic result = await createNewGroup(new
                {
                    groupName = groupName,
                    groupMembers = groupMembers,
                });

                if ((bool)result.groupCreated == true)
                {
                    OnGroupCreated(new MessageSentEventArgs(
                        (string)result.messageInfo.threadID,
                        (string)result.messageInfo.messageID,
                        (long)result.messageInfo.timestamp
                        ));
                }
                else
                {
                    OnGroupCreationFailed(new EventArgs());
                }

                if ((bool)result.groupRenamed == true)
                {
                    OnGroupRenamed(new EventArgs());
                }
                else
                {
                    OnGroupRenamedFailed(new EventArgs());
                }
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at CreateNewGroup : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }

        public async Task DeleteGroup(string threadID)
        {
            try
            {
                var deleteGroup = Edge.Func(NodeJSScriptConstant.METHOD_DELETE_THREAD);
                dynamic result = await deleteGroup(new { threadID = threadID });

                if ((string)result == FACEBOOK_STATUS_OK)
                {
                    OnGroupChatDeleted(new EventArgs());
                    return;
                }
                OnGroupChatDeleteFailed(new EventArgs());
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at Delete Group : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }

        public async Task AddUserToGroup(string userID, string threadID)
        {
            try
            {
                var addUserToGroup = Edge.Func(NodeJSScriptConstant.METHOD_GROUP_ADD_USER);
                dynamic result = await addUserToGroup(new { userID = userID, threadID = threadID });
                if ((string)result == FACEBOOK_STATUS_OK)
                {
                    OnUserAddedToGroup(new EventArgs());
                }
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at AddUserToGroup : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }

        public async Task RemoveUserFromGroup(string userID, string threadID)
        {
            try
            {
                var removeUserFromGroup = Edge.Func(NodeJSScriptConstant.METHOD_GROUP_REMOVE_USER);
                dynamic result = await removeUserFromGroup(new { userID = userID, threadID = threadID });
                if ((string)result == FACEBOOK_STATUS_OK)
                {
                    OnUserRemovedFromGroup(new EventArgs());
                }
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at RemoveUserFromGroup : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
        }

        public async Task GetThreadList(int startIndex, int endIndex)
        {
            try
            {
                var getThreadList = Edge.Func(NodeJSScriptConstant.METHOD_GET_THREADLIST);
                dynamic result = await getThreadList(new { startIndex = startIndex, endIndex = endIndex });
                if ((string)result.status == FACEBOOK_STATUS_OK)
                {
                    string threadsJson = (string)result.threadsJson;
                    List<ChatThread> chatThreads = ChatThread.Parse(threadsJson);
                    string message = (string)result.message;
                    if (chatThreads.Count == 0)
                    {
                        message += "No result found.";
                    }
                    else
                    {
                        message += chatThreads.Count + " results found.";
                    }
                    ThreadGetEventArgs e = new ThreadGetEventArgs(chatThreads, message);
                    OnThreadGet(e);
                }
            }
            catch (Exception ex)
            {
                var errMsg = "Exception occured at GetThreadList : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
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
            try
            {
                var searchForUser = Edge.Func(NodeJSScriptConstant.METHOD_SEARCH_USER);
                var result = await searchForUser(new { keyword = keyword });

                List<UserSearchResult> userList = UserSearchResult.Parse((string)result);
                string message = "Search Completed ! ";
                if (userList.Count == 0)
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
            catch (Exception ex)
            {
                var errMsg = "Exception occured at SearchForUser : " + ex.Message + "\nStack Trace : " + ex.StackTrace;
                throw new FacebookChatApiException(errMsg);
            }
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

        protected void OnLoggedOut(EventArgs e)
        {
            if (LoggedOut != null)
                LoggedOut(this, e);
        }

        protected void OnFailedLogout(EventArgs e)
        {
            if (FailedLogout != null)
                FailedLogout(this, e);
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

        protected void OnMessageSentFailed(EventArgs e)
        {
            if (MessageSentFailed != null)
                MessageSentFailed(this, e);
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

        protected void OnGroupCreated(MessageSentEventArgs e)
        {
            if (GroupCreated != null)
                GroupCreated(this, e);
        }

        protected void OnGroupCreationFailed(EventArgs e)
        {
            if (GroupCreationFailed != null)
                GroupCreationFailed(this, e);
        }

        protected void OnGroupRenamed(EventArgs e)
        {
            if (GroupRenamed != null)
                GroupRenamed(this, e);
        }

        protected void OnGroupRenamedFailed(EventArgs e)
        {
            if (GroupRenamedFailed != null)
                GroupRenamedFailed(this, e);
        }

        protected void OnThreadGet(ThreadGetEventArgs e)
        {
            if (ThreadGet != null)
                ThreadGet(this, e);
        }

        protected void OnUserAddedToGroup(EventArgs e)
        {
            if (UserAddedToGroup != null)
                UserAddedToGroup(this, e);
        }

        protected void OnUserRemovedFromGroup(EventArgs e)
        {
            if (UserRemovedFromGroup != null)
                UserRemovedFromGroup(this, e);
        }

        protected void OnGroupChatDeleted(EventArgs e)
        {
            if (GroupChatDeleted != null)
                GroupChatDeleted(this, e);
        }

        protected void OnGroupChatDeleteFailed(EventArgs e)
        {
            if (GroupChatDeleteFailed != null)
                GroupChatDeleteFailed(this, e);
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
            public bool HasResult { get; private set; }
            public string Message { get; private set; }

            internal SearchUserCompletedEventArgs(List<UserSearchResult> userList, string message)
            {
                this.UserList = userList;
                this.Message = message;
                this.HasResult = userList.Count > 0;
            }
        }

        public class ThreadGetEventArgs : EventArgs
        {
            public List<ChatThread> ThreadList { get; private set; }
            public bool HasResult { get; private set; }
            public string Message { get; private set; }

            internal ThreadGetEventArgs(List<ChatThread> threadList, string message)
            {
                this.ThreadList = threadList;
                this.Message = message;
                this.HasResult = threadList.Count > 0;
            }
        }
        #endregion
    }
}
