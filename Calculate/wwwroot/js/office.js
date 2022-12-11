$(document).ready(function () {
    $.noConflict();
    $('#officeTable').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        order: [[0, 'asc']]        
    });
});

function edit(id) {    
    $.ajax({
        url: '/Office/GetById/' + id,
        success: function (editdata) {
            $('#createPopup').modal('show');
            $("#officeName").val(editdata.name);
            $("#officeId").val(id);
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
    document.getElementById("errorofficeName").innerHTML = "";
    if ($("#officeId").val() == 0) {
        var OfficeCreate = {
            Name: $("#officeName").val()
        };

        if (validation("officeName")) {
            $.ajax({
                url: '/Office/OfficeCreate',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(OfficeCreate),
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
        Update($("#officeId").val());
    }
    
}

function Update(id) {
    var OfficeUpdate = {
        Id: id,
        Name: $("#officeName").val()
    };

    if (validation("officeName")) {
        $.ajax({
            url: '/Office/OfficeEdit',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(OfficeUpdate),
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
            url: '/Office/OfficeDelete/' + id,
            success: function (data) {
                if (data.isSuccess) {
                    toastr.success("İşlem başarılı.");
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000);
                } else {
                    toastr.error("İşlem başarısız.");
                }
            }
        });
    }
    else {
        return false;
    }
}