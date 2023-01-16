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
            'sSearch': ''
        },
        "ajax": {
            'url': urlDataTable,
            'data': function (d) {
                return {
                    target: 'customer',
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
                'mData': 'customer_name',
                'mRender': function (data,type,full) {
                    return '<a class="" href="' + full.urlDetail+ '">' + data + '</a>';
                }
            },
            { 'mData': 'customer_code' },
            { 'mData': 'customer_email' },
            { 'mData': 'customer_phone_number' },
            { 'mData': 'customer_address' },
            //{
            //    'mData': function (s) {
            //        return '<a class="btn btn-padd3 red" data-toggle="modal" href="#confirmDelete" data-id="' + s.customer_id + '">Delete</a> <a class="btn btn-padd3 green" href="' + s.urlDetail + '">Edit</a> ';
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

    $('[data-toggle="popover"]').popover();
    $('.date-picker').datepicker({
        orientation: "right",
        autoclose: true
    });

    $('input[name=to]').change(function () {
        toDate = $(this).val();
        $('#table-list').DataTable().draw();
    });

    $('input[name=from]').change(function () {
        fromDate = $(this).val();
        console.log(fromDate);
    });
});