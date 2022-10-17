$(document).ready(function () {
    $.noConflict();
    $('#example').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "createdRow": function (row, data, dataIndex) {
            if (data[2] == "London") {
                $(row).addClass('red');

            }
        }
    });
});