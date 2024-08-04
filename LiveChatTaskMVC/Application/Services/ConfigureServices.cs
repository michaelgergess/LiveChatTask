using LiveChatTask.Application.Contract;
using LiveChatTask.Application.Services.User;
using LiveChatTask.Application.Services.WorldChat;
using LiveChatTaskMVC.Application.Contract;
using LiveChatTaskMVC.Infrastructure;

namespace LiveChatTaskMVC.Application.Services
{
    public static class ConfigureServices
    {
        public static void Configure(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IFileAttachmentRepository, FileAttachmentRepository>();
            services.AddScoped<IuserService, UserService>();
            services.AddScoped<IChatService, ChatService>();
        }
    }
}
