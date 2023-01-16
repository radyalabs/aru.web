var AssignmentTable = function () {
    return {
        init: function (datasource, baseurl) {
            var baseUrl = baseurl;
            var dataSource = datasource;
            $('#taskTable').DataTable().destroy();
            var table = $('#taskTable');
            
            var mainRecordTable = table.DataTable({
                "dom": "<'row'<'col-sm-4 col-md-3 'f>><tr><'col-sm-12 col-md-2'l><'col-sm-12 col-md-5'i><'col-sm-12 col-md-5'p>",
                "data": dataSource,
                'iDisplayLength': 10,
                'autoWidth': false,
                'lengthMenu': [10, 25, 50, 75, 100],
                "columns": [
                    { "data": "sales" },
                    { "data": "assignment_date" },
                    { "data": "customer_code" },
                    { "data": "customer_name" },
                    { "data": "no_faktur" },
                    { "data": "amount" },
                    { "data": "address" },
                    { "data": "phone" }
                ]
            });
            
            //var tableWrapper = $('#taskTable_wrapper'); // datatable creates the table wrapper by adding with id {your_table_jd}_wrapper
        },
        fnDrawCallback: function (oSettings) {
            //$('.tooltips').tooltip();
        }
    };
}();
