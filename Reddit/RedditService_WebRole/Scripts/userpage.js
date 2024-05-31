function sendDataToBackend() {
    window.location.href = 'UserProfile';
}
function updateUI() {
    if (localStorage.getItem('token') == null) {
        $('.auth-container').html('<button onclick="loginNavigate()">Login</button>');
    } else {
        $('.auth-container').html('<button onclick="sendDataToBackend()">Profile</button> <button onclick="logout()">Logout</button>');
    }
}


function loginNavigate() {
    window.location.href = '../Authentication/login';
}

function logout() {
    localStorage.removeItem('token');
    updateUI();
    window.location.href = '/Authentication/Login';
}

$(document).ready(function () {
    updateUI();
});

function newTopicNavigate() {
    window.location.href = "/Theme/NewTheme";
}

function getEmailFromToken(token) {
    try {
        // Dekodiraj JWT token
        const tokenParts = token.split('.');
        const payload = JSON.parse(atob(tokenParts[1]));

        console.log(payload);

        // Pronađi email u payloadu tokena
        const email = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'];
        return email;
    } catch (error) {
        // Greška prilikom dekodiranja tokena
        console.error('Error decoding token:', error);
        return null;
    }
}

function loadThemes() {
    // Dobavi JWT token iz localStorage-a
    var token = localStorage.getItem('token');
    var userEmail = getEmailFromToken(token);
    console.log(userEmail);

    // Učitaj teme korisnika koji je ulogovan
    $.ajax({
        url: '/Theme/LoadThemes',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                var userThemes = response.themes.filter(function (theme) {
                    return theme.Publisher === userEmail; // Isključi teme korisnika koji je ulogovan
                });
                displayThemes(userThemes, 'Your topics');
            } else {
                console.error(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });

    // Učitaj sve ostale teme
    $.ajax({
        url: '/Theme/LoadThemes',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                var otherThemes = response.themes.filter(function (theme) {
                    return theme.Publisher !== userEmail; // Isključi teme korisnika koji je ulogovan
                });
                displayThemes(otherThemes, 'Other topics');
            } else {
                console.error(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

function displayThemes(themes, title) {
    var $topicsList = $('.topics');

    // Dodaj naslov
    $topicsList.append('<h3>' + title + '</h3>' + '<hr/><br/>');

    themes.forEach(function (theme) {
       
        var $themeContainer = $('<div class="theme-container"></div>');

        // Dodajemo naslov teme
        var $title = $('<h2></h2>').text(theme.Title);
        $themeContainer.append($title);

        if (theme.Publisher) {
            var $publisher = $('<h5></h5>').text(theme.Publisher);
            $themeContainer.append($publisher)
        }
        // Dodajemo sadržaj teme
        var $content = $('<p></p>').text(theme.Content);
        $themeContainer.append($content);


        if (theme.PhotoUrl) {
            var $image = $('<img>').attr('src', theme.PhotoUrl).attr('alt', 'Theme Image');
            $themeContainer.append($image);
        }

        var $votes = $('<div class="votes"></div>');

        // Dodavanje ikona za lajkove i dislajkove
        $votes.append('<i class="fas fa-arrow-up"></i>');
        $votes.append('<span class="upvote-count">' + theme.Upvote + '  </span>');

        $votes.append('<i class="fas fa-arrow-down"></i>');
        $votes.append('<span class="downvote-count">' + theme.Downvote + '</span>');

        // Dodavanje kreiranih ikona u odgovarajući kontejner na stranici
        $themeContainer.append($votes);


        $topicsList.append($themeContainer);
    });
}

loadThemes();
