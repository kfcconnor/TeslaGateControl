using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled;
using MEC;


namespace TeslaGateControl
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Player = Exiled.Events.Handlers.Player;
    using Server = Exiled.Events.Handlers.Server;
    

    public class TeslaPlugin : Plugin<config>
    {
        public eventHandlers eHandler;

        public override void OnEnabled()
        {
            eHandler = new eventHandlers(this);
            Server.RoundStarted += eHandler.onRoundStart;
            Player.TriggeringTesla += eHandler.PlayerTesla;
            Log.Info("Tesla Gate Control V1.1.0 'The Swipening' has started Successfully.");
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
        public TeslaPlugin plugin;
        public config Config;
        public bool gateLock;
        public List<lockedTesla> lt;
        public CoroutineHandle coroutine;


        public eventHandlers(TeslaPlugin plugin)
        {
            this.plugin = plugin;
            Config = plugin.Config;
        }

        public void onRoundStart()
        {
            lockedTesla temp;
            lt = new List<lockedTesla>();
            foreach (Room room in Map.Rooms)
            {
                if (room.Name.Contains("Tesla"))
                {
                    temp = new lockedTesla();
                    temp.roomName = room.Name;
                    temp.locked = false;
                    lt.Add(temp);
                }
            }

        }

        public int gateId(string currentRoom)
        {
            int roomId = 0;

            foreach (lockedTesla r in lt)
            {
                if (r.roomName == currentRoom)
                {
                    break;
                }
                roomId++;
            }

            return roomId;
        }

        public void PlayerTesla(TriggeringTeslaEventArgs ev)
        {
            string currentTelsa = ev.Player.CurrentRoom.Name;
            if (Config.cardSwipe)
            {
                foreach(lockedTesla r in lt)
                {
                    if (r.locked && currentTelsa == r.roomName)
                    {
                        ev.IsTriggerable = false;
                        return;
                    }
                       
                }
            }
            switch (Config.cardMode)
            {
                case (true):
                    {
                        bool validCard = false;
                        var playerHeldItem = ev.Player.CurrentItem;
                        var playerInventory = ev.Player.Inventory.items;
                        if (playerInventory != null)
                        {
                            
                            foreach (var item in playerInventory)
                            {
                                if (Config.allowedCards.Count > 0 && !Config.allowedCards.Contains(item.id))
                                    continue;
                                else if (Config.allowedCards.Contains(item.id))
                                {
                                    validCard = true;
                                    break;
                                }

                            }
                            if (Config.cardSwipe)
                            {
                                if (Config.allowedCards.Contains(playerHeldItem.id))
                                {
                                    int id = gateId(currentTelsa);
                                    coroutine = Timing.RunCoroutine(teslaSwipe(id));
                                }
                            }
                        }
                        if (validCard)
                        {
                            ev.IsTriggerable = false;
                        }
                        break;
                    }
                case (false):
                    {
                        if (Config.allowedRoles != null)
                        {
                            foreach (RoleType role in Config.allowedRoles)
                            {
                                if (ev.Player.Role == role)
                                {
                                    ev.IsTriggerable = false;
                                }
                            }
                        }
                        break;
                    }
            }
            

        }

        public IEnumerator<float> teslaSwipe(int id)
        {
            lockedTesla rtl = lt[id];
            rtl.locked = true;
            lt[id] = rtl;
            yield return Timing.WaitForSeconds(Config.cardHoldTime);
            rtl.locked = false;
            lt[id] = rtl;
            
        }

    }

    public struct lockedTesla
    {
        public string roomName;
        public bool locked;
    }

}
