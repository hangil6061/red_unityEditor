using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScroll : MonoBehaviour
{
    public GameObject scrollArea;
    public GameObject scrollMask;
    public GameObject scrollbar_bg;
    public GameObject scrollbar_bar;

    [ContextMenu("Reset")]
    public void Reset()
    {
        Transform tr = transform.FindChild("scrollArea");
        if (tr != null)
        {
            scrollArea = tr.gameObject;
        }

        tr = transform.FindChild("scrollMask");
        if (tr != null)
        {
            scrollMask = tr.gameObject;
            if( scrollMask.GetComponent<UIMask>() == null )
            {
                scrollMask.AddComponent<UIMask>();
            }
        }

        tr = transform.FindChild("scrollbar_bg");
        if (tr != null)
        {
            scrollbar_bg = tr.gameObject;
        }

        tr = transform.FindChild("scrollbar_bar");
        if (tr != null)
        {
            scrollbar_bar = tr.gameObject;
        }
    }
}
