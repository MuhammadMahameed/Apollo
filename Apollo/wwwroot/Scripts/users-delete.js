function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

$(document).ready(function () {
    if (currentUser == editedUser)
        $('.modal').modal('show');
});