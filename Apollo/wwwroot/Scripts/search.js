$(document).ready(function () {
});

$("#searchBox").on('input', function (e) {
    var matchingStr = $("#searchBox").val();
    $.ajax('/Songs/Search', {
        method: "GET",
        data: { matchingStr: matchingStr }
    }).done(function (jsonData) {
        var data = jsonData.$values;

        if (data == null) {
            return;
        }

        console.log(data);
        $("#content").html("");
        var numCardsPerRow = 5

        if (data.length > 0)
        {
            $("#content").append("<h1>Songs</h1>")
        }

        for (var i = 0; i < data.length; i++) {
            if (i % numCardsPerRow == 0) {
                $("#content").append('<div class="row songRow' + parseInt(i / numCardsPerRow) + ' d-flex justify-content-center"></div>');
            }

            var template = '<div class="card" style="width: 10rem;"><div class="card-body">' +
                '<h5 class="card-title">' + data[i].title + '</h5>' +
                '<p>Length: ' + data[i].length.minutes + ':' + data[i].length.seconds + '</p>' +
                '<p>Artist: ' + data[i].artist + '</p>' +
                '</div ></div>'
            $("#content .songRow" + parseInt(i / numCardsPerRow)).append(template);
        }
    });
});