using GameServerCore;
using GameServerCore.Enums;
using GameServerCore.NetInfo;
using LeagueSandbox.GameServer.Content;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class SpawnBotCommand : ChatCommandBase
    {
        private readonly IPlayerManager _playerManager;

        public override string Command => "spawnbot";
        public override string Syntax => $"{Command} botblue, botpurple";

        public SpawnBotCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.ToLower().Split(' ');

            if (split.Length < 2)
            {
                ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
            else if (split[1].StartsWith("bot"))
            {
                split[1] = split[1].Replace("bot", "team_").ToUpper();
                if (!Enum.TryParse(split[1], out TeamId team) || team == TeamId.TEAM_NEUTRAL)
                {
                    ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.SYNTAXERROR);
                    ShowSyntax();
                    return;
                }

                SpawnChampForTeam(team, userId, "NunuBot");
            }
        }

        public void SpawnChampForTeam(TeamId team, int userId, string model)
        {
            var championPos = _playerManager.GetPeerInfo(userId).Champion.Position;

            var runesTemp = new RuneCollection();
            var clientInfoTemp = new ClientInfo("", team, 0, 0, 0, "NunuBot", new string[] { "SummonerHeal", "SummonerFlash" }, -1);
            uint num = (uint)_playerManager.GetPlayers(true).Count + 1;
            var playerTemp = new Tuple<uint, ClientInfo>(num, clientInfoTemp);

            _playerManager.AddPlayer(playerTemp);

            var c = new Champion(
                Game,
                model,
                0,
                0, // Doesnt matter at this point
                runesTemp,
                clientInfoTemp,
                team: team
            );

            clientInfoTemp.Champion = c;

            c.SetPosition(championPos, false);
            c.StopMovement();
            c.UpdateMoveOrder(OrderType.Stop);
            c.LevelUp();

            Game.PacketNotifier.NotifyS2C_CreateHero(clientInfoTemp);
            Game.PacketNotifier.NotifyAvatarInfo(clientInfoTemp);
            Game.ObjectManager.AddObject(c);
            Game.PacketNotifier.NotifyEnterLocalVisibilityClient(c, ignoreVision: true);
            Game.PacketNotifier.NotifyUpdatedStats(c, partial: false);

            c.Stats.SetSpellEnabled(13, true);
            c.Stats.SetSummonerSpellEnabled(0, true);
            c.Stats.SetSummonerSpellEnabled(1, true);

            Game.PacketNotifier.NotifyEnterVisibilityClient(c, 0, true, true);

            ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.INFO, "Spawned Bot" + clientInfoTemp.Name + " as " + c.Model + " with NetID: " + c.NetId + ".");
        }
    }
}