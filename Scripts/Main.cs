using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Red;
using Red.Utill;
using UnityEditor;
using System.Net;
using System.IO;
using UnityEngine.Networking;

public class Main : MonoBehaviour
{
    public GameObject scene = null;
    public UIAtlas atlas = null;
    public AnimationClip animClip;

    public string filePath = "";

    public void SaveScene()
    {
        if (scene == null) return;
        string json = LayoutToJson.ToJson(scene.transform);
        FileIO.WriteData(json, scene.name + ".json", true, filePath);
    }

    public void SaveAtlas()
    {
        if (atlas == null) return;
        string json = AtlasToJson.ToJson(atlas);
        FileIO.WriteData(json, atlas.name + ".json", true, filePath);
        FileIO.CopyWritePng(atlas.texture as Texture2D, atlas.name + ".png");
    }

    public void SaveAnimClip()
    {
        if (animClip == null) return;
        string json = AnimationToJson.ToJson(animClip);
        FileIO.WriteData(json, animClip.name + ".json", true, filePath);
    }

    //[ContextMenu("11")]
    //public void Web()
    //{



    //    //Application.OpenURL(Application.dataPath + "/../html/index.html");

    //    byte[] myData = System.Text.Encoding.UTF8.GetBytes("{  \"range\": \"Sheet1!A1:D5\",  \"majorDimension\": \"ROWS\",  \"values\": [    [\"Item\", \"Cost\", \"Stocked\", \"Ship Date\"],    [\"Wheel\", \"$20.50\", \"4\", \"3/1/2016\"],    [\"Door\", \"$15\", \"2\", \"3/15/2016\"],    [\"Engine\", \"$100\", \"1\", \"30/20/2016\"],    [\"Totals\", \"=SUM(B2:B4)\", \"=SUM(C2:C4)\", \"=MAX(D2:D4)\"]  ],}");
    //    UnityWebRequest put = UnityWebRequest.Put("https://sheets.googleapis.com/v4/spreadsheets/1Qu1IAkmX73Euy1Ht4_szu5cprOsTGgAMwAMMr23VzJI/values/sheet1!A1:D5?valueInputOption=USER_ENTERED?access_token=ya29.GlsYBX9qUNlofvdEs_VEcc0K2lUT2kBG3hdNTwXDvMZwu-NmDf_XGQdvu1-Ily0T4mvCD3ZvvCaf0xnNaZs25E7JFmGvbBkomrWePCtWkujpov-SHAqqSqbwbeJ6", myData);
    //    //UnityWebRequest put = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/1Qu1IAkmX73Euy1Ht4_szu5cprOsTGgAMwAMMr23VzJI/export?gid=0&format=csv");
    //    put.SetRequestHeader("access_token", "ya29.GlsYBX9qUNlofvdEs_VEcc0K2lUT2kBG3hdNTwXDvMZwu-NmDf_XGQdvu1-Ily0T4mvCD3ZvvCaf0xnNaZs25E7JFmGvbBkomrWePCtWkujpov-SHAqqSqbwbeJ6");
    //    put.Send();
    //    //
    //    while ( !put.isDone )
    //    {

    //    }

    //    Debug.Log(put.downloadHandler.text);



    //    //// PUT
    //    //string url = "https://sheets.googleapis.com/v4/spreadsheets/1Qu1IAkmX73Euy1Ht4_szu5cprOsTGgAMwAMMr23VzJI/values/sheet1!A1?valueInputOption=USER_ENTERED";
    //    //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
    //    //httpWebRequest.ContentType = "text/json";
    //    //httpWebRequest.Method = "PUT";


    //    //HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
    //    //using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //    //{
    //    //    string responseText = streamReader.ReadToEnd();
    //    //    //Now you have your response.
    //    //    //or false depending on information in the response
    //    //    Debug.Log(responseText);
    //    //}


    //    //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(
    //    //    "https://sheets.googleapis.com/v4/spreadsheets/1Qu1IAkmX73Euy1Ht4_szu5cprOsTGgAMwAMMr23VzJI/values/sheet1!A1?valueInputOption=USER_ENTERED");
    //    //httpWebRequest.ContentType = "text/json";
    //    //httpWebRequest.Method = "PUT";


    //    //HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
    //    //using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //    //{
    //    //    string responseText = streamReader.ReadToEnd();
    //    //    Debug.Log(responseText);
    //    //}
    //}

    


}
