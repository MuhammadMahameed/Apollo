$(document).ready(function () {
    $(".card").hide();
    $("hr").hide()

    $("#searchBox").on("input", function () {
        var str = $("#searchBox").val();

        if (str == "") {
            $("#apolloIcon").show();
            $(".card").hide()
            $("hr").hide()
        } else {
            $("#apolloIcon").hide();
            $(".stageName").each(function () {
                if ($(this).text().toUpperCase().match(str.toUpperCase())) {
                    $(this).parent().parent().show(200);
                    $("hr").show()
                } else {
                    $(this).parent().parent().hide(200);
                }
            });
        }
    });
});