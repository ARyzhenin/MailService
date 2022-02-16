namespace TestTaskForMonq
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public string SendFrom { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string SenderName { get; set; }
    }
}
