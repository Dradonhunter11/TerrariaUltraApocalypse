using System;
using DiscordRPC;
using Terraria;
using Terraria.Utilities;
using TUA.API.Dev;
using TUA.Utilities;

namespace TUA.Discord
{
    public static class DRPSystem
    {
        private static RichPresence _presence;

        private static DiscordRpcClient _client;

        // public static DiscordRpcClient Client => _client;

        private static string _currentState;

        static DRPSystem()
        {
	        InitMessages();
        }

        private static void InitMessages()
        {
			StaticManager<DRPBossMessage>.AddItem("EoA", new DRPBossMessage(
				"The Death of a God",
				"has beaten the Eye of Apocalypse",
				delegate { return TUAWorld.EoADowned; }
			));
            StaticManager<DRPBossMessage>.AddItem("ApoMoon", new DRPBossMessage(
                "The Destruction of the Moon",
                "skygazed beneath the Apocalypse Moon",
                delegate { return TUAWorld.ApoMoonDowned; }));
        }

        public static void Boot()
        {
	        UnifiedRandom rand = new UnifiedRandom();
            _currentState = Main.netMode == 0
                ? rand.Next(new string[] { "Playing Alone", "Lone Samurai", "Singleplayer" })
                : rand.Next(new string[] { "Playing With Friends", "Multiplayer" });

            _presence = new RichPresence()
            {
                Details = $"In Main Menu ({(Environment.Is64BitProcess ? "64" : "32")}bit)",
                State = (SteamID64Checker.Instance.VerifyDevID() && TUA.devMode)
                    ? "Debugging/Developing" : _currentState,
                Assets = new Assets()
                {
                    LargeImageKey = "logo",
                    LargeImageText = "Loading mods"
                }
            };
            if (Main.netMode != 0)
            {
                _presence.Party = new Party()
                {
                    Size = Main.ActivePlayersCount,
                    Max = Main.maxNetPlayers
                };
            }
            _client = new DiscordRpcClient("528086919670792233", SteamID64Checker.CurrentSteamID64, true, -1);
            // _client.OnError += (sender, args) => { TUA.instance.Logger.ErrorFormat("Rich Presence failed. Code {1}, {0}", args.Message, args.Code); };
            _presence.Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow,
            };
            _client.Initialize();
            _client.SetPresence(_presence);
        }

