﻿using GorillaCraft.Interfaces;
using GorillaCraft.Models;
using GorillaCraft.Sounds;
using System;

namespace GorillaCraft.Blocks.Solid
{
    public class OakLogBlock : IBlock
    {
        public BlockFaceInfo Front => new("OakLog", typeof(Surface_Wood));
        public BlockFaceInfo Left => new("OakLog", typeof(Surface_Wood));
        public BlockFaceInfo Back => new("OakLog", typeof(Surface_Wood));
        public BlockFaceInfo Right => new("OakLog", typeof(Surface_Wood));
        public BlockFaceInfo Top => new("OakLogTop", typeof(Surface_Wood));
        public BlockFaceInfo Bottom => new("OakLogTop", typeof(Surface_Wood));

        public Type PlaceSoundType => typeof(Interaction_Wood);
        public Type DestroySoundType => typeof(Interaction_Wood);

        public string BlockDefinition => "Oak Log";
        public BlockForm BlockForm => BlockForm.Solid;
        public BlockPlacement BlockPlacement => BlockPlacement.FullRotation;
    }
}
