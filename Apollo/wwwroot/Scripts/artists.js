$(document).ready(function () {
});

function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getMatchingSongs(matchingStr) {
    matchingSongs = await getAjax('/Artists/Filter', { matchingStr: matchingStr });
    return matchingSongs;
}

async function getAllArtists() {
    allSongs = await getAjax('/Artists/Index', {});
    return allSongs
}

$("#searchBox").on('input', function (e) {
    var matchingStr = $("#searchBox").val();
    $("table tbody").html("");
    $("#noData").html("");

    if (matchingStr) {
        getMatchingSongs(matchingStr).then((data) => {
            if (data.$values.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');

            data.$values.forEach(record => {
                var songs = "";
                var albums = "";
                var labels = "";
                record.songs.$values.forEach(song => { songs = songs.concat(song + " ") });
                record.albums.$values.forEach(album => { albums = albums.concat(album + " ") });
                record.labels.$values.forEach(label => { labels = labels.concat(label + " ") });

                var row = "<tr><td>" + record.firstName +
                    "</td><td>" + record.lastName +
                    "</td><td>" + record.stageName +
                    "</td><td>" + record.age +
                    "</td><td>" + record.rating +
                    "</td><td>" + record.image +
                    "</td><td>" + songs +
                    "</td><td>" + albums +
                    "</td><td>" + labels +
                    "</td><td>" +
                    "<a href=\"/Albums/Edit/" + record.id + "\">Edit</a>" + " | " +
                    "<a href=\"/Albums/Details/" + record.id + "\">Details</a>" + " | " +
                    "<a href=\"/Albums/Delete/" + record.id + "\">Delete</a>" +
                    "</td></td></tr>"

                $("table tbody").append(row);
            });
        });
    } else {
        getAllArtists().then((data) => {
            var rows = $(data).find("table tbody tr");
            $("#noData").html("");
            $("table tbody").html("");
            $("table tbody").html(rows);
            if (rows.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');
        });
    }
});