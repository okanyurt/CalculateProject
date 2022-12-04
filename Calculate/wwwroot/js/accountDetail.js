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

function validation(id) {
    const inpObj = document.getElementById(id);
    if (!inpObj.checkValidity()) {
        var _id = "error" + id.toString();
        document.getElementById(_id).innerHTML = inpObj.validationMessage;
        return false;
    }
    else {
        return true;
    }
}

function Save() {
    $("#save").attr('disabled', true);
    document.getElementById("erroraccountId").innerHTML = "";
    document.getElementById("errorbankId").innerHTML = "";
    document.getElementById("erroriban").innerHTML = "";
    document.getElementById("errorbankAccountNumber").innerHTML = "";
    if ($("#Id").val() == 0) {
        var AccountDetailCreate = {
             AccountId: $("#accountId").val(),
             BankId: $("#bankId").val(),
             IbanNumber: $("#iban").val(),
             BankAccountNumber: $("#bankAccountNumber").val()
        };

        if (validation("accountId") &&
            validation("bankId") &&
            validation("iban") &&
            validation("bankAccountNumber")
        ) {
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
                        $("#save").attr('disabled', false);
                    }
                },
                error: function (response) {
                    $("#save").attr('disabled', false);
                }
            });
        }
        else {
            $("#save").attr('disabled', false);
        }
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

    if (validation("accountId") &&
        validation("bankId") &&
        validation("iban") &&
        validation("bankAccountNumber")
    ) {
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
                        $("#save").attr('disabled', false);
                        window.location.reload();
                    }, 1000);
                } else {
                    toastr.error(response.message);
                    $("#save").attr('disabled', false);
                }
            },
            error: function (response) {
                $("#save").attr('disabled', false);
            }
        });
    }
    else {
        $("#save").attr('disabled', false);
    }
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