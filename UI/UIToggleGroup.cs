using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleGroup : MonoBehaviour {
    public UIToggleButton[] toggles;
    public int defaultIndex = 0;

    [ContextMenu("Reset")]
    public void Reset()
    {
        toggles = this.GetComponentsInChildren<UIToggleButton>();
    }
}
