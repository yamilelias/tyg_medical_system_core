// Write your Javascript code.
$('#myModal').on('shown.bs.modal', function () {
    $('#Name').focus()
})

var j = 0;
var items;
var id;
var email;
var phoneNumber;
var userName;
var accessFailedCount;
var concurrencyStamp;
var emailConfirmed;
var lockoutEnabled;
var lockoutEnd;
var normalizedEmail;
var normalizedUserName;
var passwordHash;
var phoneNumberConfirmed;
var securityStamp;
var twoFactorEnabled;
var name;
var lastName;
var secondLastName;
var telephone;
var selectRole;
var role;


function getDataAjax(id, action)
{
    $.ajax({
        type: "POST",
        url: action,
        data: { id },
        success: function (response) {
            OnSuccess(response);
        }
    })
}

function OnSuccess(response) {

    items = response;
    j = 0;
    for (var i = 0; i < 3; i++)
    {
        var x = document.getElementById("select");
        //x.remove(i);
    }

    $.each(items, function (index, val) {
        $('input[name=Id]').val(val.id);
        $('input[name=Name]').val(val.name);
        $('input[name=LastName]').val(val.lastName);
        $('input[name=SecondLastName]').val(val.secondLastName);
        $('input[name=Telephone]').val(val.telephone);
        $('input[name=UserName]').val(val.userName);
        $('input[name=Email]').val(val.email);
        document.getElementById("select").options[0] = new Option(val.role, val.roleId);

    });

    //var x = document.getElementById("select").selectedIndex;
    //var y = document.getElementById("option");   

    //y[x].defaultSelected;
}

function setDataUser(action) {

    id = $(' input[name=Id]')[0].value;
    name = $(' input[name=Name]')[0].value;
    lastName = $(' input[name=LastName]')[0].value;
    secondLastName = $(' input[name=SecondLastName]')[0].value;
    telephone = $(' input[name=Telephone]')[0].value;
    userName = $(' input[name=UserName]')[0].value;
    email = $(' input[name=Email]')[0].value;
    role = document.getElementById("select");
    selectRole = role.options[role.selectedIndex].text;


    $.each(items, function (index, val) {

        accessFailedCount = val.accessFailedCount;
        concurrencyStamp = val.concurrencyStamp;
        emailConfirmed = val.emailConfirmed;
        lockoutEnabled = val.lockoutEnabled;
        lockoutEnd = val.lockoutEnd;
        normalizedEmail = val.normalizedEmail;
        normalizedUserName = val.normalizedUserName;
        passwordHash = val.passwordHash;
        phoneNumberConfirmed = val.phoneNumberConfirmed;
        securityStamp = val.securityStamp;
        twoFactorEnabled = val.twoFactorEnabled;

    });

    if (name == "")
    {
        $("#Email").focus();
        alert("Please insert a name.");
    } else
        if (lastName == "") {
            $("#LastName").focus();
            alert("Please insert a paternal name.");
        } else
            if (secondLastName == "") {
                $("#SeconLastName").focus();
                alert("Please insert a maternal name.");
            } else
                if (telephone == "") {
                    $("#Telephone").focus();
                    alert("Please insert a telephone.");
                } else
                    if (userName == "") {
                        $("#UserName").focus();
                        alert("Please insert a username.");
                    } else
                        if (email == "") {
                            $("#Email").focus();
                            alert("Please insert an email address.");
                        } else
                            if (email != userName)
                            {
                                $("#Email").focus();
                                alert("Email and username must be identical.");
                            } else {
                                $.ajax({
                                    type: "POST",
                                    url: action,
                                    data: {
                                        id, email, phoneNumber, userName, accessFailedCount, concurrencyStamp, emailConfirmed, lockoutEnabled, lockoutEnd,
                                        normalizedEmail, normalizedUserName, passwordHash, phoneNumberConfirmed, securityStamp, twoFactorEnabled, name, lastName,
                                        secondLastName, telephone, selectRole
                                    },
                                    success: function (response)
                                    {
                                        if (response == "Save")
                                        {
                                            window.location.href = "http://localhost:49957/Users";
                                        }
                                    }
                                })
                            }
}

function getRolesAjax(action) {
    $.ajax({

        type: "POST",
        url: action,
        data: {},
        success: function (response) {
            if (j == 0) {
                for (var i = 0; i < response.length; i++)
                {
                    document.getElementById("select").options[i] = new Option(response[i].text, response[i].value);
                }
                j = 1;
            }
        }
    });
}