$(document).ready(function () {
    $.noConflict();
    $('#accountDetailTable').DataTable({
        "scrollX": true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        order: [[0, 'desc']]
    });
});

function edit(id) {
    $.ajax({
        url: '/AccountDetail/GetById/' + id,
        success: function (editdata) {
            $('#createPopup').modal('show');
            $("#accountId").val(editdata.accountId);
            $("#bankId").val(editdata.bankId);
            $("#iban").val(editdata.iban);
            $("#bankAccountNumber").val(editdata.bankAccountNumber);
            $("#Id").val(id);
        }
    });
}

function Save() {
    $("#save").attr('disabled', true);
    if ($("#Id").val() == 0) {
        var AccountDetailCreate = {
             AccountId: $("#accountId").val(),
             BankId: $("#bankId").val(),
             IbanNumber: $("#iban").val(),
             BankAccountNumber: $("#bankAccountNumber").val()
        };

        $.ajax({
            url: '/AccountDetail/AccountDetailCreate',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(AccountDetailCreate),
            success: function (response) {
                if (response.isSuccess) {
                    toastr.success("İşlem başarılı.");
                    $('#createPopup').modal('toggle');

                    setTimeout(function () {
                        $("#save").attr('disabled', false);
                        window.location.reload();
                    }, 1000);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (response) {
                $("#save").attr('disabled', false);
            }
        });
    }
    else {
        Update($("#Id").val());
    }

}

function Update(id) {
    var AccountDetailUpdate = {
        Id: id,
        AccountId: $("#accountId").val(),
        BankId: $("#bankId").val(),
        IbanNumber: $("#iban").val(),
        BankAccountNumber: $("#bankAccountNumber").val()
    };

    $.ajax({
        url: '/AccountDetail/AccountDetailEdit',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(AccountDetailUpdate),
        success: function (response) {
            if (response.isSuccess) {
                toastr.success("İşlem başarılı.");
                $('#createPopup').modal('toggle');
                setTimeout(function () {
                    window.location.reload();
                }, 1000);
            } else {
                toastr.error(response.message);
            }
        },
        error: function (response) {
        }
    });
}

function remove(id) {
    if (confirm("Kayıt silinecektir. Emin misiniz?")) {
        $.ajax({
            url: '/AccountDetail/AccountDetailDelete/' + id,
            success: function (data) {
                if (data.isSuccess) {
                    toastr.success("İşlem başarılı.");
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000);
                } else {
                    toastr.error(result.message);
                }
            }
        });
    }
    else {
        return false;
    }
}