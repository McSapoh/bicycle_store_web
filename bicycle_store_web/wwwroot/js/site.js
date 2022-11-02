// admin datatables
function loadDataTable(tableId, userId) {
    tId = "#" + tableId
    let columns, controllerUrl
    switch (tableId) {
        case 'BicycleTable':
            columns = getBicyclesTableColumns()
            controllerUrl = '/Admin/GetBicycles'
            break;
        case 'TypeTable':
            columns = getTypesTableColumns()
            controllerUrl = '/Admin/GetTypes'
            break;
        case 'ProducerTable':
            columns = getProducersTableColumns()
            controllerUrl = '/Admin/GetProducers'
            break;
        case 'UserTable':
            columns = getUsersTableColumns(userId)
            controllerUrl = '/Admin/GetUsers'
            break;
        case 'CartTable':
            columns = getShoppingCartTableColumns()
            controllerUrl = '/ShoppingCart/GetShoppingCart'
            break;
        case 'UserOrderTable':
            columns = getUserOrderTableColumns()
            controllerUrl = '/Order/GetUserOrders'
            break;
        case 'AdminOrderTable':
            columns = getAdminOrderTableColumns()
            controllerUrl = '/Order/GetAdminOrders'
            break;
    }
    dataTable = $("#" + tableId).DataTable({
        ajax: {
            url: controllerUrl,
            type: "GET",
            datatype: "json"
        },
        columns: columns,
        "drawCallback": function () {
            if (tableId == 'UserOrderTable') {
                mergeColumns('UserOrderTable')
            }
            if (tableId == 'AdminOrderTable') {
                mergeColumns('AdminOrderTable')
            }
        },
        "initComplete": function () {
            if (tableId == 'CartTable') {
                loadCartFooter()
                getTotalCount()
            }
        }
    })
    loadDefaultDataTableSettings()
}

function getBicyclesTableColumns() {
    let addUrl = '/Admin/_AddBicycle?id', deleteUrl = '/Admin/DeleteBicycle'
    return [
        { data: "name", width: "15%", },
        { data: "wheelDiameter", width: "12%" },
        { data: "price", width: "9%" },
        { data: "quantity", width: "12%" },
        { data: "type.name", width: "12%" },
        { data: "producer.name", width: "12%" },
        { data: "country.name", width: "12%" },
        {
            data: "id", bSortable: false, aTargets: [1],
            render: function (data) { return loadAdminTableButtons(addUrl, deleteUrl, data) }, width: "40%"
        }
    ]
}
function getTypesTableColumns() {
    let addUrl = '/Admin/_AddType?id', deleteUrl = '/Admin/DeleteType'
    return [
        { data: "name", width: "30%" },
        { data: "description", width: "40%", 'bSortable': false, 'aTargets': [1], },
        {
            data: "id", 'bSortable': false, 'aTargets': [1],
            render: function (data) { return loadAdminTableButtons(addUrl, deleteUrl, data) }, width: "40%"
        }
    ]
}
function getProducersTableColumns() {
    let addUrl = '/Admin/_AddProducer?id', deleteUrl = '/Admin/DeleteProducer'
    return [
        { data: "name", width: "30%" },
        { data: "description", width: "40%", 'bSortable': false, 'aTargets': [1], },
        {
            data: "id", 'bSortable': false, 'aTargets': [1],
            render: function (data) { return loadAdminTableButtons(addUrl, deleteUrl, data) }, width: "40%"
        }
    ]
}
function getUsersTableColumns() {
    let userId = getUserId();
    return [
        { data: "fullName", width: "15%" },
        { data: "phone", width: "16%" },
        { data: "email", width: "17%" },
        { data: "adress", width: "12%" },
        { data: "username", width: "12%" },
        {
            data: { role: "role", id: "id", userId: userId }, bSortable: false, aTargets: [1], render: function (data) {
                let input = `<input type="checkbox" OnClick="changePermision('${data.id}')" `
                if (userId == data.id)
                    input += `disabled `
                if (data.role == "Admin")
                    return input + `checked>`
                if (data.role == "SuperAdmin")
                    return input + `checked disabled>`
                return input + `>`
            }, width: "4%",
            className: "dt-body-center"
        },
        {
            data: { photo: "photo", role: "role" }, bSortable: false, aTargets: [1], render: function (data) {
                let img = `<img src="data:image/jpeg;base64,` + data.photo + `" style="height: 100px; width: 100px;"/>`
                return img
            }, width: "8%"
        },
    ]
}
function getShoppingCartTableColumns() {
    return [
        { data: "name", width: "15%", },
        { data: "price", width: "9%" },
        { data: "quantity", width: "12%", bSortable: false, aTargets: [1], },
        { data: function (data) { return data.price * data.quantity }, width: "12%", },
        {
            data: { id: "id"}, bSortable: false, aTargets: [1],
            render: function (data) {
                console.log(data.id)
                return `<div class="text-center">
                    <a class='btn btn-danger text-white' style='cursor:pointer; width:45%;'
                        OnClick="removeFromCart('${data.id}')">
                        Remove
                    </a>
                </div>`},
            width: "40%"
        }
    ]
}
function getUserOrderTableColumns() {
    return [
        { data: "orderId", width: "3%", },
        { data: "name", width: "20%" },
        { data: "quantity", width: "12%", bSortable: false, aTargets: [1], },
        { data: "bicycleCost", width: "9%" },
        {
            data: { id: "orderId", status: "status", userId: "userId" }, bSortable: false, aTargets: [1],
            render: function (data) {
                if (data.status == 'Sended') {
                    return `<div class="text-center">
                        <a class='btn btn-success text-white' style='cursor:pointer; width:45%;'
                            onclick="confirmReceipt(${data.orderId})">
                            Сonfirm receipt
                        </a>
                    </div>`
                }

                return data.status
            },
            width: "40%"
        }
    ]
}
function getAdminOrderTableColumns() {
    return [
        { data: "orderId", width: "3%", },
        { data: "name", width: "20%" },
        { data: "quantity", width: "12%", bSortable: false, aTargets: [1], },
        { data: "bicycleCost", width: "9%" },
        { data: "fullName", width: "20%"},
        {
            data: { id: "orderId", status: "status"}, bSortable: false, aTargets: [1],
            render: function (data) {
                if (data.status == 'Processing') {
                    return `<div class="text-center">
                        <a class='btn btn-info text-white' style='cursor:pointer; width:45%;'
                            onclick="sendOrder(${data.orderId})">
                            Send
                        </a>
                    </div>`
                }
                return data.status
            },
            width: "40%"
        }
    ]
}

