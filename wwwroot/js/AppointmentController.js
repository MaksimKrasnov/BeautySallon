

$(document).ready(function () {
    $('#datepicker').change(function (e) {
        e.preventDefault();
        var selectedDate = $(this).val(); // Получаем выбранную дату
        executeAjaxRequest(selectedDate); // Вызываем функцию с передачей выбранной даты
    });

    // При изменении выбора мастера
    $('#masterSelect').change(function () {
        var selectedDate = $('#datepicker').val(); // Получаем выбранную дату
        if (selectedDate) { // Проверяем, выбрана ли дата
            console.log(selectedDate);
            executeAjaxRequest(selectedDate);
        }
    });
    function executeAjaxRequest(selectedDate) {
        var masterId = $('#masterSelect').val();
        $.get('@Url.Action("GetTimeSlot", "Appointment")', { selectedDate: selectedDate, masterId: masterId })
            .done(function (response) {
                $('#timeSlots').html(response);
            })
            .fail(function (xhr, status, error) {
                console.error(xhr.responseText);
            });
        return false; // Отменяем стандартное действие отправки формы
    };
    $('#nameInput').on('input', function () {
        var nameInput = $(this).val().trim(); // Получаем значение поля и удаляем пробелы в начале и в конце

        if (nameInput !== '') { // Если поле не пустое, скрываем сообщение об ошибке
            $('#nameInputError').hide();
        }
    });
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
    $('#signUp').click(function () {
        var serviceId = $('#idInput').val();
        var nameInput = $('#nameInput').val().trim();
        var phoneInput = $('#phoneInput').val().replaceAll(' ', '');
        var selectedDateTime = $('#selectedDateTime').val();
        var masterId = $('#masterSelect').val();

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

        // Отправляем данные на сервер
        $.post('@Url.Action("Create", "Appointment")', { serviceId: serviceId, masterId: masterId, nameInput: nameInput, phoneInput: phoneInput, selectedDateTime: selectedDateTime })
            .done(function (response) {
                alert("Вы записаны на " + selectedDateTime);
                // Обработка успешного выполнения запроса
                window.location.href = "/Home/Index";

                console.log(response);
            })
            .fail(function (xhr, status, error) {
                console.error(xhr.responseText);
            });
    });
});
