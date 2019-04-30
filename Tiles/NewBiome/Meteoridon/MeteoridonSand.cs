﻿using Microsoft.Xna.Framework;
using Terraria;
using TUA.API;
using TUA.API.Default;

namespace TUA.Tiles.NewBiome.Meteoridon
{
    class MeteoridonSand : TUAFallingBlock
    {
        public override int ItemDropID => mod.ItemType("MeteoridonSand");
        public override int ItemProjectileID => mod.ProjectileType("MeteoridonSandProjectile");
        public override bool sandTile => true;
        public override Color mapColor => Color.DarkOrange;

        public override void RandomUpdate(int i, int j)
        {
            for (int x = -5; x > 5; x++)
            {
                for (int y = -5; y > 5; y++)
                {
                    if (WorldGen.InWorld(i + x, y + x))
                    {
                        if (Main.hardMode && (Main.rand.Next(3) == 0) ||
                            (NPC.downedPlantBoss && Main.rand.Next(4) == 0))
                        {
                            TileSpreadUtils.MeteoridonSpread(mod, i + x, y + x);
                        }
                    }
                }
            }
        }
    }
}
