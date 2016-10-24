using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Helpers
{
    /// <summary>
    /// Encapsulates helpful functions for working with e-mails.
    /// </summary>
    public static class MailHelper
    {
        /// <summary>
        /// Sends an e-mail.
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        public static void SendEmail(string toAddress, string subject, string messageBody)
        {
                MailMessage message = new MailMessage();

                // Allow multiple "To" addresses to be separated by a semi-colon
                if (!String.IsNullOrEmpty(toAddress))
                {
                    foreach (string addr in toAddress.Split(';'))
                    {
                        message.To.Add(new MailAddress(addr));
                    }
                }

                // Set the subject and message body text
                message.Subject = subject;
                message.Body = messageBody;

                // Send the e-mail message
                Send(message); 
        }

        /// <summary>
        /// Sends an e-mail.
        /// </summary>
        /// <param name="message"></param>
        private static void Send(MailMessage message)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Send(message);
                }
                catch (SmtpFailedRecipientException ex)
                {
                    SmtpStatusCode statusCode = ex.StatusCode;

                    if (statusCode == SmtpStatusCode.MailboxBusy ||
                        statusCode == SmtpStatusCode.MailboxUnavailable ||
                        statusCode == SmtpStatusCode.TransactionFailed)
                    {
                        // wait 5 seconds, try a second time
                        // todo: make it async
                        Thread.Sleep(5000);
                        client.Send(message);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
