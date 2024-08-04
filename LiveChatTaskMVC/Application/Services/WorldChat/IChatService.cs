using DTO_s.ViewResult;
using DTOs.ChatDTOs;
using Model;

namespace LiveChatTask.Application.Services.WorldChat
{
    public interface IChatService
    {
        IEnumerable<AppUser> GetActiveUsers();
        void SendMessage(string reciverId, string senderName, string content);
        void SendFile(string reciverId, string senderName, string fileName, byte[] fileContent, string contentType);
        void SendVoiceRecording(string reciverId, string senderName, string fileName, byte[] fileContent);
        void MarkMessageAsSeen(int messageId);
        ResultDataList<string> GetUniqueContactNames(string userId);
        Task<ResultDataList<ChatItem>> GetMessages(string senderName, string receiverId);
    }
}