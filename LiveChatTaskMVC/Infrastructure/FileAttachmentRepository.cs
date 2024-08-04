using Context;
using LiveChatTaskMVC.Application.Contract;
using LiveChatTaskMVC.Models;

namespace LiveChatTaskMVC.Infrastructure
{
    public class FileAttachmentRepository : IFileAttachmentRepository
    {
        private readonly ApplicationDbContext _context;

        public FileAttachmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddFileAttachment(FileAttachment fileAttachment)
        {
            _context.FileAttachments.Add(fileAttachment);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
