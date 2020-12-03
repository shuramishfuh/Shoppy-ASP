namespace Shop.Models
{
    public class Mysmtp
    {
        public string email = "fifiComme@gmail.com";
        public string Password = "fifiComm974361@$";
        public string user = "Fi_12 Cool";
        public int Port = 587;
        public string Type = "user";
        public string SmtpServer = "smtp.gmail.com";
       // public string OfficialEmail = "Classicdeals.biz@gmail.com";
        public string OfficialEmail = "Classicdeals.biz@gmail.com";
        public string OfficialName = "Classicdeals";
    }
    //structure
    //var mailMessage = new MimeMessage();
    //mailMessage.From.Add(new MailboxAddress("from name", "from email"));
    //mailMessage.To.Add(new MailboxAddress("to name", "to email"));
    //mailMessage.Subject = "subject";
    //mailMessage.Body = new TextPart("plain")
    //{
    //Text = "Hello"
    //};

    //using (var smtpClient = new SmtpClient())
    //{
    //smtpClient.Connect("smtp.gmail.com", 587, true);
    //smtpClient.Authenticate("user", "password");
    //smtpClient.Send(mailMessage);
    //smtpClient.Disconnect(true);
    //}
    public class CustumerMessage {
        public string Name { get; set; }
        public string Wnumber { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class Buyproduct
    {
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string SarId { get; set; }
    }
}
