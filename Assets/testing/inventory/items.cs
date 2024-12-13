using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class items : MonoBehaviour
{
    public ItemData data;
    public Vector2 position;
    private void Update()
    {
        if(position != (Vector2)transform.localPosition)
        {
            position = (Vector2)transform.localPosition;

        }
    }


}
