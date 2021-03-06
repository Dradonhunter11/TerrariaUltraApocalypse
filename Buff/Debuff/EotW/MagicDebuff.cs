﻿using Terraria;
using Terraria.ModLoader;

namespace TUA.Buff.Debuff.EotW
{
    public sealed class MagicDebuff : TUABuff
    {
        public MagicDebuff() : base("Magic Curse", "EotW Blue: Can only inflict magic damage")
        {
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage *= 0f;
            player.magicDamage *= 5f;
            player.bulletDamage *= 0f;
            player.arrowDamage *= 0f;
            player.rocketDamage *= 0f;
            player.thrownDamage *= 0f;
            player.minionDamage *= 0f;

        }
    }
}
