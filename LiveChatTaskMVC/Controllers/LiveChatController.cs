using LiveChatTask.Application.Services.WorldChat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveChatTaskMVC.Controllers
{
    [Authorize]

    public class LiveChatController : Controller
    {
        private readonly IChatService _chatService;

        public LiveChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendFile(string reciverId, string senderId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileContent = memoryStream.ToArray();
            var fileName = file.FileName;
            var contentType = file.ContentType;

            _chatService.SendFile(reciverId, senderId, fileName, fileContent, contentType);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SendVoice(string reciverId, string senderId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileContent = memoryStream.ToArray();
            var fileName = file.FileName;

            _chatService.SendVoiceRecording(reciverId, senderId, fileName, fileContent);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult MarkMessageSeen(int messageId)
        {
            _chatService.MarkMessageAsSeen(messageId);
            return Ok();
        }
    }
}

