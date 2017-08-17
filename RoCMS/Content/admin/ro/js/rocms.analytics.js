function numberOfDays(year, month) {
    var d = new Date(year, month, 0);
    return d.getDate();
}

function isLeap(year) {
    return new Date(year, 1, 29).getMonth() == 1;
}

function getLineGraphicOptions() {
    return {
        xaxis: {
            mode: "time",
            minTickSize: [1, "day"],
        },
        yaxis : {
            min: 0
        },
        series: {
            curvedLines: {
                active: true
            }
        },
        grid: {
            hoverable: true,
        //    clickable: true
        }
    };
}

function getPieGraphicOptions() {
    return {
        series: {
            pie: {
                show: true,
                radius: 3/4,
                tilt: 0.6,
                label: {
                    show: true,
                    radius: 1,
                    formatter: function (label, series) {
                        return '<div style="font-size:8pt;text-align:center;padding:2px;color:white;">' + label + '<br/>' + series.data[0][1] + '</div>';
                    },
                    background: { opacity: 0.6 }
                }
            }
        },
        legend: {
            show: false
        }

    };
}

function getBarGraphicOptions() {
    return {
        series: {
            bars: {
                show: true,
                barWidth: 0.6,
                align: "center",
                numbers : {
                    show: true,
                    xAlign: 0.3
                }
            }
        },
        xaxis: {
            mode: "categories",
            tickLength:0,
        }
    };
}

function sortfunction(a, b) {
    if (a[0] < b[0]) {
        return -1;
    }
    if (a[0] > b[0]) {
        return 1;
    }
    return 0;
}



function getSingleDaySummary(date) {
    blockUI();
    var blocks = 2;
    var options = getBarGraphicOptions();
    var d1 = [];
    var data1 = date.format("yyyy/mm/dd");

    getJSON("/api/analytics/traffic/summary", { startDate: data1, endDate: data1 }, function (res) {
        if (!res.succeed) {
            alert(res.message);
            return;
        }
        if (!res.data.trafficSummaryCollection[0]) {
            $.plot("#placeholder", [], options);
            return;
        }
        d1.push([["Просмотры"], res.data.trafficSummaryCollection[0].pageViews]);
        d1.push([["Посетители"], res.data.trafficSummaryCollection[0].visitors]);
        $.plot("#placeholder", [ d1 ], options);
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
        if (--blocks == 0) {
            unblockUI();
        }
    });
    getJSON("/api/analytics/sources/summary", { startDate: data1, endDate: data1 }, function (res) {
        var options = getPieGraphicOptions();
        var d1 = [];

        if (res.data.social) {
            d1.push({ label: "Соцсети", data: res.data.social });
        }
        if (res.data.direct) {
            d1.push({ label: "Прямые заходы", data: res.data.direct });
        }
        if (res.data.savedPages) {
            d1.push({ label: "Закладки", data: res.data.savedPages });
        }
        if (res.data.links) {
            d1.push({ label: "Внешние cсылки", data: res.data.links });
        }
        if (res.data.internal) {
            d1.push({ label: "Внутренние переходы", data: res.data.internal });
        }
        if (res.data.mail) {
            d1.push({ label: "Почта", data: res.data.mail });
        }
        if (res.data.ads) {
            d1.push({ label: "Реклама", data: res.data.ads });
        }
        if (res.data.search) {
            d1.push({ label: "Поиск", data: res.data.search });
        }
        if (res.data.undefined) {
            d1.push({ label: "Источник неизвестен", data: res.data.undefined });
        }

        $.plot("#sources_placeholder", d1, options);

    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
        if (--blocks == 0) {
            unblockUI();
        }
    });
}

