$(document).ready(function () {
    $("select").val(selectedSongs.$values);
});

$("select").change(function () {
    console.log($("select").val());
});