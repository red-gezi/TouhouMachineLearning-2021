using UnityEngine;
namespace Info
{
    public class AudioInfo : MonoBehaviour
    {
        public AudioClip[] Clips;
        public static AudioInfo Instance;
        void Awake() => Instance = this;

        public static AudioClip[] StaticClips => Instance.Clips;
    }
}
