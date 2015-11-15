using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookChatApi
{
    public class FacebookChatApiException : Exception
    {
        public FacebookChatApiException(string message) : base(message)
        {
        }
    }
}
