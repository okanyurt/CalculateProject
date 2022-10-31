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


    $('#accountId').change(function () {
        var Id = parseInt($('#accountId').val());
        $.ajax({
            url: '/Operation/GetBank/' + Id,
            success: function (data) {
                var items = '<option value="">Lütfen bir banka seçiniz</option>';
                $.each(data, function (i, bank) {
                    items += "<option value='" + bank.value + "'>" + bank.text + "</option>";
                });
                $('#accountDetailId').html(items);
            }
        });
    });

    $('#editAccountId').change(function () {
        var Id = parseInt($('#editAccountId').val());
        $.ajax({
            url: '/Operation/GetBank/' + Id,
            success: function (data) {
                var items = '<option value="">Lütfen bir banka seçiniz</option>';
                $.each(data, function (i, bank) {
                    items += "<option value='" + bank.value + "'>" + bank.text + "</option>";
                });
                $('#editAccountDetailId').html(items);
            }
        });
    });
});

function edit(Id) {
    $.ajax({
        url: '/Operation/OperationEdit/' + Id,
        success: function (editdata) {

            $('#editPopup').modal('show');

            var Id = parseInt($('#editAccountId').val());

            $.ajax({
                url: '/Operation/GetBank/' + Id,
                success: function (data) {
                    var items = '<option>Lütfen bir banka seçiniz</option>';
                    $.each(data, function (i, bank) {
                        items += "<option value='" + bank.value + "'>" + bank.text + "</option>";
                    });
                    $('#editAccountDetailId').html(items);

                    $("#editId").val(editdata.id);
                    $("#editProcessNumber").val(editdata.processNumber);
                    $("#editAccountId").val(editdata.accountId);
                    $("#editAccountDetailId").val(editdata.accountDetailId);
                    $("#editProcessTypeId").val(editdata.processTypeId);
                    $("#editPrice").val(editdata.price);
                    $("#editProcessPrice").val(editdata.processPrice);
                }
            });           
        }
    });
}

function Update() {
    var OperationUpdate = {
        Id: parseInt($("#editId").val()),
        ProcessNumber: parseInt($("#editProcessNumber").val()),
        AccountId: parseInt($("#editAccountId").val()),
        AccountDetailId: parseInt($("#editAccountDetailId").val()),
        ProcessTypeId: parseInt($("#editProcessTypeId").val()),
        Price: parseFloat($("#editPrice").val()),
        ProcessPrice: parseFloat($("#editProcessPrice").val())
    };

    $.ajax({
        url: '/Operation/OperationEdit',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(OperationUpdate),
        success: function (response) {
            if (response.isSuccess) {
                toastr.success("İşlem başarılı.");
                $('#editPopup').modal('toggle');
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            } else {
                toastr.error(result.message);
            }
        },
        error: function (response) {
        }
    });
}