
function fn_getStatusBasedOnName(select, name) {
    debugger;
    $("#overlay").fadeIn();
    $.get("/TrackingDashboard/GetStatusDetailsByName", {
        Name: name
    }).done(function (data) {
        debugger;
        if ($.fn.dataTable.isDataTable('#TrackingDetails')) {
            table = $('#TrackingDetails').DataTable();
            table.clear();
            table.destroy();
        }
        if (data.length != 0) {
            debugger;
            $('#TrackingDetails').DataTable({
                "info": true,
                "stateSave": true,
                "dom": 'Bfrltip',
                "scrollX": true,
                "orderCellsTop": true,
                "searchDelay": 500,
                "responsive": false,
                data: data,

                "columns": [

                    {
                        data: "trackingNumber",
                        title: "Tracking Number"
                    },
                    {
                        data: "displayOrder",
                        title: "Display Order Code"
                    },
                    {
                        data: "shipmentID",
                        title: "Shipment Id"
                    },
                    {
                        data: "latestStatus",
                        title: "Status"
                    },
                    {
                        data: "courierName",
                        title: "Courier Name"
                    },
                    {
                        data: "trackingLink",
                        title: "Tracking Link",
                        render: function (data, type, row) {
                            return '<a href="' + data + '" target="_blanck">' + data + '</a>'
                        }

                    },
                    {
                        data: "customerName",
                        title: "Customer Name"
                    },
                    {
                        data: "customerPhone",
                        title: "Phone"
                    },
                    {
                        data: "facilityCode",
                        title: "Facility"
                    },
                    {
                        data: "customerCity",
                        title: "City"
                    },
                    {
                        data: "invoiceDate",
                        title: "Invoice Date"
                    },
                    {
                        data: "materialCode",
                        title: "Material Code"
                    },
                    {
                        data: "quantity",
                        title: "Quantity"
                    },
                    {
                        data: "uom",
                        title: "UOM"
                    },
                    {
                        data: "indentID",
                        title: "Indent Number"
                    },
                    {
                        data: "pincode",
                        title: "Pincode"
                    },
                    {
                        data: "state",
                        title: "State"
                    },
                    {
                        data: "region",
                        title: "Region"
                    }
                ]
            })
            $("#overlay").fadeOut();

        }
        else {
            $("#overlay").fadeOut();

            iqwerty.toast.toast("No Failed Records Found!!");
        }
    });

}
