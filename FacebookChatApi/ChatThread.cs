using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FacebookChatApi
{
    public class ChatThread
    {
        #region "CONSTANT"
        private const string CHAT_THREAD_THREAD_ID = "threadID";
        private const string CHAT_THREAD_PARTICIPANTS = "participants";
        private const string CHAT_THREAD_FORMER_PARTICIPANTS = "formerParticipants";
        private const string CHAT_THREAD_NAME = "name";
        private const string CHAT_THREAD_SNIPPET = "snippet";
        private const string CHAT_THREAD_SNIPPET_HAS_ATTACHMENT = "snippetHasAttachment";
        private const string CHAT_THREAD_SNIPPET_ATTACHMENT = "snippetAttachments";
        private const string CHAT_THREAD_SNIPPET_SENDER = "snippetSender";
        private const string CHAT_THREAD_UNREAD_COUNT = "unreadCount";
        private const string CHAT_THREAD_MESSAGE_COUNT = "messageCount";
        private const string CHAT_THREAD_IMAGE_SRC = "imageSrc";
        private const string CHAT_THREAD_TIMESTAMP = "timestamp";
        private const string CHAT_THREAD_SERVER_TIMESTAMP = "serverTimestamp";
        private const string CHAT_THREAD_IS_CANONICAL_USER = "isCanonicalUser";
        private const string CHAT_THREAD_IS_CANONICAL = "isCanonical";
        private const string CHAT_THREAD_IS_SUBCRIBED = "isSubscribed";
        private const string CHAT_THREAD_FOLDER = "folder";
        private const string CHAT_THREAD_IS_ARCHIVED = "isArchived";
        private const string CHAT_THREAD_RECIPIENTS_LOADABLE = "recipientsLoadable";
        private const string CHAT_THREAD_HAS_EMAIL_PARTICIPANT = "hasEmailParticipant";
        private const string CHAT_THREAD_READONLY = "readOnly";
        private const string CHAT_THREAD_CAN_REPLY = "canReply";
        private const string CHAT_THREAD_COMPOSER_ENABLED = "composerEnabled";
        private const string CHAT_THREAD_LAST_MESSAGE_ID = "lastMessageID";
        #endregion

        #region "PROPERTIES"
        public string ThreadID { get; private set; }
        public List<string> Participants { get; private set; }
        public List<string> FormerParticipants { get; private set; }
        public string Name { get; private set; }
        public string Snippet { get; private set; }
        public bool SnippetHasAttachment { get; private set; }
        //public List<Dictionary<string, string>> SnippetAttachments { get; private set; }
        public string SnippetSender { get; private set; }
        public int UnreadCount { get; private set; }
        public int MessageCount { get; private set; }
        public string ImageSrc { get; private set; }
        public DateTime Timestamp { get; private set; }
        public DateTime ServerTimestamp { get; private set; }
        public bool IsCanonicalUser { get; private set; }
        public bool IsCanonical { get; private set; }
        public bool IsSubscribed { get; private set; }
        public string Folder { get; private set; }
        public bool IsArchived { get; private set; }
        public bool RecipientsLoadable { get; private set; }
        public bool HasEmailParticipant { get; private set; }
        public bool ReadOnly { get; private set; }
        public bool CanReply { get; private set; }
        public bool ComposerEnabled { get; private set; }
        public string LastMessageID { get; private set; }
        #endregion

        public ChatThread(string threadID, List<string> participants, List<string> formerParticipants, string name, string snippet, bool snippetHasAttachment, /*List<Dictionary<string, string>> snippetAttachments,*/
            string snippetSender, int unreadCount, int messageCount, string imageSrc, long timestamp, long serverTimestamp, bool isCanonicalUser, bool isCanonical, bool isSubscribed, string folder,
            bool isArchived, bool recipientsLoadable, bool hasEmailParticipant, bool readOnly, bool canReply, bool composerEnabled, string lastMessageID)
        {
            ThreadID = threadID;
            Participants = participants;
            FormerParticipants = formerParticipants;
            Name = name;
            Snippet = snippet;
            SnippetHasAttachment = snippetHasAttachment;
            //SnippetAttachments = snippetAttachments;
            SnippetSender = snippetSender;
            UnreadCount = unreadCount;
            MessageCount = messageCount;
            ImageSrc = imageSrc;
            Timestamp = new DateTime(timestamp);
            ServerTimestamp = new DateTime(serverTimestamp);
            IsCanonicalUser = isCanonicalUser;
            IsCanonical = isCanonical;
            IsSubscribed = isSubscribed;
            Folder = folder;
            IsArchived = isArchived;
            RecipientsLoadable = recipientsLoadable;
            HasEmailParticipant = hasEmailParticipant;
            ReadOnly = readOnly;
            CanReply = canReply;
            ComposerEnabled = composerEnabled;
            LastMessageID = lastMessageID;
        }

        /// <summary>
        /// Oarse a JSON consist of many thread
        /// </summary>
        /// <param name="threadJsons"></param>
        /// <returns></returns>
        public static List<ChatThread> Parse(string threadsJson)
        {
            List<ChatThread> chatThreads = new List<ChatThread>();
            try
            {
                if (string.IsNullOrEmpty(threadsJson)) throw new FacebookChatApiException("Passed JSON is empty string !");
                JArray jArray = JArray.Parse(threadsJson);
                foreach (JObject jObj in jArray)
                {
                    chatThreads.Add(ParseSingle(jObj.ToString()));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new FacebookChatApiException("ChatThread.ParseSingle :" + ex.Message);
            }
            return chatThreads;
        }

        /// <summary>
        /// Parse a jSON consist of single Thread
        /// </summary>
        /// <param name="threadJson"></param>
        /// <returns></returns>
        public static ChatThread ParseSingle(string threadJson)
        {
            ChatThread thread = null;
            if (string.IsNullOrEmpty(threadJson))
            {
                return thread;
            }

            try
            {
                JObject jObj = JObject.Parse(threadJson);
                JArray jArrayParticipants = (JArray)jObj.GetValue(CHAT_THREAD_PARTICIPANTS);
                List<string> participants = new List<string>();
                foreach(string participant in jArrayParticipants)
                {
                    participants.Add(participant);
                }

                JArray jArrayFormerParticipants = (JArray)jObj.GetValue(CHAT_THREAD_FORMER_PARTICIPANTS);
                List<string> formerParticipants = new List<string>();
                foreach(string formerParticipant in jArrayFormerParticipants)
                {
                    participants.Add(formerParticipant);
                }
                
                thread = new ChatThread(
                    jObj.GetValue(CHAT_THREAD_THREAD_ID).ToString(),
                    participants,
                    formerParticipants,
                    jObj.GetValue(CHAT_THREAD_NAME).ToString(),
                    jObj.GetValue(CHAT_THREAD_SNIPPET).ToString(),
                    bool.Parse(jObj.GetValue(CHAT_THREAD_SNIPPET_HAS_ATTACHMENT).ToString()),
                    jObj.GetValue(CHAT_THREAD_SNIPPET_SENDER).ToString(),
                    int.Parse(jObj.GetValue(CHAT_THREAD_UNREAD_COUNT).ToString()),
                    int.Parse(jObj.GetValue(CHAT_THREAD_MESSAGE_COUNT).ToString()),
                    jObj.GetValue(CHAT_THREAD_IMAGE_SRC).ToString(),
                    long.Parse(jObj.GetValue(CHAT_THREAD_TIMESTAMP).ToString()),
                    long.Parse(jObj.GetValue(CHAT_THREAD_SERVER_TIMESTAMP).ToString()),
                    bool.Parse(jObj.GetValue(CHAT_THREAD_IS_CANONICAL_USER).ToString()),
                    bool.Parse(jObj.GetValue(CHAT_THREAD_IS_CANONICAL).ToString()),
                    bool.Parse(jObj.GetValue(CHAT_THREAD_IS_SUBCRIBED).ToString()),
                    jObj.GetValue(CHAT_THREAD_FOLDER).ToString(),
                    bool.Parse(jObj.GetValue(CHAT_THREAD_IS_ARCHIVED).ToString()),
                    bool.Parse(jObj.GetValue(CHAT_THREAD_IS_CANONICAL).ToString()),
                    bool.Parse(jObj.GetValue(CHAT_THREAD_HAS_EMAIL_PARTICIPANT).ToString()),
                    bool.Parse(jObj.GetValue(CHAT_THREAD_READONLY).ToString()),
                    bool.Parse(jObj.GetValue(CHAT_THREAD_CAN_REPLY).ToString()),
                    bool.Parse(jObj.GetValue(CHAT_THREAD_COMPOSER_ENABLED).ToString()),
                    jObj.GetValue(CHAT_THREAD_LAST_MESSAGE_ID).ToString()
                    );
            }
            catch (Exception ex)
            {
                throw new FacebookChatApiException("ChatThread.ParseSingle :" + ex.Message);
            }
            return thread;
        }
    }
}
