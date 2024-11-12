using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LazaProject.Application.IServices;
using LazaProject.Core.Models;

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

	public async Task SendEmailAsync(string email, string userName, string subject, string verificationCode)
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
                            padding: 20px; 
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
                            padding: 5px;
                            border-radius: 5px;
                            margin: 20px 0;
                            font-size: 18px; 
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
                            <p>Dear {userName},</p>
                            <p>Thank you for registering with us. Please verify your email address using the code below:</p>
                            <code style='text-align: center;'>{verificationCode}</code>
                            <p>If you did not request this, please ignore this message.</p>
                            <p>Best Regards,<br>LazaTeamSupport</p>
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
	public async Task SendOrderConfirmationEmailAsync(string email, string userName, Cart cart, AddressUser billingAddress, Card paymentCard)
	{
		try
		{
			using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
			{
				smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
				smtpClient.EnableSsl = true;

				string message = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        color: #333;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: auto;
                        background-color: #ffffff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                    }}
                    .header {{
                        text-align: center;
                        font-size: 24px;
                        font-weight: bold;
                        color: #333;
                        padding-bottom: 20px;
                    }}
                    .order-summary, .billing-shipping {{
                        border-top: 1px solid #e0e0e0;
                        padding-top: 20px;
                        margin-top: 20px;
                    }}
                    .product-item {{
                        display: flex;
                        margin-bottom: 10px;
                    }}
                    .product-details {{
                        flex: 1;
                    }}
                    .product-title {{
                        font-weight: bold;
                        color: #333;
                    }}
                    .price-details {{
                        text-align: right;
                    }}
                    .footer {{
                        text-align: center;
                        margin-top: 20px;
                        font-size: 14px;
                        color: #888;
                    }}
                    .total-price {{
                        font-size: 20px;
                        font-weight: bold;
                        color: #4caf50;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>ORDER CONFIRMATION</div>
                    <p>Dear {userName},</p>
                    <p>Thank you for your order! Here are the details of your purchase:</p>

                    <div class='order-summary'>
                        <h3>Order Summary</h3>
                        {string.Join("", cart.Items.Select(item => $@"
                        <div class='product-item'>
                            <div class='product-details'>
                                <div class='product-title'>{item.Product?.Name ?? "Product Name"}</div>
                                <div>Product ID: {item.ProductId}</div>
                                <div>Quantity: {item.Quantity}</div>
                            </div>
                            <div class='price-details'>
                                <div>${item.Price:0.00}</div>
                            </div>
                        </div>"))}
                    </div>

                    <div class='order-total'>
                        <p>Total price: <span class='total-price'>${cart.TotalPrice:0.00}</span></p>
                    </div>

                    <div class='billing-shipping'>
                        <h3>Billing and Shipping</h3>
                        <p><strong>Billing:</strong> {billingAddress.UserName}, {billingAddress.Address}, {billingAddress.City}, {billingAddress.Country}, {billingAddress.PhoneNumber}</p>
                        <p><strong>Payment method:</strong> {paymentCard.CardType} ending in {paymentCard.CardNumber.Substring(paymentCard.CardNumber.Length - 4)}</p>
                    </div>

                    <div class='footer'>
                        &copy; {DateTime.Now.Year} Laza. All rights reserved.
                    </div>
                </div>
            </body>
            </html>";

				var mailMessage = new MailMessage
				{
					From = new MailAddress(_smtpUser),
					Subject = "Order Confirmation",
					Body = message,
					IsBodyHtml = true,
				};

				mailMessage.To.Add(email);

				await smtpClient.SendMailAsync(mailMessage);
			}

			Console.WriteLine("Order confirmation email sent successfully.");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to send email: {ex.Message}");
		}
	}

}
