// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.22.0

using EchoBot1.Bots;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot2.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var responseMsg = "";

            if (turnContext.Activity.Text.Contains("/reset"))
            {
                ChatHistoryManager.DeleteIsolatedStorageFile();
                responseMsg = "我已經把之前的對談都給忘了!";
            }
            else
            {
                var chatHistory = ChatHistoryManager.GetMessagesFromIsolatedStorage("UserA");
                //var replyText = $"Echo: {turnContext.Activity.Text}";
                responseMsg = ChatGPT.getResponseFromGPT(turnContext.Activity.Text, chatHistory);
                //儲存聊天紀錄
                ChatHistoryManager.SaveMessageToIsolatedStorage(
                    DateTime.Now, "UserA", turnContext.Activity.Text, responseMsg);
            }

            await turnContext.SendActivityAsync(MessageFactory.Text(responseMsg, responseMsg), cancellationToken);

        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
