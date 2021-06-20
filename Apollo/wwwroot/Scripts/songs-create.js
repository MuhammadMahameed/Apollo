function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getAlbumsByCategory(categoryId) {
    matchingAlbums = await getAjax('/Albums/FilterAlbumsByCategory', { categoryId: categoryId });
    return matchingAlbums;
}

function setAlbumDropDownListValues(categoryId) {
    $("#Album").html("<option value=" + 0 + " >N/A</option>")

    getAlbumsByCategory(categoryId).then((data) => {
        data.$values.forEach((record) => {
            $("#Album").append("<option value=" + record.id + " >" + record.title + "</option>");
        });
    });
}

$(document).ready(function () {
    var categoryId = $("#Category").val();
    setAlbumDropDownListValues(categoryId)
});

$("#Category").on('input', function (e) {
    var categoryId = $("#Category").val();
    setAlbumDropDownListValues(categoryId)
});