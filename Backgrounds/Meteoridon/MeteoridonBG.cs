﻿using BiomeLibrary;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Backgrounds.Meteoridon
{
    class MeteoridonBG : ModSurfaceBgStyle
    {
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            
        }

        public override int ChooseFarTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/Meteoridon/Meteoridon_BG");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return mod.GetBackgroundSlot("Backgrounds/Meteoridon/Meteoridon_BG");
        }

        public override int ChooseMiddleTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/Meteoridon/Meteoridon_BG");
        }

        public override bool ChooseBgStyle()
        {            
            return !Main.gameMenu && mod.GetBiome("Meteoridon").InBiome(Main.LocalPlayer);
        }
    }
}
