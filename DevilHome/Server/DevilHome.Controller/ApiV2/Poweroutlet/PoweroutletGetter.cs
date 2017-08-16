using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevilHome.Common.Interfaces.Values;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Controller.ApiV2.Poweroutlet
{
    internal class PoweroutletGetter : ControllerBase
    {
        public async Task<string> ProcessingGetRequest(IQueryValue queryValue)
        {
            switch (queryValue.GetModifier.ToLower())
            {
                case "room":
                    return GetPoweroutletsByRoomId(Convert.ToInt32(queryValue.Action));
                default:
                    return "";
            }
        }

        private string GetPoweroutletsByRoomId(int roomId)
        {
            List<IPoweroutlet> poweroutlets = DbPoweroutlet.GetValuesByRoomId(roomId);
            return ConvertToJson(poweroutlets);
        }

    }
}