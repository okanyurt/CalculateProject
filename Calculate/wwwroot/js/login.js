function loginPost() {
    var login = {
        MobilePhone: $("#MobilePhone").val(),
        Password: $("#Password").val()
    };

    $.ajax({
        url: '/Login/Login',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(login),
        success: function (response) {
            if (Message != null && Message == "Uygulama bulunamadı.") {
                $.ajax({
                    url: '/login/login/' + UserId,
                    success: function (data) {
                    }
                });
            }
            else {
                window.location.href = "/Home/Index";
            }
        },
        error: function (response) {
            // window.location.href = response.redirectToUrl;
        }
    });
}