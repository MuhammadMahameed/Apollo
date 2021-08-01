$(document).ready(function () {
});

function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getMatchingUsers(matchingStr) {
    matchingLabels = await getAjax('/Users/Filter', { matchingStr: matchingStr });
    return matchingLabels;
}

async function getAllUsers() {
    allLabels = await getAjax('/Users/Index', {});
    return allLabels
}

$("#searchBox").on('input', function (e) {
    var matchingStr = $("#searchBox").val();
    $("table tbody").html("");
    $("#noData").html("");

    if (matchingStr) {
        getMatchingUsers(matchingStr).then((data) => {
            if (data.$values.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');

            data.$values.forEach(record => {
                var row = "<tr>" + 
                    "<td>" + record.name + "</td>" +
                    "<td>" + record.password + "</td>" +
                    "<td>" + record.age + "</td>" +
                    "<td>" + record.email + "</td>" +
                    "<td>" + record.roleType + "</td>" + "<td>" +
                    "<a href=\"/Branches/Edit/" + record.id + "\">Edit</a>" + " | " +
                    "<a href=\"/Branches/Details/" + record.id + "\">Details</a>" + " | " +
                    "<a href=\"/Branches/Delete/" + record.id + "\">Delete</a>" +
                    "</td></tr>"

                $("table tbody").append(row);
            });
        });
    } else {
        getAllUsers().then((data) => {
            var rows = $(data).find("table tbody tr");
            $("#noData").html("");
            $("table tbody").html("");
            $("table tbody").html(rows);
            if (rows.length == 0)
                $("#noData").append('<img src="Assets/nothing_found.png">');
        });
    }
});