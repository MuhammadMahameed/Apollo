function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getLabel(artistStageName) {
    labels = await getAjax('/Labels/GetAllLabels', {});
    return labels;
}

function setArtistLabelsDropDownListValues() {
    $("#Labels").html("");
    getLabel().then((data) => {
        data.$values.forEach((record) => {
            $("#Labels").append("<option value=" + record.id + " >" + record.name + "</option>");
        });
        $("#Labels").val(selectedLabels.$values);
    });
}

// on page start
$(document).ready(function () {
    var artistStageName = $("#StageName").val();
    setArtistLabelsDropDownListValues();
});