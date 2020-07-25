using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled;

namespace TeslaGateControl
{
    using Exiled.API.Interfaces;
    using Log = Exiled.API.Features.Log;
    public class config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        //Switches between Role determining who avoids the tesla gate and Card determining who avoids the tesla gate
        public bool cardMode { get; set; } = false;
        //lists allowed roles that can get through the gate without setting it off. IDs or names can be used 
        public List<RoleType> allowedRoles { get; set; } = new List<RoleType>();
        //lists allowed Items that can get through the gate without setting it off. IDs or names can be used
        public List<ItemType> allowedCards { get; set; } = new List<ItemType>();

        public void ConfigValidator()
        {
            foreach(ItemType i in allowedCards)
            {
                int id = (int) i;
                if (id > 7 || id < 0)
                {
                    Log.Warn("Only Card IDs can be used here. Restoring Default Config for Allowed Cards");
                    allowedCards = new List<ItemType>();
                }
            }
        }
    }
}
