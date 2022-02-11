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
        public async Task<IActionResult> Get()
        {
            var logs = await _repository.GetLogs();
            return Ok(logs);
        }

        [HttpPost]
        public IActionResult Post(MailInfoDto model)
        {
            _mailService.SendMailAsync(model);
            foreach (var recipient in model.Recipients)
            {
                try
                {
                    var log = new Log
                    {
                        Body = model.Body,
                        Recipient = recipient,
                        Subject = model.Subject,
                        DateOfCreation = DateTime.Now
                    };

                    _repository.PostLog(log);
                    
                    //return Ok(log);  //может это не надо?
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            return Ok();

        }
    }
}