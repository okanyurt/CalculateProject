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

    $('#caseId').change(function () {
        var Id = parseInt($('#caseId').val());
        $.ajax({
            url: '/Operation/GetAccount/' + Id,
            success: function (data) {
                var items = '<option value="">Lütfen bir hesap seçiniz</option>';
                $.each(data, function (i, account) {
                    items += "<option value='" + account.id + "'>" + account.name + "</option>";
                });
                $('#accountId').html(items);
            }
        });
    });


    $('#editCaseId').change(function () {
        var Id = parseInt($('#editCaseId').val());
        $.ajax({
            url: '/Operation/GetAccount/' + Id,
            success: function (data) {
                var items = '<option value="">Lütfen bir hesap seçiniz</option>';
                $.each(data, function (i, account) {
                    items += "<option value='" + account.id + "'>" + account.name + "</option>";
                });
                $('#editAccountId').html(items);
            }
        });
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

function edit(id) {
    $.ajax({
        url: '/Operation/OperationEdit/' + id,
        success: function (editdata) {

            $('#editPopup').modal('show');

            $("#editId").val(editdata.id);
            $("#editCaseId").val(editdata.caseId);
            $("#editProcessNumber").val(editdata.processNumber);
            $("#editAccountId").val(editdata.accountId);
            $("#editAccountDetailId").val(editdata.accountDetailId);
            $("#editProcessTypeId").val(editdata.processTypeId);
            $("#editPrice").val(editdata.price);
            $("#editProcessPrice").val(editdata.processPrice);

            var Id = parseInt($('#editCaseId').val());

            $.ajax({
                url: '/Operation/GetAccount/' + Id,
                success: function (data) {
                    var items = '<option value="">Lütfen bir hesap seçiniz</option>';
                    $.each(data, function (i, account) {
                        items += "<option value='" + account.id + "'>" + account.name + "</option>";
                    });
                    $('#editAccountId').html(items);
                    $("#editAccountId").val(editdata.accountId);

                    var Id = parseInt($('#editAccountId').val());

                    $.ajax({
                        url: '/Operation/GetBank/' + Id,
                        success: function (data) {
                            var items = '<option>Lütfen bir banka seçiniz</option>';
                            $.each(data, function (i, bank) {
                                items += "<option value='" + bank.value + "'>" + bank.text + "</option>";
                            });
                            $('#editAccountDetailId').html(items);

                            $("#editAccountDetailId").val(editdata.accountDetailId);

                        }
                    });
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

function UploadFile() {
    var excelFile = document.getElementById('fileUpload');
    formData = new FormData();

    for (var i = 0; i < excelFile.files.length; i++) {
        var file = excelFile.files[i];
        formData.append("excelFile", file);
    }
    $.ajax({
        type: "POST",
        url: "/Operation/uploadData",
        data: formData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            }
            else {
                toastr.error(data.message);
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    });
}