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

function loadThemes() {
    $.ajax({
        url: '/Theme/LoadThemes',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                var themes = response.themes;
                var $topicsList = $('.topics');

                themes.forEach(function (theme) {
                    // Kreiramo kontejner za temu
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
            } else {
                console.error(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}


loadThemes();