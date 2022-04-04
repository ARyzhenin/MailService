namespace TestTaskForMonq.DtoControllerModels
{
    /// <summary>
    /// Сlass that accepts data from json in POST method in controller
    /// </summary>
    public class EmailDTO
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string[] Recipients { get; set; }
    }
}