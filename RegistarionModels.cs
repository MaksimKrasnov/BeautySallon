using System.ComponentModel.DataAnnotations;

namespace BeautySaloon
{
	public class RegisterationModel
	{
		[Required(ErrorMessage = "Не указан Email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required(ErrorMessage = "Не указано имя")]
		[DataType(DataType.Text)]
		[StringLength(3)]
		public string Name { get; set; }

		[Required(ErrorMessage = "Не указан пароль")]
		[DataType(DataType.Password)]
		[StringLength(8, ErrorMessage = "Пароль должен содержать как минимум 8 символов")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Не указан номер телефона")]
		[Phone(ErrorMessage = "Неверный формат номера телефона")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароль введен неверно")]
		public string ConfirmPassword { get; set; }
	}
}
