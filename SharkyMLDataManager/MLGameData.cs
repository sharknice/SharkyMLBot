﻿using Sharky;

namespace SharkyMLDataManager
{
    public class MLGameData
    {
        public Game? Game { get; set; }
        public Dictionary<int, MLFrameData>? MLFramesData { get; set; }
    }
}
