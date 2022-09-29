// admin datatables
function loadBicycleDataTable(tableId) {
    let controllerUrl = "/Admin/GetBicycles", addUrl = '/Admin/_AddBicycle?id', deleteUrl = '/Admin/DeleteBicycle'
    loadDefaultDataTableSettings()
    dataTable = $(tableId).DataTable({
        stripeClasses: [],
        ajax: {
            url: controllerUrl,
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: "name", width: "15%",  },
            { data: "wheelDiameter", width: "12%"},
            { data: "price", width: "9%"},
            { data: "quantity", width: "12%"},
            { data: "type.name", width: "12%"},
            { data: "producer.name", width: "12%"},
            { data: "country.name", width: "12%"},
            { data: "id", bSortable: false, aTargets: [1],
                render: function (data) { return loadAdminTableButtons(addUrl, deleteUrl, data) }, width: "40%"
            }
        ],
    })
    console.log(dataTable)
}
function loadTypeDataTable(tableId) {
    let controllerUrl = "/Admin/GetTypes", addUrl = '/Admin/_AddType?id', deleteUrl = '/Admin/DeleteType'
    loadDefaultDataTableSettings()
    dataTable = $(tableId).DataTable({
        ajax: {
            url: controllerUrl,
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: "name", width: "30%"},
            { data: "description", width: "40%", 'bSortable': false, 'aTargets': [1], },
            { data: "id", 'bSortable': false, 'aTargets': [1],
                render: function (data) { return loadAdminTableButtons(addUrl, deleteUrl, data) }, width: "40%"
            }
        ]
    });
}
function loadProducerDataTable(tableId) {
    let controllerUrl = "/Admin/GetProducers", addUrl = '/Admin/_AddProducer?id', deleteUrl = '/Admin/DeleteProducer'
    loadDefaultDataTableSettings()
    dataTable = $(tableId).DataTable({
        ajax: {
            url: controllerUrl,
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: "name", width: "30%"},
            { data: "description", width: "40%", 'bSortable': false, 'aTargets': [1],},
            { data: "id", 'bSortable': false, 'aTargets': [1],
                render: function (data) { return loadAdminTableButtons(addUrl, deleteUrl, data) }, width: "40%"
            }
        ]
    });
}
function loadUserDataTable(tableId) {
    let controllerUrl = "/Admin/GetUsers"
    loadDefaultDataTableSettings()
    dataTable = $(tableId).DataTable({
        ajax: {
            url: controllerUrl,
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: "fullName", width: "15%"},
            { data: "phone", width: "12%"},
            { data: "email", width: "9%"},
            { data: "adress", width: "12%"},
            { data: "username", width: "12%"},
            {
                data: { isAdmin: "isAdmin", id: "id"}, bSortable: false, aTargets: [1], render: function (data) {
                    let input = `<input type="checkbox" OnClick="changePermision('${data.id}')" `
                    console.log(input)
                    if (data.isAdmin === true)
                        return input + `checked>`
                    return input + `>`
                }, width: "12%",
                className: "dt-body-center"
            },
        ],
    })
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
function saveObject(url, formId) {
    $.ajax({
        type: 'POST',
        url: url,
        data: $(formId).serialize(),
        success: function (data) {
            closePopup()
            if (data.success) {
                dataTable.ajax.reload();
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
    $.ajax({
        type: 'GET',
        url: url,
        success: function (data) {
            $("#PopupPlaceHolder").html(`<div class="col-md-4 offset-md-4 bg-secondary border container">` + data + `</div>`);
            $("#PopupPlaceHolder").modal('show');
        }
    })
}