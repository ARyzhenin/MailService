using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Mails.Models;
using Mails.Repository;
using Mails.DtoControllerModels;
using Mails.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mails.Controllers
{
    [ApiController]
    [Route("api/mails")]
    public class MailController : ControllerBase
    {
        private readonly ILogRepository _repository;
        private readonly IMailService _mailService;

        public MailController(ILogRepository repository, IMailService mailService)
        {
            _repository = repository;
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var logs = _repository.GetLogs();
            return Ok(logs);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(MailInfoDto model)
        {
            try
            {
                await _mailService.SendMailAsync(model);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Проблемы с отправкой сообщения");
            }
           
            

        }
    }
}