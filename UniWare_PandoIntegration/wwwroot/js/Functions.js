﻿function fn_SaleOrder() {
    debugger;

    $.get("/Home/ErrorListDataObject", {}).done(function (data) {

        debugger;
        if ($.fn.dataTable.isDataTable('#saleorderTblId')) {
            table = $('#saleorderTblId').DataTable();
            table.clear();
            table.destroy();
        }

        if (data.length != 0) {
            debugger;
            $('#saleorderTblId').DataTable({
                "processing": true,
                "info": true,
                "stateSave": true,
                data: data,

                "columns": [

                    { data: "code" },
                    { data: "itemSku" },
                    { data: "triggerid" },
                    { data: "reason" }
                ]
            })

        }
        else {
            iqwerty.toast.toast("No Failed Records Found!!");            
        }
    });
}

function fn_WaybillErrorListDataObject() {
    
    $.get("/Home/WaybillErrorListDataObject", {}).done(function (data) {
        
        if ($.fn.dataTable.isDataTable('#wayBillTblId')) {
            table = $('#wayBillTblId').DataTable();
            table.clear();
            table.destroy();
        }
        if (data.length != 0) {
            debugger;
            $('#wayBillTblId').DataTable({
                "processing": true,
                "info": true,
                "stateSave": true,
                data: data,

                "columns": [

                    { data: "reason" }
                ]
            })           
        }
        else {
            iqwerty.toast.toast("No Failed Records Found!!");            
        }
    });
}

function fn_ReturnOrderErrorListDataObject() {
    debugger;    
    $.get("/Home/ReturnOrderErrorListDataObject", {}).done(function (data) {
        //console.log(data);
       
        if ($.fn.dataTable.isDataTable('#returnOrderTblId')) {
            table = $('#returnOrderTblId').DataTable();
            table.clear();
            table.destroy();
        }        
        if (data.length != 0) {
            debugger;
            $('#returnOrderTblId').DataTable({
                "processing": true,
                "info": true,
                "stateSave": true,
                data: data,

                "columns": [
                    //{ data:  },
                    { data: "code" },
                    { data: "itemSku" },
                    { data: "triggerid" },
                    { data: "reason" }
                ]
            })
            //$("#overlay").fadeOut(500);
        }
        else {
            iqwerty.toast.toast("No Failed Records Found!!");
            //$("#overlay").fadeOut(500);
        }
    });
}
function fn_STOWaybillErrorListDataObject() {
    debugger;
   
    $.get("/Home/STOWaybillErrorListDataObject", {}).done(function (data) {
        debugger;
        if ($.fn.dataTable.isDataTable('#stoWayBillTblId')) {
            table = $('#stoWayBillTblId').DataTable();
            table.clear();
            table.destroy();
        }
       

        if (data.length != 0) {
            $("#id_triggerbtn").css("display", "block");      
            $('#stoWayBillTblId').DataTable({
                "processing": true,
                "info": true,
                "stateSave": true,
                data: data,

                "columns": [
                    //{ data:  },
                    { data: "code" },
                    { data: "itemSku" },
                    { data: "triggerid" },
                    { data: "reason" }
                ]
            })
           
        }
        else {
            iqwerty.toast.toast("No Failed Records Found!!");
           
        }
        
    });
}

function fn_STOAPIDataObject() {
    debugger;
    //$("#overlay").fadeIn(500);
    $.get("/Home/STOAPIErrorListData", {}).done(function (data) {
        console.log(data);
        //$("#overlay").fadeOut(500);
        debugger;
        //$("a").removeClass("dropdown-toggle");
        if ($.fn.dataTable.isDataTable('#stoApiId')) {
            table = $('#stoApiId').DataTable();
            table.clear();
            table.destroy();
        }
        if (data.length != 0) {
            debugger;
            $('#stoApiId').DataTable({
                "processing": true,
                "info": true,
                "stateSave": true,
                data: data,

                "columns": [
                    { data: "code" },
                    { data: "itemSku" },
                    { data: "triggerid" },
                    { data: "reason" }
                ]
            })
            //$("#overlay").fadeOut(500);
        }
        else {
            iqwerty.toast.toast("No Failed Records Found!!");
            //$("#overlay").fadeOut(500);
        }
    });
}

function fn_UpdateShippingDataObject() {
    debugger;
    //$("#overlay").fadeIn(500);
    $.get("/Home/UpdateShippingErrorListData", {}).done(function (data) {
        console.log(data);
        //$("#overlay").fadeOut(500);
        debugger;
        //$("a").removeClass("dropdown-toggle");
        if ($.fn.dataTable.isDataTable('#UpdateShippingTblId')) {
            table = $('#UpdateShippingTblId').DataTable();
            table.clear();
            table.destroy();
        }
        if (data.length != 0) {
            debugger;
            $('#UpdateShippingTblId').DataTable({
                "processing": true,
                "info": true,
                "stateSave": true,
                data: data,
                "columns": [
                    { data: "reason" }
                ]
            })
            //$("#overlay").fadeOut(500);
        }
        else {
            iqwerty.toast.toast("No Failed Records Found!!");
            //$("#overlay").fadeOut(500);
        }
    });
}
function fn_AllocateShippingDataObject() {
    debugger;
    //$("#overlay").fadeIn(500);
    $.get("/Home/AlocateShippingErrorListData", {}).done(function (data) {
        console.log(data);
        //$("#overlay").fadeOut(500);
        debugger;
        //$("a").removeClass("dropdown-toggle");
        if ($.fn.dataTable.isDataTable('#AlocateShippingTblId')) {
            table = $('#AlocateShippingTblId').DataTable();
            table.clear();
            table.destroy();
        }
        if (data.length != 0) {
            debugger;
            $('#AlocateShippingTblId').DataTable({
                "processing": true,
                "info": true,
                "stateSave": true,
                data: data,
                "columns": [
                    { data: "reason" }
                ]
            })
            //$("#overlay").fadeOut(500);
        }
        else {
            iqwerty.toast.toast("No Failed Records Found!!");
            //$("#overlay").fadeOut(500);
        }
    });
}
function fn_ReversePickupDataObject() {
    debugger;
    //$("#overlay").fadeIn(500);
    $.get("/Home/AlocateShippingErrorListData", {}).done(function (data) {
        console.log(data);
        //$("#overlay").fadeOut(500);
        debugger;
        //$("a").removeClass("dropdown-toggle");
        if ($.fn.dataTable.isDataTable('#ReversePickupTblId')) {
            table = $('#ReversePickupTblId').DataTable();
            table.clear();
            table.destroy();
        }
        if (data.length != 0) {
            debugger;
            $('#ReversePickupTblId').DataTable({
                "processing": true,
                "info": true,
                "stateSave": true,
                data: data,
                "columns": [
                    { data: "reason" }
                ]
            })
            //$("#overlay").fadeOut(500);
        }
        else {
            iqwerty.toast.toast("No Failed Records Found!!");
            //$("#overlay").fadeOut(500);
        }
    });
}