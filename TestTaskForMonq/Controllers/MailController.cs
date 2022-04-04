using System;
using System.Threading.Tasks;
using TestTaskForMonq.Repository;
using TestTaskForMonq.DtoControllerModels;
using TestTaskForMonq.Services;
using Microsoft.AspNetCore.Mvc;
using TestTaskForMonq.Models;
using System.Linq;
using TestTaskForMonq.Helpers;

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
            try
            {
                var logs = await _repository.GetLogsAsync();

                if (logs.Length == 0)
                {
                    return NotFound("Logs not found in database");
                }

                return Ok(logs);

            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }

        /// <summary>
        /// Post request for sending email and logging information about this sending in database
        /// </summary>
        /// <param name="emailDTO">Information about the body, recipients and subject of the email</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(EmailDTO emailDTO)
        {
            if (!ValidationEmailDTO.IsValid(emailDTO))
            {
                return BadRequest("Incorrect data");
            }

            await _mailService.SendMailAsync(emailDTO);

            return Ok(emailDTO);
        }
    }
}