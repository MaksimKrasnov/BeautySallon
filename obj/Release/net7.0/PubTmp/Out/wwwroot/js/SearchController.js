
//$(document).ready(function () {
//    $('#searchInput').keyup(function () {
//        var searchText = $(this).val();
//        if (searchText.trim() === "") {
//            $('#searchResults').html("<p>/p>");
//            return;
//        }
//        $.ajax({
//            url: "/Search/Search?searchString=" + searchText,
//            type: 'GET',
//            success: function (result) {
//                $('#searchResults').html(result);
//            }
//        });
//    });
//});
document.addEventListener('DOMContentLoaded', function () {
var mainContentVisible = document.getElementById('mainContent').getAttribute('data-state');

    if (mainContentVisible) {
        $('#mainContent').show();
        $('#searchResults').hide();
    } else {
        $('#mainContent').hide();
        $('#searchResults').show();
    }

    $('#searchInput').on('keyup', function () {
        var searchText = $(this).val();
        if (searchText.trim() === "") {
            $('#searchResults').html("<p></p>");
            $('#mainContent').show();
            $('#searchResults').hide();
            return;
        } else {
            $('#mainContent').hide();
            $('#searchResults').show();
        }

        $.ajax({
            url: "/Search/Search?searchString=" + searchText,
            type: 'GET',
            success: function (result) {
                $('#searchResults').html(result);
            }
        });
    });
});
