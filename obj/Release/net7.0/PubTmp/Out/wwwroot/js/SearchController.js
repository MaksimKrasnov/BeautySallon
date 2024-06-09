
$(document).ready(function () {
    $('#searchInput').keyup(function () {
        var searchText = $(this).val();
        if (searchText.trim() === "") {
            $('#searchResults').html("<p>/p>");
            return;
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
