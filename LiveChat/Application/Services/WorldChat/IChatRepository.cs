using DTO_s.ViewResult;
using DTOs.ChatDTOs;
using LiveChatTask.Application.Contract;
using Model;

namespace LiveChatTask.Application.Services.WorldChat
{
    public interface IChatRepository : IRepository<ChatSession, int>
    {
        Task<ChatSession> GetByAdminAndUserAsync(int adminId, int userId);
        Task<List<ChatMessage>> GetMessagesBySessionIdAsync(int sessionId);
        Task SaveMessageAsync(ChatMessage message);
    }
}