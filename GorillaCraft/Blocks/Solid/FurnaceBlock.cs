﻿using GorillaCraft.Interfaces;
using GorillaCraft.Models;
using GorillaCraft.Sounds;
using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaCraft.Blocks.Solid
{
    public class FurnaceBlock : IBlock
    {
        public BlockFaceInfo Front => new("FurnaceFront", typeof(Surface_Default));
        public BlockFaceInfo Left => new("FurnaceSide", typeof(Surface_Default));
        public BlockFaceInfo Back => new("FurnaceSide", typeof(Surface_Default));
        public BlockFaceInfo Right => new("FurnaceSide", typeof(Surface_Default));
        public BlockFaceInfo Up => new("FurnaceTop", typeof(Surface_Default));
        public BlockFaceInfo Down => new("FurnaceTop", typeof(Surface_Default));

        public Type PlaceSoundType => typeof(Interaction_Default);
        public Type DestroySoundType => typeof(Interaction_Default);

        public string BlockDefinition => "Furnace";
        public BlockForm BlockForm => BlockForm.Solid;
        public BlockPlacement BlockPlacement => BlockPlacement.LimitedRotation;
    }
}