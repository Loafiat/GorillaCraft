﻿using GorillaCraft.Interfaces;
using GorillaCraft.Models;
using GorillaCraft.Sounds;
using System;

namespace GorillaCraft.Blocks.Solid
{
    public class DarkOakLeavesBlock : IBlock
    {
        public BlockFaceInfo Front => new("DarkOakLeaves", typeof(Surface_Grass));
        public BlockFaceInfo Left => new("DarkOakLeaves", typeof(Surface_Grass));
        public BlockFaceInfo Back => new("DarkOakLeaves", typeof(Surface_Grass));
        public BlockFaceInfo Right => new("DarkOakLeaves", typeof(Surface_Grass));
        public BlockFaceInfo Top => new("DarkOakLeaves", typeof(Surface_Grass));
        public BlockFaceInfo Bottom => new("DarkOakLeaves", typeof(Surface_Grass));

        public Type PlaceSoundType => typeof(Interaction_Grass);
        public Type DestroySoundType => typeof(Interaction_Grass);

        public string BlockDefinition => "Dark Oak Leaves";
        public BlockForm BlockForm => BlockForm.Solid;
        public BlockPlacement BlockPlacement => BlockPlacement.Default;
    }
}
