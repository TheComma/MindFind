//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MindFind_V1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Photos
    {
        public int Photo_id { get; set; }
        public Nullable<int> Tag_id { get; set; }
        public string ImagePath { get; set; }
    
        public virtual Tags Tags { get; set; }
    }
}