﻿using Terraria;
using Terraria.ModLoader;

namespace TUA.Items.Armor
{
    // Serves as an example, may or may not implement this later
    abstract class DruidicArmor : ArmorBase
    {
        public override bool IsArmorSetSafe(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("DruidicHelmet")
                && body.type == mod.ItemType("DruidicBreastplate")
                && legs.type == mod.ItemType("DruidicLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<DruidicArmorPlayer>().equipped = true;
        }
    }

    [AutoloadEquip(EquipType.Head)]
    abstract class DruidicHelmet : DruidicArmor { }
    [AutoloadEquip(EquipType.Body)]
    abstract class DruidicBreastplate : DruidicArmor { }
    [AutoloadEquip(EquipType.Legs)]
    abstract class DruidicLeggings : DruidicArmor { }

    class DruidicArmorPlayer : ModPlayer
    {
        public bool equipped;

        public override void GetWeaponCrit(Item item, ref int crit)
        {
            if (equipped)
            {
                crit += (int)(crit * .6);
            }
        }

        public override void GetWeaponDamage(Item item, ref int damage)
        {
            if (equipped) damage += (int)(damage * .3);
        }

        public override void GetWeaponKnockback(Item item, ref float knockback)
        {
            if (equipped) knockback *= 1.2f;
        }
    }
}
