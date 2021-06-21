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
    getMatchingSongs(limit).then((data) => {
        console.log(data);
        var numCardsPerRow = 5

        for (var i = 0; i < data.length; i++) {
            if (i % numCardsPerRow == 0) {
                $("#updates").append('<div class="row row' + parseInt(i / numCardsPerRow) + ' d-flex justify-content-center"></div>');
            }

            var template = '<div class="card"><div class="card-body center">' +
                '<a href="' + data[i].link + '">' +
                '<img class="card-img-top" src=' + data[i].imageUrl + ' alt="Card image cap">' +
                '<h5 class="card-title">' + data[i].name + '</h5>' +
                "</a>" +
                '<p>' + data[i].artists + '</p>' +
                '<p>' + data[i].date + '</p>' +
                '</div></div>'
            $("#updates .row" + parseInt(i / numCardsPerRow)).append(template);
        }
    })
});