using System.ComponentModel.DataAnnotations;

namespace BeautySaloon
{
	public class RegisterationModel
	{
		[Required(ErrorMessage = "Не указан Email")]
		[EmailAddress(ErrorMessage = "Некорректный Email")]

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required(ErrorMessage = "Не указано имя")]
		[DataType(DataType.Text)]
		
		[StringLength(100, MinimumLength = 3, ErrorMessage = "Имя должно содержать минимум 3 символа")]

		public string Name { get; set; }

		[Required(ErrorMessage = "Не указан пароль")]
		[DataType(DataType.Password)]
		[StringLength(100, MinimumLength = 8, ErrorMessage = "Пароль должен содержать как минимум 8 символов")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Не указан номер телефона")]
		[Phone(ErrorMessage = "Неверный формат номера телефона")]
		[RegularExpression(@"\+7\d{10}", ErrorMessage = "Телефон должен начинаться с +7 и содержать 12 символов")]

		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Подтверждение пароля обязательно")]

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароль не совпадают")]
		public string ConfirmPassword { get; set; }
	}
}


