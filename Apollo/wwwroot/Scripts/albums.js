$(document).ready(function () {
});

function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getMatchingSongs(matchingStr) {
    matchingSongs = await getAjax('/Albums/Filter', { matchingStr: matchingStr });
    return matchingSongs;
}

async function getAllSongs() {
    allSongs = await getAjax('/Albums/Index', {});
    return allSongs
}

$("#searchBox").on('input', function (e) {
    var matchingStr = $("#searchBox").val();
    $("table tbody").html("");

    if (matchingStr) {
        getMatchingSongs(matchingStr).then((data) => {
            data.$values.forEach(record => {
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
        getAllSongs().then((data) => {
            var rows = $(data).find("table tbody tr");
            $("table tbody").html(rows);
        });
    }
});