var list = JSON.parse(getCookie("listaReposicion"));
var username;
if (list.length > 0) {
    displayList();
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function AddToRepositionList(y) {
    if (list == null) {
        list = new Array();
    }
    var inList = stockInList(y);

    var content = document.getElementById("content-" + y).value;
    var description = document.getElementById("description-" + y).value;
    var type = document.getElementById("type-" + y).value;
    var actualQuantity = parseInt(document.getElementById("actualQuantity-" + y).value);
    var requestQuantity = parseInt(document.getElementById("requestQuantity-" + y).value);
    username = document.getElementById("username").value;

    if ((actualQuantity > 0) && (requestQuantity > 0)) {
        if (inList == false) {
            createItem(y, description,content,type,actualQuantity,requestQuantity);
        } else if (inList == true) {
            updateItem(actualQuantity, requestQuantity, y);
        }

        if (list != null) {
            displayList();
        }
    }

    setCookie("listaReposicion", JSON.stringify(list), 1);
}

function stockInList(y) {
    for (var x = 0; x < list.length; x++) {
        if (list[x].Id == y)
            return true;
    }
    return false;
}

function createItem(y, description, content, type, actualQuantity, requestQuantity) {
    var itemList = new Object();
    itemList.Id = y;
    itemList.Description = description;
    itemList.Content = content;
    itemList.Type = type;
    itemList.ActualQuantity = actualQuantity;
    itemList.RequestQuantity = requestQuantity;
    list.push(itemList);
}

function updateItem(actualQuantity, requestQuantity, y) {
    for (var x = 0; x < list.length; x++) {
        if (list[x].Id == y) {
            list[x].ActualQuantity = actualQuantity;
            list[x].RequestQuantity = requestQuantity;
        }
    }

}

function removeItem(y) {
    for (var x = 0; x < list.length; x++) {
        if (list[x].Id == y)
            list.splice(x, 1);
    }
    if (list.length == 0)
        document.getElementById("lista").innerHTML = "";
    else
        displayList();
    setCookie("listaReposicion", JSON.stringify(list), 1);
}

function displayList() {
    var finalButton = "<input type=\"submit\" value=\"Aceptar\" class=\"btn btn-default\" onclick=\"sendRepositionList() \" autocomplete=\"off\" />";
    var s = "<table class=\"table\"><thead><tr><th>Descripcion</th><th>Contenido</th><th>Tipo</th><th>Cantidad Actual</th><th>Cantidad a Solicitar</th></tr></thead><tbody>";
    for (var x = 0; x < list.length; x++) {
        s += "<tr>";
        s += "<td>" + list[x].Description + "</td><td>" + list[x].Content + "</td><td>" + list[x].Type + "</td><td>" + list[x].ActualQuantity + "</td><td>" + list[x].RequestQuantity + "</td>"+
            "<td> " + "<input type=\"submit\" value=\"Quitar\" class=\"btn btn-default\" onclick=\"removeItem(" + list[x].Id + ")\" />" + "</td>";
        s += "</tr>";
    }
    s += "</tbody></table > ";
    document.getElementById("lista").innerHTML = "<h2>Lista</h2>" + s + finalButton;
}

function sendRepositionList() {
    var jsonListstring = JSON.stringify(list);
    var postDataList = { values: jsonListstring,username };

    //POST
    $.ajax({
        type: "POST",
        url: "/Requests/SaveReposition",
        data: postDataList,
        traditional: true,
        dataType: "json",
        success: function (data) {
            setCookie("listaReposicion", JSON.stringify([]), 1);
            location.reload();
        }
    });
}