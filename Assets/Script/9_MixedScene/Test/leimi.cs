using GameEnum;
using UnityEngine;

public class leimi : MonoBehaviour
{
    [Range(0, 1)]
    public float time;
    Vector3 start;
    Vector3 end;
    // Start is called before the first frame update
    void Start()
    {

        CardSet cardSet = Info.AgainstInfo.cardSet;
        //我方手牌
        cardSet = cardSet[Orientation.My][RegionTypes.Hand];
        //我方手牌，领袖牌
        cardSet = cardSet[Orientation.My][RegionTypes.Hand,RegionTypes.Leader];
        //我方计时牌
        cardSet = cardSet[Orientation.My][CardField.Timer];
        //敌方间谍牌
        cardSet = cardSet[Orientation.Op][CardState.Spy];
        //含有机器标签的牌
        cardSet = cardSet[CardTag.Machine];

    }

    private void RefreshPos()
    {
        start = transform.position;
        end = Random.onUnitSphere * 10;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            time = 0;
            RefreshPos();
        }
        transform.position = Vector3.Lerp(start, end, time);
    }
}

