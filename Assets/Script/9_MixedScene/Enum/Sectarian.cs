using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEnum
{
    public enum Sectarian
    {
        [LabelText("����")]
        Neutral,
        [InspectorName("����")]
        Taoism,
        Shintoism,
        Buddhism,
        science
    }
}