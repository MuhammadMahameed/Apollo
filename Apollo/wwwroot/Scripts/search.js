$(document).ready(function () {
});

function getAjax(url, matchingStr) {
    return $.ajax(url, {
        method: "GET",
        data: { matchingStr: matchingStr }
    });
}

async function getData(matchingStr) {
    var dataTypes = [];
    dataTypes[dataTypes.length] = await getAjax('/Songs/Search', matchingStr);
    dataTypes[dataTypes.length] = await getAjax('/Albums/Search', matchingStr);
    return dataTypes;
}

$("#searchBox").on('input', function (e) {
    var matchingStr = $("#searchBox").val();

    if (!matchingStr) {
        $("#apolloIcon").show();
    } else {
        $("#apolloIcon").hide();
    }

    $("#content").html("");
    var numCardsPerRow = 5
    dataTypes = [];

    getData(matchingStr).then((data) => {
        dataTypes = data;

        for (var k = 0; k < dataTypes.length; k++) {
            data = dataTypes[k].$values

            if (data == null) {
                continue;
            }

            if (data.length > 0) {
                if (k == 0) {
                    $("#content").append("<h1>Songs</h1>")

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
                } else if (k == 1) {
                    $("#content").append("<h1>Albums</h1>")

                    for (var i = 0; i < data.length; i++) {
                        if (i % numCardsPerRow == 0) {
                            $("#content").append('<div class="row albumRow' + parseInt(i / numCardsPerRow) + ' d-flex justify-content-center"></div>');
                        }

                        var template = '<div class="card" style="width: 10rem;"><div class="card-body">' +
                            '<img class="card-img-top" src=' + data[i].cover + ' alt="Card image cap">' +
                            '<h5 class="card-title">' + data[i].title + '</h5>' +
                            '<p>Length: ' + data[i].listenTime.minutes + ':' + data[i].listenTime.seconds + '</p>' +
                            '<p>Artist: ' + data[i].artist + '</p>' +
                            '<p>Category: ' + data[i].category + '</p>' +
                            '</div ></div>'
                        $("#content .albumRow" + parseInt(i / numCardsPerRow)).append(template);
                    }
                }
            }
        }
    })
});