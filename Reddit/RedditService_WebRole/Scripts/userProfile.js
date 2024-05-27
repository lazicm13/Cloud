document.addEventListener("DOMContentLoaded", function () {
    function fetchUserData() {
        const token = localStorage.getItem('token');

        if (!token) {
            console.error('Token not found!');
            return;
        }

        fetch('/User/ProfilePage', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ token: token })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                // Ažuriranje UI sa podacima o korisniku
                document.querySelector('img.profile-photo').src = data.PhotoUrl;
                document.querySelector('h1').textContent = data.Ime + ' ' + data.Prezime;
                document.querySelector('p.email').textContent = 'Email: ' + data.Email;
                document.querySelector('p.address').textContent = 'Address: ' + data.Adresa + ', ' + data.Grad + ', ' + data.Drzava;
                document.querySelector('p.phone').textContent = 'Phone: ' + data.Broj_telefona;

            })
            .catch(error => {
                console.error('Error fetching user data:', error);
            });
    }

    // Poziv funkcije za preuzimanje podataka o korisniku prilikom učitavanja stranice
    fetchUserData();
});

function sendTokenToServer() {
    const token = localStorage.getItem('token');
    fetch('/User/RedirectToEdit', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ token: token })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Redirect to the profile page
                window.location.href = `/User/UpdateUser?email=${encodeURIComponent(data.email)}`;
            } else {
                // Handle error
                alert('Failed to update user.');
            }
        })
        .catch(error => console.error('Error:', error));
}