// default datatable settings
function loadAdminTableButtons(addUrl, deleteUrl, objId) {
    return `<div class="text-center">
                <a data-toggle="ajax-modal"
                    OnClick="openPopup('${addUrl + "=" + objId}')" class='btn btn-success text-white' style='cursor:pointer; width:45%;'>
                    Edit
                </a>
                &nbsp;
                <a class='btn btn-danger text-white' style='cursor:pointer; width:45%;'
                    OnClick="deleteObj('${deleteUrl}', '${objId}')">
                    Delete
                </a>
            </div>`
}
function loadDefaultDataTableSettings() {
    dataTable = {
        retrieve: true,
        paging: false,
        language: {
            emptyTable: "no data found"
        },
        width: "100%",
    };
}

// object settings
function deleteObj(functionUrl, objectId) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: functionUrl,
                data: { id: objectId },
                success: function (data) {
                    if (data.success) {
                        console.log(tId)
                        $(tId).DataTable().ajax.reload()
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
function saveObject(formId) {
    let data, contentType, processData
    try {
        event.preventDefault();
        const form = document.getElementById(formId)
        let formData = new FormData(form)
        let files = $("#photo")[0].files;
        formData.append("Photo", files[0]);
        data = formData
        processData = false
        contentType = false
    } catch (e) {
        data = $("#" + formId).serialize()
        processData = true
        contentType = 'application/x-www-form-urlencoded'
    }
    $.ajax({
        processData: processData,
        contentType: contentType,
        type: 'POST',
        url: $("#" + formId).attr("action"),
        data: data,
        success: function (data) {
            try {
                console.log("success")
                closePopup();
                console.log("popup closed")
                $(tId).DataTable().ajax.reload()
                console.log("dataTable reloaded")
            } catch (e) { }
            toastr.success(data.message);
        }
    })
}
function changePermision(objectId) {
    let functionUrl = '/Admin/ChangePermisions'
    $.ajax({
        type: 'POST',
        url: functionUrl,
        data: { id: objectId },
        success: function (data) {
            if (data.success) {
                dataTable.ajax.reload();
                toastr.success(data.message);
            }
            else {
                toastr.error(data.message);
            }
        }
    });
}

// popup actions
function closePopup() {
    $("#PopupPlaceHolder").empty();
    $('#PopupPlaceHolder').modal('hide')
}
window.onclick = function (event) {
    if ($('#PopupPlaceHolder').modal == 'show' && event.target == modal) {
        closePopup();
    }
}
function openPopup(url) {
    //console.log(url)
    $.ajax({
        type: 'GET',
        url: url,
        success: function (data) {
            $("#PopupPlaceHolder").html(`<div class="col-md-4 offset-md-4 bg-secondary border container ">` + data + `</div>`);
            $("#PopupPlaceHolder").modal('show');
        }
    })
}

// shopping cart actions
function addToCart(bicycleId) {
    $.ajax({
        type: 'POST',
        url: '/ShoppingCart/AddToShoppingCart',
        data: { BicycleId: bicycleId },
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
            }
            else {
                toastr.error(data.message);
            }
        }
    })
}
function removeFromCart(bicycleId) {
    $.ajax({
        type: 'POST',
        url: '/ShoppingCart/RemoveFromShoppingCart',
        data: { BicycleId: bicycleId },
        success: function (data) {
            if (data.success) {
                $('#CartTable').DataTable().ajax.reload()
                toastr.success(data.message);
                getTotalCount('total')
            }
            else {
                toastr.error(data.message);
            }            
        }
    })
}
function getTotalCount() {
    let cellId = '#total'
    let table = document.getElementById('CartTable')
    let totalCount = 0
    for (let i = 1; i < table.rows.length-1; i++) {
        totalCount += Number(table.rows[i].cells[3].innerText)
    }
    $(cellId).html(totalCount)
}
function loadCartFooter() {
    $('#CartTable').append(
        `<tfoot>
            <tr>
                <th class="border-0"></th>
                <th class="border-0"></th>
                <th class="border-0">
                    <a class='btn btn-success text-white' style='cursor:pointer; width:45%;'
                        OnClick="createOrder()">
                        Buy
                    </a>
                </th>
                <th class="border-0 ">Total price</th>
                <th id="total" class="border-0 col-4"></th>
            </tr>
        </tfoot>`
    )
}
function createOrder() {
    swal({
        title: "Are you sure that you want to buy all theese goods?",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: '/Order/CreateOrder',
                type: "POST",
                datatype: "json",
                success: function (data) {
                    if (data.success) {
                        $(tId).DataTable().ajax.reload()
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

// order actions
function sendOrder(orderId) {
    $.ajax({
        type: 'POST',
        url: '/Order/SendOrder',
        data: { OrderId: orderId },
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $(tId).DataTable().ajax.reload()
            }
            else {
                toastr.error(data.message);
                $(tId).DataTable().ajax.reload()
            }
        }
    })
    
}
function confirmReceipt(orderId) {
    $.ajax({
        type: 'POST',
        url: '/Order/ConfirmReceipt',
        data: { OrderId: orderId },
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $(tId).DataTable().ajax.reload()
            }
            else {
                toastr.error(data.message);
                $(tId).DataTable().ajax.reload()
            }
        }
    })

}
function setOrderTableButtons() {
    console.log('setOrderTableButtons')
    let role = getUserRole();
    let table = document.getElementById('UserOrderTable')
    for (var i = 1; i < table.rows.length; i++) {
        console.log(i)
        console.log(table.rows[i].cells[3].innerText)
        if (table.rows[i].cells[4].innerText == 'Processing' && (role.data == 'SuperAdmin' || role.data == 'Admin')) {
            console.log("admin")
            table.rows[i].cells[4].html = (`<div class="text-center">
                <a class='btn btn-danger text-white' style='cursor:pointer; width:45%;'
                    >
                    Send
                </a>
            </div>`)
        }
    }
    //for (let row of table.rows) {
    //    console.log(row.cells[4].innerText)
    //    console.log(row.cells[4].innerText == 'Processing')
    //    console.log((role.data == 'SuperAdmin' || role.data == 'Admin'))
    //    if (row.cells[4].innerText == 'Processing' && (role.data == 'SuperAdmin' || role.data == 'Admin')) {
    //        console.log("admin")
    //        row.cells[4].html(`<div class="text-center">
    //            <a class='btn btn-danger text-white' style='cursor:pointer; width:45%;'
    //                OnClick="removeFromCart('${data.id}')">
    //                Send
    //            </a>
    //        </div>`)
    //    }
    //    else
    //        return `<div></div>`
    //    return `<div></div>`
    //}
}
function mergeColumns(tableId) {
    let table = document.getElementById(tableId)
    let headerCell = null, statusCell
    for (let row of table.rows) {
        const firstCell = row.cells[0];
        if (headerCell === null || firstCell.innerText !== headerCell.innerText) {
            headerCell = firstCell;
            statusCell = row.cells[4]
            if (tableId == ('AdminOrderTable'))
                userCell = row.cells[5]
        } else {
            statusCell.rowSpan++
            headerCell.rowSpan++
            if (tableId == ('AdminOrderTable')) {
                userCell.rowSpan++
                row.cells[5].remove()
            }
            row.cells[4].remove()
            firstCell.remove();
        }
    }
}

// user actions
function getUserRole() {
    let res
    $.ajax({
        type: 'GET',
        url: '/User/GetUserRole',
        async: false,
        success: function (data) {
            res = data
        }
    });
    return res
}
function getUserId() {
    let res
    $.ajax({
        type: 'GET',
        url: '/User/GetUserId',
        async: false,
        success: function (data) {
            res = data
        }
    });
    return res
}