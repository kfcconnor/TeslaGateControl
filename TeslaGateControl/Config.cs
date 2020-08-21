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
        //Determines if Holding an active card would allow for the gate to be held open for an amount of time
        public bool cardSwipe { get; set; } = false;
        //lists allowed roles that can get through the gate without setting it off. IDs or names can be used 
        public List<RoleType> allowedRoles { get; set; } = new List<RoleType>();
        //lists allowed Items that can get through the gate without setting it off. IDs or names can be used
        public List<ItemType> allowedCards { get; set; } = new List<ItemType>();
        //sets the amount of time the gate will be held if the card swipe is active
        public int cardHoldTime { get; set; } = 5;

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
