﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TUA.Tiles.Furniture.Ingots
{
    class MeteoridonIngot : TUAIngot
    {
        public override Color MapEntryColor => Color.BlanchedAlmond;
        public override string MapNameLegend => "Meteoride Bar";
        public override int IngotDropName => mod.ItemType("MeteoridonBar");
    }
}
