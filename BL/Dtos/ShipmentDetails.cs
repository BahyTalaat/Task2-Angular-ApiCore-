using ShipmentService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dtos
{
   public class ShipmentDetails
    {
        public string APIKey { get; set; }
        public decimal cODAmount { get; set; }

        public decimal declaredValue { get; set; }
        public string customerData { get; set; }
        public QuoteType quoteType { get; set; }

        public ServicePoint servicePoint { get; set; }
        public ServicePoint Destination { get; set; }

        public Item[] items { get; set; }
        public RQAccessorial[] accessorials { get; set; }

        public OverDimension[] overDimensions { get; set; }

       public Pallet[] pallets { get; set; }






    }
}
