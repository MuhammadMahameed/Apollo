$(document).ready(function () {
    $(".card").hide();

    $("#searchBox").on("input", function () {
        var str = $("#searchBox").val();

        if (str == "") {
            $("#apolloIcon").show();
            $(".card").hide()
        } else {
            $("#apolloIcon").hide();
            $(".stageName").each(function () {
                if ($(this).text().toUpperCase().match(str.toUpperCase())) {
                    $(this).parent().parent().show(200);
                } else {
                    $(this).parent().parent().hide(200);
                }
            });
        }
    });
});