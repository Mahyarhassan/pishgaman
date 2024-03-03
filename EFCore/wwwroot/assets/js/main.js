//$(document).ready(function () {
jQuery(window).on('load', function () {



    $("#rigester").on('click', function () {
        console.log("reg");
        $("#formRigester").addClass("o1");
        $("#formRigester").removeClass("o0");
        $("#formLogin").addClass("o0");
        $("#formLogin").removeClass("o1");

    })
    $("#login").on('click', function () {
        console.log("log");
        $("#formLogin").addClass("o1");
        $("#formLogin").removeClass("o0");
        $("#formRigester").addClass("o0");
        $("#formRigester").removeClass("o1");

    })

    $("#cop").on('click', function () {

        var FirstName = $("#FirstName").val();
        var LastName = $("#LastName").val();
        var Phone = $("#Phone").val();
        var Email = $("#Email").val();
        var UserName = $("#UserName").val();
        var Password = $("#Password").val();
        var token = $('input[name=__RequestVerificationToken]').val();
        $.post("/Home/UserManeger", { UserName: UserName, Password: Password, FirstName: FirstName, LastName: LastName, Phone: Phone, Email: Email, __RequestVerificationToken: token })
            .done(function (res) {
                if (res == "ok") {
                    Swal.fire(
                        'موفق',
                        'اطلاعات شما به عنوان عضو جدید ثبت شد',
                        'success'
                    )
                    setTimeout(function () {
                        window.location.href = "/Home/index";
                    }, 3000)
                }
                else {
                    Swal.fire(
                        'خطا',
                        `${res}`,
                        'error'
                    )
                }

            })
            .fail(function () {

            })
            .always(function () {

            });
    })
    $("#cop2").on('click', function () {

        var UserName = $("#LUserName").val();
        var Password = $("#LPassword").val();

        console.log(UserName);
        console.log(Password);
        var token = $('input[name=__RequestVerificationToken]').val();
        console.log(token);
        $.post("/Home/LoginUser", { username: UserName, password: Password, __RequestVerificationToken: token })
            .done(function (res) {
                if (res == "ok") {
                    Swal.fire(
                        'موفق',
                        'درحال ورود به صفحه سایت',
                        'success'
                    )
                    setTimeout(function () {
                        window.location.href = "/Home/index";
                    }, 3000)
                }
                else {
                    Swal.fire(
                        'خطا',
                        `${res}`,
                        'error'
                    )
                }

            })
            .fail(function () {
                console.log("خطا در برقراری ارتباط با سرور");
            })
            .always(function () {

            });
    })
















});     