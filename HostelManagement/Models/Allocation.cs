//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HostelManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Allocation
    {
        public Nullable<int> Room_no { get; set; }
        public Nullable<int> User_id { get; set; }
        public Nullable<System.DateTime> Date_of_allocation { get; set; }
        public int Id { get; set; }
    
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
