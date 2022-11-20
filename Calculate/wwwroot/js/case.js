$(document).ready(function () {
    $.noConflict();
    $('#caseTable').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        order: [[0, 'asc']]        
    });
});

function edit(id,officeId) {  
    $.ajax({
        url: '/Case/GetById/' + id,
        success: function (editdata) {
            $('#createPopup').modal('show');
            $("#caseName").val(editdata.name);
            $("#caseId").val(id);
            $("#officeId").val(officeId);
        }
    });
}

function Save() {
    $("#save").attr('disabled', true);
    if ($("#caseId").val() == 0) {
        var CaseCreate = {
            Name: $("#caseName").val(),
            OfficeId: $("#officeId").val()
        };

        $.ajax({
            url: '/Case/CaseCreate',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(CaseCreate),
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
        Update($("#caseId").val());
    }

}

function Update(id) {
    var CaseUpdate = {
        Id: id,
        Name: $("#caseName").val(),
        OfficeId: $("#officeId").val()
    };

    $.ajax({
        url: '/Case/CaseEdit',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(CaseUpdate),
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
            url: '/Case/CaseDelete/' + id,
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