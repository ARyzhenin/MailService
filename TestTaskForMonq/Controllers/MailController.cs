using System;
using System.Threading.Tasks;
using TestTaskForMonq.Repository;
using TestTaskForMonq.DtoControllerModels;
using TestTaskForMonq.Services;
using Microsoft.AspNetCore.Mvc;
using TestTaskForMonq.Models;
using System.Linq;

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
                    return NotFound("Error occurred when returning logs from the database");
                }

                return Ok(logs);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Post request for sending email and logging information about this sending in database
        /// </summary>
        /// <param name="model">Information about the body, recipients and subject of the email</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(MailInfoDto model)
        {
            if (model.Body == null || model.Recipients.Length == 0)
            {
                return BadRequest("Incorrect data");
            }

            try
            {
                await _mailService.SendMailAsync(model);
                return Ok();
            }
            catch (Exception e)
            {
                await _repository.PostLogAsync(new Log
                {
                    Body = model.Body,
                    DateOfCreation = DateTime.Now,
                    Subject = model.Subject,
                    Recipients = model.Recipients.Select(recipient => new Recipient()
                    {
                        EMailAdress = recipient
                    }).ToList(),
                    FailedMessage = e.Message,
                    Result = Status.Failed.ToString()
                });
                return BadRequest(e);
            }
        }
    }
}