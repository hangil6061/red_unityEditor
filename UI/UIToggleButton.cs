using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleButton : MonoBehaviour {
    public Button button;
    public Image onImage;

    [ContextMenu("Reset")]
    public void Reset()
    {
        button = this.GetComponent<Button>();
        

        for( int i = 0; i < transform.childCount; i++ )
        {
            Transform tr = transform.GetChild(i);
            if( tr.name.Contains( "on" ) )
            {
                Image img = tr.gameObject.GetComponent<Image>();
                if( img != null )
                {
                    onImage = img;
                    break;
                }
            }
        }
    }
}
