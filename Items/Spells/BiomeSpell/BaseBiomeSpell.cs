﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.Items.Spells.BiomeSpell
{
    public abstract class BaseBiomeSpell : ModItem, ISpell
    {
        public abstract bool Cast(Player player);

        public virtual bool GetColor(out Color color)
        {
            color = default;
            return false;
        }

        public sealed override bool UseItem(Player player) => Cast(player);

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage,
            ref float knockBack)
        {
            BaseBiomeSpellProjectile proj = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY),
                item.shoot, damage, knockBack, player.whoAmI).modProjectile as BaseBiomeSpellProjectile;
            GetColor(out proj.color);
            return true;
        }
    }
}
