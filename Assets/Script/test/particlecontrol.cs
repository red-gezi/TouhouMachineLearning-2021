using Extension;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
using UnityEngine.UI;

public class particlecontrol : MonoBehaviour
{
    public ParticleSystem particle;
    public Animation Animation;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ = ShowParticle();
        }
    }

    private async Task ShowParticle()
    {
        particle.Play();
        Animation.Play();
        text.gameObject.SetActive(true); 
    }
}
