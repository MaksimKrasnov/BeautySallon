

$(document).ready(function () {
    $(".buttonService").click(function () {
        var serviceId = $(this).data("id");
        $.ajax({
            url: "/Home/GetServices?id=" + serviceId,
            type: "GET",
            success: function (data) {
                $("#textContainer").html(data);
            }
        });

        // Удаляем класс active у всех кнопок
        $('.buttonService').removeClass('active');
        // Добавляем класс active к нажатой кнопке
        $(this).addClass('active');
    });

    // Инициализация по умолчанию при загрузке страницы
    $.ajax({
        url: "/Home/GetServices?id=" + 1,
        type: "GET",
        success: function (data) {
            $("#textContainer").html(data);
        }
    });
});

