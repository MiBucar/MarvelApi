using MarvelApi_Mvc.Models.DTOs.UserDTOs;

namespace MarvelApi_Mvc.Services.IServices
{
    public interface IMailReceiverService
    {
        Task SendEmailAsync(MailDTO mailDto);
    }
}
