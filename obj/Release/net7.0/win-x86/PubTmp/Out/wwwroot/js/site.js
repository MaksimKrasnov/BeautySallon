// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
	$('#phoneInput').on('input', function () {
		var value = this.value.replace(/\D/g, ''); // Удаление всех символов, кроме цифр
		if (value.length > 11) { // Ограничение на количество цифр
			value = value.slice(0, 11);
		}
		this.value = '+7 ' + value.slice(1); // Добавление префикса +7 и введенных цифр
		if (phoneInput !== '' || phoneInput.length == 12) { // Если поле не пустое, скрываем сообщение об ошибке
			$('#phoneInputError').hide();
		}
	});
	$("#submitBtn").click(function () {
		// Получаем значения из полей ввода
		var nameInput = $('#nameInput').val().trim();
		var phoneInput = $('#phoneInput').val().replaceAll(' ', '')
		if (nameInput === '') { // Проверяем, не пустое ли поле
			$('#nameInputError').show(); // Показываем сообщение об ошибке
			return; // Останавливаем выполнение функции, если поле пустое
		} else {
			$('#nameInputError').hide(); // Скрываем сообщение об ошибке, если поле заполнено
		}
		if (phoneInput === '') { // Проверяем, не пустое ли поле
			$('#phoneInputError').show(); // Показываем сообщение об ошибке
			return; // Останавливаем выполнение функции, если поле пустое
		} else {
			$('#phoneInputError').hide(); // Скрываем сообщение об ошибке, если поле заполнено
		}
		if (phoneInput.length != 12) {
			$('#phoneCorrectlytError').show(); // Показываем сообщение об ошибке
			console.log(phoneInput);
			return;

		} else {
			$('#phoneCorrectlytError').hide(); // Скрываем сообщение об ошибке, если поле заполнено
		}
	
		$.post('/Home/SendMessage', { name: nameInput, phone: phoneInput })
			.done(function (response) {
				Swal.fire({
					position: "center",
					background: '#343a40',
					icon: "success",
					title: "Ваша заявка приянта",
					text: "В ближайшее время менеджер вам перезвонит",
					showConfirmButton: false,	
					allowOutsideClick: true,
					timer: 5500,
					timerProgressBar: true,
					customClass: {
						popup: 'sweatAlert',
					}
				}).then((result) => {
					window.location.href = "/Home/Index";
				});
				// Обработка успешного выполнения запроса
				

				console.log(response);
			})
			

	});
});




		// $(document).ready(function () {
		//     // Устанавливаем маску для поля ввода телефона
		//     $('#phoneInput').on('focus', function () {
		//         var phoneInput = $(this);
		//         phoneInput.val('+7 (___) ____-_____');
		//         phoneInput.on('input', function (e) {
		//             var value = phoneInput.val().replace(/\D/g, '');
		//             if (value.length > 10) {
		//                 value = value.slice(0, 10);
		//             }
		//             phoneInput.val('+7 (' + value.slice(0, 3) + ') ' + value.slice(3, 6) + '-' + value.slice(6));
		//         });
		//     });
		// });
