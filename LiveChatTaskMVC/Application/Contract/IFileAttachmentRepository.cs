using LiveChatTaskMVC.Models;

namespace LiveChatTaskMVC.Application.Contract
{
    public interface IFileAttachmentRepository
    {
        void AddFileAttachment(FileAttachment fileAttachment);
        void Save();
    }
}
