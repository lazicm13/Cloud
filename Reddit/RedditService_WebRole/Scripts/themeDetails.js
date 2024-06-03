// themeDetails.js

$(document).ready(function () {
    loadThemeDetails();
    loadComments();
    console.log("RADI LI OVOOOO");
    /*
    $('#submit-comment').click(function () {
        submitComment();
    });*/
});

function goBackToHome() {
    window.location.href = '/User/UserPage';
}



function getThemeId() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('id');
}

function loadThemeDetails() {
    const token = localStorage.getItem('token');
    const themeId = getThemeId();

    $.ajax({
        url: '/Theme/GetThemeDetails',
        type: 'GET',
        data: { id: themeId },
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                $('#theme-title').text(response.theme.Title);
                $('#theme-content').text(response.theme.Content);
                $('#theme-image').attr('src', response.theme.PhotoUrl);
                $('#theme-upvotes').text(response.theme.Upvote);
                $('#theme-downvotes').text(response.theme.Downvote);
                $('#theme-time').text(new Date(response.theme.Time_published).toLocaleString());
                $('#theme-publisher').text(response.theme.Publisher);
            } else {
                console.error(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

function loadComments() {
    const token = localStorage.getItem('token');
    const themeId = getThemeId();

    $.ajax({
        url: '/Theme/GetComments',
        type: 'POST',
        data: { id: themeId },
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                const commentsList = $('#comments-list');
                commentsList.empty();
                response.comments.forEach(function (comment) {
                    const commentItem = $('<li></li>').text(comment.Content);
                    commentsList.append(commentItem);
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





function AddComment() {
    
    const commentContent = document.getElementById('comment-content').value;   
    const token = localStorage.getItem('token'); 
    const themeId = getThemeId(); 

    const data = {
        token: token,
        themeId: themeId,
        content: commentContent
    };

    // Send the data to the backend using AJAX
    $.ajax({
        url: '/Theme/SubmitComment',
        type: 'POST',
        data: data,
        dataType: 'json',
        success: function (response) {
            if (response.success) {

                loadComments();
                // Clear the textarea
                document.getElementById('comment-content').value = '';
            } else {
                // Handle error
                console.error(response.message);
            }
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error(error);
        }
    });
}