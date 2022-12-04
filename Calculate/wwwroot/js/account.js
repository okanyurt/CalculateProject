$(document).ready(function () {
    $.noConflict();
    $('#caseTable').DataTable({
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
        url: '/Account/GetById/' + id,
        success: function (editdata) {
            $('#createPopup').modal('show');
            $("#Name").val(editdata.name);
            $("#phoneNumber").val(editdata.phoneNumber);
            $("#identityNumber").val(editdata.identityNumber);
            $("#note").val(editdata.note);
            $("#caseId").val(editdata.caseId);
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
    document.getElementById("errorName").innerHTML = "";
    document.getElementById("errorphoneNumber").innerHTML = "";
    document.getElementById("erroridentityNumber").innerHTML = "";
    document.getElementById("errornote").innerHTML = "";
    document.getElementById("errorcaseId").innerHTML = "";

    if ($("#Id").val() == 0) {
        var AccountCreate = {
            Name: $("#Name").val(),
            PhoneNumber: $("#phoneNumber").val(),
            IdentityNumber: $("#identityNumber").val(),
            Note: $("#note").val(),
            CaseId: $("#caseId").val()
        };

        if (validation("caseId") &&
            validation("Name") &&
            validation("phoneNumber") &&
            validation("identityNumber") &&
            validation("note")        
        ) {
            $.ajax({
                url: '/Account/AccountCreate',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(AccountCreate),
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
    var AccountUpdate = {
        Id: id,
        Name: $("#Name").val(),
        PhoneNumber: $("#phoneNumber").val(),
        IdentityNumber: $("#identityNumber").val(),
        Note: $("#note").val(),
        CaseId: $("#caseId").val()
    };

    if (validation("caseId") &&
        validation("Name") &&
        validation("phoneNumber") &&
        validation("identityNumber") &&
        validation("note")
    ) {
        $.ajax({
            url: '/Account/AccountEdit',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(AccountUpdate),
            success: function (response) {
                if (response.isSuccess) {
                    toastr.success("İşlem başarılı.");
                    $('#createPopup').modal('toggle');
                    setTimeout(function () {
                        window.location.reload();
                        $("#save").attr('disabled', false);
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
            url: '/Account/AccountDelete/' + id,
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