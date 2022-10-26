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

    //setTimeout(function () {
    //    $("#example_wrapper .dt-buttons").append('<button class="btn btn-success btn-sm" onclick="location.href=' + '"<%: Url.Action("OperationCreate", "Home") %>"' + '"><i class="ft-plus white"></i> Ekle</button>');
    //}, 100);
});