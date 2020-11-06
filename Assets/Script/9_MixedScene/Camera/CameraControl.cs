using UnityEngine;
namespace Control
{
    public class CameraControl : MonoBehaviour
    {
        public Vector3 DefaultView;
        float OffsetX = 0;
        float OffsetY = 0;
        void Start() => DefaultView = transform.eulerAngles;
        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                OffsetX += Input.GetAxis("Mouse X");
                OffsetY -= Input.GetAxis("Mouse Y");
                Vector3 OffsetVector = new Vector3(OffsetY, OffsetX, 0);
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, DefaultView + OffsetVector, Time.deltaTime * 2);
            }
            else
            {
                OffsetX = 0;
                OffsetY = 0;
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, DefaultView, Time.deltaTime * 2);
            }
        }
    }
}