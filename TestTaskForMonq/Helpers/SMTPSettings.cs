namespace TestTaskForMonq
{
    /// <summary>
    /// Class for setting SmtpClient
    /// </summary>
    public class SMTPSettings
    {
        public string Host { get; set; }
        public string SendFrom { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string SenderName { get; set; }
    }
}
