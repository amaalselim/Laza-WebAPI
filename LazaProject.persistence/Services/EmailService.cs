using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LazaProject.Application.IServices;

public class EmailService : IEmailService
{
	private readonly string _smtpServer;
	private readonly int _smtpPort;
	private readonly string _smtpUser;
	private readonly string _smtpPass;

	public EmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
	{
		_smtpServer = smtpServer;
		_smtpPort = smtpPort;
		_smtpUser = smtpUser;
		_smtpPass = smtpPass;
	}

	public async Task SendEmailAsync(string email, string subject, string verificationCode)
	{
		try
		{
			using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
			{
				smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
				smtpClient.EnableSsl = true;

				string imageUrl = "https://img.icons8.com/color/96/000000/checkmark.png";

				string message = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #ffffff; 
                        margin: 0;
                        padding: 20px;
                    }}
                    .container {{
                        background-color: #ffffff; 
                        border-radius: 8px;
                        padding: 30px; 
                        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                    }}
                    .header {{
                        text-align: center;
                        padding-bottom: 20px;
                    }}
                    .verification-code {{
                        font-size: 36px; 
                        font-weight: bold; 
                        color: #000000; 
                        margin: 20px 0;
                        text-align: center; 
                    }}
                    .footer {{
                        text-align: center;
                        margin-top: 20px;
                        font-size: 14px; 
                        color: #888;
                    }}
                    .message {{
                        color: black; 
                        background-color: #ffffff; 
                        padding: 20px; 
                        border-radius: 5px; 
                        font-size: 20px; 
                        text-align: center; 
                    }}
                    code {{
                        display: inline-block; 
                        background-color: #f0f0f0; 
                        padding: 10px;
                        border-radius: 5px;
                        margin: 20px 0;
                        font-size: 24px; 
                        color: black; 
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2 style='color: #000000;'>Email Verification</h2>
                    </div>
                    <img src='{imageUrl}' alt='Verification Icon' style='display: block; margin: 0 auto; max-width: 100px;' />
                    <div class='message'>
                        <p>Hello!</p>
                        <code style='text-align: center;'>{verificationCode}</code>
                        <p>If you did not request this, please ignore this message.</p>
                    </div>
                    <div class='footer'>
                        <p>&copy; {DateTime.Now.Year} Laza. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>";

				var mailMessage = new MailMessage
				{
					From = new MailAddress(_smtpUser),
					Subject = subject,
					Body = message,
					IsBodyHtml = true,
				};

				mailMessage.To.Add(email);

				await smtpClient.SendMailAsync(mailMessage);
			}

			Console.WriteLine("Email sent successfully.");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to send email: {ex.Message}");
		}
	}
}
