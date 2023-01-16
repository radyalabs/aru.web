var Dashboard = function() {

    var initChartSample1 = function() {
        var chart = AmCharts.makeChart("chart_1", {
          "type": "serial",
          "theme": "light",
          "pathToImages": "assets/plugins/amcharts/amcharts/images/",
          "autoMargins": false,
          "marginLeft": 30,
          "marginRight": 0,
          "marginTop": 10,
          "marginBottom": 26,
          "valueAxes": [{
            "id": "v1",
            "stackType": "none",
            "title": "",
            "position": "left",
            "autoGridCount": false,
            "labelFunction": function(value) {
              return Math.round(value);
            }
          }],
          "graphs": [{
            "id": "graph_line1",
            "valueAxis": "v1",
            "bullet": "round",
            "bulletBorderAlpha": 1,
            "bulletColor": "#FFFFFF",
            "bulletSize": 8,
            "hideBulletsCount": 50,
            "lineThickness": 3,
            "lineColor": "#3C599C",
            "type": "smoothedLine",
            "title": "Facebook",
            "useLineColorForBulletBorder": true,
            "valueField": "fb",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          },{
            "id": "graph_line2",
            "valueAxis": "v1",
            "bullet": "round",
            "bulletBorderAlpha": 1,
            "bulletColor": "#FFFFFF",
            "bulletSize": 8,
            "hideBulletsCount": 50,
            "lineThickness": 3,
            "lineColor": "#50AE54",
            "type": "smoothedLine",
            "title": "Line Messenger",
            "useLineColorForBulletBorder": true,
            "valueField": "line",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          },{
            "id": "graph_line3",
            "valueAxis": "v1",
            "bullet": "round",
            "bulletBorderAlpha": 1,
            "bulletColor": "#FFFFFF",
            "bulletSize": 8,
            "hideBulletsCount": 50,
            "lineThickness": 3,
            "lineColor": "#c0392b",
            "type": "smoothedLine",
            "title": "Web Chat",
            "useLineColorForBulletBorder": true,
            "valueField": "webchat",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          },{
            "id": "graph_line4",
            "valueAxis": "v1",
            "bullet": "round",
            "bulletBorderAlpha": 1,
            "bulletColor": "#FFFFFF",
            "bulletSize": 8,
            "hideBulletsCount": 50,
            "lineThickness": 3,
            "lineColor": "#2CA1DF",
            "type": "smoothedLine",
            "title": "Telegram",
            "useLineColorForBulletBorder": true,
            "valueField": "telegram",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          }],
          "chartCursor": {
            "pan": true,
            "valueLineEnabled": true,
            "valueLineBalloonEnabled": true,
            "cursorAlpha": 0,
            "valueLineAlpha": 0.2
          },
          "categoryField": "month",
          "categoryAxis": {
            "dashLength": 1,
            "minorGridEnabled": true
          },
          "legend": {
            "useGraphSettings": true,
            "position": "top",
            "autoMargins": false,
            "marginTop": 0,
            "align": "center"
          },
          "balloon": {
            "borderThickness": 1,
            "shadowAlpha": 0.2
          },
          "dataProvider": [{
            "month": "Jan",
            "fb": 5,
            "line": 10,
            "webchat": 6,
            "telegram": 8
          }, {
            "month": "Feb",
            "fb": 4,
            "line": 6,
            "webchat": 10,
            "telegram": 11
          }, {
            "month": "Mar",
            "fb": 5,
            "line": 3,
            "webchat": 4,
            "telegram": 3
          }, {
            "month": "Apr",
            "fb": 8,
            "line": 4,
            "webchat": 10,
            "telegram": 9
          }, {
            "month": "May",
            "fb": 9,
            "line": 12,
            "webchat": 5,
            "telegram": 3
          }, {
            "month": "Jun",
            "fb": 3,
            "line": 4,
            "webchat": 9,
            "telegram": 2
          }, {
            "month": "Jul",
            "fb": 5,
            "line": 7,
            "webchat": 3,
            "telegram": 4
          }, {
            "month": "Aug",
            "fb": 7,
            "line": 3,
            "webchat": 6,
            "telegram": 10
          }, {
            "month": "Sep",
            "fb": 9,
            "line": 5,
            "webchat": 5,
            "telegram": 8
          }, {
            "month": "Oct",
            "fb": 5,
            "line": 4,
            "webchat": 10,
            "telegram": 22
          }, {
            "month": "Nov",
            "fb": 4,
            "line": 2,
            "webchat": 7,
            "telegram": 20
          }, {
            "month": "Des",
            "fb": 3,
            "line": 5,
            "webchat": 9,
            "telegram": 12
          }]
        });
    }
    var initChartSample2 = function() {
        var chart = AmCharts.makeChart("chart_2", {
          "type": "serial",
          "theme": "light",
          "pathToImages": "assets/plugins/amcharts/amcharts/images/",
          "autoMargins": false,
          "marginLeft": 45,
          "marginRight": 0,
          "marginTop": 15,
          "marginBottom": 30,
          "valueAxes": [{
            "id": "v1",
            "stackType": "regular",
            "title": "Total User",
            "position": "left",
            "autoGridCount": false,
            "labelFunction": function(value) {
              return Math.round(value);
            }
          }],
          "graphs": [ {
            "id": "graph_column1",
            "valueAxis": "v1",
            "lineColor": "#3C599C",
            "fillColors": "#3C599C",
            "fillAlphas": 1,
            "type": "column",
            "title": "Facebook",
            "valueField": "fb",
            "clustered": true,
            "columnWidth": 0.8,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          }, {
            "id": "graph_column2",
            "valueAxis": "v1",
            "lineColor": "#50AE54",
            "fillColors": "#50AE54",
            "fillAlphas": 1,
            "type": "column",
            "title": "Line Messenger",
            "valueField": "line",
            "clustered": true,
            "columnWidth": 0.8,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          }, {
            "id": "graph_column3",
            "valueAxis": "v1",
            "lineColor": "#c0392b",
            "fillColors": "#c0392b",
            "fillAlphas": 1,
            "type": "column",
            "title": "Web Chat",
            "valueField": "webchat",
            "clustered": true,
            "columnWidth": 0.8,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          }, {
            "id": "graph_column4",
            "valueAxis": "v1",
            "lineColor": "#2CA1DF",
            "fillColors": "#2CA1DF",
            "fillAlphas": 1,
            "type": "column",
            "title": "Telegram",
            "valueField": "telegram",
            "clustered": true,
            "columnWidth": 0.8,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          }],
          "chartCursor": {
            "pan": true,
            "valueLineEnabled": true,
            "valueLineBalloonEnabled": false,
            "cursorAlpha": 0,
            "valueLineAlpha": 0.2
          },
          "categoryField": "month",
          "categoryAxis": {
            "dashLength": 1,
            "minorGridEnabled": true
          },
          "legend": {
            "useGraphSettings": false,
            "position": "top",
            "autoMargins": false,
            "marginTop": 0,
            "align": "center"
          },
          "balloon": {
            "borderThickness": 1,
            "shadowAlpha": 0.2
          },
          "dataProvider": [{
            "month": "Jan",
            "fb": 20,
            "line": 5,
            "webchat": 6,
            "telegram": 8
          }, {
            "month": "Feb",
            "fb": 10,
            "line": 12,
            "webchat": 9,
            "telegram": 15
          }, {
            "month": "Mar",
            "fb": 15,
            "line": 10,
            "webchat": 25,
            "telegram": 15
          }, {
            "month": "Apr",
            "fb": 35,
            "line": 6,
            "webchat": 28,
            "telegram": 4
          }, {
            "month": "May",
            "fb": 40,
            "line": 15,
            "webchat": 35,
            "telegram": 30
          }, {
            "month": "Jun",
            "fb": 12,
            "line": 10,
            "webchat": 4,
            "telegram": 10
          }, {
            "month": "Jul",
            "fb": 20,
            "line": 8,
            "webchat": 15,
            "telegram": 12
          }, {
            "month": "Aug",
            "fb": 15,
            "line": 4,
            "webchat": 25,
            "telegram": 20
          }, {
            "month": "Sep",
            "fb": 25,
            "line": 6,
            "webchat": 4,
            "telegram": 2
          }, {
            "month": "Oct",
            "fb": 30,
            "line": 7,
            "webchat": 10,
            "telegram": 11
          }, {
            "month": "Nov",
            "fb": 40,
            "line": 3,
            "webchat": 20,
            "telegram": 10
          }, {
            "month": "Des",
            "fb": 10,
            "line": 4,
            "webchat": 22,
            "telegram": 45
          }]
        });
    }

    var initChartSample3 = function() {
      var data = [{label: "On going", data: [8], color: "#43a2d3"}, 
                  {label: "Finished", data: [40], color: "#80b857"},
                  {label: "Cancelled", data: [6], color: "#f0c242"},
                  {label: "Failed", data: [3], color: "#ef732e"}];

      // GRAPH 5
      if ($('#pie_chart').size() !== 0) {
          $.plot($("#pie_chart"), data, {
              series: {
                  pie: {
                      show: true,
                      radius: 1,
                      label: {
                          show: true,
                          radius: 3 / 4,
                          formatter: function(label, series) {
                              return '<div style="font-size:8pt; text-align:center; padding:4px 10px; color:white;">' + Math.round(series.percent) + ' %</div>';
                          },
                          background: {
                              opacity: 0.3,
                              color: '#000'
                          }
                      }
                  }
              },
              grid: {
                  hoverable: true
              }                
          });
      }
    }

    var initChartSample4 = function() {
        var chart = AmCharts.makeChart("chart_4", {
          "type": "serial",
          "theme": "light",
          "pathToImages": "assets/plugins/amcharts/amcharts/images/",
          "autoMargins": false,
          "marginLeft": 30,
          "marginRight": 0,
          "marginTop": 15,
          "marginBottom": 30,
          "valueAxes": [{
            "id": "v1",
            "stackType": "regular",
            "title": "Jumlah Appointment",
            "position": "left",
            "autoGridCount": false,
            "labelFunction": function(value) {
              return Math.round(value);
            }
          }],
          "graphs": [ {
            "id": "graph_column1",
            "valueAxis": "v1",
            "lineColor": "#43a2d3",
            "fillColors": "#43a2d3",
            "fillAlphas": 1,
            "type": "column",
            "title": "On going",
            "valueField": "ongoing",
            "clustered": false,
            "columnWidth": 0.5,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          },{
            "id": "graph_column2",
            "valueAxis": "v1",
            "lineColor": "#80b857",
            "fillColors": "#80b857",
            "fillAlphas": 1,
            "type": "column",
            "title": "Finished",
            "valueField": "finished",
            "clustered": false,
            "columnWidth": 0.5,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          },{
            "id": "graph_column3",
            "valueAxis": "v1",
            "lineColor": "#f0c242",
            "fillColors": "#f0c242",
            "fillAlphas": 1,
            "type": "column",
            "title": "Cancelled",
            "valueField": "cancelled",
            "clustered": false,
            "columnWidth": 0.5,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          },{
            "id": "graph_column4",
            "valueAxis": "v1",
            "lineColor": "#ef732e",
            "fillColors": "#ef732e",
            "fillAlphas": 1,
            "type": "column",
            "title": "Failed",
            "valueField": "failed",
            "clustered": false,
            "columnWidth": 0.5,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          },{
            "id": "graph_line1",
            "valueAxis": "v1",
            "bullet": "round",
            "bulletBorderAlpha": 2,
            "bulletColor": "#FFFFFF",
            "bulletSize": 8,
            "hideBulletsCount": 50,
            "lineThickness": 3,
            "lineColor": "#34495e",
            "type": "smoothedLine",
            "useLineColorForBulletBorder": true,
            "title": "Total Appointment",
            "valueField": "total",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          }],
          "chartCursor": {
            "pan": true,
            "valueLineEnabled": true,
            "valueLineBalloonEnabled": false,
            "cursorAlpha": 0,
            "valueLineAlpha": 0.2
          },
          "categoryField": "sales",
          "categoryAxis": {
            "dashLength": 1,
            "minorGridEnabled": true
          },
          "legend": {
            "useGraphSettings": true,
            "position": "top",
            "autoMargins": false,
            "marginTop": 0,
            "align": "center"
          },
          "balloon": {
            "borderThickness": 1,
            "shadowAlpha": 0.2
          },
          "dataProvider": [{
            "sales": "Salesperson 1",
            "ongoing": 20,
            "finished": 5,
            "cancelled": 8,
            "failed": 3,
            "total": 36
          }, {
            "sales": "Salesperson 2",
            "ongoing": 10,
            "finished": 12,
            "cancelled": 4,
            "failed": 1,
            "total": 27
          }, {
            "sales": "Salesperson 3",
            "ongoing": 15,
            "finished": 10,
            "cancelled": 8,
            "failed": 7,
            "total": 40
          }, {
            "sales": "Salesperson 4",
            "ongoing": 35,
            "finished": 6,
            "cancelled": 2,
            "failed": 9,
            "total": 52
          }, {
            "sales": "Salesperson 5",
            "ongoing": 40,
            "finished": 15,
            "cancelled": 2,
            "failed": 6,
            "total": 63
          }]
        });
    }

    var initChartSample5 = function() {
        var chart = AmCharts.makeChart("chart_5", {
          "type": "serial",
          "theme": "light",
          "pathToImages": "assets/plugins/amcharts/amcharts/images/",
          "autoMargins": false,
          "marginLeft": 30,
          "marginRight": 0,
          "marginTop": 15,
          "marginBottom": 30,
          "valueAxes": [{
            "id": "v1",
            "stackType": "regular",
            "title": "Jumlah Appointment",
            "position": "left",
            "autoGridCount": false,
            "labelFunction": function(value) {
              return Math.round(value);
            }
          }],
          "graphs": [ {
            "id": "graph_column1",
            "valueAxis": "v1",
            "lineColor": "#43a2d3",
            "fillColors": "#43a2d3",
            "fillAlphas": 1,
            "type": "column",
            "title": "On going",
            "valueField": "ongoing",
            "clustered": false,
            "columnWidth": 0.5,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          },{
            "id": "graph_column2",
            "valueAxis": "v1",
            "lineColor": "#80b857",
            "fillColors": "#80b857",
            "fillAlphas": 1,
            "type": "column",
            "title": "Finished",
            "valueField": "finished",
            "clustered": false,
            "columnWidth": 0.5,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          },{
            "id": "graph_column3",
            "valueAxis": "v1",
            "lineColor": "#f0c242",
            "fillColors": "#f0c242",
            "fillAlphas": 1,
            "type": "column",
            "title": "Cancelled",
            "valueField": "cancelled",
            "clustered": false,
            "columnWidth": 0.5,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          },{
            "id": "graph_column4",
            "valueAxis": "v1",
            "lineColor": "#ef732e",
            "fillColors": "#ef732e",
            "fillAlphas": 1,
            "type": "column",
            "title": "Failed",
            "valueField": "failed",
            "clustered": false,
            "columnWidth": 0.5,
            "legendValueText": "[[value]]",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          },{
            "id": "graph_line1",
            "valueAxis": "v1",
            "bullet": "round",
            "bulletBorderAlpha": 2,
            "bulletColor": "#FFFFFF",
            "bulletSize": 8,
            "hideBulletsCount": 50,
            "lineThickness": 3,
            "lineColor": "#34495e",
            "type": "smoothedLine",
            "useLineColorForBulletBorder": true,
            "title": "Total Appointment",
            "valueField": "total",
            "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
          }],
          "chartCursor": {
            "pan": true,
            "valueLineEnabled": true,
            "valueLineBalloonEnabled": false,
            "cursorAlpha": 0,
            "valueLineAlpha": 0.2
          },
          "categoryField": "sales",
          "categoryAxis": {
            "dashLength": 1,
            "minorGridEnabled": true
          },
          "legend": {
            "useGraphSettings": true,
            "position": "top",
            "autoMargins": false,
            "marginTop": 0,
            "align": "center"
          },
          "balloon": {
            "borderThickness": 1,
            "shadowAlpha": 0.2
          },
          "dataProvider": [{
            "sales": "Salesperson 1",
            "ongoing": 20,
            "finished": 5,
            "cancelled": 8,
            "failed": 3,
            "total": 36
          }, {
            "sales": "Salesperson 2",
            "ongoing": 10,
            "finished": 12,
            "cancelled": 4,
            "failed": 1,
            "total": 27
          }, {
            "sales": "Salesperson 3",
            "ongoing": 15,
            "finished": 10,
            "cancelled": 8,
            "failed": 7,
            "total": 40
          }, {
            "sales": "Salesperson 4",
            "ongoing": 35,
            "finished": 6,
            "cancelled": 2,
            "failed": 9,
            "total": 52
          }, {
            "sales": "Salesperson 5",
            "ongoing": 40,
            "finished": 15,
            "cancelled": 2,
            "failed": 6,
            "total": 63
          }]
        });
    }

    return {
        //main function to initiate the module

        init: function() {
            initChartSample1();
            initChartSample2();
            initChartSample3();
            initChartSample4();
            initChartSample5();
        }

    };

}();