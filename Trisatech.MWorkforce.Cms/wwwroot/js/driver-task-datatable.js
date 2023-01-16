jQuery(document).ready(function () {
    var fromDate = currentDate;
    var toDate = currentDate;
    var status = null;
    var oTable = $('#datatable_default').DataTable({
        "dom": "<'row'<'col-sm-12 col-md-3'f>><tr><'col-sm-12 col-md-2'l><'col-sm-12 col-md-4'i><'col-sm-12 col-md-6'p>",
        "bServerSide": true,
        "bProcessing": true,
        'autoWidth': false,
        'order': [[0, 'desc']],
        'iDisplayLength': 100,
        'lengthMenu': [10, 25, 50, 75, 100],
        'oLanguage': {
            'sSearch': ''
        },
        "ajax": {
            'url': urlDataTable,
            'data': function (d) {
                return {
                    target: 'assignment',
                    search: d.search.value,
                    length: d.length,
                    start: d.start,
                    orderCol: d.order[0].column,
                    orderType: d.order[0].dir,
                    startDate: fromDate,
                    endDate: toDate,
                    status: status
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
                'mData': 'assignment_code',
                'mRender': function (data, type, full) {
                    return '<a class="" href="' + full.url_detail + '">' + data + '</a>';
                }
            },
            //{
            //    'mData': function (s) {
            //        return '<a class="" href="' + s.url_detail + '">' +  + '</a>';
            //    }, 'bSortable': false, sClass: 'text-center'
            //},
            { 'mData': 'assignment_name' },
            { 'mData': 'agent_code' },
            { 'mData': 'agent_name' },
            { 'mData': 'assignment_address' },
            { 'mData': 'date' },
            {
                'mData': function (s) {
                    if (s.assignment_status_code === 'TASK_RECEIVED')
                        return '<span class="badge access">Received</span>';
                    else if (s.assignment_status_code === 'AGENT_STARTED')
                        return '<span class="badge badge-default">Started</span>';
                    else if (s.assignment_status_code === 'TASK_COMPLETED')
                        return '<span class="badge badge-success">Completed</span>';
                    else
                        return '<span class="badge badge-failed">Failed</span>';
                }, 'bSortable': false, sClass: 'text-center'
            }
            //{
            //    'mData': function (s) {
            //        return '<a class="btn btn-block btn-sm blue" href="' + s.url_detail + '">Detail</a>';
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

    $('#datatable_default_wrapper'); // datatable creates the table wrapper by adding with id {your_table_jd}_wrapper

    // initialize select2 dropdown
    //tableWrapper.find('.dataTables_length select').select2();

    // Filter Table
    //tableWrapper.find('.dataTables_length').append(filterTable);

    $('[data-toggle="popover"]').popover();
    $('.date-picker').datepicker({
        orientation: "right",
        autoclose: true
    });
    //----------------------------------------------------------------------------------------------------

    $(function () {

        $('input[name="daterange"]').daterangepicker({
            autoUpdateInput: false,
            locale: {
                cancelLabel: 'Clear'
            },
            opens: "left"
        });

        $('input[name="daterange"]').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));

            if (fromDate !== picker.startDate.format('MM/DD/YYYY') && toDate !== picker.endDate.format('MM/DD/YYYY')) {
                fromDate = picker.startDate.format('MM/DD/YYYY');
                toDate = picker.endDate.format('MM/DD/YYYY');

                $('#datatable_default').DataTable().draw();
            }
        });

        $('input[name="daterange"]').on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
        });
    });

    $.fn.select2.defaults;
    $('#statuspicker').select2({

    });
    $('#statuspicker').select2().on("change", function (e) {
        var obj = $("#statuspicker").select2("data");
        console.log("change val=" + obj[0].id);
        if (obj[0].id !== "null" && obj[0].id !== status) {
            status = obj[0].id;
            $('#datatable_default').DataTable().draw();
        } else {
            status = null;
            $('#datatable_default').DataTable().draw();
        }
    });
});