using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEffectInfo : MonoBehaviour
{
    public TheWorldEffect _theWorldEffect;
    public static TheWorldEffect theWorldEffect;
    private void Awake() => theWorldEffect = _theWorldEffect;

}
