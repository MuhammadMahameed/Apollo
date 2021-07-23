function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getTimelineEmbed(url) {
    timelineEmbed = await getAjax('/Twitter/GetTimelineEmbed', { url: url });
    return timelineEmbed;
}


$(document).ready(function () {
    draw();
    setInterval(rotate, 100);

    getTimelineEmbed("https://twitter.com/Spotify").then((embedData) => {
        console.log(embedData);
        $("#spotifyTimeline").append(embedData);
    });
});

var drawing = document.getElementById("canvas");
var ctx = drawing.getContext("2d");
var pic = document.getElementById("Apollo");
var deg = 30;

function draw() {
    ctx.drawImage(pic, 0, 0);
    
}
function rotate() {
    
    ctx.fillRect(0, 0, ctx.width, ctx.height);
    ctx.save();
    ctx.translate(pic.width * 0.5, pic.height * 0.5);
    ctx.rotate(deg * Math.PI / 180)
    ctx.translate(-pic.width * 0.5, -pic.height * 0.5);
    ctx.drawImage(pic, 0, 0);
    ctx.restore();
    deg += 30;    
    }


