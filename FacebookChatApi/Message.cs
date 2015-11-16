using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FacebookChatApi
{
    public class Message
    {
        #region "Constant"
        private const string JSON_KEY_TIMESTAMP = "timestamp";
        private const string JSON_KEY_SENDER_NAME = "senderName";
        private const string JSON_KEY_SENDER_ID = "senderID";
        private const string JSON_KEY_PARTICIPANT_NAMES = "participantNames";
        private const string JSON_KEY_PARTICIPANT_IDS = "participantIDs";
        private const string JSON_KEY_BODY = "body";
        private const string JSON_KEY_THREAD_ID = "threadID";
        private const string JSON_KEY_THREAD_NAME = "threadName";
        private const string JSON_KEY_LOCATION = "location";
        private const string JSON_KEY_MESSAGE_ID = "messageID";
        private const string JSON_KEY_ATTACHMENT = "attachments";

        private const string MESSAGE_TYPE_TEXTONLY = "text_only";
        private const string MESSAGE_TYPE_TEXTLESS = "no_text";


        #endregion

        #region "Class Variables"
        public DateTime Timestamp { get; private set; }
        public string SenderName { get; private set; }
        public string SenderID { get; private set; }
        public List<string> ParticipantNames { get; private set; }
        public List<string> ParticipantIDs { get; private set; }
        public string Body { get; private set; }
        public string ThreadID { get; private set; }
        public string ThreadName { get; private set; }
        public string Location { get; private set; }
        public string MessageID { get; private set; }
        public List<Dictionary<string, string>> Attachments { get; private set; }
        #endregion

        public Message(string senderName, string senderID, List<string> participantNames, List<string> participantIDs,
            string body, string threadID, string threadName, string location, string messageID,
            List<Dictionary<string, string>> attachments, long timestamp)
        {
            this.SenderName = senderName;
            this.SenderID = senderID;
            this.ParticipantNames = ParticipantNames;
            this.ParticipantIDs = participantIDs;
            this.Body = body;
            this.ThreadID = threadID;
            this.ThreadName = threadName;
            this.Location = location;
            this.MessageID = messageID;
            this.Attachments = attachments;
            this.Timestamp = new DateTime(timestamp);
        }

        /// <summary>
        /// Whether the message contain any attachment
        /// </summary>
        /// <returns></returns>
        public bool ContainAttachments()
        {
            return Attachments.Count > 0;
        }

        /// <summary>
        /// Whether the message contain any file attachment
        /// </summary>
        /// <returns></returns>
        public bool ContainFileAttachments()
        {
            return Attachments.Where(x => x.Where(y => y.Key == Attachment.TYPE && y.Value == Attachment.TYPE_FILE)
            .ToArray().Length > 0).ToArray().Length > 0;
        }

        /// <summary>
        /// Whether the message contain any photo attachment
        /// </summary>
        /// <returns></returns>
        public bool ContainPhotoAttachments()
        {
            return Attachments.Where(x => x.Where(y => y.Key == Attachment.TYPE && y.Value == Attachment.TYPE_PHOTO)
            .ToArray().Length > 0).ToArray().Length > 0;
        }

        /// <summary>
        /// Whether the message contain any sticker attachment
        /// </summary>
        /// <returns></returns>
        public bool ContainStickerAttachments()
        {
            return Attachments.Where(x => x.Where(y => y.Key == Attachment.TYPE && y.Value == Attachment.TYPE_STICKER)
            .ToArray().Length > 0).ToArray().Length > 0;
        }

        /// <summary>
        /// Whether the message contain any animated image attachment
        /// </summary>
        /// <returns></returns>
        public bool ContainAnimatedImageAttachments()
        {
            return Attachments.Where(x => x.Where(y => y.Key == Attachment.TYPE && y.Value == Attachment.TYPE_ANIMATED_IMAGE)
            .ToArray().Length > 0).ToArray().Length > 0;
        }

        /// <summary>
        /// Whether the message contain any shared attachment
        /// </summary>
        /// <returns></returns>
        public bool ContainShareAttachments()
        {
            return Attachments.Where(x => x.Where(y => y.Key == Attachment.TYPE && y.Value == Attachment.TYPE_SHARE)
            .ToArray().Length > 0).ToArray().Length > 0;
        }

        /// <summary>
        /// Parse a JSON formatted string into a message object
        /// </summary>
        /// <param name="msgJson"></param>
        /// <returns></returns>
        public static Message Parse(string msgJson)
        {
            Message message = null;
            try
            {
                List<string> participantIDs = new List<string>();
                List<string> participantNames = new List<string>();
                List<Dictionary<string, string>> attachments = new List<Dictionary<string, string>>();

                JObject jObj = JObject.Parse(msgJson);
                string senderName = jObj.GetValue(JSON_KEY_SENDER_NAME).ToString(),
                 senderID = jObj.GetValue(JSON_KEY_SENDER_ID).ToString(),
                 body = jObj.GetValue(JSON_KEY_BODY).ToString(),
                 threadID = jObj.GetValue(JSON_KEY_THREAD_ID).ToString(),
                 threadName = jObj.GetValue(JSON_KEY_THREAD_NAME).ToString(),
                 location = jObj.GetValue(JSON_KEY_LOCATION).ToString(),
                 messageID = jObj.GetValue(JSON_KEY_MESSAGE_ID).ToString(),
                 timestamp = jObj.GetValue(JSON_KEY_TIMESTAMP).ToString();

                JArray jArrayParticipantID = (JArray)jObj.GetValue(JSON_KEY_PARTICIPANT_IDS);
                foreach(string id in jArrayParticipantID)
                {
                    participantIDs.Add(id);
                }

                JArray jArrayParticipantName = (JArray)jObj.GetValue(JSON_KEY_PARTICIPANT_NAMES);
                foreach (string name in jArrayParticipantID)
                {
                    participantNames.Add(name);
                }

                JArray jArrayAttachments = (JArray)jObj.GetValue(JSON_KEY_ATTACHMENT);
                attachments = Attachment.Parse(jArrayAttachments.ToString());

                message = new Message(senderName, senderID,participantNames,participantIDs,body,
                    threadID,threadName, location, messageID, attachments, long.Parse(timestamp));
            }
            catch (Exception ex)
            {
                throw new FacebookChatApiException(ex.Message);
            }

            return message;
        }
    }
}
