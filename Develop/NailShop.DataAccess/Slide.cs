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
    
    public partial class Slide
    {
        public Slide()
        {
            this.SlideLangs = new HashSet<SlideLang>();
        }
    
        public long SlideID { get; set; }
        public int SiteID { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<SlideLang> SlideLangs { get; set; }
    }
}
