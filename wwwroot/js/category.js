var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#categoryTbl').DataTable({
        "ajax": { url: '/admin/category/getall' },
        order: [[1, 'asc']],
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "displayOrder", "width": "15%" },
            { "data": "isActive", "width": "15%" },
            {
                "data": "createDate", "width": "15%",
                "render": function (data) {
                    if (moment(data).isValid()) {
                        return (moment(data).format('MM/DD/YYYY HH:mm:ss'));
                    }
                    return "";
                    console.log(data)
                }
            },
            {
                "data": "updateDate", "width": "15%",
                "render": function (data) {
                    if (moment(data).isValid()) {
                        return (moment(data).format('MM/DD/YYYY HH:mm:ss'));
                    }
                    return "";
                    console.log(data)
                }
            },
            {
                data: 'id',
                "render": function (data, type, row, meta) {
                    let isDeleteDisabled = 'false'
                    let buttonClass = ''

                    if (row["isActive"] == false) {
                        isDeleteDisabled = 'true'
                        buttonClass = 'disabled'
                    }

                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/category/update?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=Delete('/admin/category/delete/${data}') class="btn btn-danger mx-2 ${buttonClass}" aria-disabled="${isDeleteDisabled}" style="color:white"> <i class="bi bi-trash-fill"></i> Delete</a>
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