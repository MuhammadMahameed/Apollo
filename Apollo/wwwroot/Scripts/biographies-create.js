function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getArtistsCount() {
    artists = await getAjax('/Artists/GetAllArtists', {});
    return artists.$values.length;
}

$(document).ready(function () {
    getArtistsCount().then((data) => {
        if (data == 0)
            $('.modal').modal('show');
    });
});