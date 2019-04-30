﻿using Terraria;
using TUA.API;

namespace TUA.Items.Dimension
{
    class StardustCrystal : TUADimItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stardust crystal");
            Tooltip.SetDefault("Allow you to travel in a new universe\nUltra mode");
        }

        public override void SetDefaults()
        {
            Ultra = true;
            item.width = 32;
            item.height = 32;
            item.useStyle = 4;
            item.useTime = 5;
            item.useAnimation = 20;
            dimensionName = "stardust";
            dimensionMessage = "You are entering into the cold environnement of stardust...";
            base.SetDefaults();
        }

        public override void UpdateInventory(Player player)
        {
            dimensionMessage = "You are entering into the cold environnement of stardust...";
            itemUseCooldown--;
            if (itemUseCooldown < 0)
            {
                itemUseCooldown = 0;
            }
        }
    }
}
