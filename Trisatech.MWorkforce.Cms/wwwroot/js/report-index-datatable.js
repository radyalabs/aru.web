jQuery(document).ready(function () {
    var fromDate = currentDate;
    var oTable = $('#datatable_default').DataTable({
        "dom": "<'row'<'col-sm-12 col-md-3'f>><tr><'col-sm-12 col-md-2'l><'col-sm-12 col-md-4'i><'col-sm-12 col-md-6'p>",
        "bServerSide": true,
        "bProcessing": true,
        'autoWidth': false,
        'order': [[0, 'desc']],
        'iDisplayLength': 5,
        'lengthMenu': [5, 10, 25, 50, 75, 100],
        'oLanguage': {
            'sSearch': ''
        },
        "ajax": {
            'url': urlDataTable,
            'data': function (d) {
                return {
                    target: 'report',
                    search: d.search.value,
                    length: d.length,
                    start: d.start,
                    orderCol: d.order[0].column,
                    orderType: d.order[0].dir,
                    startDate: fromDate
                };
            },
            'error': function (xhr, error, thrown) {
                console.log('error', xhr, error);
                window.alert('Akses ke database terlalu lama, silahkan refresh/reload untuk menampilkan laporannya.');
            }
        },
        "aoColumns": [
            {
                'mData': function (s) {
                    return '';
                }, 'bSortable': false, sClass: 'text-center'
            },
            {
                'mData': 'user_code',
                'mRender': function (data, type, full) {
                    return '<a class="" href="' + full.url_detail + '">' + data + '</a>';
                },'bSortable': false
            },
            { 'mData': 'user_name', 'bSortable': false },
            { 'mData': 'start_time' },
            { 'mData': 'end_time' },
            { 'mData': 'loss_time_at_first_checkin' },
            { 'mData': 'first_checkin_time', 'bSortable': false },
            { 'mData': 'total_work_time', 'bSortable': false },
            { 'mData': 'total_loss_time', 'bSortable': false },
            { 'mData': 'total_time_at_store', 'bSortable': false },
            { 'mData': 'cms_total_payment', 'bSortable': false },
            { 'mData': 'total_task_completed', 'bSortable': false  },
            { 'mData': 'cms_total_kunjungan', 'bSortable': false },
            { 'mData': 'total_verified_task', 'bSortable': false }
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

    $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
        console.log('error datatable', message);
        window.alert('Akses ke database terlalu lama, silahkan refresh/reload untuk menampilkan laporannya.');
    };

    $('#datatable_default_wrapper'); // datatable creates the table wrapper by adding with id {your_table_jd}_wrapper
    
    $('[data-toggle="popover"]').popover();
    $('.date-picker').datepicker({
        orientation: "right",
        autoclose: true
    });

    $('input[name=from]').change(function () {
        if (fromDate !== $(this).val()) {
            console.log('date', fromDate);
            fromDate = $(this).val();
            $('#datatable_default').DataTable().draw();
        }
    });
});