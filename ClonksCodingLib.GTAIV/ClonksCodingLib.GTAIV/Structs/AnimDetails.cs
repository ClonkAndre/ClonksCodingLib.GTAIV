using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.GTAIV
{
    public struct AnimDetails
    {

        public uint AnimGroupId;
        public uint AnimId;
        public uint Flags;
        public uint BlendDelta;
        public uint Offset12;
        public uint Offset16;
        public float Offset88;
        public float CurrentAnimTime;
        public float CurrentAnimTimeNormalized;

        public AnimDetails(uint animGroupId, uint animId, uint flags, uint blendDelta, uint offset12, uint offset16, float offset88, float currentAnimTime, float normalizedTime)
        {
            AnimGroupId = animGroupId;
            AnimId = animId;
            Flags = flags;
            BlendDelta = blendDelta;
            Offset12 = offset12;
            Offset16 = offset16;
            Offset88 = offset88;
            CurrentAnimTime = currentAnimTime;
            CurrentAnimTimeNormalized = normalizedTime;
        }

    }
}
