$(document).ready(function () {
    show();

    $("#searchRecord").click(function () {
        var date = $("#searchDate").val();
        $.ajax({
            url: '/EndDayReport/GetAllSelectDate',
            type: 'GET',
            data: {
                _date: date
            },
            success: function (response) {
                if ($("#reportTable tbody").length > 0) {
                    var dataset = [];
                    response.forEach((element, index) => {
                        var m = new Date(element.updatedDate);
                        m.setHours(m.getHours() + 3);
                        var dateString =
                            ("0" + m.getUTCDate()).slice(-2) + "." +
                            ("0" + (m.getUTCMonth() + 1)).slice(-2) + "." +
                            m.getUTCFullYear() + " " +
                            ("0" + m.getUTCHours()).slice(-2) + ":" +
                            ("0" + m.getUTCMinutes()).slice(-2) + ":" +
                            ("0" + m.getUTCSeconds()).slice(-2);

                        var row = [];
                        var row = [element.caseName.toString(),
                        element.totalPayMoney.toString() + " TL",
                        element.totalTransfer.toString() + " TL",
                        element.totalWithdraw.toString() + " TL",
                        element.totalCommission.toString() + " TL",
                        element.totalInboundTransfer.toString() + " TL",
                        element.totalOutgoingTransfer.toString() + " TL",
                        element.totalBalance.toString() + " TL",
                        element.totalProcessPrice.toString() + " TL",
                        element.totalProcessNumber.toString()
                        ];

                        dataset.push(row);
                    });
                    table = $("#reportTable").DataTable();
                    table.rows().remove().draw();
                    table.rows.add(dataset).draw();
                }

            },
            error: function (response) {
            }
        });
    });
});

function show() {
    $.ajax({
        url: '/EndDayReport/GetAll/',
        success: function (data) {
            var dataset = [];
            data.forEach((d, i) => {
                var row = [d.caseName.toString(),
                d.totalPayMoney.toString() + " TL",
                d.totalTransfer.toString() + " TL",
                d.totalWithdraw.toString() + " TL",
                d.totalCommission.toString() + " TL",
                d.totalInboundTransfer.toString() + " TL",
                d.totalOutgoingTransfer.toString() + " TL",
                d.totalBalance.toString() + " TL",
                d.totalProcessPrice.toString() + " TL",
                d.totalProcessNumber.toString()
                ];
                dataset.push(row);
            });

            if ($("#reportTable tbody").length > 0) {
                table.rows().remove().draw();
                table.rows.add(dataset).draw();
            }
            else {
                $.noConflict();
                table = $('#reportTable').DataTable({
                    "scrollX": true,
                    buttons: [
                        'copy', 'csv', 'excel', 'pdf', 'print'
                    ],
                    "order": [[0, 'asc']],
                    data: dataset,
                    columns: [
                        { title: 'Kasa Adı' },
                        { title: 'Yatırım' },
                        { title: 'Devir' },
                        { title: 'Çekim' },
                        { title: 'Komisyon' },
                        { title: 'Gelen Transfer' },
                        { title: 'Giden Transfer' },
                        { title: 'Kalan' },
                        { title: 'İşlem Ücreti' },
                        { title: 'İşlem Adedi' }
                    ],
                    deferLoading: 57
                });
            }
        }
    });
}