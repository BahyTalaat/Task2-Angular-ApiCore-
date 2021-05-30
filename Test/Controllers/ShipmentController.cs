using BL.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipmentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using ServicePoint = ShipmentService.ServicePoint;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {

        [HttpPost("Rate")]
        public async Task<IActionResult> Rate(ShipmentDetails shipmentDetails)
        {

            RateQuoteServiceSoapClient.EndpointConfiguration context = new RateQuoteServiceSoapClient.EndpointConfiguration();
            RateQuoteServiceSoapClient client = new RateQuoteServiceSoapClient(context);


            RateQuoteRequest request = new RateQuoteRequest();
             request.CODAmount = shipmentDetails.cODAmount;
            request.DeclaredValue = shipmentDetails.declaredValue;
            request.CustomerData = shipmentDetails.customerData;

            request.QuoteType = shipmentDetails.quoteType;
             

            ServicePoint origin = new ServicePoint { City = shipmentDetails.servicePoint.City, CountryCode = shipmentDetails.servicePoint.CountryCode,
                  StateOrProvince = shipmentDetails.servicePoint.StateOrProvince, ZipOrPostalCode = shipmentDetails.servicePoint.ZipOrPostalCode };
            request.Origin = origin;

            ServicePoint servicePointDes = new ServicePoint
            {
                City = shipmentDetails.Destination.City,
                CountryCode = shipmentDetails.Destination.CountryCode,
                StateOrProvince = shipmentDetails.Destination.StateOrProvince,
                ZipOrPostalCode = shipmentDetails.Destination.ZipOrPostalCode
            };

            request.Destination = servicePointDes;


            //Item[] items = new Item[]
            //   {
            //      new Item { Class = "sdad", Height = 18, Length = 58, Weight = 250, Width = 46 },
            //      new Item { Class = "sdad", Height = 18, Length = 58, Weight = 250, Width = 46 }

            //   };
            request.Items = shipmentDetails.items;

            //RQAccessorial[] accessorials = new RQAccessorial[]
            //  {
            //      RQAccessorial.DoorToDoor,
            //      RQAccessorial.InsideDelivery

            //  };

            request.Accessorials = shipmentDetails.accessorials;


            //OverDimension[] overDimensions = new OverDimension[]
            //  {
            //     new OverDimension{Inches=12,Pieces=56},
            //     new OverDimension{Inches=63,Pieces=86}
            //  };

            request.OverDimensionList = shipmentDetails.overDimensions;

            //Pallet[] pallets = new Pallet[]
            // {
            //     new Pallet{Code="a456",Quantity=20,Weight=150},
            //     new Pallet{Code="e123",Quantity=25,Weight=250}
            // };
            request.Pallets = shipmentDetails.pallets;





            ////1-object from request
            var res = client.GetRateQuoteAsync(shipmentDetails.APIKey, request);

            return Ok(res);
        }
    }
}
