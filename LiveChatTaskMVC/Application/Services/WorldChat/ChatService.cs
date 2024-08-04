using DTO_s.ViewResult;
using DTOs.ChatDTOs;
using LiveChatTask.Application.Contract;
using LiveChatTaskMVC.Application.Contract;
using LiveChatTaskMVC.Models;
using Microsoft.EntityFrameworkCore;
using Model;

namespace LiveChatTask.Application.Services.WorldChat
{
    public class ChatService : IChatService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IFileAttachmentRepository _fileAttachmentRepository;

        public ChatService(IUserRepository userRepository, IMessageRepository messageRepository, IFileAttachmentRepository fileAttachmentRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _fileAttachmentRepository = fileAttachmentRepository;
        }

        public  IEnumerable<AppUser> GetActiveUsers()
        {
            return  _userRepository.GetActiveUsers();
        }
        public ResultDataList<string> GetUniqueContactNames(string senderName)
        {
            AppUser sender = _userRepository.GetUserByName(senderName);

            var contactNames = _messageRepository.GetUniqueContactNames(sender.Id);

            if (contactNames.Any())
            {
                return new ResultDataList<string> { Entities = contactNames, Count = contactNames.Count };
            }

            return new ResultDataList<string> { Entities = new List<string>(), Count = 0 };
        }

        public  void SendMessage(string reciverId, string senderName, string content)
        {
           AppUser sender =  _userRepository.GetUserByName(senderName);
            var message = new Message
            {
               
                ReciverId = reciverId,
                SenderId = sender.Id,
                Content = content,
                IsSeen = false
            };

            _messageRepository.AddMessage(message);
            _messageRepository.Save();
        }

        public void SendFile(string reciverId, string senderName, string fileName, byte[] fileContent, string contentType)
        {
            AppUser sender = _userRepository.GetUserByName(senderName);


            var fileAttachment = new FileAttachment
            {
                FileName = fileName,
                FileContent = fileContent,
                ContentType = contentType,
                ReciverId = reciverId,
                SenderId = sender.Id,
                
            };

            _fileAttachmentRepository.AddFileAttachment(fileAttachment);
            _fileAttachmentRepository.Save();
        }

        public void SendVoiceRecording(string reciverId, string senderId, string fileName, byte[] fileContent)
        {
            SendFile(reciverId, senderId, fileName, fileContent, "audio/wav");
        }
        public void MarkMessageAsSeen(int messageId)
        {
            var message = _messageRepository.GetMessageById(messageId);
            if (message != null)
            {
                message.IsSeen = true;
                _messageRepository.UpdateMessage(message);
                _messageRepository.Save();
            }
        }

        public async Task<ResultDataList<ChatItem>> GetMessages(string senderName, string receiverName)
        {
            // Fetch the users from the repository
            AppUser sender = _userRepository.GetUserByName(senderName);
            AppUser receiver = _userRepository.GetUserByName(receiverName);

            var chatItems = await _messageRepository.GetMessagesBy(receiver.Id, sender.Id);

            if (chatItems.Any())
            {
                var chatItemsList = chatItems.ToList();
                return new ResultDataList<ChatItem> { Entities = chatItemsList, Count = chatItemsList.Count };
            }

            return new ResultDataList<ChatItem> { Entities = new List<ChatItem>(), Count = 0 };
        }


    }
}

 