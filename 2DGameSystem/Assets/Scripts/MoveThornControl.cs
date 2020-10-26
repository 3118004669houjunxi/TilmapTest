using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThornControl : MonoBehaviour
{
    public float speedMagnification = 1;
    Vector2 begin = new Vector2();
    Vector2 end = new Vector2();
    float i = 0;
    bool isBack = false;
    void Start()
    {
        begin.x = transform.position.x;
        begin.y = transform.position.y;
        end.x = transform.parent.GetComponent<Transform>().position.x;
        end.y = transform.parent.GetComponent<Transform>().position.y;
    }
    void Update()
    {
        if (i >= 1)
            isBack = true;
        if (i <= 0)
            isBack = false;
        transform.position = Vector2.Lerp(begin,end,i);
        if (isBack)
            i -= Time.deltaTime * speedMagnification;
        else
            i += Time.deltaTime * speedMagnification;
    }
}
