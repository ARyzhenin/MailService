using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestTaskForMonq.Models
{
    public enum Status
    {
        OK,
        Failed
    }

    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public List<Recipient> Recipients { get; set; }

        public DateTime DateOfCreation { get; set; }

        public string Result { get; set; }

        public string FailedMessage { get; set; }
    }

    public class Recipient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        public string EMailAdress { get; set; }
        
        [JsonIgnore]
        public int LogId { get; set; }

        [JsonIgnore]
        public Log Log { get; set; }
    }
}