function edit(Id) {
    $.ajax({
        url: '/Operation/OperationEdit/' + Id,
        success: function (data) {
            
            $('#editPopup').modal('show');
            $("#EditId").val(data.id);
            $("#EditProcessNumber").val(data.processNumber);
            $("#EditAccountId").val(data.accountId);
            $("#EditAccountDetailId").val(data.accountDetailId);
            $("#EditProcessTypeId").val(data.processTypeId);
            $("#EditPrice").val(data.price);
            $("#EditProcessPrice").val(data.processPrice);
        }
    });
}

function Update() {
    var OperationUpdate = {
        Id: parseInt($("#EditId").val()),
        ProcessNumber: parseInt($("#EditProcessNumber").val()),
        AccountId: parseInt($("#EditAccountId").val()),
        AccountDetailId: parseInt($("#EditAccountDetailId").val()),
        ProcessTypeId: parseInt($("#EditProcessTypeId").val()),
        Price: parseFloat($("#EditPrice").val()),
        ProcessPrice: parseFloat($("#EditProcessPrice").val())
    };

    $.ajax({
        url: '/Operation/OperationEdit',
        type: "POST",
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(OperationUpdate),
        success: function (data) {

        }
    });
}