function getDefaultSummary() {
    blockUI();
    var blocks = 2;

    getJSON("/api/analytics/traffic/summary/default", {}, function (res) {
        var options = getLineGraphicOptions();
        var d1 = [], d2 = [];
        if (!res.succeed) {
            alert(res.message);
            return;
        }
        $(res.data.trafficSummaryCollection).each(function (index, element) {
            
            var date = new Date(element.endDate);
            // Поправка на московский часовой пояс
            var ms = date.getTime() + 4*60*60*1000;
            d1.push([ms, element.pageViews]);
            d2.push([ms, element.visitors]);
        });
        d1.sort(sortfunction);
        d2.sort(sortfunction);
        $.plot("#placeholder", [
            {
                data: d1,
                lines: {
                    show: true
                },
                curvedLines: {
                    apply: true,
                },
                color: "#AFD8F8",
                hoverable: false,
            },
            {
                data: d1,
                label: "Просмоты",
                points: {
                    show: true
                },
                
                color: "#AFD8F8"
            },
            {
                data: d2,
                curvedLines: {
                    apply: true,
                },
                hoverable: false,
                color: "#ff7518"
            }, {
                data: d2,
                label: "Посетители",
                points: {
                    show: true
                },
                color: "#ff7518"
            }
        ], options);
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
        if (--blocks == 0) {
            unblockUI();
        }
    });
    
    getJSON("/api/analytics/sources/summary/default", {}, function (res) {
        var options = getPieGraphicOptions();
        var d1 = [];

        if (res.data.social) {
            d1.push({ label: "Соцсети", data: res.data.social });
        }
        if (res.data.direct) {
            d1.push({ label: "Прямые заходы", data: res.data.direct });
        }
        if (res.data.savedPages) {
            d1.push({ label: "Закладки", data: res.data.savedPages });
        }
        if (res.data.links) {
            d1.push({ label: "Внешние cсылки", data: res.data.links });
        }
        if (res.data.internal) {
            d1.push({ label: "Внутренние переходы", data: res.data.internal });
        }
        if (res.data.mail) {
            d1.push({ label: "Почта", data: res.data.mail });
        }
        if (res.data.ads) {
            d1.push({ label: "Реклама", data: res.data.ads });
        }
        if (res.data.undefined) {
            d1.push({ label: "Источник неизвестен", data: res.data.undefined });
        }
        if (res.data.search) {
            d1.push({ label: "Поиск", data: res.data.search });
        }
        $.plot("#sources_placeholder", d1, options);

    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
        if (--blocks == 0) {
            unblockUI();
        }
    });
}

function getSpecificSummary(date1, date2) {
    blockUI();
    var blocks = 2;
    
    var data1 = date1.format("yyyy/mm/dd");
    var data2 = date2.format("yyyy/mm/dd");
    getJSON("/api/analytics/traffic/summary", { startDate: data1, endDate: data2 }, function (res) {
        var options = getLineGraphicOptions();
        var d1 = [], d2 = [];
        if (!res.succeed) {
            alert(res.message);
            return;
        }
        $(res.data.trafficSummaryCollection).each(function (index, element) {
            var date = new Date(element.endDate);
            // Поправка на московский часовой пояс
            var ms = date.getTime() + 4 * 60 * 60 * 1000;
            d1.push([ms, element.pageViews]);
            d2.push([ms, element.visitors]);
        });
        d1.sort(sortfunction);
        d2.sort(sortfunction);
        $.plot("#placeholder", [
            {
                data: d1,
                lines: {
                    show: true
                },
                curvedLines: {
                    apply: true,
                },
                color: "#AFD8F8",
                hoverable: false,
            },
            {
                data: d1,
                label: "Просмоты",
                points: {
                    show: true
                },

                color: "#AFD8F8"
            },
            {
                data: d2,
                curvedLines: {
                    apply: true,
                },
                hoverable: false,
                color: "#ff7518"
            }, {
                data: d2,
                label: "Посетители",
                points: {
                    show: true
                },
                color: "#ff7518"
            }
        ], options);
        
    })
        .fail(function () {
        smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
    })
        .always(function () {
        unblockUI();
    });
    

    getJSON("/api/analytics/sources/summary", { startDate: data1, endDate: data2 }, function (res) {
        var options = getPieGraphicOptions();
        var d1 = [];

        if (res.data.social) {
            d1.push({ label: "Соцсети", data: res.data.social });
        }
        if (res.data.direct) {
            d1.push({ label: "Прямые заходы", data: res.data.direct });
        }
        if (res.data.savedPages) {
            d1.push({ label: "Закладки", data: res.data.savedPages });
        }
        if (res.data.links) {
            d1.push({ label: "Внешние cсылки", data: res.data.links });
        }
        if (res.data.internal) {
            d1.push({ label: "Внутренние переходы", data: res.data.internal });
        }
        if (res.data.mail) {
            d1.push({ label: "Почта", data: res.data.mail });
        }
        if (res.data.ads) {
            d1.push({ label: "Реклама", data: res.data.ads });
        }
        if (res.data.search) {
            d1.push({ label: "Поиск", data: res.data.search });
        }
        if (res.data.undefined) {
            d1.push({ label: "Источник неизвестен", data: res.data.undefined });
        }
        
        $.plot("#sources_placeholder", d1, options);

    })
        .fail(function () {
        smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
    })
        .always(function () {
        if (--blocks == 0) {
            unblockUI();
        }
    });
}

