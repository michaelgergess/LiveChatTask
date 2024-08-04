using Microsoft.AspNetCore.SignalR;

namespace UserPresentation.Hubs
{

    //public class ChatHub : Hub
    //{
    //    private readonly IChatService _chatService;

    //    public ChatHub(IChatService chatService)
    //    {
    //        _chatService = chatService;
    //    }

    //    public async Task SendPrivateMessageToUsers(List<int> userIds, string message)
    //    {
    //        var adminId = Context.UserIdentifier;
    //        foreach (var userId in userIds)
    //        {
    //            var chatSession = await _chatService.GetOrCreateChatSessionAsync(int.Parse(adminId), userId);
    //            var chatMessage = new ChatMessage
    //            {
    //                SenderId = int.Parse(adminId),
    //                ReceiverId = userId,
    //                Content = message,
    //                Timestamp = DateTime.UtcNow,
    //                Type = "Text"
    //            };

    //            await _chatService.SaveMessageAsync(chatMessage, chatSession.Id);
    //            await Clients.User(userId.ToString()).SendAsync("ReceiveMessage", chatMessage);
    //        }
    //    }

    //    public async Task SendFile(int userId, string fileUrl, string fileType)
    //    {
    //        var adminId = Context.UserIdentifier;
    //        var chatSession = await _chatService.GetOrCreateChatSessionAsync(int.Parse(adminId), userId);
    //        var chatMessage = new ChatMessage
    //        {
    //            SenderId = int.Parse(adminId),
    //            ReceiverId = userId,
    //            FileUrl = fileUrl,
    //            Timestamp = DateTime.UtcNow,
    //            Type = fileType
    //        };

    //        await _chatService.SaveMessageAsync(chatMessage, chatSession.Id);
    //        await Clients.User(userId.ToString()).SendAsync("ReceiveMessage", chatMessage);
    //    }
    //}
}
