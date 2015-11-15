using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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

        public const string ATTACHMENT_TYPE = "type";
        public const string ATTACHMENT_TYPE_FILE = "file";
        public const string ATTACHMENT_TYPE_STICKER = "sticker";
        public const string ATTACHMENT_TYPE_ANIMATED_IMAGE = "animated_image";
        public const string ATTACHMENT_TYPE_PHOTO = "photo";
        public const string ATTACHMENT_TYPE_SHARE = "share";

        public const string ATTACHMENT_STICKER_URL = "url";
        public const string ATTACHMENT_STICKER_STICKER_ID = "stickerID";
        public const string ATTACHMENT_STICKER_PACK_ID = "packID";
        public const string ATTACHMENT_STICKER_FRAME_COUNT = "frameCount";
        public const string ATTACHMENT_STICKER_FRAMERATE = "frameRate";
        public const string ATTACHMENT_STICKER_FRAMES_PER_ROW = "framesPerRow";
        public const string ATTACHMENT_STICKER_FRAMES_PER_COLUMN = "framesPerCol";
        public const string ATTACHMENT_STICKER_SPRITE_URI = "spriteURI";
        public const string ATTACHMENT_STICKER_SPRITE_URI_2X = "spriteURI2x";
        public const string ATTACHMENT_STICKER_HEIGHT = "height";
        public const string ATTACHMENT_STICKER_WIDTH = "width";
        public const string ATTACHMENT_STICKER_CAPTION = "caption";

        public const string ATTACHMENT_FILE_NAME = "name";
        public const string ATTACHMENT_FILE_URL = "url";
        public const string ATTACHMENT_FILE_ID = "ID";
        public const string ATTACHMENT_FILE_SIZE = "fileSize";
        public const string ATTACHMENT_FILE_IS_MALICIOUS = "isMalicious";
        public const string ATTACHMENT_FILE_MIME_TYPE = "mimeType";

        public const string ATTACHMENT_PHOTO_NAME = "name";
        public const string ATTACHMENT_PHOTO_HIRES_URL = "hiresUrl";
        public const string ATTACHMENT_PHOTO_THUMBNAIL_URL = "thumbnailUrl";
        public const string ATTACHMENT_PHOTO_PREVIEW_URL = "previewUrl";
        public const string ATTACHMENT_PHOTO_PREVIEW_WIDTH = "previewWidth";
        public const string ATTACHMENT_PHOTO_PREVIEW_HEIGHT = "previewHeight";
        public const string ATTACHMENT_PHOTO_FACEBOOK_URL = "facebookUrl";
        public const string ATTACHMENT_PHOTO_ID = "ID";
        public const string ATTACHMENT_PHOTO_FILENAME = "filename";
        public const string ATTACHMENT_PHOTO_MIME_TYPE = "mimeType";
        public const string ATTACHMENT_PHOTO_URL = "url";
        public const string ATTACHMENT_PHOTO_WIDTH = "width";
        public const string ATTACHMENT_PHOTO_HEIGHT = "height";

        public const string ATTACHMENT_ANIMATED_IMAGE_NAME = "name";
        public const string ATTACHMENT_ANIMATED_IMAGE_FACEBOOK_URL = "facebookUrl";
        public const string ATTACHMENT_ANIMATED_IMAGE_PREVIEW_URL = "previewUrl";
        public const string ATTACHMENT_ANIMATED_IMAGE_PREVIEW_WIDTH = "previewWidth";
        public const string ATTACHMENT_ANIMATED_IMAGE_PREVIEW_HEIGHT = "previewHeight";
        public const string ATTACHMENT_ANIMATED_IMAGE_THUMBNAIL_URL = "thumbnailUrl";
        public const string ATTACHMENT_ANIMATED_IMAGE_ID = "ID";
        public const string ATTACHMENT_ANIMATED_IMAGE_FILENAME = "filename";
        public const string ATTACHMENT_ANIMATED_IMAGE_MIME_TYPE = "mimeType";
        public const string ATTACHMENT_ANIMATED_IMAGE_WIDTH = "width";
        public const string ATTACHMENT_ANIMATED_IMAGE_HEIGHT = "height";
        public const string ATTACHMENT_ANIMATED_IMAGE_URL = "url";
        public const string ATTACHMENT_ANIMATED_IMAGE_RAW_GIF_IMAGE = "rawGifImage";
        public const string ATTACHMENT_ANIMATED_IMAGE_RAW_WEBP_IMAGE = "rawWebpImage";
        public const string ATTACHMENT_ANIMATED_IMAGE_GIF_URL = "animatedGifUrl";
        public const string ATTACHMENT_ANIMATED_IMAGE_GIF_PREVIEW_URL = "animatedGifPreviewUrl";
        public const string ATTACHMENT_ANIMATED_IMAGE_WEBP_URL = "animatedWebpUrl";
        public const string ATTACHMENT_ANIMATED_IMAGE_WEBP_PREVIEW_URL = "animatedWebpPreviewUrl";

        public const string ATTACHMENT_SHARE_DESCRIPTION = "description";
        public const string ATTACHMENT_SHARE_ID = "ID";
        public const string ATTACHMENT_SHARE_SUBATTACHMENT = "subattachments";
        public const string ATTACHMENT_SHARE_ANIMATED_IMAGE_SIZE = "animatedImageSize";
        public const string ATTACHMENT_SHARE_WIDTH = "width";
        public const string ATTACHMENT_SHARE_HEIGHT = "height";
        public const string ATTACHMENT_SHARE_IMAGE = "image";
        public const string ATTACHMENT_SHARE_PLAYABLE = "playable";
        public const string ATTACHMENT_SHARE_DURATION = "duration";
        public const string ATTACHMENT_SHARE_SOURCE = "source";
        public const string ATTACHMENT_SHARE_TITLE = "title";
        public const string ATTACHMENT_SHARE_FACEBOOK_URL = "facebookUrl";
        public const string ATTACHMENT_SHARE_URL = "url";
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
            return Attachments.Where(x => x.Where(y => y.Key == ATTACHMENT_TYPE && y.Value == ATTACHMENT_TYPE_FILE)
            .ToArray().Length > 0).ToArray().Length > 0;
        }

        /// <summary>
        /// Whether the message contain any photo attachment
        /// </summary>
        /// <returns></returns>
        public bool ContainPhotoAttachments()
        {
            return Attachments.Where(x => x.Where(y => y.Key == ATTACHMENT_TYPE && y.Value == ATTACHMENT_TYPE_PHOTO)
            .ToArray().Length > 0).ToArray().Length > 0;
        }

        /// <summary>
        /// Whether the message contain any sticker attachment
        /// </summary>
        /// <returns></returns>
        public bool ContainStickerAttachments()
        {
            return Attachments.Where(x => x.Where(y => y.Key == ATTACHMENT_TYPE && y.Value == ATTACHMENT_TYPE_STICKER)
            .ToArray().Length > 0).ToArray().Length > 0;
        }

        /// <summary>
        /// Whether the message contain any animated image attachment
        /// </summary>
        /// <returns></returns>
        public bool ContainAnimatedImageAttachments()
        {
            return Attachments.Where(x => x.Where(y => y.Key == ATTACHMENT_TYPE && y.Value == ATTACHMENT_TYPE_ANIMATED_IMAGE)
            .ToArray().Length > 0).ToArray().Length > 0;
        }

        /// <summary>
        /// Whether the message contain any shared attachment
        /// </summary>
        /// <returns></returns>
        public bool ContainShareAttachments()
        {
            return Attachments.Where(x => x.Where(y => y.Key == ATTACHMENT_TYPE && y.Value == ATTACHMENT_TYPE_SHARE)
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
                foreach (JObject obj in jArrayAttachments)
                {
                    Dictionary<string, string> attachment = new Dictionary<string, string>();
                    switch (obj.GetValue(ATTACHMENT_TYPE).ToString())
                    {
                        case "sticker":
                            attachment.Add(ATTACHMENT_TYPE, obj.GetValue(ATTACHMENT_TYPE).ToString());
                            attachment.Add(ATTACHMENT_STICKER_CAPTION, obj.GetValue(ATTACHMENT_STICKER_CAPTION).ToString());
                            attachment.Add(ATTACHMENT_STICKER_URL, obj.GetValue(ATTACHMENT_STICKER_URL).ToString());
                            attachment.Add(ATTACHMENT_STICKER_STICKER_ID, obj.GetValue(ATTACHMENT_STICKER_STICKER_ID).ToString());
                            attachment.Add(ATTACHMENT_STICKER_PACK_ID, obj.GetValue(ATTACHMENT_STICKER_PACK_ID).ToString());
                            attachment.Add(ATTACHMENT_STICKER_FRAME_COUNT, obj.GetValue(ATTACHMENT_STICKER_FRAME_COUNT).ToString());
                            attachment.Add(ATTACHMENT_STICKER_FRAMERATE, obj.GetValue(ATTACHMENT_STICKER_FRAMERATE).ToString());
                            attachment.Add(ATTACHMENT_STICKER_FRAMES_PER_ROW, obj.GetValue(ATTACHMENT_STICKER_FRAMES_PER_ROW).ToString());
                            attachment.Add(ATTACHMENT_STICKER_FRAMES_PER_COLUMN, obj.GetValue(ATTACHMENT_STICKER_FRAMES_PER_COLUMN).ToString());
                            attachment.Add(ATTACHMENT_STICKER_SPRITE_URI, obj.GetValue(ATTACHMENT_STICKER_SPRITE_URI).ToString());
                            attachment.Add(ATTACHMENT_STICKER_SPRITE_URI_2X, obj.GetValue(ATTACHMENT_STICKER_SPRITE_URI_2X).ToString());
                            attachment.Add(ATTACHMENT_STICKER_HEIGHT, obj.GetValue(ATTACHMENT_STICKER_HEIGHT).ToString());
                            attachment.Add(ATTACHMENT_STICKER_WIDTH, obj.GetValue(ATTACHMENT_STICKER_WIDTH).ToString());
                            break;
                        case "file":
                            attachment.Add(ATTACHMENT_TYPE, obj.GetValue(ATTACHMENT_TYPE).ToString());
                            attachment.Add(ATTACHMENT_FILE_ID, obj.GetValue(ATTACHMENT_FILE_ID).ToString());
                            attachment.Add(ATTACHMENT_FILE_URL, obj.GetValue(ATTACHMENT_FILE_URL).ToString());
                            attachment.Add(ATTACHMENT_FILE_SIZE, obj.GetValue(ATTACHMENT_FILE_SIZE).ToString());
                            attachment.Add(ATTACHMENT_FILE_NAME, obj.GetValue(ATTACHMENT_FILE_NAME).ToString());
                            attachment.Add(ATTACHMENT_FILE_MIME_TYPE, obj.GetValue(ATTACHMENT_FILE_MIME_TYPE).ToString());
                            attachment.Add(ATTACHMENT_FILE_IS_MALICIOUS, obj.GetValue(ATTACHMENT_FILE_IS_MALICIOUS).ToString());
                            break;
                        case "photo":
                            attachment.Add(ATTACHMENT_TYPE, obj.GetValue(ATTACHMENT_TYPE).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_FACEBOOK_URL, obj.GetValue(ATTACHMENT_PHOTO_FACEBOOK_URL).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_FILENAME, obj.GetValue(ATTACHMENT_PHOTO_FILENAME).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_HEIGHT, obj.GetValue(ATTACHMENT_PHOTO_HEIGHT).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_HIRES_URL, obj.GetValue(ATTACHMENT_PHOTO_HIRES_URL).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_ID, obj.GetValue(ATTACHMENT_PHOTO_ID).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_MIME_TYPE, obj.GetValue(ATTACHMENT_PHOTO_MIME_TYPE).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_NAME, obj.GetValue(ATTACHMENT_PHOTO_NAME).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_PREVIEW_HEIGHT, obj.GetValue(ATTACHMENT_PHOTO_PREVIEW_HEIGHT).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_PREVIEW_URL, obj.GetValue(ATTACHMENT_PHOTO_PREVIEW_URL).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_PREVIEW_WIDTH, obj.GetValue(ATTACHMENT_PHOTO_PREVIEW_WIDTH).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_THUMBNAIL_URL, obj.GetValue(ATTACHMENT_PHOTO_THUMBNAIL_URL).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_URL, obj.GetValue(ATTACHMENT_PHOTO_URL).ToString());
                            attachment.Add(ATTACHMENT_PHOTO_WIDTH, obj.GetValue(ATTACHMENT_PHOTO_WIDTH).ToString());
                            break;
                        case "animated_image":
                            attachment.Add(ATTACHMENT_TYPE, obj.GetValue(ATTACHMENT_TYPE).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_FACEBOOK_URL, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_FACEBOOK_URL).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_FILENAME, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_FILENAME).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_GIF_PREVIEW_URL, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_GIF_PREVIEW_URL).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_GIF_URL, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_GIF_URL).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_HEIGHT, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_HEIGHT).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_ID, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_ID).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_MIME_TYPE, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_MIME_TYPE).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_NAME, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_NAME).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_PREVIEW_HEIGHT, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_PREVIEW_HEIGHT).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_PREVIEW_URL, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_PREVIEW_URL).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_PREVIEW_WIDTH, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_PREVIEW_WIDTH).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_RAW_GIF_IMAGE, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_RAW_GIF_IMAGE).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_RAW_WEBP_IMAGE, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_RAW_WEBP_IMAGE).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_THUMBNAIL_URL, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_THUMBNAIL_URL).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_URL, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_URL).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_WEBP_PREVIEW_URL, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_WEBP_PREVIEW_URL).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_WEBP_URL, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_WEBP_URL).ToString());
                            attachment.Add(ATTACHMENT_ANIMATED_IMAGE_WIDTH, obj.GetValue(ATTACHMENT_ANIMATED_IMAGE_WIDTH).ToString());
                            break;
                            //TODO
                        case "share":
                            attachment.Add(ATTACHMENT_TYPE, obj.GetValue(ATTACHMENT_TYPE).ToString());
                            break;
                    }
                    attachments.Add(attachment);
                }

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
