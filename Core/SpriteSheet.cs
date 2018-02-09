using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Red;
using Red.Utill;

public class SpriteSheet : MonoBehaviour
{
    public Sprite[] arr;

    [ContextMenu("Create")]
    public void Create()
    {
        string json = SpriteSheetToJspm.ToJson(arr);
        FileIO.WriteData(json, arr[0].texture.name + ".json", true);
        FileIO.CopyWritePng(arr[0].texture as Texture2D, arr[0].texture.name + ".png");
    }
}