        // We should get some images pertaining to each boss
        // client.UpdateLargeAsset("EoC logo", Main.rand.NextBool() ? "Playing TUA" : "The start of a new day");
        public static void Update()
        {
	        if (_client == null)
		        return;

            // Runs through all of discord-rpc's logging stuff, basically
            _client.Invoke();


            _presence.Assets.LargeImageKey = "logo";
            if (!Main.gameMenu)
            {
                _presence.Details = "Playing Terraria";
                /*if (Main.LocalPlayer.GetModPlayer<DimPlayer>().getCurrentDimension() == "Solar")
                {
                    presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "Exploring solar";
                    presence.Details = Main.LocalPlayer.name + " is exploring the solar dimension";
                }*/

                DRPBossMessage validMessage = null;
		        var list = StaticManager<DRPBossMessage>.GetItems();
                for (int k = 0; k < list.Length; k++)
                {
			        var msg = list[k];
	                if (msg.Item3.CanCall())
                    {
                        validMessage = msg.Item3;
                        goto FoundValidMessage;
                    }
				}
                FoundValidMessage:
                {
                    _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : validMessage.Header);
                    _client.UpdateDetails(Main.LocalPlayer.name + " " + validMessage.Message);
                }
				
				/*if (!Main.npc.Any(i => i.boss) && !MoonEventManagerWorld.moonEventList.Any(i => i.Value.IsActive))
                {
                    if (TUAWorld.EoADowned)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The death of a god");
                        client.UpdateDetails(Main.LocalPlayer.name + " has beaten the eye of apocalypse");
                    }
                    else if (TUAWorld.ApoMoonDowned)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The destruction of the moon");
                        client.UpdateDetails(Main.LocalPlayer.name + " has beaten the apocalypse moon");
                    }
                    else if (TUAWorld.EoCPostMLDowned)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The revenge of the eye");
                        client.UpdateDetails(Main.LocalPlayer.name + " has beaten the ultra eye of cthulhu");
                    }
                    else if (NPC.downedMoonlord)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Before the lord, there were the god");
                        client.UpdateDetails(Main.LocalPlayer.name + " has beaten the moon lord");
                    }
                    else if (NPC.downedGolemBoss)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The ancient cult of the moon");
                        client.UpdateDetails(Main.LocalPlayer.name + " is before the cultist");
                    }
                    else if (NPC.downedPlantBoss)
                    {
                        client.UpdateLargeAsset(Main.rand.NextBool() ? "Playing TUA" : "The legends of a lizhard divinity");
                        client.UpdateDetails(Main.LocalPlayer.name + " is before the golem");
                    }
                    else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The jungle arise");
                        client.UpdateDetails(Main.LocalPlayer.name + " is before plantera");
                    }
                    else if (NPC.downedMechBossAny && !(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The mechanical arise");
                        client.UpdateDetails(Main.LocalPlayer.name + " is after the death of a mechanical boss");
                    }
                    else if (Main.hardMode)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "A new era of challenge");
                        client.UpdateDetails(Main.LocalPlayer.name + " is in hardmode");
                    }
                    else if (!Main.hardMode)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Preparing for the initial raids");
                        client.UpdateDetails(Main.LocalPlayer.name + " is close to hardmode");
                    }
                    else if (!NPC.downedBoss3)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Trying to help the old man");
                        client.UpdateDetails(Main.LocalPlayer.name + " hasn't entered the Dungeon");
                    }
                    else if (!NPC.downedBoss2)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Preparing to fight the evil");
                        client.UpdateDetails(Main.LocalPlayer.name + " hasn't purged their world");
                    }
                    else if (!NPC.downedBoss1)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The start of a new day");
                        client.UpdateDetails(Main.LocalPlayer.name + " is preparing for EoC");
                    }
                }
                else
                {
                    if (Main.npc.Any(i => i.type == TUA.instance.NPCType("Eye_of_Apocalypse")))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The incarnated destruction");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the eye of azathoth - god of destruction");
                    }
                    else if (Main.npc.Any(i => i.type == TUA.instance.NPCType("UEoC")))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The fallen eyes");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Ultra Eye of Cthulhu");
                    }
                    else if (NPC.LunarApocalypseIsUp)
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The dimension started to clash");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the lunar apocalypse");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.MoonLordCore))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The lord of the final frontier");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Moon Lord");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.CultistBoss))
                    {
                        client.UpdateLargeAsset(null, "The psychotic ritual");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Lunatic Cultist");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.Golem))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The lizhard divinity");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Golem");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.Plantera))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The jungle terror");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting Plantera");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.TheDestroyer))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The mechanical worm");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the destroyer");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.Retinazer) || (Main.npc.Any(i => i.type == NPCID.Spazmatism)))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The mechanical eyes");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Twins");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.SkeletronPrime))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The mechanical skeleton");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting Skeletron Prime");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.WallofFlesh))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The great wall made of flesh!");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Wall of Flesh");
                    }
                    else if (Main.npc.Any(i => i.type == TUA.instance.NPCType("HeartOfTheWasteland") && i.boss)
                    ) //There is a chance the the heart will be asleep, so making sure
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The ruined amalgamate");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Heart of the Wasteland");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.SkeletronHead))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The cursed man");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting Skeletron");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.QueenBee))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "NOT THE BEES");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Queen Bee");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.EaterofWorldsHead))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The corrupted abomination");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Eater of the World");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.BrainofCthulhu))
                    {
                        client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The bloody brain");
                        client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Brain of Cthulhu");
                    }
                }*/
                


                // presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "exploring the wasteland";
                // presence.Details = Main.LocalPlayer.name + " is currently in the wasteland";
                // client.SetPresence(presence);
            }
            else
            {
                _presence.Details = $"In Main Menu ({(Environment.Is64BitProcess ? "64" : "32")}bit)";
                _presence.State = (SteamID64Checker.Instance.VerifyDevID() && TUA.devMode)
                    ? "Debugging/Developing" : "Doing nothing";

                _presence.Assets.LargeImageText = "Doing nothing";
                _client.SetPresence(_presence);
            }

        }

        public static void Kill()
        {
            _client.UpdateEndTime(DateTime.UtcNow);
            _client.Dispose();
        }
    }
}