$(document).ready(function () {
    $.noConflict();
    $('#example').DataTable({
        "scrollX": true,
        lengthMenu: [
            [50, 100, 500, -1],
            [50, 100, 500, 'All'],
        ],
        dom: 'Bfrtip',
        buttons: [
            'pageLength', 'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        order: [[0, 'desc']]
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

    $("#searchRecord").click(function () {
        var date = $("#searchDate").val();
        $.ajax({
            url: '/Operation/GetAllSelectDate',
            type: 'GET',
            data: {
                _date: date
            },
            success: function (response) {
                if ($("#example tbody").length > 0) {
                    var dataset = [];
                    response.forEach((element, index) => {
                        var m = new Date(element.updatedDate);
                        m.setHours(m.getHours() + 3);
                        var dateString =
                            ("0" + m.getUTCDate()).slice(-2) + "." +
                            ("0" + (m.getUTCMonth() + 1)).slice(-2) + "." +
                            m.getUTCFullYear() + " " +
                            ("0" + m.getUTCHours()).slice(-2) + ":" +
                            ("0" + m.getUTCMinutes()).slice(-2) + ":" +
                            ("0" + m.getUTCSeconds()).slice(-2);

                        var row = [];
                        row.push(dateString);
                        row.push(element.caseName);
                        row.push(element.processNumber);
                        row.push(element.account);
                        row.push(element.accountDetail);
                        row.push(element.processType);
                        row.push(element.price);
                        row.push(element.processPrice);
                        row.push("<td><a class='btn btn-warning btn-sm' onclick='edit(" + element.id + ")'><i class='la la-pencil'></i> Güncelle</a></td>");
                        row.push("<td><a class='btn btn-danger btn-sm' onclick='remove(" + element.id + ")'><i class='la la-trash'></i> Sil</a></td>");
                        dataset.push(row);
                    });
                    table = $("#example").DataTable();
                    table.rows().remove().draw();
                    table.rows.add(dataset).draw();
                }

            },
            error: function (response) {
            }
        });
    });

    document.getElementById('fileUpload').addEventListener("change", (event) => {
        let selectedFile;
        let data = [{
            "name": "jayanth",
            "data": "scd",
            "abc": "sdef"
        }]
        console.log(window.XLSX);
        selectedFile = event.target.files[0];
        XLSX.utils.json_to_sheet(data, 'out.xlsx');
        if (selectedFile) {
            let fileReader = new FileReader();
            fileReader.readAsBinaryString(selectedFile);
            fileReader.onload = (event) => {
                let data = event.target.result;
                let workbook = XLSX.read(data, { type: "binary" });
                console.log(workbook);
                workbook.SheetNames.forEach(sheet => {
                    let rowObject = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheet]);
                    console.log(rowObject);
                    var arr = [];
                    rowObject.forEach(function (r) {
                        var item = {};                       
                        var i = 0;
                        for (var key in r) {
                            if (r.hasOwnProperty(key))
                                item[i] = r[key];
                            i++;
                        }

                        var uploadItem = {
                            CaseName: item[9].toString(),
                            ProcessNumber: item[12].toString(),
                            Account: item[5].toString(),
                            BankName: item[4].toString(),
                            ProcessType: item[10].toString(),
                            Price: item[8].toString(),
                            ProcessPrice: item[11].toString()
                        };
                        arr.push(uploadItem);
                    });
                    var list = JSON.stringify(arr)

                    $.ajax({
                        type: "POST",
                        url: "/Operation/uploadData",
                        data: list,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
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
                });
            }
        }
    })
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
    document.getElementById("errorcaseId").innerHTML = "";
    document.getElementById("errorprocessNumber").innerHTML = "";
    document.getElementById("erroraccountId").innerHTML = "";
    document.getElementById("erroraccountDetailId").innerHTML = "";
    document.getElementById("errorprocessTypeId").innerHTML = "";
    document.getElementById("errorprice").innerHTML = "";
    document.getElementById("errorprocessPrice").innerHTML = "";
    var OperationCreate = {
        CaseId: parseInt($("#caseId").val()),
        ProcessNumber: parseInt($("#processNumber").val()),
        AccountId: parseInt($("#accountId").val()),
        AccountDetailId: parseInt($("#accountDetailId").val()),
        ProcessTypeId: parseInt($("#processTypeId").val()),
        Price: parseFloat($("#price").val() == "" ? 0 : $("#price").val().replace(",", ".")),
        ProcessPrice: parseFloat($("#processPrice").val() == "" ? 0 : $("#processPrice").val().replace(",", "."))
    };

    if (validation("caseId") &&
        validation("accountId") &&
        validation("accountDetailId") &&
        validation("processTypeId") &&
        validation("price") &&
        validation("processPrice")
    ) {
        $.ajax({
            url: '/Operation/OperationCreate',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(OperationCreate),
            success: function (response) {
                if (response.isSuccess) {
                    toastr.success("İşlem başarılı.");
                    $('#createPopup').modal('toggle');

                    setTimeout(function () {
                        $("#save").attr('disabled', false);
                        window.location.reload();
                    }, 2000);
                } else {
                    toastr.error(response.message);
                    $("#save").attr('disabled', false);
                }
                $("#save").attr('disabled', false);
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

function Update() {
    $("#update").attr('disabled', true);
    document.getElementById("erroreditCaseId").innerHTML = "";
    document.getElementById("erroreditProcessNumber").innerHTML = "";
    document.getElementById("erroreditAccountId").innerHTML = "";
    document.getElementById("erroreditAccountDetailId").innerHTML = "";
    document.getElementById("erroreditProcessTypeId").innerHTML = "";
    document.getElementById("erroreditPrice").innerHTML = "";
    document.getElementById("erroreditProcessPrice").innerHTML = "";
    var OperationUpdate = {
        Id: parseInt($("#editId").val()),
        CaseId: parseInt($("#editCaseId").val()),
        ProcessNumber: parseInt($("#editProcessNumber").val()),
        AccountId: parseInt($("#editAccountId").val()),
        AccountDetailId: parseInt($("#editAccountDetailId").val()),
        ProcessTypeId: parseInt($("#editProcessTypeId").val()),
        Price: parseFloat($("#editPrice").val().replace(",", ".")),
        ProcessPrice: parseFloat($("#editProcessPrice").val().replace(",", "."))
    };

    if (validation("editCaseId") &&
        validation("editProcessNumber") &&
        validation("editAccountId") &&
        validation("editAccountDetailId") &&
        validation("editProcessTypeId") &&
        validation("editPrice") &&
        validation("editProcessPrice")
    ) {
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
                        $("#update").attr('disabled', false);
                        window.location.reload();
                    }, 1000);
                } else {
                    toastr.error(response.message);
                    $("#update").attr('disabled', false);
                }
            },
            error: function (response) {
                $("#update").attr('disabled', false);
            }
        });
    }
    else {
        $("#update").attr('disabled', false);
    }
}

function remove(id) {
    if (confirm("Kayıt silinecektir. Emin misiniz?")) {
        $.ajax({
            url: '/Operation/OperationDelete/' + id,
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