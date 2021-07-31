$(document).ready(function () {

});

function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getAlbumsByCategory(categoryId, artistId) {
    matchingAlbums = await getAjax('/Songs/FilterSongsByCategoryAndArtist', {
        categoryId: categoryId,
        artistId: artistId
    });
    return matchingAlbums;
}

function setAlbumDropDownListValues(categoryId, artistId) {
    $("#Songs").html("");
    getAlbumsByCategory(categoryId, artistId).then((data) => {
        data.$values.forEach((record) => {
            $("#Songs").append("<option value=" + record.id + " >" + record.title + "</option>");
        });
    });
}

// on page start
$(document).ready(function () {
    var categoryId = $("#Category").val();
    var artistId = $("#Artist").val();
    setAlbumDropDownListValues(categoryId, artistId);

    if (categories.$values.length == 0 || artists.$values.length == 0 || songs.$values.length == 0)
        $('.modal').modal('show');
});

// on category change
$("#Category").on('input', function (e) {
    var categoryId = $("#Category").val();
    var artistId = $("#Artist").val();
    setAlbumDropDownListValues(categoryId, artistId);
});

// on artist change
$("#Artist").on('input', function (e) {
    var categoryId = $("#Category").val();
    var artistId = $("#Artist").val();
    setAlbumDropDownListValues(categoryId, artistId);
});