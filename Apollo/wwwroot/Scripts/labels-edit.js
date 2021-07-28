function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getAllArtists() {
    artists = await getAjax('/Artists/GetAllArtists', {});
    return artists;
}

function setLabelsArtistsDropDownListValues() {
    $("#Artists").html("");
    getAllArtists().then((data) => {
        data.$values.forEach((record) => {
            $("#Artists").append("<option value=" + record.id + " >" + record.stageName + "</option>");
        });
        $("#Artists").val(selectedArtists.$values);
    });
}

// on page start
$(document).ready(function () {
    var labelName = $("#Name").val();
    setLabelsArtistsDropDownListValues(labelName);
});