using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class OmsToPandoRoot
    {
        public string serviceType { get; set; }
        public string handOverMode { get; set; }
        public string returnShipmentFlag { get; set; }
        public Shipment Shipment { get; set; }
        public string deliveryAddressId { get; set; }
        public DeliveryAddressDetails deliveryAddressDetails { get; set; }
        public string pickupAddressId { get; set; }
        public PickupAddressDetails pickupAddressDetails { get; set; }
        public string returnAddressId { get; set; }
        public ReturnAddressDetails returnAddressDetails { get; set; }
        public string currencyCode { get; set; }
        public string paymentMode { get; set; }
        public string totalAmount { get; set; }
        public string collectableAmount { get; set; }
        public string courierName { get; set; }
        public CustomField customField { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public  class CustomFieldValue
    {     
        public string name { get; set; }
        public string value { get; set; }
    }
    public class addCustomFieldValue
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class DeliveryAddressDetails
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string pincode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string gstin { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string description { get; set; }
        public int? quantity { get; set; }
        public string skuCode { get; set; }
        public float? itemPrice { get; set; }
        public string imageURL { get; set; }
        public string hsnCode { get; set; }
        public string tags { get; set; }
    }

    public class PickupAddressDetails
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string pincode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string gstin { get; set; }
    }

    public class ReturnAddressDetails
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string pincode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }

    public class Shipment
    {
        public string code { get; set; }
        public string SaleOrderCode { get; set; }
        public string orderCode { get; set; }
        public string channelCode { get; set; }
        public string channelName { get; set; }
        public string invoiceCode { get; set; }
        public string orderDate { get; set; }
        public string fullFilllmentTat { get; set; }
        public string weight { get; set; }
        public string length { get; set; }
        public string height { get; set; }
        public string breadth { get; set; }
        public List<Item> items { get; set; }
        public List<CustomField> customField { get; set; }
    }

    public class RootResponse
    {
        public ErrorResponse ErrorResponse { get; set; }
        public SuccessResponse SuccessResponse { get; set; }
      
    }
    public class ErrorResponse
    {
        public string status { get; set; }
        public string reason { get; set; }
        public string message { get; set; }
    }
    public class SuccessResponse
    {
        public string status { get; set; }
        public string waybill { get; set; }
        public string shippingLabel { get; set; }
        public string courierName { get; set; }
    }
}
