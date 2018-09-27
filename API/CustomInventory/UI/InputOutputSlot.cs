﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace TerrariaUltraApocalypse.API.CustomInventory.UI
{
    class InputOutputSlot : UIElement
    {
        protected ExtraSlot boundSlot;
        private Texture2D slotTexture;
        public bool debug = false;

        protected Vector2 currentSlotVector;

        public int customOffsetX = 2;
        public int customOffsetY = 0;

        public InputOutputSlot(ExtraSlot boundSlot, Texture2D slotTexture)
        {
            CalculatedStyle s = GetInnerDimensions();
            currentSlotVector = new Vector2(s.X, s.Y);
            this.boundSlot = boundSlot;
            this.slotTexture = slotTexture;
        }

        public sealed override void OnInitialize()
        {
            Width.Set(slotTexture.Width, 0f);
            Height.Set(slotTexture.Height, 0f);
            OnClick += OnMouseClick;
            OnRightClick += OnMouseRightClick;
            OnMouseOver += MouseHover;
            NewInitialize();
        }

        public virtual void NewInitialize()
        {

        }

        public void MouseHover(UIMouseEvent mouseEvent, UIElement listeningElement)
        {
            if (!boundSlot.isEmpty())
            {
                Main.hoverItemName = boundSlot.getItem(false).HoverName;
            }
        }

        public void OnMouseClick(UIMouseEvent mouseEvent, UIElement listeningElement)
        {
            Item item = Main.mouseItem;

            if (!item.IsAir)
            {
                if (boundSlot.isEmpty())
                {
                    boundSlot.setItem(ref Main.mouseItem);
                }
                else if (boundSlot.setItem(ref Main.mouseItem))
                {
                    Main.mouseItem.TurnToAir();
                }
                else if (boundSlot.manipulateCurrentItem(item))
                {
                    Main.mouseItem = item;
                }
            } else if (!boundSlot.isEmpty() && item.IsAir)
            {
                boundSlot.swap(ref Main.mouseItem);
            }
            
        }

        public void OnMouseRightClick(UIMouseEvent mouseEvent, UIElement listeningElement)
        {
            Item item = Main.mouseItem.Clone();
            item.stack = 1;

            if (!item.IsAir)
            {
                if (boundSlot.isEmpty())
                {
                    boundSlot.setItem(ref Main.mouseItem);
                    Main.mouseItem.stack--;
                }
                else if (boundSlot.manipulateCurrentItem(item))
                {
                    Main.mouseItem.stack--;
                }

                if (Main.mouseItem.stack == 0)
                {
                    Main.mouseItem.TurnToAir();
                }
            }

            if (Main.netMode != 0)
            {
                NetMessage.SendData(MessageID.SyncItem);
            }

        }


        public virtual void PreDraw(SpriteBatch spriteBatch)
        {

        }

        public virtual void PostDraw(SpriteBatch spriteBatch)
        {

        }

        private void DrawDebug(SpriteBatch spriteBatch)
        {
            Texture2D bound2 = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
            bound2.SetData<Color>(new Color[] { Color.White });

            Rectangle rec2 = GetInnerDimensions().ToRectangle();
            spriteBatch.Draw(bound2, new Rectangle(rec2.X, rec2.Y, rec2.Width, rec2.Height), Color.Green);
            spriteBatch.Draw(bound2, new Rectangle(rec2.X, rec2.Y, 1, rec2.Height), Color.Green);
            spriteBatch.Draw(bound2, new Rectangle(rec2.X + rec2.Width - 1, rec2.Y, 1, rec2.Height), Color.Green);
            spriteBatch.Draw(bound2, new Rectangle(rec2.X, rec2.Y + rec2.Height - 1, rec2.Width, 1), Color.Green);

        }

        private void drawItem(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDimension = GetInnerDimensions();
            innerDimension.X += customOffsetX;
            innerDimension.Y += customOffsetY;
            Texture2D itemTexture = boundSlot.getItemTexture();
            Item item = boundSlot.getItem(true);
            float scale = 0.90f;

            Vector2 vector = slotTexture.Size() * scale;
            Rectangle rectangle2;
            if (Main.itemAnimations[boundSlot.getItem(false).type] != null)
            {
                rectangle2 = Main.itemAnimations[boundSlot.getItem(false).type].GetFrame(itemTexture);
            }
            else
            {
                rectangle2 = itemTexture.Frame(1, 1, 0, 0);
            }
            Color color = Color.White;
            float num8 = 1f;
            ItemSlot.GetItemLight(ref color, ref num8, boundSlot.getItem(false), false);
            float num9 = 1f;
            float AvailableWidth = slotTexture.Width * scale;
            if (rectangle2.Width > slotTexture.Width || rectangle2.Height > slotTexture.Width)
            {
                if (rectangle2.Width > rectangle2.Height)
                {
                    num9 = AvailableWidth / (float)rectangle2.Width;
                }
                else
                {
                    num9 = AvailableWidth / (float)rectangle2.Height;
                }
            }

            num9 *= scale;
            Vector2 position2 = innerDimension.Position() + vector / 2f - rectangle2.Size() * num9 / 2f;
            Vector2 origin = rectangle2.Size() * (num8 / 2f - 0.5f);
            if (ItemLoader.PreDrawInInventory(boundSlot.getItem(false), spriteBatch, position2, rectangle2, boundSlot.getItem(false).GetAlpha(color), boundSlot.getItem(false).GetColor(color), origin, num9 * num8))
            {
                spriteBatch.Draw(itemTexture, position2, new Rectangle?(rectangle2), item.GetAlpha(color), 0f, origin, num9 * num8, SpriteEffects.None, 0f);
                if (item.color != Color.Transparent)
                {
                    spriteBatch.Draw(itemTexture, position2, new Rectangle?(rectangle2), item.GetColor(color), 0f, origin, num9 * num8, SpriteEffects.None, 0f);
                }
            }
            ItemLoader.PostDrawInInventory(item, spriteBatch, position2, rectangle2, item.GetAlpha(color), item.GetColor(color), origin, num9 * num8);
            if (ItemID.Sets.TrapSigned[item.type])
            {
                spriteBatch.Draw(Main.wireTexture, innerDimension.Position() + new Vector2(40f, 40f) * scale, new Rectangle?(new Rectangle(4, 58, 8, 8)), color, 0f, new Vector2(4f), 1f, SpriteEffects.None, 0f);
            }
            if (item.stack > 1)
            {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, item.stack.ToString(), position2 + new Vector2(10f, 26f) * scale, color, 0f, Vector2.Zero, new Vector2(scale), -1f, scale);
            }
            

        }

        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDimension = GetInnerDimensions();
            PreDraw(spriteBatch);
            DrawChildren(spriteBatch);
            DrawSelf(spriteBatch);
            spriteBatch.Draw(slotTexture, new Vector2(innerDimension.X, innerDimension.Y), Color.White);
            if (!boundSlot.isEmpty())
            {
                drawItem(spriteBatch);
            }
            PostDraw(spriteBatch);
            
            //DrawDebug(spriteBatch);
            
        }

        
    }
}
