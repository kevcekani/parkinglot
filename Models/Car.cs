using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Parking.Models
{
    public class Car
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Car Plate")]
        public string CarPlate { get; set; }

        [Display(Name = "Driver ID")]
        public string OwnerID { get; set; }

        [Display(Name = "Time of entry")]
        public DateTime EnterDate { get; set; }

        public string Hour { get; set; }

        public bool CheckInOut { get; set; }

    }
}