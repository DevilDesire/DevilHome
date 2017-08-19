using System.Collections.Generic;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Values;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;

namespace DevilHome.CoreServer.Controllers.Raum
{
    [RestController(InstanceCreationType.Singleton)]
    public class RaumController : ControllersBase
    {
        [UriFormat("/raum")]
        public IGetResponse GetAllRaeume()
        {
            List<IRoom> raeume = DbRoom.GetAllValues();
            List<Room> convertedValues = new List<Room>();
            raeume.ForEach(x => convertedValues.Add(new Room
            {
                Id = x.Id,
                Name = x.Name,
                Date = x.Date
            }));

            return new GetResponse(GetResponse.ResponseStatus.OK, convertedValues);
        }
    }
}