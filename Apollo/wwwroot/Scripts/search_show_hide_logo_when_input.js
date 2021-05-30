$(document).ready(function () {
    $("#searchBox").on("input", function () {
        var str = $("#searchBox").val();

        if (str == "") {
            $("#apolloIcon").show(600);
        } else {
            $("#apolloIcon").hide(600);
        }
    });
    val
});