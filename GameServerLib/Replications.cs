using System;
using System.Collections.Generic;
using LeaguePackets.Game;
using LeaguePackets.Game.Common;
using LeagueSandbox.GameServer.Content.Navigation;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using PacketDefinitions420;

namespace GameServerLib;

internal static class Replications
{
    internal static Dictionary<int, List<ReplicationData>> HeldReplicationData;
    internal static Dictionary<int, List<MovementDataNormal>> HeldMovementData;
    internal static NavigationGrid NavGrid;

    static Replications()
    {
        HeldReplicationData = [];
        HeldMovementData = [];
    }
    
    /// <summary>
    /// Creates a package and puts it in the queue that will be emptied with the NotifyOnReplication call.
    /// </summary>
    /// <param name="u">Unit who's stats have been updated.</param>
    /// <param name="userId">UserId to send the packet to. If not specified or zero, the packet is broadcasted to all players that have vision of the specified unit.</param>
    /// <param name="partial">Whether or not the packet should only include stats marked as changed.</param>
    internal static void HoldReplicationDataUntilOnReplicationNotification(AttackableUnit u, int userId, bool partial = true)
    {
        var data = u.Replication.GetData(partial);

        if (!HeldReplicationData.TryGetValue(userId, out var list))
        {
            HeldReplicationData[userId] = list = [];
        }
        list.Add(data);
    }
    
    /// <summary>
    /// Creates a package and puts it in the queue that will be emptied with the NotifyWaypointGroup call.
    /// </summary>
    /// <param name="u">AttackableUnit that is moving.</param>
    /// <param name="userId">UserId to send the packet to. If not specified or zero, the packet is broadcasted to all players that have vision of the specified unit.</param>
    /// <param name="useTeleportID">Whether or not to teleport the unit to its current position in its path.</param>
    internal static void HoldMovementDataUntilWaypointGroupNotification(AttackableUnit u, int userId, bool useTeleportID = false)
    {
        var data = PacketExtensions.CreateMovementDataNormal(u, NavGrid, useTeleportID);

        if (!HeldMovementData.TryGetValue(userId, out var list))
        {
            HeldMovementData[userId] = list = [];
        }
        list.Add(data);
    }
    
    /// <summary>
    /// Sends all packets queued by HoldReplicationDataUntilOnReplicationNotification and clears queue.
    /// </summary>
    internal static void NotifyReplications()
    {
        foreach (var kv in HeldReplicationData)
        {
            int userId = kv.Key;
            var list = kv.Value;

            if (list.Count <= 0)
            {
                continue;
            }
            
            
            
            var packet = new OnReplication()
            {
                SyncID = (uint)Environment.TickCount,
                ReplicationData = list
            };

            //_packetHandlerManager.SendPacket(userId, packet.GetBytes(), Channel.CHL_LOW_PRIORITY, PacketFlags.UNSEQUENCED);

            list.Clear();
        }
    }
}