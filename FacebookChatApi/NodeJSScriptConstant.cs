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
        console.log(err);
        api = null;
        GLOBAL.apiInstance = null;
    });
};
";

        internal const string METHOD_SEND_MESSAGE = @"
var api = GLOBAL.apiInstance;
var fs = require('fs');

return function(options, cb) {
    
    if (options === null) return cb(null, 'Object cannot be null');
    if (!options.receiver) return cb(null, 'No message receiver is specified');
    
    var receiver = options.receiver;    

    switch (options.type) {
        case 'text':
            var message = { body : options.message };
            api.sendMessage(message, receiver, function(err, messageInfo){
                if (err) {
                    options.onMessageSent(err, function(error, result) {
                        if(error) throw error;
                    });
                    return;
                }
                
                options.onMessageSent(JSON.stringify(messageInfo), function(error, result) {
                    if(error) throw error;
                });
                return;
            });
            break;
        case 'file':
            var message = { attachment : fs.createReadStream(options.attachment), body : '2222' };
            api.sendMessage(message, receiver, function(err, messageInfo){
                if (err) {
                    options.onMessageSent(err, function(error, result) {
                        if(error) throw error;
                    });
                    return;
                }
                
                options.onMessageSent(JSON.stringify(messageInfo), function(error, result) {
                    if(error) throw error;
                });
                return;
            });
            break;
        case 'sticker': 
            //TODO
            break;
        case 'url':
            var message = { url : options.url };
            api.sendMessage(message, receiver, function(err, messageInfo){
                if (err) {
                    options.onMessageSent(err, function(error, result) {
                        if(error) throw error;
                    });
                    return;
                }
                
                options.onMessageSent(JSON.stringify(messageInfo), function(error, result) {
                    if(error) throw error;
                });
                return;
            });
            break;
        default:
            break;
    }

    cb(null, 'Operation Finish');
};

";
        internal const string METHOD_MESSAGE_LISTENER = @"
var api = GLOBAL.apiInstance;

return function(options, cb){
    if (!options.listenMessage){
        (api.listen())();
        return cb();
    }

    api.setOptions({selfListen: true});
    api.listen(function(err, event){
        if(err) return;
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
        if(err) return cb(null, JSON.stringify([]));
        cb(null, JSON.stringify(obj));
    });
};
";
    }
}
