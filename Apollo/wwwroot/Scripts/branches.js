$(document).ready(function () {
});

function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getMatchingBranches(matchingStr) {
    matchingBranches = await getAjax('/Branches/Filter', { matchingStr: matchingStr });
    return matchingBranches;
}

async function getAllBranches() {
    allBranches = await getAjax('/Branches/Index', {});
    return allBranches
}

$("#searchBox").on('input', function (e) {
    var matchingStr = $("#searchBox").val();
    $("table tbody").html("");
    $("#noData").html("");

    if (matchingStr) {
        getMatchingBranches(matchingStr).then((data) => {
            if (data.$values.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');

            data.$values.forEach(record => {
                var row = "<tr>" + 
                    "<td>" + record.addressName + "</td>" + 
                    "<td>" + record.coordinate + "</td><td>" +
                    "<a href=\"/Branches/Edit/" + record.id + "\">Edit</a>" + " | " +
                    "<a href=\"/Branches/Details/" + record.id + "\">Details</a>" + " | " +
                    "<a href=\"/Branches/Delete/" + record.id + "\">Delete</a>" +
                    "</td></tr>"

                $("table tbody").append(row);
            });
        });
    } else {
        getAllBranches().then((data) => {
            var rows = $(data).find("table tbody tr");
            $("#noData").html("");
            $("table tbody").html("");
            $("table tbody").html(rows);
            if (rows.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');
        });
    }
});