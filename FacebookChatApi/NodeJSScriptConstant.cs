using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookChatApi
{
    internal static class NodeJSScriptConstant
    {
        internal const string METHOD_LOGIN = @"
GLOBAL.apiInstance = null;
var login = require('facebook-chat-api');

return function(options, cb) {
    login({email : options.email, password: options.password }, function callback(err, api){
        if(err) return cb(null, '0');
        if(options.forceLogin) api.setOptions({ forceLogin : true });

        GLOBAL.apiInstance = api;
        var out = {};
        out.currentUserID = api.getCurrentUserID();
        out.status = '200';
        cb(null, out);
    });
};
";
        internal const string METHOD_LOGOUT = @"
var api = GLOBAL.apiInstance;

return function(options, cb) {
    api.logout(function(err){
        if(err) throw err;

        api = null;
        GLOBAL.apiInstance = null;
        cb(null, '200');
    });
};
";

        internal const string METHOD_SEND_MESSAGE = @"
var api = GLOBAL.apiInstance;
var fs = require('fs');

return function(options, cb) {
    
    console.log(options);
    var result = {};

    if (options === null) {
        result.status = '0';
        result.message = 'Passed in argument is empty';
        return cb(null, result);
    }
    if (!options.receiver) {
        result.status = '0';
        result.message = 'No message receiver is specified';
        return cb(null, result);
    }
    
    if (!options.type) {
        result.status = '0';
        result.message = 'No message type is specified';
        return cb(null, result);
    }
    var receiver = options.receiver;  
    switch (options.type) {
        case 'text':
            var message = { body : options.message };
            api.sendMessage(message, receiver, function(err, messageInfo){
                if (err) {
                    result.status = '0';
                    result.message = 'Text Message not sent';
                    return cb(null, result);
                }
                
                result.status = '0';
                result.message = 'Text Message sent';
                result.messageInfo = messageInfo;
                return cb(null, result);
            });
            break;
        case 'file':
            var message = { attachment : fs.createReadStream(options.attachment), body : options.message };
            api.sendMessage(message, receiver, function(err, messageInfo){
                if (err) {
                    result.status = '0';
                    result.message = 'Text Message not sent';
                    return cb(null, result);
                }
                
                result.status = '0';
                result.message = 'File Message sent';
                result.messageInfo = messageInfo;
                return cb(null, result);
            });
            break;
        case 'sticker': 
            //TODO
            break;
        case 'url':
            var message = { url : options.url };
            api.sendMessage(message, receiver, function(err, messageInfo){
                if (err) {
                    result.status = '0';
                    result.message = 'URL Message not sent';
                    return cb(null, result);
                }
                
                result.status = '0';
                result.message = 'URL Message sent';
                result.messageInfo = messageInfo;
                return cb(null, result);
                return;
            });
            break;
        default:
            break;
    }
};

";
        internal const string METHOD_MESSAGE_LISTENER = @"
var api = GLOBAL.apiInstance;

return function(options, cb){
    api.setOptions({selfListen: true});
    api.listen(function(err, event){
        //Block the callback if listen message is false
        if(err || !options.listenMessage) return;
        if(event.type === 'message'){
            options.onMessageReceived(JSON.stringify(event), function(error, result){
                if(error) throw error; 
            });
        }
    });
};
";

        internal const string METHOD_SEARCH_USER = @"
var api = GLOBAL.apiInstance;

return function(options, cb){
    var keyword = options.keyword.trim();

    api.getUserID(keyword, function(err, obj) {
        if(err) return cb(null, '');
        cb(null, JSON.stringify(obj));
    });
};
";
        internal const string METHOD_CREATE_GROUP = @"
var api = GLOBAL.apiInstance;

return function(options, cb){
    var groupName = options.groupName,
        groupMembers = options.groupMembers,
        result = {};
    
    api.sendMessage('Created Group ' + groupName, groupMembers, function(err, msgInfo){
        if(err){
            result.groupCreated = false;
            result.messageInfo = null;
        }
        else{
            result.groupCreated = true;
            result.messageInfo = msgInfo;
        } 

        api.setTitle(groupName, msgInfo.threadID, function(err, obj){
            if(err) {
                result.groupRenamed = false;
                return cb(null, result);
            }
            result.groupRenamed = true;
            return cb(null, result);
        });        
    });
};
";

        internal const string METHOD_DELETE_THREAD = @"
var api = GLOBAL.apiInstance;

return function(options,cb){
    api.deleteThread(options.threadID, function(err) {
        if(err) return cb(null, '0');
        return cb(null, '200');
    });
};
";

        internal const string METHOD_GET_THREADLIST = @"
var api = GLOBAL.apiInstance;

return function(options, cb){
    var result = {};

    api.getThreadList(options.startIndex, options.endIndex, function(err, arr){
        if(err){
            result.status = '0';
            result.message = 'Unable to get any thread';
            return cb(null, result);
        }
        result.status = '200';
        result.message = 'Got thread list';
        result.threadsJson = JSON.stringify(arr);
        return cb(null, result);
    });
};
";

        internal const string METHOD_GROUP_ADD_USER = @"
var api = GLOBAL.apiInstance;

return function(options, cb){
    api.addUserToGroup(options.userID, options.threadID, function(err){
        if(err) return cb(null, '0');
        return cb(null, '200')
    });
};
";

        internal const string METHOD_GROUP_REMOVE_USER = @"
var api = GLOBAL.apiInstance;

return function(options, cb){
    api.removeUserFromGroup(options.userID, options.threadID, function(err){
        if(err) return cb(null, '0');
        return cb(null, '200');
    });
};
";
    }
}
