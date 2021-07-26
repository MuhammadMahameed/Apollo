function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getSelectData() {
    var dataTypes = [];
    dataTypes[dataTypes.length] = (await getAjax('/Albums/GetAllAlbums')).$values;
    dataTypes[dataTypes.length] = (await getAjax('/Categories/GetAllCategories')).$values;
    dataTypes[dataTypes.length] = (await getAjax('/Artists/GetAllArtists')).$values;
    return dataTypes;
}

$(document).ready(function () {
    $("#albumSelect").prop("disabled", true);
    $("#categorySelect").prop("disabled", true);
    $("#artistSelect").prop("disabled", true);

    getSelectData().then((data) => {
        console.log(data);

        data[0].forEach(album => {
            $("#albumSelect").append("<option value=" + album.id + ">" + album.title + "</option>");
        });
        data[1].forEach(category => {
            $("#categorySelect").append("<option value=" + category.id + ">" + category.name + "</option>");
        });
        data[2].forEach(artist => {
            $("#artistSelect").append("<option value=" + artist.id + ">" + artist.stageName + "</option>");
        })
    });
});

async function getMatchingAlbums(matchingStr) {
    matchingSongs = await getAjax('/Albums/Filter', { matchingStr: matchingStr });
    return matchingSongs;
}

async function getAllAlbums() {
    allSongs = await getAjax('/Albums/Index', {});
    return allSongs
}

$("#albumFilter").change(function () {
    if (this.checked) {
        $("#albumSelect").prop("disabled", false);
    } else {
        $("#albumSelect").prop("disabled", true);
        $("#albumSelect").val(0)
        updateSongList()
    }
});

$("#categoryFilter").change(function () {
    if (this.checked) {
        $("#categorySelect").prop("disabled", false);
    } else {
        $("#categorySelect").prop("disabled", true);
        $("#categorySelect").val(0)
        updateSongList()
    }
});

$("#artistFilter").change(function () {
    if (this.checked) {
        $("#artistSelect").prop("disabled", false);
    } else {
        $("#artistSelect").prop("disabled", true);
        $("#artistSelect").val(0)
        updateSongList()
    }
});

$("#searchBox").on('input', function (e) {
    updateSongList();
});

$("#albumSelect,#categorySelect,#artistSelect").on('change', function () {
    updateSongList();
});

async function updateSongList() {
    var matchingStr = $("#searchBox").val();
    $("table tbody").html("");
    var albumSelect = $("#albumSelect option:selected").val()
    var categorySelect = $("#categorySelect option:selected").val()
    var artistSelect = $("#artistSelect option:selected").val()

    if (matchingStr || albumSelect > 0 || artistSelect > 0 || categorySelect > 0) {
        getMatchingAlbums(matchingStr).then((data) => {
            data.$values.forEach(record => {
                // 0 is unselected
                if (albumSelect > 0)
                    if (record.title.toUpperCase() != $("#albumSelect option:selected").text().toUpperCase())
                        return;
                if (categorySelect > 0)
                    if (record.category.toUpperCase() != $("#categorySelect option:selected").text().toUpperCase())
                        return;
                if (artistSelect > 0)
                    if (record.artist.toUpperCase() != $("#artistSelect option:selected").text().toUpperCase())
                        return;

                var releaseDate = new Date(record.releaseDate);
                var date = [
                    parseInt(releaseDate.getMonth() + 1),
                    parseInt(releaseDate.getDate()),
                    parseInt(releaseDate.getFullYear())
                ]

                for (var i = 0; i < date.length; i++) {
                    if (date[i] < 10) {
                        date[i] = "0" + date[i];
                    }
                }

                var formatedDate = date[0] + "/" + date[1] + "/" + date[2];

                var listenTime = [
                    record.listenTime.hours,
                    record.listenTime.minutes,
                    record.listenTime.seconds]

                for (var i = 0; i < listenTime.length; i++) {
                    if (listenTime[i] < 10) {
                        listenTime[i] = "0" + listenTime[i];
                    }
                }

                var formatedListenTime = listenTime[0] + ":" + listenTime[1] + ":" + listenTime[2];

                var songs = "";
                record.songs.$values.forEach(song => { songs = songs.concat(song + " ") });

                var row = "<tr><td>" + record.title +
                    "</td><td>" + record.category +
                    "</td><td>" + record.artist +
                    "</td><td>" + formatedListenTime +
                    "</td><td>" + record.plays +
                    "</td><td>" + record.rating +
                    "</td><td>" + formatedDate +
                    "</td><td>" + record.cover +
                    "</td><td>" + songs +
                    "</td><td>" +
                    "<a href=\"/Albums/Edit/" + record.id + "\">Edit</a>" + " | " +
                    "<a href=\"/Albums/Details/" + record.id + "\">Details</a>" + " | " +
                    "<a href=\"/Albums/Delete/" + record.id + "\">Delete</a>" +
                    "</td></td></tr>"

                $("table tbody").append(row);
            });
        });
    } else {
        getAllAlbums().then((data) => {
            var rows = $(data).find("table tbody tr");
            $("table tbody").html(rows);
        });
    }
}