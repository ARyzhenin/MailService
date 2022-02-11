using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mails.Models
{
    public enum Status
    {
        OK,
        Failed
    }

    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Recipient { get; set; }

        public DateTime DateOfCreation { get; set; }

        public Status? Result { get; set; }

        public string FailedMessage { get; set; }
    }
}