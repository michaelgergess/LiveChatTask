using Context;
using Microsoft.EntityFrameworkCore;
using Model;

namespace LiveChatTask.Application.Services.WorldChat
{
    public class ChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<ChatSession> GetOrCreateChatSession(string adminId, string userId,ChatMessage message)
        //{
        //    var session = await _context.ChatSessions
        //        .FirstOrDefaultAsync(cs => cs.AdminId == adminId && cs.UserId == userId);

        //    if (session == null)
        //    {
        //        session = new ChatSession
        //        {
        //            AdminId = adminId,
        //            UserId = userId,
        //            CreatedAt = DateTime.UtcNow,
        //            Messages = new List<ChatMessage>()
        //        };

        //        _context.ChatSessions.Add(session);
        //        await _context.SaveChangesAsync();
        //    }

        //    return session;
        //}

        public async Task SaveMessageAsync(ChatMessage message, int sessionId)
        {
            var session = await _context.ChatSessions.FindAsync(sessionId);
            session.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        internal async Task GetOrCreateChatSessionAsync(int v, int userId)
        {
            throw new NotImplementedException();
        }

        // Other service methods...
    }
}

