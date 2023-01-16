jQuery(document).ready(function () {
    var oTable = $('#table-list').DataTable({
        "dom": "<'row'<'col-sm-12 col-md-3'f>><tr><'col-sm-12 col-md-2'l><'col-sm-12 col-md-4'i><'col-sm-12 col-md-6'p>",
        "bServerSide": true,
        "bProcessing": true,
        'autoWidth': false,
        'order': [[0, 'desc']],
        'iDisplayLength': 10,
        'lengthMenu': [10, 25, 50, 75, 100],
        'oLanguage': {
            'sSearch': 'Search'
        },
        "ajax": {
            'url': urlDataTable,
            'data': function (d) {
                return {
                    target: 'product',
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
                'mData': 'product_code',
                'mRender': function (data, type, full) {
                    return '<a class="" href="' + full.urlDetail + '">' + data + '</a>';
                }
            },
            { 'mData': 'product_name' },
            { 'mData': 'product_model' },
            { 'mData': 'product_price' },
            //{
            //    'mData': function (s) {
            //        return '<a class="btn btn-sm btn-block green" href="' + s.urlDetail + '">Detail</a> <a class="btn btn-sm btn-block blue" href="' + s.urlEdit + '">Edit</a> <a class="btn btn-sm btn-block red btn-delete" data-toggle="modal" href="#confirmDelete" data-id="' + s.product_id + '">Delete</a>';
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