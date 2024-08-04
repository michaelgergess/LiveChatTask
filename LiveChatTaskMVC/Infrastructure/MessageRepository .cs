using Context;
using DTOs.ChatDTOs;
using LiveChatTaskMVC.Application.Contract;
using Microsoft.EntityFrameworkCore;
using Model;

namespace LiveChatTaskMVC.Infrastructure
{
    public class MessageRepository :IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatItem>> GetMessagesBy(string receiverId, string senderId)
        {
            // Fetch messages from the database
            var messages = await _context.Messages
                .Where(m => m.ReciverId == receiverId && m.SenderId == senderId)
                .Select(m => new ChatItem
                {
                    IsMessage = true,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReciverId,
                    Content = m.Content,
                    SentAt = m.SentAt
                }).ToListAsync();

            // Fetch file attachments from the database
            var files = await _context.FileAttachments
                .Where(f => f.ReciverId == receiverId && f.SenderId == senderId)
                .Select(f => new ChatItem
                {
                    IsMessage = false,
                    SenderId = f.SenderId,
                    ReceiverId = f.ReciverId,
                    FileName = f.FileName,
                    ContentType = f.ContentType,
                    SentAt = f.SentAt
                }).ToListAsync();

            // Concatenate messages and files, then order by sent date
            var chatItems = messages.Concat(files)
                                    .OrderBy(ci => ci.SentAt)
                                    .ToList();

            return chatItems;
        }

        public Message GetMessageById(int id)
        {
            return _context.Messages.Find(id);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void UpdateMessage(Message message)
        {
            _context.Entry(message).State = EntityState.Modified;
        }
        public List<string> GetUniqueContactNames(string userId)
        {
            var messageContacts = _context.Messages
                .Where(m => m.ReciverId == userId || m.SenderId == userId)
                .Select(m => new { m.SenderId, m.ReciverId });

            var fileContacts = _context.FileAttachments
                .Where(f => f.ReciverId == userId || f.SenderId == userId)
                .Select(f => new { f.SenderId, f.ReciverId });

            var contactIds = messageContacts.Concat(fileContacts)
                .Select(c => c.SenderId == userId ? c.ReciverId : c.SenderId)
                .Distinct();

            var contactNames = _context.Users
                .Where(u => contactIds.Contains(u.Id))
                .Select(u => u.UserName)
                .ToList();

            return contactNames;
        }




        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
