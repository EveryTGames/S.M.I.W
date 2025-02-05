using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class items : MonoBehaviour
{
    public ItemData data;
    public Vector2 position;
    public Vector2 Dimention;
    public int ZRotation;
   public  bool overrid = false;
    private void Start()
    {
        if (!overrid)
        {

            Dimention = data.dimention;
            Debug.Log(Dimention);
        }
        overrid = false;
    }
    private void Update()
    {
        if (position != (Vector2)transform.localPosition)
        {
            position = (Vector2)transform.localPosition;

        }
    }

    public void Rotate()
    {
        if (!overrid)
        {

            (Dimention.x, Dimention.y) = (Dimention.y, Dimention.x);
        }
        ZRotation = (ZRotation == 0) ? -90 : 0;
        transform.eulerAngles = new Vector3 { z = ZRotation };
    }

    public void setDimention(Vector2 overrideDimention)
    {
        overrid = true;

        Dimention = overrideDimention;
        Debug.Log(Dimention);
    }
}
