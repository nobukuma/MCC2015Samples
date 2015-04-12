using Microsoft.WindowsAzure.Mobile.Service;
using System;

namespace ThermoMobileServiceService.DataObjects
{
    public class TemperatureData : EntityData
    {
        public DateTime DateTime { get; set; }

        public float Temperature { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }
    }
}