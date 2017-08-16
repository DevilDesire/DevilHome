using System;
using System.Collections.Generic;
using System.Linq;
using DevilHome.Common.Interfaces.Values;

namespace DevilHome.Controller.ApiV2.Poweroutlet
{
    internal class PoweroutletSetter : ControllerBase
    {
        public string CreateNewPoweroutlet(IQueryValue queryValue)
        {
            try
            {
                List<string> parameters = queryValue.Action.Split(';').ToList();

                if (parameters.Count != 4)
                {
                    throw new Exception("Not enough parameters!");
                }

                DbPoweroutlet.Insert(new DevilHome.Database.Implementations.Values.Poweroutlet
                {
                    Name = parameters[0],
                    Fk_Raum_Id = Convert.ToInt32(parameters[1]),
                    HausCode = parameters[2],
                    DeviceCode = parameters[3]
                });
            }
            catch (Exception ex)
            {
                return string.Format("ERROR: {0}", ex.Message);
            }

            return "SUCCESS";
        }
    }
}