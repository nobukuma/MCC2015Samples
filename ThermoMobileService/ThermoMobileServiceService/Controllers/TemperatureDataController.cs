using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using ThermoMobileServiceService.DataObjects;
using ThermoMobileServiceService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace ThermoMobileServiceService.Controllers
{
    //[AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class TemperatureDataController : TableController<TemperatureData>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ThermoMobileServiceContext context = new ThermoMobileServiceContext();
            DomainManager = new EntityDomainManager<TemperatureData>(context, Request, Services);
        }

        // GET tables/TemperatureData
        public IQueryable<TemperatureData> GetAllTemperatureData()
        {
            return Query();
        }

        // GET tables/TemperatureData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TemperatureData> GetTemperatureData(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TemperatureData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TemperatureData> PatchTemperatureData(string id, Delta<TemperatureData> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TemperatureData
        public async Task<IHttpActionResult> PostTemperatureData(TemperatureData item)
        {
            TemperatureData current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TemperatureData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTemperatureData(string id)
        {
            return DeleteAsync(id);
        }
    }
}