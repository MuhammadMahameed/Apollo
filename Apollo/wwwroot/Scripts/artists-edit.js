function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getLabelsByArtistStageName(artistStageName) {
    matchingLabels = await getAjax('/Labels/FilterLabelsByArtistStageName', {
        artistStageName: artistStageName
    });
    return matchingLabels;
}

function setArtistLabelsDropDownListValues(artistId) {
    $("#Labels").html("");
    getLabelsByArtistStageName(artistId).then((data) => {
        data.$values.forEach((record) => {
            $("#Labels").append("<option value=" + record.id + " >" + record.name + "</option>");
        });
        $("#Labels").val(selectedLabels.$values);
    });
}

// on page start
$(document).ready(function () {
    var artistStageName = $("#StageName").val();
    setArtistLabelsDropDownListValues(artistStageName);
});