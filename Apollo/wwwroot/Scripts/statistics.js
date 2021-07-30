function getAjax(url, data) {
    return $.ajax(url, {
        method: "GET",
        data: data
    });
}

async function getSongsDevision() {
    var songsDevision = await getAjax('/Songs/GetNumberOfSongsPerCategory', {});
    return songsDevision.$values;
}

async function getArtistsPerCategoryHeatmapData() {
    var artistHeatMap = await getAjax('/Songs/GetArtistsPerCategoryHeatmapData', {});
    return artistHeatMap.$values;
}

$(document).ready(function () {
    var hasData = [true, true];

    getSongsDevision().then((data) => {
        console.log(data.length)
        if (data.length == 0) {
            hasData[0] = false;
        }

        if (hasData[0]) {
            // set the dimensions and margins of the graph
            var width = 450
            height = 450
            margin = 40

            var radius = Math.min(width, height) / 2 - margin

            var svg = d3.select("#songsCategoryDevision")
                .append("svg")
                .attr("width", width)
                .attr("height", height)
                .append("g")
                .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

            // set the color scale
            var color = d3.scaleOrdinal()
                .domain(data)
                .range(d3.schemeSet2);

            // Compute the position of each group on the pie:
            var pie = d3.pie()
                .value(function (d) { return d.value.numSongs; })
            var data_ready = pie(d3.entries(data))

            // shape helper to build arcs:
            var arcGenerator = d3.arc()
                .innerRadius(0)
                .outerRadius(radius)

            svg
                .selectAll('mySlices')
                .data(data_ready)
                .enter()
                .append('path')
                .attr('d', arcGenerator)
                .attr('fill', function (d) { return (color(d.data.value.category)) })
                .attr("stroke", "black")
                .style("stroke-width", "2px")
                .style("opacity", 0.7)

            svg
                .selectAll('mySlices')
                .data(data_ready)
                .enter()
                .append('text')
                .text(function (d) { return d.data.value.category + " Genre (" + d.data.value.numSongs + ")" })
                .attr("transform", function (d) { return "translate(" + arcGenerator.centroid(d) + ")"; })
                .style("text-anchor", "middle")
                .style("font-size", 17)
        }
    }).then(getArtistsPerCategoryHeatmapData().then((data) => {
        console.log(data.length)
        if (data.length == 0)
            hasData[1] = false;

        if (hasData[1]) {
            // set the dimensions and margins of the graph
            var margin = { top: 10, right: 25, bottom: 30, left: 100 },
                width = 500 - margin.left - margin.right,
                height = 500 - margin.top - margin.bottom;

            // append the svg object to the body of the page
            var svg = d3.select("#artistsPerGenre")
                .append("svg")
                .attr("width", width + margin.left + margin.right)
                .attr("height", height + margin.top + margin.bottom)
                .append("g")
                .attr("transform",
                    "translate(" + margin.left + "," + margin.top + ")");

            // Labels of row and columns -> unique identifier of the column called 'group' and 'variable'
            var myGroups = d3.map(data, function (d) {
                return d.group;
            }).keys()
            var myVars = d3.map(data, function (d) {
                return d.variable;
            }).keys()

            // Build X scales and axis:
            var x = d3.scaleBand()
                .range([0, width])
                .domain(myGroups)
                .padding(0.05);
            svg.append("g")
                .style("font-size", 15)
                .attr("transform", "translate(0," + height + ")")
                .call(d3.axisBottom(x).tickSize(0))
                .select(".domain").remove()

            // Build Y scales and axis:
            var y = d3.scaleBand()
                .range([height, 0])
                .domain(myVars)
                .padding(0.05);
            svg.append("g")
                .style("font-size", 15)
                .call(d3.axisLeft(y).tickSize(0))
                .select(".domain").remove()

            // Build color scale
            var myColor = d3.scaleSequential()
                .interpolator(d3.interpolateInferno)
                .domain([1, myGroups.length * myVars.length])

            // create a tooltip
            var tooltip = d3.select("#artistsPerGenre")
                .append("div")
                .style("opacity", 0)
                .attr("class", "tooltip")
                .style("background-color", "black")
                .style("border", "solid")
                .style("border-width", "2px")
                .style("border-radius", "5px")
                .style("padding", "5px")

            var mouseover = function (d) {
                tooltip
                    .style("opacity", 1)
                d3.select(this)
                    .style("stroke", "black")
                    .style("opacity", 1)
            }
            var mousemove = function (d) {
                tooltip
                    .html(d.variable + " has " + d.value + " songs in the " + d.group + " category")
                    .style("left", (d3.mouse(this)[0] + 60) + "px")
                    .style("top", (d3.mouse(this)[1] + 90) + "px")
            }
            var mouseleave = function (d) {
                tooltip
                    .style("opacity", 0)
                d3.select(this)
                    .style("stroke", "none")
                    .style("opacity", 0.8)
            }

            // add the squares
            svg.selectAll()
                .data(data, function (d) { return d.group + ':' + d.variable; })
                .enter()
                .append("rect")
                .attr("x", function (d) { return x(d.group) })
                .attr("y", function (d) { return y(d.variable) })
                .attr("rx", 4)
                .attr("ry", 4)
                .attr("width", x.bandwidth())
                .attr("height", y.bandwidth())
                .style("fill", function (d) { return myColor(d.value) })
                .style("stroke-width", 4)
                .style("stroke", "none")
                .style("opacity", 0.8)
                .on("mouseover", mouseover)
                .on("mousemove", mousemove)
                .on("mouseleave", mouseleave)
        }

        if (!hasData[0] && !hasData[1]) {
            $(".statsGraphs").hide();
            $("#noData").append('<img src="Assets/nothing_found.png">');
        }
    }));
});
