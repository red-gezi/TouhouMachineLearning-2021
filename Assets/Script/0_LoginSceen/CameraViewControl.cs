using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//控制菜单中的摄像机镜头
namespace Control
{
    public class CameraViewControl : MonoBehaviour
    {
        public Transform startPosition;
        public Transform endPosition;
        static Transform targetTransform;
        static CameraViewControl cameraControl;
        // Start is called before the first frame update
        void Awake()
        {
            cameraControl = this;
            targetTransform = startPosition;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * 3);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetTransform.eulerAngles, Time.deltaTime * 3);
        }
        public static void MoveToView(Transform transform) => targetTransform = transform;
        public static void MoveToInitView() => MoveToView(cameraControl.startPosition);

        public static void MoveToBookView() => MoveToView(cameraControl.endPosition);
    }
}

