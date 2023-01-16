jQuery(document).ready(function () {
    console.log("Testing Table-List Datatable");
    var oTable = $('#table-list').DataTable({
        "dom": "<'row'<'col-sm-12 col-md-3'f>><tr><'col-sm-12 col-md-2'l><'col-sm-12 col-md-4'i><'col-sm-12 col-md-6'p>",
        "bServerSide": true,
        "bProcessing": true,
        'autoWidth': false,
        'order': [[2, 'desc']],
        'iDisplayLength': 10,
        'lengthMenu': [10, 25, 50, 75, 100],
        'oLanguage': {
            'sSearch': ''
        },
        "ajax": {
            'url': urlDataTable,
            'data': function (d) {
                return {
                    target: 'news',
                    search: d.search.value,
                    length: d.length,
                    start: d.start,
                    orderCol: d.order[0].column,
                    orderType: d.order[0].dir,
                };
            }
        },
        "aoColumns": [
            {
                'mData': function (s) {
                    return '';
                }, 'bSortable': false, sClass: 'text-center'
            },
            {
                'mData': 'news_title',
                'mRender': function (data, type, full) {
                    return '<a class="" href="'+ full.urlDetail +'">' + data + '</a>';
                }
            },
            { 'mData': 'news_publishdate', 'bSortable': false },
            //{ 'mData': 'news_ispublish' },
            //{ 'mData': 'userEmail' },
            //{ 'mData': 'userPhone' },

            //{
            //    'mData': function (s) {
            //        return '<a class="btn btn-sm btn-block blue" href="' + s.urlDetail + '">Detail</a>';
            //    }, 'bSortable': false, sClass: 'text-center'
            //}
        ],
        rowCallback: function (row, data, index) {
            var dtInfo = oTable.page.info();

            var arrTd = $(row).children();
            $(arrTd[0]).html(dtInfo.start + index + 1);

            return row;
        },
        fnDrawCallback: function (oSettings) {
            //$('.tooltips').tooltip();
        }
    });
});