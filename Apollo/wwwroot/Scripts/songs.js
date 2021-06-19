$(document).ready(function () {
});

function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getMatchingSongs(matchingStr) {
    matchingSongs = await getAjax('/Songs/Filter', { matchingStr: matchingStr });
    return matchingSongs;
}

async function getAllSongs() {
    allSongs = await getAjax('/Songs/Index', {});
    return allSongs
}

$("#searchBox").on('input', function (e) {
    var matchingStr = $("#searchBox").val();
    $("table tbody").html("");

    if (matchingStr) {
        getMatchingSongs(matchingStr).then((data) => {
            data.$values.forEach(record => {
                var releaseDate = new Date(record.releaseDate);
                var formatedDate = releaseDate.getDate() + "/" +
                    (releaseDate.getMonth() + 1) + "/" +
                    releaseDate.getFullYear();

                var row = "<tr><td>" + record.title +
                    "</td><td>" + record.category +
                    "</td><td>" + record.artist +
                    "</td><td>" + record.plays +
                    "</td><td>" + record.rating +
                    "</td><td>" + record.length.minutes + ":" + record.length.seconds +
                    "</td><td>" + formatedDate +
                    "</td><td>" + record.album +
                    "</td><td>" +
                    "<a href=\"/Songs/Edit/" + record.id + "\">Edit</a>" + " | " +
                    "<a href=\"/Songs/Details/" + record.id + "\">Details</a>" + " | " +
                    "<a href=\"/Songs/Delete/" + record.id + "\">Delete</a>" +
                    "</td></td></tr>"

                $("table tbody").append(row);
            });
        });
    } else {
        getAllSongs().then((data) => {
            var rows = $(data).find("table tbody tr");
            $("table tbody").html(rows);
        });
    }
});