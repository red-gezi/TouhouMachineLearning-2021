using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEnum
{
    public enum Sectarian
    {
        [LabelText("中立")]
        Neutral,
        [InspectorName("道教")]
        Taoism,
        Shintoism,
        Buddhism,
        science
    }
}