using System;
using System.Threading.Tasks;
using TestTaskForMonq.Repository;
using TestTaskForMonq.DtoControllerModels;
using TestTaskForMonq.Services;
using Microsoft.AspNetCore.Mvc;

namespace TestTaskForMonq.Controllers
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
        public async Task<IActionResult> GetAsync()
        {
            var logs = await _repository.GetLogsAsync();
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