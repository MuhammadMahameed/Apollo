function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getMatchingSongs(limit) {
    var updates = await getAjax('/Updates/GetReleases', { limit: limit });
    return updates.$values;
}

$(document).ready(function () {
    var limit = 20;
    var now = Date.now();

    getMatchingSongs(limit).then((data) => {
        console.log(data);
        var numCardsPerRow = 5

        for (var i = 0; i < data.length; i++) {
            if (i % numCardsPerRow == 0) {
                $("#updates").append('<div class="row row' + parseInt(i / numCardsPerRow) + ' d-flex justify-content-center"></div>');
            }

            var release = Math.trunc((new Date(Date.now() - Date.parse(data[i].date))) / 24 / 60 / 60 / 1000);

            if (release == 0) {
                release = "TODAY";
            } else if (release == 1) {
                release += " DAY AGO";
            } else {
                release += " DAYS AGO";
            }

            var template = '<a href="' + data[i].link + '">' +
                '<div class="card"><div class="card-body center">' +
                '<img class="card-img-top" src=' + data[i].imageUrl + ' alt="Card image cap">' +
                '<h4 class="card-title">' + data[i].name + '</h4>' +
                '<p class="artists">' + data[i].artists + '</p>' +
                '<p>RELEASED ' + release + '</p>' +
                '</div></div></a>'
            $("#updates .row" + parseInt(i / numCardsPerRow)).append(template);
        }
    })
});