function onAnalyticsLoaded() {
    //Get type of object:
    //alert(Object.prototype.toString.call(_object_).replace(/^\[object (.+)\]$/, "$1").toLowerCase());

    getDefaultSummary();


    $("<div id='tooltip'></div>").css({
        position: "absolute",
        display: "none",
        border: "1px solid #fdd",
        padding: "2px",
        "background-color": "#fee",
        opacity: 0.80
    }).appendTo("body");


    $("#placeholder").bind("plothover", function(event, pos, item) {
        if (item) {
            var x = item.datapoint[0],
                y = item.datapoint[1];
            var date = new Date(x);
            $("#tooltip").html(item.series.label + " " + date.format("dd.mm.yyyy") + ": <strong>" + y + "</strong>")
                .css({ top: item.pageY + 5, left: item.pageX + 5 })
                .fadeIn(200);
        } else {
            $("#tooltip").hide();
        }
    }
    );

    //$("#placeholder").bind("plotclick", function (event, pos, item) {
    //    if (item) {
    //        $("#clickdata").text(" - click point " + item.dataIndex + " in " + item.series.label);
    //        plot.highlight(item.series, item.datapoint);
    //    }
    //});

    $("#adminContent").on("click", "#today", function() {
        var todayTemp = new Date();
        var today = new Date(todayTemp.getFullYear(), todayTemp.getMonth(), todayTemp.getDate(), 0, 0, 0, 0);
        getSingleDaySummary(today);
    });

    $("#adminContent").on("click", "#yesterday", function() {
        var todayTemp = new Date();
        var today = new Date(todayTemp.getFullYear(), todayTemp.getMonth(), todayTemp.getDate(), 0, 0, 0, 0);
        var yesterday = new Date((new Date()).setDate(today.getDate() - 1));
        getSingleDaySummary(yesterday);
    });

    $("#adminContent").on("click", "#week", function() {
        var todayTemp = new Date();
        var today = new Date(todayTemp.getFullYear(), todayTemp.getMonth(), todayTemp.getDate(), 0, 0, 0, 0);
        var weekAgo = new Date((new Date()).setDate(today.getDate() - 7));
        getSpecificSummary(weekAgo, today);
    });

    $("#adminContent").on("click", "#month", function() {
        var todayTemp = new Date();
        var today = new Date(todayTemp.getFullYear(), todayTemp.getMonth(), todayTemp.getDate(), 0, 0, 0, 0);
        var monthAgo = new Date((new Date()).setDate(today.getDate() - numberOfDays(today.getFullYear(), today.getMonth())));
        getSpecificSummary(monthAgo, today);
    });

    //$("#adminContent").on("click", "#year", function() {
    //    var todayTemp = new Date();
    //    var today = new Date(todayTemp.getFullYear(), todayTemp.getMonth(), todayTemp.getDate(), 0, 0, 0, 0);
    //    var dayCount = isLeap(todayTemp.getFullYear()) ? 366 : 365;
    //    var yearAgo = new Date((new Date()).setDate(today.getDate() - dayCount));
    //    getSpecificSummary(yearAgo, today);
    //});

    $("#adminContent").on("click", "#quarter", function() {
        var todayTemp = new Date();
        var today = new Date(todayTemp.getFullYear(), todayTemp.getMonth(), todayTemp.getDate(), 0, 0, 0, 0);
        var dayCount = 90;
        var yearAgo = new Date((new Date()).setDate(today.getDate() - dayCount));
        getSpecificSummary(yearAgo, today);
    });

    $("#adminContent").on("click", "#customRange", function() {
        var options = {
            title: 'Выбор дат',
            modal: true,
            resizable: false,
            width: 300,
            height: 200,
            draggable: false,
            open: function () {
                var now = new Date();
                $.datepicker.setDefaults($.datepicker.regional["ru"]);
                $(".analytics-begin-date").datepicker({
                    defaultDate: "-1w",
                    dateFormat: "dd.mm.yy",
                    changeMonth: true,
                    numberOfMonths: 1,
                    maxDate: now,
                    onClose: function (selectedDate) {
                        $(".analytics-end-date").datepicker("option", "minDate", selectedDate);
                        $(".analytics-end-date")[0].focus();
                    }
                });
                $(".analytics-end-date").datepicker({
                    changeMonth: true,
                    numberOfMonths: 1,
                    dateFormat: "dd.mm.yy",
                    maxDate: now,
                    onClose: function (selectedDate) {
                        $(".analytics-begin-date").datepicker("option", "maxDate", selectedDate);
                    }
                });
                $(".analytics-begin-date")[0].blur();
                $(".analytics-begin-date")[0].focus();
            },
            buttons: [
                {
                    text: "Выбрать",
                    click: function () {

                        var startDate = $(".analytics-begin-date").datepicker("getDate");
                        var endDate = $(".analytics-end-date").datepicker("getDate");
                        getSpecificSummary(startDate, endDate);
                        $(this).dialog("close");
                    }
                },
                {
                    text: "Отмена",
                    click: function() {
                        $(this).dialog("close");
                    }
                }
            ]
        };
        showDialogFromUrl("/Admin/SelectAnalyticsRange", options);
    });

}




