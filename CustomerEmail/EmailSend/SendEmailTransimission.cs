using System.Net.Mail;
using EmailSend.Helper;
using EmailSend.Models;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace EmailSend
{
    public class SendEmailTransimission
    {
        private readonly IConfiguration _config;

        public SendEmailTransimission()
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Make sure this is the root directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _config = config;

        }

        public async Task SendEmailAsync(EmailValues email)
        {
            var attempts = 0;
            var success = false;
            var port = Int32.Parse(_config["EmailPort"]);

            while (!success && attempts < HelperValues.MaxEmailAttempts)
            {
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(_config["ClientName"], _config["ClientEmailAddress"]));
                    message.To.Add(new MailboxAddress(email.Name, email.EmailAddress));
                    message.Subject = email.Subject;
                    message.Body = new TextPart("plain") { Text = email.Message };

                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        await client.ConnectAsync(_config["EmailHost"], port);
                        await client.AuthenticateAsync(_config["ClientEmailAddress"], _config["EmailPassword"]);
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }

                    success = true;
                    WriteEmailLog(success, email);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    foreach (SmtpFailedRecipientException t in ex.InnerExceptions)
                    {
                        Console.WriteLine($"Failed to deliver message to {t.FailedRecipient}: {t.Message}");
                    }

                    WriteEmailLog(false, email);
                    attempts++;
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($"SMTP error occurred: {ex.Message}");
                    WriteEmailLog(success, email);
                    attempts++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SMTP error occurred: {ex.Message}");
                    WriteEmailLog(success, email);
                    attempts++;
                }
            }
        }   

        private async void WriteEmailLog(bool success, EmailValues email)
        {
            var status = success ? "Sent" : "Failed";

            if (!Directory.Exists(_config["LoggingPath"]))
            {
                Directory.CreateDirectory(_config["LoggingPath"]);
            }

            string line = email.EmailAddress + ", " + email.Subject + ", " + email.Message + "," + DateTime.Now.ToString() + ", " + status + "\n";
            await File.AppendAllTextAsync(Path.Combine(_config["LoggingPath"], _config["EmailLogFile"]), line);

        }
    }
}
