﻿using Red;
using Red.Utill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlasMain : MonoBehaviour {

    public UIAtlas atlas = null;
    public string filePath = "";

    public void SaveAtlas()
    {
        if (atlas == null) return;
        string json = AtlasToJson.ToJson(atlas);
        FileIO.WriteData(json, atlas.name + ".json", true, filePath);
        FileIO.CopyWritePng(atlas.texture as Texture2D, atlas.name + ".png", filePath);
    }
}
