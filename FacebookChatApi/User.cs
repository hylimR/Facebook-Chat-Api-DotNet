using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FacebookChatApi
{
    public class UserSearchResult
    {
        const string CONST_USER_ID = "userID";
        const string CONST_PHOTO_URL = "photoUrl";
        const string CONST_INDEX_RANK = "indexRank";
        const string CONST_NAME = "name";
        const string CONST_IS_VERIFIED = "isVerified";
        const string CONST_PROFILE_URL = "profileUrl";
        const string CONST_CATEGORY = "category";
        const string CONST_SCORE = "score";

        public string UserID { get; private set; }
        public string PhotoURL { get; private set; }
        public string IndexRank { get; private set; }
        public string Name { get; private set; }
        public bool IsVerified { get; private set; }
        public string ProfileURL { get; private set; }
        public string Category { get; private set; }
        public string Score { get; private set; }

        public UserSearchResult(string userID, string photoURL, string indexRank, string name,
            bool isVerified, string profileURL, string category, string score)
        {
            this.UserID = userID;
            this.PhotoURL = photoURL;
            this.IndexRank = indexRank;
            this.Name = name;
            this.IsVerified = IsVerified;
            this.ProfileURL = profileURL;
            this.Category = category;
            this.Score = score;
        }

        public static List<UserSearchResult> Parse(string json)
        {
            List<UserSearchResult> resultList = new List<UserSearchResult>();
            try
            {
                JArray jArray = JArray.Parse(json);
                foreach (JObject jObj in jArray)
                {
                    resultList.Add(Parse(jObj));
                }
            }
            catch (Exception ex)
            {
                throw new FacebookChatApiException("UserSearchResult Parse : " + ex.Message);
            }
            return resultList;
        }

        public static UserSearchResult Parse(JObject jObj)
        {
            try
            {
                UserSearchResult userSearchResult = new UserSearchResult(
                    jObj.GetValue(CONST_USER_ID).ToString(),
                    jObj.GetValue(CONST_PHOTO_URL).ToString(),
                    jObj.GetValue(CONST_INDEX_RANK).ToString(),
                    jObj.GetValue(CONST_NAME).ToString(),
                    bool.Parse(jObj.GetValue(CONST_IS_VERIFIED).ToString()),
                    jObj.GetValue(CONST_PROFILE_URL).ToString(),
                    jObj.GetValue(CONST_CATEGORY).ToString(),
                    jObj.GetValue(CONST_SCORE).ToString()
                    );

                return userSearchResult;
            }
            catch (Exception ex)
            {
                throw new FacebookChatApiException("UserSearchResult Parse : " + ex.Message);
            }
        }
    }
}
