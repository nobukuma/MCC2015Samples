using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using ThermoMobileServiceService.DataObjects;
using ThermoMobileServiceService.Models;

namespace ThermoMobileServiceService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            Database.SetInitializer(new ThermoMobileServiceInitializer());
        }
    }

    public class ThermoMobileServiceInitializer : ClearDatabaseSchemaIfModelChanges<ThermoMobileServiceContext>
    {
        protected override void Seed(ThermoMobileServiceContext context)
        {
            List<TemperatureData> temperatureData = new List<TemperatureData>
            {
                new TemperatureData { Id = Guid.NewGuid().ToString(), DateTime = new DateTime(2015, 1, 31, 13, 0, 0), Temperature = 25.0f, Latitude = 35.0f, Longitude = 135.0f, },
                new TemperatureData { Id = Guid.NewGuid().ToString(), DateTime = new DateTime(2015, 1, 31, 14, 0, 0), Temperature = 26.0f, Latitude = 35.0f, Longitude = 135.0f, },
            };

            foreach (TemperatureData item in temperatureData)
            {
                context.Set<TemperatureData>().Add(item);
            }

            base.Seed(context);
        }
    }
}

