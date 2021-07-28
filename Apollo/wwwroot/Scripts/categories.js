$(document).ready(function () {
});

function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getMatchingCategories(matchingStr) {
    matchingCategories = await getAjax('/Categories/Filter', { matchingStr: matchingStr });
    return matchingCategories;
}

async function getAllCategories() {
    allCategories = await getAjax('/Categories/Index', {});
    return allCategories
}

$("#searchBox").on('input', function (e) {
    var matchingStr = $("#searchBox").val();
    $("table tbody").html("");
    $("#noData").html("");

    if (matchingStr) {
        getMatchingCategories(matchingStr).then((data) => {
            if (data.$values.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');

            data.$values.forEach(record => {
                var row = "<tr><td>" + record.title +
                    "</td><td>" +
                    "<a href=\"/Categories/Edit/" + record.id + "\">Edit</a>" + " | " +
                    "<a href=\"/Categories/Details/" + record.id + "\">Details</a>" + " | " +
                    "<a href=\"/Categories/Delete/" + record.id + "\">Delete</a>" +
                    "</td></td></tr>"

                $("table tbody").append(row);
            });
        });
    } else {
        getAllCategories().then((data) => {
            var rows = $(data).find("table tbody tr");
            $("#noData").html("");
            $("table tbody").html("");
            $("table tbody").html(rows);
            if (rows.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');
        });
    }
});