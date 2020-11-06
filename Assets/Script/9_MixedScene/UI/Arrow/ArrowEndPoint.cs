using UnityEngine;
namespace GameUI
{
    public class ArrowEndPoint : MonoBehaviour
    {
        public float High;
        public float Distance;
        static Ray SceneRay;
        public Vector3 v1;
        public Vector3 v2;
        public float v3;
        void Update()
        {
            v1 = SceneRay.origin;
            v2 = SceneRay.direction;
            v3 = (Camera.main.transform.position.y - High);
            SceneRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Distance = Mathf.Abs((Camera.main.transform.position.y - High) / -SceneRay.direction.normalized.y);
            transform.position = SceneRay.GetPoint(High);
        }
    }
}

