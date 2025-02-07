﻿using DTO_s.ViewResult;
using DTOs.ChatDTOs;
using LiveChatTask.Application.Contract;
using LiveChatTask.Application.Services.WorldChat;
using LiveChatTaskMVC.Application.Contract;
using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using NuGet.Protocol.Plugins;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly IUserRepository _userRepository;

    private static readonly ConcurrentDictionary<string, string> _connectedUsers = new ConcurrentDictionary<string, string>();

    public ChatHub(IChatService chatService, IUserRepository userRepository)
    {
        _chatService = chatService;
        _userRepository = userRepository;
    }

    public async Task SendMessage(string receiverId, string senderName, string message)
    {
        _chatService.SendMessage(receiverId, senderName, message);
        string senderId =  _userRepository.GetUserByName(senderName).Id;

        await Clients.User(receiverId).SendAsync("ReceiveMessage", senderName, message);
        await Clients.User(senderId).SendAsync("ReceiveMessage", senderName, message);

    }

    public async Task SendFile(string receiverId, string senderName, string fileName, string base64FileContent, string contentType)
    {
        string senderId = _userRepository.GetUserByName(senderName).Id;

        byte[] fileContent = Convert.FromBase64String(base64FileContent);
        _chatService.SendFile(receiverId, senderName, fileName, fileContent, contentType);
        await Clients.User(receiverId).SendAsync("ReceiveFileMessage", senderName, fileName, fileContent, contentType);
        await Clients.User(senderId).SendAsync("ReceiveFileMessage", senderName, fileName, fileContent, contentType);
    }

    public async Task SendVoiceRecording(string receiverId, string senderName, string fileName, byte[] fileContent)
    {
        string senderId = _userRepository.GetUserByName(senderName).Id;

        _chatService.SendVoiceRecording(receiverId, senderName, fileName, fileContent);
        await Clients.User(receiverId).SendAsync("ReceiveVoiceRecording", senderName, fileName, fileContent);
        await Clients.User(senderId).SendAsync("ReceiveVoiceRecording", senderName, fileName, fileContent);
    }


    public override async Task OnConnectedAsync()
    {
        var userName = Context.User.Identity.Name;
        _connectedUsers[Context.ConnectionId] = userName;

        // Notify all clients about the new connection
        await NotifyUsersUpdate();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        _connectedUsers.TryRemove(Context.ConnectionId, out _);

        // Notify all clients about the disconnection
        await NotifyUsersUpdate();
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task<ResultDataList<ChatItem>> GetMessages(string senderName, string receiverName)
    {
        var chatHistory = await _chatService.GetMessages(senderName, receiverName);
        return new ResultDataList<ChatItem> { Entities = chatHistory.Entities };
    }

    private async Task NotifyUsersUpdate()
    {
        var activeUsers = _connectedUsers.Values.Distinct().ToList();
        await Clients.All.SendAsync("UpdateUserList", activeUsers);
    }
    
    public async Task<ResultDataList<string>> GetUniqueContactNames(string userId)
    {
        var result = _chatService.GetUniqueContactNames(userId);
        return await Task.FromResult(result);
    }
}
