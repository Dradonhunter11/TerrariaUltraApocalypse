﻿using Terraria;
using Terraria.ModLoader;

namespace TUA.Dusts.EotW
{
    class MagicDust : ModDust
    {
        private int timer = 40;


        public override void OnSpawn(Terraria.Dust dust)
        {
            dust.noGravity = true;
            dust.scale *= 1f;
            dust.velocity.Y = Main.rand.Next(1, 2) * 0.7f;
            dust.alpha = 50;
        }

        public override bool MidUpdate(Terraria.Dust dust)
        {
            dust.position += dust.velocity;
            timer--;
            if (timer == 0)
            {
                dust.active = false;
                return false;
            }
            return true;
        }
    }
}
