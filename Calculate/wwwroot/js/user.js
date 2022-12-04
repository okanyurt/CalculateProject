$(document).ready(function () {
    $.noConflict();
    $('#userTable').DataTable({
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
        url: '/User/GetById/' + id,
        success: function (editdata) {
            $('#createPopup').modal('show');
            $("#userName").val(editdata.userName);
            $("#roleId").val(editdata.roleID);
            $("#isEnabledId").val(editdata.isEnabled ? 1 : 0);
            $("#officeId").val(editdata.officeIdList);
            $("#phoneNumber").val(editdata.phoneNumber);
            $("#password").val(editdata.passwordHash);
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
    document.getElementById("erroruserName").innerHTML = "";
    document.getElementById("errorroleId").innerHTML = "";
    document.getElementById("errorisEnabledId").innerHTML = "";
    document.getElementById("errorofficeId").innerHTML = "";
    document.getElementById("errorphoneNumber").innerHTML = "";
    document.getElementById("errorpassword").innerHTML = "";
    if ($("#Id").val() == 0) {
        var isenabled = true;
        if ($("#isEnabledId").val() == 1) {
            isenabled = true;
        }
        else {
            isenabled = false;
        }

        var UserCreate = {
            UserName: $("#userName").val(),
            RoleID: $("#roleId").val(),
            IsEnabled: isenabled,
            officeIdList: $("#officeId").val(),
            PhoneNumber: $("#phoneNumber").val(),
            PasswordHash: $("#password").val()
        };

        if (validation("userName") &&
            validation("roleId") &&
            validation("isEnabledId") &&
            validation("officeId") &&
            validation("phoneNumber") &&
            validation("password")
        ) {
            $.ajax({
                url: '/User/UserCreate',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(UserCreate),
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
    var isenabled = true;
    if ($("#isEnabledId").val() == 1) {
        isenabled = true;
    }
    else {
        isenabled = false;
    }

    var UserUpdate = {
        Id: id,
        UserName: $("#userName").val(),
        RoleID: $("#roleId").val(),
        IsEnabled: isenabled,
        officeIdList: $("#officeId").val(),
        PhoneNumber: $("#phoneNumber").val(),
        PasswordHash: $("#password").val()
    };

    if (validation("userName") &&
        validation("roleId") &&
        validation("isEnabledId") &&
        validation("officeId") &&
        validation("phoneNumber") &&
        validation("password")
    ) {
        $.ajax({
            url: '/User/UserEdit',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(UserUpdate),
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
            url: '/User/UserDelete/' + id,
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