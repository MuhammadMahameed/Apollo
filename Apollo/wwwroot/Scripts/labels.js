$(document).ready(function () {
    updateLabelsList();
});

function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getMatchingLabels(matchingStr) {
    matchingLabels = await getAjax('/Labels/Filter', { matchingStr: matchingStr });
    return matchingLabels;
}

async function getAllLabels() {
    allLabels = await getAjax('/Labels/Index', {});
    return allLabels
}

$("#searchBox").on('input', function () {
    updateLabelsList();
});


async function updateLabelsList() {
    var matchingStr = $("#searchBox").val();
    $("table tbody").html("");
    $("#noData").html("");

    if (matchingStr) {
        getMatchingLabels(matchingStr).then((data) => {
            if (data.$values.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');

            data.$values.forEach(record => {
                var releaseDate = new Date(record.founded);
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

                var artists = "";
                record.artists.$values.forEach(artist => { artists = artists.concat(artist + " ") });

                var row = "<tr>" + 
                    "<td>" + record.name + "</td>" +
                    "<td>" + record.status + "</td>" +
                    "<td>" + record.country + "</td>" +
                    "<td>" + formatedDate + "</td>" +
                    "<td>" + artists + "</td>" + "<td>" +
                    "<a href=\"/Branches/Edit/" + record.id + "\">Edit</a>" + " | " +
                    "<a href=\"/Branches/Details/" + record.id + "\">Details</a>" + " | " +
                    "<a href=\"/Branches/Delete/" + record.id + "\">Delete</a>" +
                    "</td></tr>"

                $("table tbody").append(row);
            });
        });
    } else {
        getAllLabels().then((data) => {
            var rows = $(data).find("table tbody tr");
            $("#noData").html("");
            $("table tbody").html("");
            $("table tbody").html(rows);
            if (rows.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');
        });
    }
}