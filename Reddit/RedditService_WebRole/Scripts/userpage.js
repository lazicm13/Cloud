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

