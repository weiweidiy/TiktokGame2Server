﻿namespace Tiktok
{
    public class RequestAddSamuraiExp
    {
        public int TargetSamuraiId { get; set; }

        public required List<int> ExpSamuraisIds { get; set; }
    }
}