using DTO_s.ViewResult;
using DTOs.UserDTOs;

namespace LiveChatTask.Application.Contract
{
    public interface IUserRepository
    {

        Task<ResultView<UserRegisterDTO>> UserUpdate(UserRegisterDTO user);
        Task<ResultView<UserRegisterDTO>> softDelete(string userID);

    }
}
