$(document).ready(function () {
    updateBiographiesList();
});

function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getMatchingBiographies(matchingStr) {
    matchingBiographies = await getAjax('/Biographies/Filter', { matchingStr: matchingStr });
    return matchingBiographies;
}

async function getAllBiographies() {
    allBiographies = await getAjax('/Biographies/Index', {});
    return allBiographies
}

$("#searchBox").on('input', function () {
    updateBiographiesList();
});

async function updateBiographiesList() {
    var matchingStr = $("#searchBox").val();
    $("table tbody").html("");
    $("#noData").html("");

    if (matchingStr) {
        getMatchingBiographies(matchingStr).then((data) => {
            if (data.$values.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');

            data.$values.forEach(record => {
                var row = "<tr>" +
                    "<td>" + record.artistStageName + "</td>" +
                    "<td>" + record.earlyLife + "</td>" +
                    "<td>" + record.career + "</td>" +
                    "<td>" + record.artistry + "</td>" +
                    "<td>" + record.personalLife + "</td>" +
                    "<td>" + record.numberOfSongs + "</td>" +
                    "<td>" + record.numberOfAlbums + "</td>" + "<td>" +
                    "<a href=\"/Branches/Edit/" + record.id + "\">Edit</a>" + " | " +
                    "<a href=\"/Branches/Details/" + record.id + "\">Details</a>" + " | " +
                    "<a href=\"/Branches/Delete/" + record.id + "\">Delete</a>" +
                    "</td></tr>"

                $("table tbody").append(row);
            });
        });
    } else {
        getAllBiographies().then((data) => {
            var rows = $(data).find("table tbody tr");
            $("#noData").html("");
            $("table tbody").html("");
            $("table tbody").html(rows);
            if (rows.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');
        });
    }
}