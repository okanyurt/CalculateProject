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


    $('#AccountId').change(function () {
        var Id = parseInt($('#AccountId').val());
        $.ajax({
            url: '/Operation/GetBank/' + Id,
            success: function (data) {
                var items = '<option>Lütfen bir banka seçiniz</option>';
                $.each(data, function (i, bank) {
                    items += "<option value='" + bank.value + "'>" + bank.text + "</option>";
                });
                $('#AccountDetailId').html(items);
            }
        });
    });

    $('#EditAccountId').change(function () {
        var Id = parseInt($('#EditAccountId').val());
        $.ajax({
            url: '/Operation/GetBank/' + Id,
            success: function (data) {
                var items = '<option>Lütfen bir banka seçiniz</option>';
                $.each(data, function (i, bank) {
                    items += "<option value='" + bank.value + "'>" + bank.text + "</option>";
                });
                $('#EditAccountDetailId').html(items);
            }
        });
    });
});

function edit(Id) {
    $.ajax({
        url: '/Operation/OperationEdit/' + Id,
        success: function (editdata) {

            $('#editPopup').modal('show');

            var Id = parseInt($('#EditAccountId').val());

            $.ajax({
                url: '/Operation/GetBank/' + Id,
                success: function (data) {
                    var items = '<option>Lütfen bir banka seçiniz</option>';
                    $.each(data, function (i, bank) {
                        items += "<option value='" + bank.value + "'>" + bank.text + "</option>";
                    });
                    $('#EditAccountDetailId').html(items);

                    $("#EditId").val(editdata.id);
                    $("#EditProcessNumber").val(editdata.processNumber);
                    $("#EditAccountId").val(editdata.accountId);
                    $("#EditAccountDetailId").val(editdata.accountDetailId);
                    $("#EditProcessTypeId").val(editdata.processTypeId);
                    $("#EditPrice").val(editdata.price);
                    $("#EditProcessPrice").val(editdata.processPrice);
                }
            });           
        }
    });
}

function Update() {
    var OperationUpdate = {
        Id: parseInt($("#EditId").val()),
        ProcessNumber: parseInt($("#EditProcessNumber").val()),
        AccountId: parseInt($("#EditAccountId").val()),
        AccountDetailId: parseInt($("#EditAccountDetailId").val()),
        ProcessTypeId: parseInt($("#EditProcessTypeId").val()),
        Price: parseFloat($("#EditPrice").val()),
        ProcessPrice: parseFloat($("#EditProcessPrice").val())
    };

    $.ajax({
        url: '/Operation/OperationEdit',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(OperationUpdate),
        success: function (response) {
            window.location.href = response.redirectToUrl;
        },
        error: function (response) {
            window.location.href = response.redirectToUrl;
        }
    });
}