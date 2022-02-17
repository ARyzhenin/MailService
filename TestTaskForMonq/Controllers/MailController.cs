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

        /// <summary>
        /// Get request for return all logs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // todo add try-catch
            var logs = await _repository.GetLogsAsync();

            if (logs.Length == 0)
            {
                return NotFound();
            }

            return Ok(logs);
        }

        /// <summary>
        /// Post request for sending email and logging information about this sending in database
        /// </summary>
        /// <param name="model">Information about the body, recipients and subject of the email</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(MailInfoDto model)
        {
            //todo check model to be correct
            if (model.Body == null || model.Subject == null || model.Recipients.Length == 0)
            {
                return BadRequest("Твоя модель хуйня");
            }

            try
            {
                await _mailService.SendMailAsync(model);
                return Ok();
            }
            catch (Exception)
            {
                // todo: Возникла ошибка отправки сообщения, пожалуйста попробуйте позже
                return BadRequest("Проблемы с отправкой сообщения");
            }
        }
    }
}