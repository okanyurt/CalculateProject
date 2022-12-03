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

function Save() {
    $("#save").attr('disabled', true);
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