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
    
    public partial class Photo
    {
        public int Photo_id { get; set; }
        public byte[] ImageReference { get; set; }
        public Nullable<int> Tag_id { get; set; }
    
        public virtual Tag Tag { get; set; }
    }
}
