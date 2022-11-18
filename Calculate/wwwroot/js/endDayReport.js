$(document).ready(function () {
    show();
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
                d.totalBalance.toString() + " TL",
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
                        { title: 'Kalan' },
                        { title: 'İşlem Adedi' }
                    ],
                    deferLoading: 57
                });
            }
        }
    });
}