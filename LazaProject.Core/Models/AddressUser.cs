using System.ComponentModel.DataAnnotations;

namespace LazaProject.Core.Models
{
	public class AddressUser
	{
		[Key]
		public string Id { get; set; }= Guid.NewGuid().ToString();
		[Required(ErrorMessage = "UserId is required.")]
		public string UserId { get; set; }
		[Required(ErrorMessage = "User Name is required.")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "City is required.")]
		[StringLength(100, ErrorMessage = "City cannot be longer than 100 characters.")]
		public string City { get; set; }

		[Required(ErrorMessage = "Country is required.")]
		[StringLength(100, ErrorMessage = "Country cannot be longer than 100 characters.")]
		public string Country { get; set; }

		[Required(ErrorMessage = "Phone number is required.")]
		[Phone(ErrorMessage = "Invalid phone number format.")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Address is required.")]
		[StringLength(250, ErrorMessage = "Address cannot be longer than 250 characters.")]
		public string Address { get; set; }
	}
}
