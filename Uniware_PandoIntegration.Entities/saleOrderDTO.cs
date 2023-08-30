using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class Address
    {
        public string Code { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public object district { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
        public string phone { get; set; }
        public object email { get; set; }
        public object type { get; set; }
    }

    public class BillingAddress
    {
        public string id { get; set; }
        public string name { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public object district { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
        public string phone { get; set; }
        public object email { get; set; }
        public object type { get; set; }
    }

    //public class CustomFieldValue
    //{
    //    public string fieldName { get; set; }
    //    public object fieldValue { get; set; }
    //    public string valueType { get; set; }
    //    public string displayName { get; set; }
    //    public bool required { get; set; }
    //    public object possibleValues { get; set; }
    //}

    public class Items
    {
        public string itemSku { get; set; }
        public string itemName { get; set; }
        public object itemTypeImageUrl { get; set; }
        public string itemTypePageUrl { get; set; }
        public int? quantity { get; set; }
        public string Code { get; set;}
    }

    public class salesorderRoot
    {
     
        public SaleOrderDTO saleOrderDTO { get; set; }
       
    }

    public class SaleOrderDTO
    {
        public string code { get; set; }
        public string displayOrderCode { get; set; }
        public string channel { get; set; }
        public string source { get; set; }
        public long displayOrderDateTime { get; set; }
        public string status { get; set; }
        public long created { get; set; }
        public long updated { get; set; }
        public long fulfillmentTat { get; set; }
        public string notificationEmail { get; set; }
        public string notificationMobile { get; set; }
        public object customerGSTIN { get; set; }
        public long channelProcessingTime { get; set; }
        public bool cod { get; set; }
        public bool thirdPartyShipping { get; set; }
        public int? priority { get; set; }
        public string currencyCode { get; set; }
        public object customerCode { get; set; }
        public BillingAddress billingAddress { get; set; }
        public List<Address> addresses { get; set; }
        public List<ShippingPackage> shippingPackages { get; set; }
        public List<SaleOrderItem> saleOrderItems { get; set; }
        public List<object> returns { get; set; }
        public List<CustomFieldValue> customFieldValues { get; set; }
        public bool cancellable { get; set; }
        public bool reversePickable { get; set; }
        public bool packetConfigurable { get; set; }
        public bool cFormProvided { get; set; }
        public object totalDiscount { get; set; }
        public object totalShippingCharges { get; set; }
        public object additionalInfo { get; set; }
        public object paymentInstrument { get; set; }
        public object paymentDetail { get; set; }
    }

    public class SaleOrderItem
    {
        public string Code { get; set; }
        public int? id { get; set; }
        public string shippingPackageCode { get; set; }
        public string shippingPackageStatus { get; set; }
        public string facilityCode { get; set; }
        public string facilityName { get; set; }
        public object alternateFacilityCode { get; set; }
        public object reversePickupCode { get; set; }
        public int? shippingAddressId { get; set; }
        public int? packetNumber { get; set; }
        public object combinationIdentifier { get; set; }
        public object combinationDescription { get; set; }
        public string type { get; set; }
        public string item { get; set; }
        public string shippingMethodCode { get; set; }
        public string itemName { get; set; }
        public string itemSku { get; set; }
        public string sellerSkuCode { get; set; }
        public string channelProductId { get; set; }
        public object imageUrl { get; set; }
        public string statusCode { get; set; }
        public string code { get; set; }
        public string shelfCode { get; set; }
        public double? totalPrice { get; set; }
        public double? sellingPrice { get; set; }
        public double? shippingCharges { get; set; }
        public double? shippingMethodCharges { get; set; }
        public double? cashOnDeliveryCharges { get; set; }
        public double? prepaidAmount { get; set; }
        public object voucherCode { get; set; }
        public double? voucherValue { get; set; }
        public double? storeCredit { get; set; }
        public double? discount { get; set; }
        public object giftWrap { get; set; }
        public double? giftWrapCharges { get; set; }
        public double? taxPercentage { get; set; }
        public object giftMessage { get; set; }
        public bool cancellable { get; set; }
        public bool editAddress { get; set; }
        public bool reversePickable { get; set; }
        public bool packetConfigurable { get; set; }
        public long created { get; set; }
        public long updated { get; set; }
        public bool onHold { get; set; }
        public object saleOrderItemAlternateId { get; set; }
        public object cancellationReason { get; set; }
        public object cancelledBySeller { get; set; }
        public string pageUrl { get; set; }
        public string color { get; set; }
        public string brand { get; set; }
        public string size { get; set; }
        public object replacementSaleOrderCode { get; set; }
        public object bundleSkuCode { get; set; }
        public List<object> customFieldValues { get; set; }
        public List<object> itemDetailFieldDTOList { get; set; }
        public string hsnCode { get; set; }
        public double? totalIntegratedGst { get; set; }
        public double? integratedGstPercentage { get; set; }
        public double? totalUnionTerritoryGst { get; set; }
        public double? unionTerritoryGstPercentage { get; set; }
        public double? totalStateGst { get; set; }
        public double? stateGstPercentage { get; set; }
        public double? totalCentralGst { get; set; }
        public double? centralGstPercentage { get; set; }
        public object maxRetailPrice { get; set; }
        public double? sellingPriceWithoutTaxesAndDiscount { get; set; }
        public object batchDTO { get; set; }
        public object shippingChargeTaxPercentage { get; set; }
        public double? tcs { get; set; }
        public object ucBatchCode { get; set; }
        public object channelMrp { get; set; }
        public object channelExpiryDate { get; set; }
        public object channelVendorBatchNumber { get; set; }
        public object channelMfd { get; set; }
        public object countryOfOrigin { get; set; }
        public object expectedDeliveryDate { get; set; }
        public object itemDetailFields { get; set; }
        public string channelSaleOrderItemCode { get; set; }
        public object effectiveTolerance { get; set; }
    }

    public class ShippingPackage
    {
        public string code { get; set; }
        public object channelShipmentCode { get; set; }
        public string saleOrderCode { get; set; }
        public string channel { get; set; }
        public string status { get; set; }
        public string shippingPackageType { get; set; }
        public string shippingProvider { get; set; }
        public object shippingCourier { get; set; }
        public string shippingMethod { get; set; }
        public string trackingNumber { get; set; }
        public object trackingStatus { get; set; }
        public string courierStatus { get; set; }
        public double? estimatedWeight { get; set; }
        public double? actualWeight { get; set; }
        public string customer { get; set; }
        public long? created { get; set; }
        public long? updated { get; set; }
        public long? dispatched { get; set; }
        public object delivered { get; set; }
        public int? invoice { get; set; }
        public string invoiceCode { get; set; }
        public string invoiceDisplayCode { get; set; }
        public long? invoiceDate { get; set; }
        public int? noOfItems { get; set; }
        public string city { get; set; }
        public double? collectableAmount { get; set; }
        public object collectedAmount { get; set; }
        public bool paymentReconciled { get; set; }
        public object podCode { get; set; }
        public string shippingManifestCode { get; set; }
        public Items items { get; set; }
        public List<object> customFieldValues { get; set; }
        public string shippingLabelLink { get; set; }
        public object irn { get; set; }
    }
    public class parentList
    {
        public List<Address> address { get; set; }
        public List<SaleOrderItem> saleOrderItems { get; set; }
        public List<ShippingPackage> Shipment { get; set; }
        public List<Items> qtyitems { get; set; }
        public List<SaleOrderDTO> elements { get; set; }
    }
  

}
