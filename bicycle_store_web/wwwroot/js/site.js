// admin datatables
function loadDataTable(tableId, userId) {
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
    }
    dataTable = $("#" + tableId).DataTable({
        stripeClasses: [],
        ajax: {
            url: controllerUrl,
            type: "GET",
            datatype: "json"
        },
        columns: columns,
    })
    loadDefaultAdminTableSettings()
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
function getUsersTableColumns(userId) {
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
function loadDefaultAdminTableSettings() {
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
                        dataTable.ajax.reload();
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
        url: $("#" +formId).attr("action"),
        data: data,
        success: function (data) {
            if (data.success) {
                try {
                    dataTable.ajax.reload();
                    closePopup();
                } catch (e) {}
                toastr.success(data.message);
            }
            else {
                toastr.error(data.message);
            }
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
    console.log(url)
    $.ajax({
        type: 'GET',
        url: url,
        success: function (data) {
            $("#PopupPlaceHolder").html(`<div class="col-md-4 offset-md-4 bg-secondary border container ">` + data + `</div>`);
            $("#PopupPlaceHolder").modal('show');
        }
    })
}