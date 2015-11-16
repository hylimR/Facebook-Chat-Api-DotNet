using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FacebookChatApi
{
    public class Attachment
    {
        public const string TYPE = "type";
        public const string TYPE_FILE = "file";
        public const string TYPE_STICKER = "sticker";
        public const string TYPE_ANIMATED_IMAGE = "animated_image";
        public const string TYPE_PHOTO = "photo";
        public const string TYPE_SHARE = "share";

        public const string STICKER_URL = "url";
        public const string STICKER_STICKER_ID = "stickerID";
        public const string STICKER_PACK_ID = "packID";
        public const string STICKER_FRAME_COUNT = "frameCount";
        public const string STICKER_FRAMERATE = "frameRate";
        public const string STICKER_FRAMES_PER_ROW = "framesPerRow";
        public const string STICKER_FRAMES_PER_COLUMN = "framesPerCol";
        public const string STICKER_SPRITE_URI = "spriteURI";
        public const string STICKER_SPRITE_URI_2X = "spriteURI2x";
        public const string STICKER_HEIGHT = "height";
        public const string STICKER_WIDTH = "width";
        public const string STICKER_CAPTION = "caption";

        public const string FILE_NAME = "name";
        public const string FILE_URL = "url";
        public const string FILE_ID = "ID";
        public const string FILE_SIZE = "fileSize";
        public const string FILE_IS_MALICIOUS = "isMalicious";
        public const string FILE_MIME_TYPE = "mimeType";

        public const string PHOTO_NAME = "name";
        public const string PHOTO_HIRES_URL = "hiresUrl";
        public const string PHOTO_THUMBNAIL_URL = "thumbnailUrl";
        public const string PHOTO_PREVIEW_URL = "previewUrl";
        public const string PHOTO_PREVIEW_WIDTH = "previewWidth";
        public const string PHOTO_PREVIEW_HEIGHT = "previewHeight";
        public const string PHOTO_FACEBOOK_URL = "facebookUrl";
        public const string PHOTO_ID = "ID";
        public const string PHOTO_FILENAME = "filename";
        public const string PHOTO_MIME_TYPE = "mimeType";
        public const string PHOTO_URL = "url";
        public const string PHOTO_WIDTH = "width";
        public const string PHOTO_HEIGHT = "height";

        public const string ANIMATED_IMAGE_NAME = "name";
        public const string ANIMATED_IMAGE_FACEBOOK_URL = "facebookUrl";
        public const string ANIMATED_IMAGE_PREVIEW_URL = "previewUrl";
        public const string ANIMATED_IMAGE_PREVIEW_WIDTH = "previewWidth";
        public const string ANIMATED_IMAGE_PREVIEW_HEIGHT = "previewHeight";
        public const string ANIMATED_IMAGE_THUMBNAIL_URL = "thumbnailUrl";
        public const string ANIMATED_IMAGE_ID = "ID";
        public const string ANIMATED_IMAGE_FILENAME = "filename";
        public const string ANIMATED_IMAGE_MIME_TYPE = "mimeType";
        public const string ANIMATED_IMAGE_WIDTH = "width";
        public const string ANIMATED_IMAGE_HEIGHT = "height";
        public const string ANIMATED_IMAGE_URL = "url";
        public const string ANIMATED_IMAGE_RAW_GIF_IMAGE = "rawGifImage";
        public const string ANIMATED_IMAGE_RAW_WEBP_IMAGE = "rawWebpImage";
        public const string ANIMATED_IMAGE_GIF_URL = "animatedGifUrl";
        public const string ANIMATED_IMAGE_GIF_PREVIEW_URL = "animatedGifPreviewUrl";
        public const string ANIMATED_IMAGE_WEBP_URL = "animatedWebpUrl";
        public const string ANIMATED_IMAGE_WEBP_PREVIEW_URL = "animatedWebpPreviewUrl";

        public const string SHARE_DESCRIPTION = "description";
        public const string SHARE_ID = "ID";
        public const string SHARE_SUBATTACHMENT = "subattachments";
        public const string SHARE_ANIMATED_IMAGE_SIZE = "animatedImageSize";
        public const string SHARE_WIDTH = "width";
        public const string SHARE_HEIGHT = "height";
        public const string SHARE_IMAGE = "image";
        public const string SHARE_PLAYABLE = "playable";
        public const string SHARE_DURATION = "duration";
        public const string SHARE_SOURCE = "source";
        public const string SHARE_TITLE = "title";
        public const string SHARE_FACEBOOK_URL = "facebookUrl";
        public const string SHARE_URL = "url";

        public static List<Dictionary<string, string>> Parse(string attachmentsJson)
        {
            List<Dictionary<string, string>> attachments = new List<Dictionary<string, string>>();
            if (string.IsNullOrEmpty(attachmentsJson) == true)
            {
                return attachments;
            }

            JArray jArrayAttachments = JArray.Parse(attachmentsJson);
            foreach (JObject obj in jArrayAttachments)
            {
                attachments.Add(ParseSingle(obj.ToString()));
            }

            return attachments;
        }

        public static Dictionary<string, string> ParseSingle(string attachmentJson)
        {
            Dictionary<string, string> attachment = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(attachmentJson))
            {
                return attachment;
            }

            JObject obj = JObject.Parse(attachmentJson);
            switch (obj.GetValue(Attachment.TYPE).ToString())
            {
                case TYPE_STICKER:
                    attachment.Add(Attachment.TYPE, obj.GetValue(Attachment.TYPE).ToString());
                    attachment.Add(Attachment.STICKER_CAPTION, obj.GetValue(Attachment.STICKER_CAPTION).ToString());
                    attachment.Add(Attachment.STICKER_URL, obj.GetValue(Attachment.STICKER_URL).ToString());
                    attachment.Add(Attachment.STICKER_STICKER_ID, obj.GetValue(Attachment.STICKER_STICKER_ID).ToString());
                    attachment.Add(Attachment.STICKER_PACK_ID, obj.GetValue(Attachment.STICKER_PACK_ID).ToString());
                    attachment.Add(Attachment.STICKER_FRAME_COUNT, obj.GetValue(Attachment.STICKER_FRAME_COUNT).ToString());
                    attachment.Add(Attachment.STICKER_FRAMERATE, obj.GetValue(Attachment.STICKER_FRAMERATE).ToString());
                    attachment.Add(Attachment.STICKER_FRAMES_PER_ROW, obj.GetValue(Attachment.STICKER_FRAMES_PER_ROW).ToString());
                    attachment.Add(Attachment.STICKER_FRAMES_PER_COLUMN, obj.GetValue(Attachment.STICKER_FRAMES_PER_COLUMN).ToString());
                    attachment.Add(Attachment.STICKER_SPRITE_URI, obj.GetValue(Attachment.STICKER_SPRITE_URI).ToString());
                    attachment.Add(Attachment.STICKER_SPRITE_URI_2X, obj.GetValue(Attachment.STICKER_SPRITE_URI_2X).ToString());
                    attachment.Add(Attachment.STICKER_HEIGHT, obj.GetValue(Attachment.STICKER_HEIGHT).ToString());
                    attachment.Add(Attachment.STICKER_WIDTH, obj.GetValue(Attachment.STICKER_WIDTH).ToString());
                    break;
                case TYPE_FILE:
                    attachment.Add(Attachment.TYPE, obj.GetValue(Attachment.TYPE).ToString());
                    attachment.Add(Attachment.FILE_ID, obj.GetValue(Attachment.FILE_ID).ToString());
                    attachment.Add(Attachment.FILE_URL, obj.GetValue(Attachment.FILE_URL).ToString());
                    attachment.Add(Attachment.FILE_SIZE, obj.GetValue(Attachment.FILE_SIZE).ToString());
                    attachment.Add(Attachment.FILE_NAME, obj.GetValue(Attachment.FILE_NAME).ToString());
                    attachment.Add(Attachment.FILE_MIME_TYPE, obj.GetValue(Attachment.FILE_MIME_TYPE).ToString());
                    attachment.Add(Attachment.FILE_IS_MALICIOUS, obj.GetValue(Attachment.FILE_IS_MALICIOUS).ToString());
                    break;
                case TYPE_PHOTO:
                    attachment.Add(Attachment.TYPE, obj.GetValue(Attachment.TYPE).ToString());
                    attachment.Add(Attachment.PHOTO_FACEBOOK_URL, obj.GetValue(Attachment.PHOTO_FACEBOOK_URL).ToString());
                    attachment.Add(Attachment.PHOTO_FILENAME, obj.GetValue(Attachment.PHOTO_FILENAME).ToString());
                    attachment.Add(Attachment.PHOTO_HEIGHT, obj.GetValue(Attachment.PHOTO_HEIGHT).ToString());
                    attachment.Add(Attachment.PHOTO_HIRES_URL, obj.GetValue(Attachment.PHOTO_HIRES_URL).ToString());
                    attachment.Add(Attachment.PHOTO_ID, obj.GetValue(Attachment.PHOTO_ID).ToString());
                    attachment.Add(Attachment.PHOTO_MIME_TYPE, obj.GetValue(Attachment.PHOTO_MIME_TYPE).ToString());
                    attachment.Add(Attachment.PHOTO_NAME, obj.GetValue(Attachment.PHOTO_NAME).ToString());
                    attachment.Add(Attachment.PHOTO_PREVIEW_HEIGHT, obj.GetValue(Attachment.PHOTO_PREVIEW_HEIGHT).ToString());
                    attachment.Add(Attachment.PHOTO_PREVIEW_URL, obj.GetValue(Attachment.PHOTO_PREVIEW_URL).ToString());
                    attachment.Add(Attachment.PHOTO_PREVIEW_WIDTH, obj.GetValue(Attachment.PHOTO_PREVIEW_WIDTH).ToString());
                    attachment.Add(Attachment.PHOTO_THUMBNAIL_URL, obj.GetValue(Attachment.PHOTO_THUMBNAIL_URL).ToString());
                    attachment.Add(Attachment.PHOTO_URL, obj.GetValue(Attachment.PHOTO_URL).ToString());
                    attachment.Add(Attachment.PHOTO_WIDTH, obj.GetValue(Attachment.PHOTO_WIDTH).ToString());
                    break;
                case TYPE_ANIMATED_IMAGE:
                    attachment.Add(Attachment.TYPE, obj.GetValue(Attachment.TYPE).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_FACEBOOK_URL, obj.GetValue(Attachment.ANIMATED_IMAGE_FACEBOOK_URL).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_FILENAME, obj.GetValue(Attachment.ANIMATED_IMAGE_FILENAME).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_GIF_PREVIEW_URL, obj.GetValue(Attachment.ANIMATED_IMAGE_GIF_PREVIEW_URL).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_GIF_URL, obj.GetValue(Attachment.ANIMATED_IMAGE_GIF_URL).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_HEIGHT, obj.GetValue(Attachment.ANIMATED_IMAGE_HEIGHT).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_ID, obj.GetValue(Attachment.ANIMATED_IMAGE_ID).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_MIME_TYPE, obj.GetValue(Attachment.ANIMATED_IMAGE_MIME_TYPE).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_NAME, obj.GetValue(Attachment.ANIMATED_IMAGE_NAME).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_PREVIEW_HEIGHT, obj.GetValue(Attachment.ANIMATED_IMAGE_PREVIEW_HEIGHT).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_PREVIEW_URL, obj.GetValue(Attachment.ANIMATED_IMAGE_PREVIEW_URL).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_PREVIEW_WIDTH, obj.GetValue(Attachment.ANIMATED_IMAGE_PREVIEW_WIDTH).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_RAW_GIF_IMAGE, obj.GetValue(Attachment.ANIMATED_IMAGE_RAW_GIF_IMAGE).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_RAW_WEBP_IMAGE, obj.GetValue(Attachment.ANIMATED_IMAGE_RAW_WEBP_IMAGE).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_THUMBNAIL_URL, obj.GetValue(Attachment.ANIMATED_IMAGE_THUMBNAIL_URL).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_URL, obj.GetValue(Attachment.ANIMATED_IMAGE_URL).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_WEBP_PREVIEW_URL, obj.GetValue(Attachment.ANIMATED_IMAGE_WEBP_PREVIEW_URL).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_WEBP_URL, obj.GetValue(Attachment.ANIMATED_IMAGE_WEBP_URL).ToString());
                    attachment.Add(Attachment.ANIMATED_IMAGE_WIDTH, obj.GetValue(Attachment.ANIMATED_IMAGE_WIDTH).ToString());
                    break;
                
                case TYPE_SHARE:
                    attachment.Add(Attachment.TYPE, obj.GetValue(Attachment.TYPE).ToString());
                    attachment.Add(SHARE_DESCRIPTION, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    attachment.Add(SHARE_ID, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    //These two are array and object respectively , ignore them by now
                    //attachment.Add(SHARE_SUBATTACHMENT, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    //attachment.Add(SHARE_ANIMATED_IMAGE_SIZE, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    attachment.Add(SHARE_WIDTH, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    attachment.Add(SHARE_HEIGHT, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    attachment.Add(SHARE_IMAGE, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    attachment.Add(SHARE_PLAYABLE, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    attachment.Add(SHARE_DURATION, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    attachment.Add(SHARE_SOURCE, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    attachment.Add(SHARE_TITLE, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    attachment.Add(SHARE_FACEBOOK_URL, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    attachment.Add(SHARE_URL, obj.GetValue(SHARE_DESCRIPTION).ToString());
                    break;
            }

            return attachment;
        }
    }
}
