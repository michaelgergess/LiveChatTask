namespace Infrastructure
{
    //public class ChatRepository : Repository<ChatSession, int>
    //{
    //    private readonly ApplicationDbContext _context;

    //    public ChatRepository(ApplicationDbContext context) : base(context)
    //    {
    //        _context = context;
    //    }



    //    public async Task<ChatSession> GetByAdminAndUserAsync(int adminId, int userId)
    //    {
    //        return await _context.ChatSessions
    //            .Include(cs => cs.Messages)
    //            .FirstOrDefaultAsync(cs => cs.AdminId == adminId && cs.UserId == userId);
    //    }

    //    public async Task<List<ChatMessage>> GetMessagesBySessionIdAsync(int sessionId)
    //    {
    //        return await _context.Messages
    //            .Where(m => m.ChatSessionId == sessionId)
    //            .ToListAsync();
    //    }

    //    public async Task SaveMessageAsync(ChatMessage message)
    //    {
    //        _context.Messages.Add(message);
    //        await _context.SaveChangesAsync();
    //    }


    //}
}
