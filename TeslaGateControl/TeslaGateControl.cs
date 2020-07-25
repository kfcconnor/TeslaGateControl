using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled;


namespace TeslaGateControl
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Player = Exiled.Events.Handlers.Player;
    

    public class TeslaPlugin : Plugin<config>
    {
        public eventHandlers eHandler;
        public bool AllowCards;
        public List<ItemType> allowedCards;
        public List<RoleType> allowedRoles;
        public override void OnEnabled()
        {
            AllowCards = Config.cardMode;
            if (AllowCards)
            {
                allowedCards = Config.allowedCards;
                eHandler = new eventHandlers(allowedCards);
            }
            else
            {
                allowedRoles = Config.allowedRoles;
                eHandler = new eventHandlers(allowedRoles);
            }
            Player.TriggeringTesla += eHandler.PlayerTesla;
            Log.Info("Tesla Gate Control V1.0.0 has started Successfully.");
        }

        public override void OnDisabled()
        {
            // Make it dynamically updatable.
            // We do this by removing the listener for the event and then nulling the event handler.
            // The more events the more times you have to do this for each one.
            Player.TriggeringTesla -= eHandler.PlayerTesla;
            eHandler = null;
        }


    }

    public class eventHandlers
    {
        private bool allowCard;
        private List<ItemType> AllowedTypes;
        private List<RoleType> allowedRoles;
        public eventHandlers(List<RoleType> aRoles)
        {
            allowedRoles = aRoles;
            allowCard = false;
        }
        public eventHandlers(List<ItemType> aType)
        {
            AllowedTypes = aType;
            allowCard = true;
        }

        public void PlayerTesla(TriggeringTeslaEventArgs ev)
        {
            if (allowCard)
            {
                bool validCard = false;
                var playerInventory = ev.Player.Inventory.items;
                if (playerInventory != null)
                {
                    foreach (var item in playerInventory)
                    {
                        if (AllowedTypes.Count > 0 && !AllowedTypes.Contains(item.id))
                            continue;
                        else if (AllowedTypes.Contains(item.id))
                        {
                            validCard = true;
                            break;
                        }
                           
                    }
                }
                if (validCard)
                {
                    ev.IsTriggerable = false;
                }
            }


            if (allowedRoles != null)
            {
                foreach (RoleType role in allowedRoles)
                {
                    if (ev.Player.Role == role)
                    {
                        ev.IsTriggerable = false;
                    }
                }
            }

        }
        
    }

}
