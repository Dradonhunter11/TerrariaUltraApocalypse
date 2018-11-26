using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using TerrariaUltraApocalypse.API;
using TerrariaUltraApocalypse.NPCs.Gods.EoA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace TerrariaUltraApocalypse.Items.EoA
{
    class Spawner : TUAModLegacyItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Apoclypsio");
            Tooltip.SetDefault("Look in the sky! The moon have changed...\nSummon the god of destruction");
            DisplayName.AddTranslation(GameCulture.French, "Apoclypsio");
            Tooltip.AddTranslation(GameCulture.French, "Invoque le premier des anciens dieux\nUne fois vaincu, votre monde sera en mode Ultra");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 4;
            item.rare = 2;
            item.stack = 20;
            item.UseSound = SoundID.Item1;
            item.consumable = false;

            
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            //if (downedMoonlord && TerrariaUltraApocalypse.EoCDeath >= 10)
            //{
            if (!Main.expertMode)
            {
                Main.expertMode = true;
            }

            if (Main.netMode != 1)
            {
                Main.NewText("The apocalypse is coming, be aware...", Color.DarkGoldenrod);
            }
            else
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("The apocalypse is coming, be aware..."), Color.DarkGoldenrod);
            }
            Main.PlaySound(SoundID.MoonLord, player.position, 0);
            NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - (76*16), mod.NPCType<Eye_of_ApocalypseNew>());
            Main.spriteBatch.Draw(ModLoader.GetTexture("Projectile_490"), new Vector2(player.Center.X, player.Center.Y), Color.DarkRed);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddIngredient(ItemID.SuspiciousLookingEye, 5);
            recipe.AddIngredient(ItemID.MechanicalEye, 1);
            recipe.AddIngredient(null, "SuspiciousBurnedEye", 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
