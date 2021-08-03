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

    updateAlbumList();
});

async function getMatchingAlbums(matchingStr) {
    matchingAlbums = await getAjax('/Albums/Filter', { matchingStr: matchingStr });
    return matchingAlbums;
}

async function getAllAlbums() {
    allAlbums = await getAjax('/Albums/Index', {});
    return allAlbums
}

$("#albumFilter").change(function () {
    if (this.checked) {
        $("#albumSelect").prop("disabled", false);
    } else {
        $("#albumSelect").prop("disabled", true);
        $("#albumSelect").val(0)
        updateAlbumList()
    }
});

$("#categoryFilter").change(function () {
    if (this.checked) {
        $("#categorySelect").prop("disabled", false);
    } else {
        $("#categorySelect").prop("disabled", true);
        $("#categorySelect").val(0)
        updateAlbumList()
    }
});

$("#artistFilter").change(function () {
    if (this.checked) {
        $("#artistSelect").prop("disabled", false);
    } else {
        $("#artistSelect").prop("disabled", true);
        $("#artistSelect").val(0)
        updateAlbumList()
    }
});

$("#searchBox").on('input', function (e) {
    updateAlbumList();
});

$("#albumSelect,#categorySelect,#artistSelect").on('change', function () {
    updateAlbumList();
});

async function updateAlbumList() {
    var matchingStr = $("#searchBox").val();
    $("table tbody").html("");
    var albumSelect = $("#albumSelect option:selected").val()
    var categorySelect = $("#categorySelect option:selected").val()
    var artistSelect = $("#artistSelect option:selected").val()

    if (matchingStr || albumSelect > 0 || artistSelect > 0 || categorySelect > 0) {
        var hasData = false;

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

                hasData = true;
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
                record.songs.$values.forEach(song => { songs = songs.concat("<div>" + song + "</div>") });

                var row = "<tr><td>" + record.title +
                    "</td><td>" + record.category +
                    "</td><td>" + record.artist +
                    "</td><td>" + formatedListenTime +
                    "</td><td>" + record.rating +
                    "</td><td>" + formatedDate +
                    "</td><td>" + record.cover +
                    "</td><td>" + songs +
                    "</td><td>" +
                    `<i class="bi bi-star" id="a_${record.id}-star1"></i>` +
                    `<i class="bi bi-star" id="a_${record.id}-star2"></i>` +
                    `<i class="bi bi-star" id="a_${record.id}-star3"></i>` +
                    `<i class="bi bi-star" id="a_${record.id}-star4"></i>` +
                    `<i class="bi bi-star" id="a_${record.id}-star5"></i>` +
                    '</td><td>' +
                    "<a href=\"/Albums/Edit/" + record.id + "\">Edit</a>" + " | " +
                    "<a href=\"/Albums/Details/" + record.id + "\">Details</a>" + " | " +
                    "<a href=\"/Albums/Delete/" + record.id + "\">Delete</a>" +
                    "</td></td></tr>"

                $("table tbody").append(row);
            });
        }).then(() => {
            matchingStr = $("#searchBox").val();
            $("#noData").html("");
            if ((matchingStr != "" && !hasData) || (matchingStr == "" && !hasData && (albumSelect > 0 || categorySelect > 0 || artistSelect > 0)))
                $("#noData").append('<img src="Assets/nothing_found.png">');
            else
                fillUserStars(type, currentUser);
        });
    } else {
        getAllAlbums().then((data) => {
            var rows = $(data).find("table tbody tr");
            $("#noData").html("");
            $("table tbody").html("");
            $("table tbody").html(rows);
            fillUserStars(type, currentUser).then(() => {
                if (rows.length == 0)
                    $("#noData").append('<img src="Assets/nothing_found.png">');
            });
        });
    }
}

var type = "album";
var changeStarOnHover = {};

function fill_empty_star(star) {
    $(star).removeClass("bi-star");
    $(star).addClass("bi-star-fill");
}

function hollow_full_star(star) {
    $(star).removeClass("bi-star-fill");
    $(star).addClass("bi-star");
}

function lock_vote(star) {
    var album_id = parseInt($(star).attr('id').match(/a_\d+/)[0].match(/\d+/)[0]);
    var star_id = parseInt($(star).attr('id').match(/\d$/)[0]);

    for (var i = 1; i <= 5; i++) {
        hollow_full_star($(`#a_${album_id}-star${i}`));
    }

    for (var i = 1; i <= star_id; i++) {
        fill_empty_star($(`#a_${album_id}-star${i}`));
    }

    changeStarOnHover[album_id] = false;
}

