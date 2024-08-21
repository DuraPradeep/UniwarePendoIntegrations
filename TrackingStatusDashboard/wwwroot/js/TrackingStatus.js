function GetTrackingLink() {
    var number = $("#trackingno").val();
    var name = $('input[name="ordertype"]:checked').val();
    $("#overlay").fadeIn();
    if (number == 0) {
        iqwerty.toast.toast("Please Input Tracking No.");
        if ($.fn.dataTable.isDataTable('#TrackingDetails')) {
            table = $('#TrackingDetails').DataTable();
            table.clear();
            table.destroy();
        }
        $("#overlay").fadeOut();
    }
    else {

        $.get("/DashBoard/GettrackingLink", {
            SearchBy: name,
            trackingNo: number
        }).done(function (data) {
            debugger;


            if ($.fn.dataTable.isDataTable('#TrackingDetails')) {
                table = $('#TrackingDetails').DataTable();
                table.clear();
                table.destroy();
            }
            if (data.length != 0) {
                $("#table_tracking_id").css("display", "block");
                debugger;
                $('#TrackingDetails').DataTable({
                    "info": false,
                    //"responsive": true,
                    "paging": false,
                    "scrollX": true,
                    "searching": false,
                    "ordering": false,
                    "stateSave": true,
                    "dom": 'Bfrltip',
                    "orderCellsTop": true,
                    "searchDelay": 500,
                    data: data,

                    "columns": [

                        {
                            data: "trackingNumber",
                            title: "Tracking Number"
                        },
                        {
                            data: "displayOrder",
                            title: "Order No."
                        },
                        {
                            data: "orderStatus",
                            title: "Order Status"
                        },
                        {
                            data: "courierName",
                            title: "Courier Name"
                        },
                        {
                            data: "trackingLink",
                            title: "Tracking Link",
                            render: function (data, type, row) {
                                /* return '<a href="' + data + '" target="_blank">track</a>'*/
                                if (data == null || data == "") { return "" }
                                else {
                                    return '<a href="' + data + '" target="_blank">track</a>'
                                }
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
                            data: "customerCity",
                            title: "City"
                        }
                        ,
                        {
                            data: "pincode",
                            title: "Pincode"
                        }
                    ]
                }).columns.adjust();
                $("#overlay").fadeOut();
            }
            else {
                $("#table_tracking_id").css("display", "none");
                iqwerty.toast.toast("No Records Found!!");
            }
            $("#overlay").fadeOut();
        });

    }
}