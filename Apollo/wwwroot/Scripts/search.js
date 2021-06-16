$(document).ready(function () {
});

$("#searchBox").on('input', function (e) {
    var matchingStr = $("#searchBox").val();

    $.ajax('/Songs/Search', {
        method: "GET",
        data: { matchingStr: matchingStr }
    }).done(function (data) {
        $("#content").html("");
        var numCardsPerRow = 5

        for (i = 0; i < data.length; i++) {
            if (i % numCardsPerRow == 0) {
                $("#content").append('<div class="row songRow' + parseInt(i / numCardsPerRow) + '"></div>');
            }

            var template = '<div class="col-sm"><div class="card" style="width: 10rem;"><div class="card-body"><h5 class="card-title">' + data[i].title + '</h5><p>' + data[i].length.minutes + ':' + data[i].length.seconds + "</p></div></div></div>"
            $("#content .songRow" + parseInt(i / numCardsPerRow) + ":eq(" + parseInt(i / numCardsPerRow) + ")").append(template);
        }
    });
});