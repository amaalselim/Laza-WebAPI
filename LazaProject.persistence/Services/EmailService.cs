using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LazaProject.Application.IServices;
using LazaProject.Core.DTO_S;
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
	public async Task<bool> SendOrderConfirmationEmailAsync(string email, string userName, CartDTO cart, AddressUser billingAddress, Card paymentCard)
	{
		try
		{
			using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
			{
				smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
				smtpClient.EnableSsl = true;

				string baseUrl = "https://laza.runasp.net/"; // تأكد من صحة الرابط الخاص بموقعك

				// بناء الرسالة
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
            .greeting {{
                text-align: center;
                font-size: 18px;
                font-weight: bold;
                color: #333;
                margin-bottom: 20px;
            }}
            .order-summary {{
                background-color: #ffffff;
                border-radius: 8px;
                padding: 20px;
                margin-top: 20px;
                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            }}
            .product-list {{
                margin-top: 20px;
            }}
            .product-item {{
                display: flex;
                align-items: flex-start;
                margin-bottom: 15px;
                padding-bottom: 15px;
                border-bottom: 1px solid #e0e0e0;
            }}
            .product-image {{
                width: 80px;
                height: 80px;
                margin-right: 15px;
                border-radius: 8px;
                object-fit: cover;
            }}
            .product-details {{
                flex: 1;
            }}
            .product-title {{
                font-weight: bold;
                color: #333;
                margin-bottom: 5px;
            }}
            .quantity {{
                color: #555;
                margin-bottom: 5px;
            }}
            .price-details {{
                font-weight: bold;
                color: #666;
                margin-top: 5px;
            }}
            .total-price {{
                font-size: 20px;
                font-weight: bold;
                color: #333;
                text-align: center;
                margin-top: 20px;
            }}
            .billing-shipping {{
                display: flex;
                justify-content: space-between;
                margin-top: 20px;
                padding-top: 10px;
                border-top: 1px solid #e0e0e0;
            }}
            .address-section {{
                width: 48%;
                font-size: 14px;
            }}
            .address-section h4 {{
                margin-bottom: 10px;
                font-size: 16px;
                color: #666;
            }}
            .address-line {{
                margin: 5px 0;
            }}
            .footer {{
                text-align: center;
                margin-top: 20px;
                font-size: 14px;
                color: #888;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>ORDER CONFIRMATION</div>
            
            <!-- Greeting -->
            <div class='greeting'>Dear {userName}</div>
            
            <p>Thank you for your order! Here are the details of your purchase:</p>

            <!-- Order Summary -->
            <div class='order-summary'>
                <h3>Order Summary</h3>
                <div class='product-list'>
                    {string.Join("", cart.Items.Select(item => {
					string imageUrl = baseUrl + item.ProductImg; // الحصول على المسار الكامل للصورة
					return $@"
                        <div class='product-item'>
                            <img src='{imageUrl}' alt='Product Image' class='product-image' />
                            <div class='product-details'>
                                <div class='product-title'>{item.ProductName ?? "Product Name"}</div>
                                <div class='quantity'>Quantity: {item.Quantity}</div>
                                <div class='price-details'>{item.Price:0.00} EGP</div>
                            </div>
                        </div>";
				}))}
                </div>

                <!-- Order Total -->
                <div class='total-price'>
                    Total price: {cart.TotalPrice:0.00} EGP
                </div>
            </div>

            <!-- Billing and Shipping Section -->
            <div class='billing-shipping'>
                <!-- Billing Address Section -->
                <div class='address-section'>
                    <h4>Billing Address</h4>
                    <p class='address-line'><strong>Name:</strong> {billingAddress.UserName}</p>
                    <p class='address-line'><strong>Address:</strong> {billingAddress.Address}</p>
                    <p class='address-line'><strong>City:</strong> {billingAddress.City}</p>
                    <p class='address-line'><strong>Country:</strong> {billingAddress.Country}</p>
                    <p class='address-line'><strong>Phone:</strong> {billingAddress.PhoneNumber}</p>
                </div>
            </div>

            <!-- Payment Method Section -->
            <div class='billing-shipping'>
                <div class='address-section'>
                    <h4>Payment Method</h4>
                    <p class='address-line'><strong>Card Number:</strong> **** **** **** {paymentCard.CardNumber.Substring(paymentCard.CardNumber.Length - 4)}</p>
                    <p class='address-line'><strong>Expiry Date:</strong> {paymentCard.ExpirationDate}</p>
                </div>
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
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to send email: {ex.Message}");
			return false;
		}
	}




}
