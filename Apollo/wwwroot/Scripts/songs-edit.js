function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getAlbumsByCategory(categoryId, artistId) {
    matchingAlbums = await getAjax('/Albums/FilterAlbumsByCategoryAndArtist', {
        categoryId: categoryId,
        artistId: artistId
    });
    return matchingAlbums;
}

function setAlbumDropDownListValues(categoryId, artistId) {
    $("#Album").html("<option value=" + 0 + " >N/A</option>")

    getAlbumsByCategory(categoryId, artistId).then((data) => {
        data.$values.forEach((record) => {
            $("#Album").append("<option value=" + record.id + " >" + record.title + "</option>");
        });

        if (selectedAlbum == null || data.$values.length == 0)
            $("#Album").val(0);
        else
            $("#Album").val(selectedAlbum);
    });
}

// on page start
$(document).ready(function () {
    var categoryId = $("#Category").val();
    var artistId = $("#Artist").val();
    setAlbumDropDownListValues(categoryId, artistId);
    changeSongLength(songLengthInSeconds);
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

$("#songLengthRange").on('change', function (e) {
    changeSongLength()
});

function changeSongLength(songLengthInSeconds) {
    var val, minutes, seconds;

    if (songLengthInSeconds != null) {
        val = songLengthInSeconds;
        $("#songLengthRange").val(songLengthInSeconds);
    }
    else
        val = $("#songLengthRange").val();

    minutes = Math.trunc(val / 60)
    seconds = (val % 60);

    if (minutes < 10)
        minutes = "0" + minutes;
    if (seconds < 10)
        seconds = "0" + seconds;

    var time = `00:${minutes}:${seconds}`
    $("#songLengthVal").text(time + " minutes");
    $("#Length").val(time);
}