$('.table').on('mouseover', '.bi-star', function () {
    var album_id = parseInt($(this).attr('id').match(/a_\d+/)[0].match(/\d+/)[0]);
    var star_id = parseInt($(this).attr('id').match(/\d$/)[0]);

    if (changeStarOnHover[album_id])
        for (var i = 1; i <= star_id; i++)
            fill_empty_star($(`#a_${album_id}-star${i}`));
});

$('.table').on('mouseout', '.bi-star', function () {
    var album_id = parseInt($(this).attr('id').match(/a_\d+/)[0].match(/\d+/)[0]);
    var star_id = parseInt($(this).attr('id').match(/\d$/)[0]);

    if (changeStarOnHover[album_id])
        for (var i = 1; i <= star_id; i++)
            hollow_full_star($(`#a_${album_id}-star${i}`));
});

$('.table').on('mouseover', '.bi-star-fill', function () {
    var album_id = parseInt($(this).attr('id').match(/a_\d+/)[0].match(/\d+/)[0]);
    var star_id = parseInt($(this).attr('id').match(/\d$/)[0]);

    if (changeStarOnHover[album_id])
        for (var i = 1; i <= star_id; i++)
            fill_empty_star($(`#a_${album_id}-star${i}`));
});

$('.table').on('mouseout', '.bi-star-fill', function () {
    var album_id = parseInt($(this).attr('id').match(/a_\d+/)[0].match(/\d+/)[0]);
    var star_id = parseInt($(this).attr('id').match(/\d$/)[0]);

    if (changeStarOnHover[album_id])
        for (var i = 1; i <= star_id; i++)
            hollow_full_star($(`#a_${album_id}-star${i}`));
});

async function checkVoteExists(type, recordId, username) {
    var exists = await getAjax('/Votes/CheckVoteExists', { type: type, recordId: recordId, username: username });
    return exists;
}

async function vote(type, recordId, username, score) {
    await getAjax('/Votes/Create', { type: type, recordId: recordId, username: username, score: score });
}

async function changeVote(type, recordId, username, score) {
    await getAjax('/Votes/Edit', { type: type, recordId: recordId, username: username, score: score });
}

async function getUserVotes(type, username) {
    var userVotes = await getAjax('/Votes/GetUserVotes', { type: type, username: username });
    return userVotes.$values;
}

async function getAlbumsIds() {
    var albumsIds = await getAjax('/Albums/GetAlbumsIds', {});
    return albumsIds.albumsIds.$values;
}

async function fillUserStars(type, username) {
    var currentUserVotes = await getUserVotes(type, username);
    var albumsIds = await getAlbumsIds();

    for (var i = 0; i < albumsIds.length; i++)
        changeStarOnHover[albumsIds[i]] = true;

    for (var i = 0; i < currentUserVotes.length; i++) {
        for (var j = 1; j <= 5; j++)
            hollow_full_star($(`#a_${currentUserVotes[i].recordId}-star${currentUserVotes[i].score}`));
        for (var j = 1; j <= currentUserVotes[i].score; j++)
            fill_empty_star($(`#a_${currentUserVotes[i].recordId}-star${j}`));

        changeStarOnHover[currentUserVotes[i].recordId] = false;
    }
}

async function manageVotes(type, recordId, username, score) {
    var voteExists = await checkVoteExists(type, recordId, username);

    if (!voteExists)
        await vote(type, recordId, currentUser, score);
    else
        await changeVote(type, recordId, currentUser, score)

    await updateAlbumList();
}

$('.table').on('click', '.bi-star', function () {
    lock_vote(this);

    var album_id = parseInt($(this).attr('id').match(/a_\d+/)[0].match(/\d+/)[0]);
    var star_id = parseInt($(this).attr('id').match(/\d$/)[0]);

    manageVotes(type, album_id, currentUser, star_id);
});

$('.table').on('click', '.bi-star-fill', function () {
    lock_vote(this);

    var album_id = parseInt($(this).attr('id').match(/a_\d+/)[0].match(/\d+/)[0]);
    var star_id = parseInt($(this).attr('id').match(/\d$/)[0]);

    manageVotes(type, album_id, currentUser, star_id);
});