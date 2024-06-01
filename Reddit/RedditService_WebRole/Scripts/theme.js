
$(document).on('click', '.trash', function () {
    var $themeContainer = $(this).closest('.theme-container');
    var themeId = $themeContainer.attr('data-rowKey');
    var token = localStorage.getItem('token');
    console.log(themeId);

    // Pozivamo akciju brisanja teme na serveru
    $.ajax({
        url: '/Theme/DeleteTheme',
        type: 'POST',
        dataType: 'json',
        data: { themeId: themeId, token: token}, // Prosleđujemo ID teme koju želimo da obrišemo
        success: function (response) {
            if (response.success) {
                $themeContainer.remove();
                alert("Topic is successfully deleted!");
            } else {
                console.error(response.message); // Ako dođe do greške, ispišemo poruku u konzoli
            }
        },
        error: function (xhr, status, error) {
            console.error(error); // Ukoliko dođe do greške u AJAX pozivu, ispišemo je u konzoli
        }
    });
});
