//using Live2D.Cubism.Core;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Live2d : MonoBehaviour
//{
//    public bool FaceRank;
//    private CubismModel _model;
//    private CubismParameter _paramAngleZ;
//    public float num;
//    [SerializeField]
//    public string ParameterID = "ParamAngleX";


//    private void Start()
//    {
//        _model = this.FindCubismModel();
//        _paramAngleZ = _model.Parameters.FindById(ParameterID);
//        _paramAngleZ.Value = 5;
//    }

//    // Update is called once per frame
//    void LateUpdate()
//    {
//        //print(Mathf.Lerp(_paramAngleZ.Value, FaceRank == 0 ? 30 : -30, Time.deltaTime));
//        //
//       num = Mathf.Lerp(num, FaceRank ? -10 : 10, Time.deltaTime * 2);
//        _paramAngleZ.Value = num;
//        if (Input.GetMouseButtonDown(0))
//        {
//            FaceRank = !FaceRank;
//        }
//    }
    
//}
