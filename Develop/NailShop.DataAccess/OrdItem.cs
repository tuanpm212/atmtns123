//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NailShop.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrdItem
    {
        public long OrdItemID { get; set; }
        public long InvoiceID { get; set; }
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public Nullable<int> ProdType { get; set; }
        public Nullable<bool> NoTax { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<int> RecordState { get; set; }
    
        public virtual Invoice Invoice { get; set; }
    }
}