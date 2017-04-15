var list;
var finalButton = "<input type=\"submit\" value=\"Aceptar\" class=\"btn btn-default\" onclick=\"sendList() \" autocomplete=\"off\" />";

function addToList(y) {

    if (list == null) {
        list = new Array();
    }
    var inList = stockInList(y);

    var total = parseInt(document.getElementById("total-" + y).value);
    var expiration = document.getElementById("expiration-" + y).value;
    var description = document.getElementById("description-" + y).value;
    var quantity = parseInt(document.getElementById("quantity-" + y).value);
    //document.getElementById("debug").innerHTML = document.getElementById("debug").innerHTML + " Total" + total + "->" + quantity;

    if ((quantity > 0) && (quantity <= total)) {
        if (inList == false) {
            createItem(y, description, quantity, total, expiration);
        } else if (inList == true) {
            updateItem(quantity, y);
        }

        if (list != null) {
            displayList();
        }
    }

}

function stockInList(y) {
    for (var x = 0; x < list.length; x++) {
        if (list[x].Id == y)
            return true;
    }
    return false;
}

function createItem(y,description,quantity,total,expiration) {
    var itemList = new Object();
    itemList.Id = y;
    itemList.Description = description;
    itemList.Quantity = quantity;
    itemList.Total = total;
    itemList.Expiration = expiration;
    list.push(itemList);
}

function updateItem(q, y) {
    for (var x = 0; x < list.length; x++) {
        if (list[x].Id == y)
            list[x].Quantity = q;
    }

}

function removeItem(y) {
    for (var x = 0; x < list.length; x++) {
        if (list[x].Id == y)
            list.splice(x, 1);
    }
    if (list.length == 0) {
        document.getElementById("lista").innerHTML = "";
    }
    else
        displayList();
}

function displayList() {
    var s = "<table class=\"table\"><thead><tr><th>Total</th><th>Caducidad</th><th>Medicamento</th><th>Salida</th><th></th></tr></thead><tbody>";
            for (var x = 0; x < list.length; x++) {
                s += "<tr>";
                s += "<td>" + list[x].Total + "</td><td>" + list[x].Expiration + "</td><td>" + list[x].Description + "</td><td>" + list[x].Quantity + "</td>" +
                    "<td> " + "<input type=\"submit\" value=\"Quitar\" class=\"btn btn-default\" onclick=\"removeItem(" + list[x].Id + ")\" />" + "</td>";
                s += "</tr>";
            }
            s += "</tbody></table > ";
            document.getElementById("lista").innerHTML = "<h2>Lista</h2>" + s + finalButton;
}

function sendList() {
    var jsonListstring = JSON.stringify(list);
    var postDataList = {values:jsonListstring};

    //POST
    $.ajax({
        type: "POST",
        url: "/MedicamentOut/MedicamentOut",
        data: postDataList,
        traditional:true,
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        //success: function (data) {
        //    // do what ever you want with the server response
        //    alert("success ");
        //},
        //error: function () {
        //    // error handling
        //    alert("error");
        //}
    });
}