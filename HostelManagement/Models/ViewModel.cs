using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HostelManagement.Models
{
    public class ViewModel
    {
        public int Room_no;
        public int User_id;
        public string Name;
        public string Mobile;
        public string Rent;

        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy }")]
        public Nullable<System.DateTime> Date_of_payment;
       public Nullable<int>  Amount;
    }
}