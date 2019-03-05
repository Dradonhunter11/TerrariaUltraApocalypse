﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.API.LiquidAPI.LiquidMod;
using WorldGen = On.Terraria.WorldGen;

namespace TUA.API.LiquidAPI
{
    public class ModLiquid
    {
        public bool gravity = true;
        public int customDelay = 1; //Default value, aka 
        public Color liquidColor = Color.Gray;
        public Mod mod { get; internal set; }

        public virtual string name { get; }

        public Liquid liquid { get; } = new Liquid();

        public virtual Texture2D texture
        {
            get { return null; }
        }
        internal int liquidIndex;

        /// <summary>
        /// Take an array that contain legacy style texture and 1.3.4+ texture style
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool Autoload(ref string name)
        {
            return true;
        }

        public virtual void Update()
        {
            
        }

        //Normally trigger if gravity is at false
        public virtual bool CustomPhysic(int x, int y)
        {
            LiquidRef liquidLeft = LiquidCore.grid[x - 1, y];
            LiquidRef liquidRight = LiquidCore.grid[x + 1, y];
            LiquidRef liquidUp = LiquidCore.grid[x, y - 1];
            LiquidRef liquidDown = LiquidCore.grid[x, y + 1];
            LiquidRef liquidSelf = LiquidCore.grid[x, y];

            if (!Liquid.quickFall)
            {
                if (liquid.delay < 50)
                {
                    ++liquid.delay;
                    return false;
                }
                liquid.delay = 0;
                if (liquidLeft.liquidsType() == liquidSelf.liquidsType())
                {
                    LiquidExtension.AddModdedLiquidAround(liquid.x, liquid.y);
                }

                if (liquidRight.liquidsType() == liquidSelf.liquidsType())
                {
                    LiquidExtension.AddModdedLiquidAround(liquid.x, liquid.y);
                }

                if (liquidUp.liquidsType() == liquidSelf.liquidsType())
                {
                    LiquidExtension.AddModdedLiquidAround(liquid.x, liquid.y);
                }

                if (liquidDown.liquidsType() == liquidSelf.liquidsType())
                {
                    LiquidExtension.AddModdedLiquidAround(liquid.x, liquid.y);
                }
            }
            return true;
        }

        public virtual float SetLiquidOpacity()
        {
            return 1;
        }
        

        public virtual void PreDrawValueSet(ref bool bg, ref int style, ref float Alpha)
        {
        }

        public virtual void PreDraw(TileBatch batch)
        {
        }

        public virtual void Draw(TileBatch batch)
        {
        }

        public virtual void PostDraw(TileBatch batch)
        {
        }

        public virtual void PlayerInteraction(Player target)
        {
        }

        public virtual void NpcInteraction(NPC target)
        {

        }

        public virtual void ItemInteraction(Item target)
        {

        }

        public virtual void LiquidInteraction(int x, int y, ModLiquid target)
        {

        }

        public virtual void LavaInteraction(int x, int y)
        {

        }

        public virtual void HoneyInteraction(int x, int y)
        {

        }
    }

    public class ModBucket : ModItem
    {
        private int liquidType = -1;

        private string name
        {
            get
            {
                switch (liquidType)
                {
                    case -1:
                        return "Empty bucket";
                    case 0:
                        return "Water bucket";
                    case 1:
                        return "Lava bucket";
                    case 2:
                        return "Honey bucket";
                    default:
                        return LiquidRegistery.liquidList[liquidType-3].name + "bucket";
                }
            }
        }

        public override bool Autoload(ref string name)
        {
            
            return base.Autoload(ref name);
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 100;
            item.useAnimation = 1;
            item.consumable = true;
        }

        public override void UpdateInventory(Player player)
        {
            item.SetNameOverride(name);
        }

        public override bool UseItem(Player player)
        {
            if (Main.tile[Player.tileTargetX, Player.tileTargetY].liquid < 240)
            {
                return false;
            }

            byte getLiquidType = LiquidCore.liquidGrid[Player.tileTargetX, Player.tileTargetY].data;
            Item newItem = Main.item[Item.NewItem(player.position, item.type)];
            ModBucket newBucket = newItem.modItem as ModBucket;
            newBucket.liquidType = getLiquidType;
            player.PutModItemInInventory(newBucket);
            item.stack--;
            return true;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor,
            Vector2 origin, float scale)
        {
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
            if (liquidType == -1)
            {
                return;
            }

            Texture2D liquidTexture = TerrariaUltraApocalypse.instance.GetTexture("Texture/Bucket/liquid");
            Color liquidColor;
            switch (liquidType)
            {
                case 0:
                    liquidColor = Color.Blue;
                    break;
                case 1:
                    liquidColor = Color.Red;
                    break;
                case 2:
                    liquidColor = Color.Yellow;
                    break;
                default:
                    liquidColor = LiquidRegistery.liquidList[liquidType - 3].liquidColor;
                    break;
            }
            spriteBatch.Draw(liquidTexture, Vector2.Zero, liquidColor);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale,
            int whoAmI)
        {
            if (liquidType == -1)
            {
                return;
            }

            Texture2D liquidTexture = TerrariaUltraApocalypse.instance.GetTexture("Texture/Bucket/liquid");
            Color liquidColor;
            switch (liquidType)
            {
                case 0:
                    liquidColor = Color.Blue;
                    break;
                case 1:
                    liquidColor = Color.Red;
                    break;
                case 2:
                    liquidColor = Color.Yellow;
                    break;
                default:
                    liquidColor = LiquidRegistery.liquidList[liquidType - 3].liquidColor;
                    break;
            }
            spriteBatch.Draw(liquidTexture, Vector2.Zero, liquidColor);
        }
    }
}
