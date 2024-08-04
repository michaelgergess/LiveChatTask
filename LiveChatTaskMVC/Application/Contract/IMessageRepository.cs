using DTOs.ChatDTOs;
using Model;

namespace LiveChatTaskMVC.Application.Contract
{
    public interface IMessageRepository
    {
        Message GetMessageById(int id);
        void AddMessage(Message message);
        void UpdateMessage(Message message);
        void Save();
        List<string> GetUniqueContactNames(string userId);
        Task<IEnumerable<ChatItem>> GetMessagesBy(string receiverId, string senderId);
    }
}
