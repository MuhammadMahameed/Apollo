function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

$(document).ready(function () {
    if (currentUser == editedUser) {
        $(".usernameClass").hide();
        $(".roleTypeClass").hide();
        $('.modal').modal('show');
    }
});

$("#editingSameUser").click(function () {
    $('.modal').modal('hide');
});