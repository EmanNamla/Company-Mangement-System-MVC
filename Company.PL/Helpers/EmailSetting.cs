using Company.DAL.Models;
using System;
using System.Net;
using System.Net.Mail;

namespace Company.PL.Helpers
{
	public static class EmailSetting
	{
		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient("smtp.gmail.com", 587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential("emanrnamla222@gmail.com", "famz mchd mqin cvna");
			Client.Send("emanrnamla222@gmail.com", email.To, email.Subject, email.Body);

		}
	}
}
