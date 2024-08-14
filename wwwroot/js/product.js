var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#productTbl').DataTable({
        "ajax": { url: '/admin/product/getall' },
        order: [[1, 'asc']],
        scrollX: true,
        "columns": [
            { "data": "name", "width": "10%" },
            {
                "data": "description",
                "width": "10%",
                "render": function (data, type, row, meta) {
                    return type === 'display' && data.length > 40
                        ? '<span title="' + data + '">' + data.substr(0, 38) + '...</span>'
                        : data;
                }
            },
            { "data": "code", "width": "10%" },
            { "data": "price", "width": "10%" },
            { "data": "discountedPrice", "width": "10%" },
            { "data": "isActive", "width": "10%" },
            {
                "data": "createDate", "width": "10%",
                "render": function (data) {
                    if (moment(data).isValid()) {
                        return (moment(data).format('MM/DD/YYYY HH:mm:ss'));
                    }
                    return "";
                    console.log(data)
                }
            },
            {
                "data": "updateDate",
                "width": "10%",
                "render": function (data) {
                    if (moment(data).isValid()) {
                        return (moment(data).format('MM/DD/YYYY HH:mm:ss'));
                    }
                    return "";
                    console.log(data)
                }
            },
            {
                "name" : 'Actions',
                data: 'id',
                "render": function (data, type, row, meta) {
                    let isDeleteDisabled = 'false'
                    let buttonClass = ''

                    if (row["isActive"] == false) {
                        isDeleteDisabled = 'true'
                        buttonClass = 'disabled'
                    }

                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/product/update?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2 ${buttonClass}" aria-disabled="${isDeleteDisabled}" style="color:white"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}