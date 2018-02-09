using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Red.Utill;

namespace Red
{

    //public class AnimClipData : ToJson
    //{
    //    public string name;
    //    public List<AnimObjectData> datas = new List<AnimObjectData>();

    //    public string ToJson()
    //    {
    //        string str = "";
    //        for (int i = 0; i < datas.Count; i++)
    //        {
    //            str += datas[i].ToJson();
    //            if (i == datas.Count - 1) continue;

    //            str += ",\n";
    //        }

    //        return StringHelper.KeyValueTojson(name, StringHelper.AddBrace(str));
    //    }

    //    public void AddObject(string path, string propertyName, Keyframe[] keys)
    //    {
    //        AnimObjectData data = datas.Find(o => o.path == path);

    //        if (data == null)
    //        {
    //            data = new AnimObjectData();
    //            data.path = path;
    //            datas.Add(data);
    //        }

    //        data.AddProperty(propertyName, keys);
    //    }
    //}

    //public class AnimObjectData : ToJson
    //{
    //    public string path;
    //    public List<AnimPropertyData> property = new List<AnimPropertyData>();

    //    public string ToJson()
    //    {
    //        string str = "";
    //        for (int i = 0; i < property.Count; i++)
    //        {
    //            str += property[i].ToJson();
    //            if (i == property.Count - 1) continue;

    //            str += ",";
    //        }

    //        return StringHelper.KeyValueTojson(path, StringHelper.AddBrace(str));
    //    }

    //    public void AddProperty( string propertyName, Keyframe[] keys )
    //    {
    //        int idx = AnimationToJson.PropertyName.FindIndex(item => item == propertyName);
    //        AnimationToJson.Property prop = AnimationToJson.PropertyArr[idx];

    //        if (property.Find( p => p.property == prop) == null)
    //       {
    //            property.Add(new AnimPropertyData(propertyName, keys));
    //       }
    //       else
    //       {
    //            Debug.Log("이미 키가 추가되어 있음");
    //       }
    //    }   
    //}

    //public class AnimPropertyData : ToJson
    //{
    //    public AnimationToJson.Property property;
    //    public List<KeyFrameData> keys = new List<KeyFrameData>();

    //    public string ToJson()
    //    {
    //        string str = "";
    //        for( int i = 0; i < keys.Count; i++ )
    //        {
    //            str += keys[i].ToJson();
    //            if (i == keys.Count - 1) continue;

    //            str += ",";
    //        }

    //        return StringHelper.KeyValueTojson(property.ToString(), StringHelper.AddBracket(str));
    //    }

    //    public AnimPropertyData(string name, Keyframe[] keyFrames)
    //    {
    //        int idx = AnimationToJson.PropertyName.FindIndex(item => item == name);
    //        property = AnimationToJson.PropertyArr[idx];

    //        foreach ( Keyframe k in keyFrames )
    //        {
    //            keys.Add(new KeyFrameData(k.time, k.value));
    //        }
    //    }
    //}

    public class AnimClipData : ToJson
    {
        public string name;
        public List<AnimKeyData> keyDatas = new List<AnimKeyData>();
        public List<EventKeyData> eventDatas = new List<EventKeyData>();

        public string ToJson()
        {
            string str = "";
            for (int i = 0; i < keyDatas.Count; i++)
            {
                str += keyDatas[i].ToJson();
                if (i == keyDatas.Count - 1) continue;
                str += ",";
            }

            string str2 = "";
            for (int i = 0; i < eventDatas.Count; i++)
            {
                str2 += eventDatas[i].ToJson();
                if (i == eventDatas.Count - 1) continue;
                str2 += ",";
            }

            str = string.Format("{0},{1},{2}",
                StringHelper.KeyValueTojson("name", StringHelper.AddDoubleQuotation(name))
                , StringHelper.KeyValueTojson("keys", StringHelper.AddBracket(str))
                , StringHelper.KeyValueTojson("events", StringHelper.AddBracket(str2)));

            return StringHelper.AddBrace( StringHelper.AddNewLine( str ));
        }

        public void AddKeyData(string path, string property, Keyframe[] keys)
        {
            int idx = AnimationToJson.PropertyName.FindIndex(item => item == property);

            if (idx == -1)
            {
                Debug.Log(property + " 제거");
                return;
            }

            string propertyName = AnimationToJson.PropertyArr[idx];

            AnimKeyData data = new AnimKeyData(path, propertyName, keys);
            if (propertyName == "position.y")      //  "position.y"  y값 음수 곱해줌
            {
                data.MultiValue(-1);     
            }

            if (propertyName == "position.x")
            {
                data.MultiValue(1);
            }

            if (propertyName == "rotation")
            {
                data.MultiValue(-1);
            }

            keyDatas.Add(data);
        }

        public void AddEvenetData( AnimationEvent eventData )
        {
            EventKeyData data = new EventKeyData( eventData.functionName, eventData.time, eventData.floatParameter,
                eventData.intParameter, eventData.stringParameter);

            eventDatas.Add(data);
        }
    }

    public class AnimKeyData : ToJson
    {
        public string path;
        public string property;
        public List<KeyFrameData> keys = new List<KeyFrameData>();

        public string ToJson()
        {
            string str = string.Format("{0},{1},",
                StringHelper.KeyValueTojson("path", StringHelper.AddDoubleQuotation(path))
                , StringHelper.KeyValueTojson("property", StringHelper.AddDoubleQuotation(property)));

            string keyString = "";
            for (int i = 0; i < keys.Count; i++)
            {
                keyString += keys[i].ToJson();
                if (i == keys.Count - 1) continue;
                keyString += ",";
            }

            keyString = StringHelper.AddBracket(keyString);

            str += StringHelper.KeyValueTojson("keys", keyString);

            return StringHelper.AddBrace(str);
        }

        public AnimKeyData(string path, string property, Keyframe[] keys)
        {
            this.path = path;
            this.property = property;

            for (int i = 0; i < keys.Length; i++)
            {
                this.keys.Add(new KeyFrameData(keys[i].time, keys[i].value));
            }
        }

        public void MultiValue(float m)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                KeyFrameData data = keys[i];
                data.value *= m;
                keys[i] = data;
            }
        }        
    }

    public struct KeyFrameData : ToJson
    {
        public float time;
        public float value;

        public string ToJson()
        {
            return StringHelper.AddBrace(string.Format("{0},{1}"
                , StringHelper.KeyValueTojson("time", time)
                , StringHelper.KeyValueTojson("value", value)));
        }

        public KeyFrameData(float t, float v)
        {
            time = t;
            value = v;
        }
    }

    public static class AnimationToJson
    {
        //
        public static string[] Property = 
        {
            "none",
            "visible",          //1
            "scale.x",          //2
            "scale.y",          //3
            "scale.z",          //4
            "color.r",          //5
            "color.g",          //6
            "color.b",          //7
            "alpha",            //8
            "position.x",       //9
            "position.y",       //10
            "position.z",       //11
            "rotation.x",        //12
            "rotation.y",        //13
            "rotation",          //14
            "width",            //15
            "height",           //16
        };

        public static List<string> PropertyName = new List<string>()
        {                                                
            //"m_IsActive",                       
            //"m_LocalScale.x",                   
            //"m_LocalScale.y",                   
            //"m_LocalScale.z",                   
            //"m_Color.r",                        
            //"m_Color.g",                        
            //"m_Color.b",                        
            //"m_Color.a",                        
            //"m_LocalPosition.x",                
            //"m_LocalPosition.y",                
            //"m_LocalPosition.z",                
            //"localEulerAnglesRaw.x",            
            //"localEulerAnglesRaw.y",            
            //"localEulerAnglesRaw.z",                  
            //"m_AnchoredPosition.x",
            //"m_AnchoredPosition.y",
            //"m_SizeDelta.x",
            //"m_SizeDelta.y",

            //안쓰는 프로퍼티 제거
            "m_IsActive",
            "m_LocalScale.x",
            "m_LocalScale.y",
            "",
            "",
            "",
            "",
            "m_Color.a",
            "m_LocalPosition.x",
            "m_LocalPosition.y",
            "",
            "",
            "",
            "localEulerAnglesRaw.z",
            "m_AnchoredPosition.x",
            "m_AnchoredPosition.y",
            "m_SizeDelta.x",
            "m_SizeDelta.y",
        };

        public static string[] PropertyArr =
        {
            Property[1],
            Property[2],
            Property[3],
            Property[4],
            Property[5],
            Property[6],
            Property[7],
            Property[8],
            Property[9],
            Property[10],
            Property[11],
            Property[12],
            Property[13],
            Property[14],
            Property[9],
            Property[10],
            Property[15],
            Property[16],
        };

        public static string ToJson(AnimationClip clip)
        {
            AnimationClipCurveData[] dataArr = AnimationUtility.GetAllCurves(clip, true);
            AnimationEvent[] eventArr = AnimationUtility.GetAnimationEvents(clip); 

            AnimClipData clipData = new AnimClipData();
            clipData.name = clip.name;

            for (int i = 0; i < dataArr.Length; i++)
            {
                string path = dataArr[i].path;
                string property = dataArr[i].propertyName;
                Keyframe[] keys = dataArr[i].curve.keys;
                clipData.AddKeyData(path, property, keys);
            }

            for (int i = 0; i < eventArr.Length; i++)
            {
                clipData.AddEvenetData(eventArr[i]);
            }

            return clipData.ToJson();
        }

        //public static string ToJson(AnimationClip clip)
        //{
        //    AnimationClipCurveData[] dataArr = AnimationUtility.GetAllCurves(clip, true);

        //    AnimClipData clipData = new AnimClipData();
        //    clipData.name = clip.name;

        //    for (int i = 0; i < dataArr.Length; i++)
        //    {
        //        string path = dataArr[i].path;
        //        string property = dataArr[i].propertyName;
        //        Keyframe[] keys = dataArr[i].curve.keys;
        //        clipData.AddObject(path, property, keys);
        //    }                                        

        //    return StringHelper.AddBrace (clipData.ToJson());
        //}
    }

    public class EventKeyData : ToJson
    {
        public string eventName;
        public float time;
        public float floatParameter;
        public int intParameter;
        public string stringParameter;
        
        public EventKeyData(string n, float t, float fp = 0, int ip = 0, string sp = "" )
        {
            eventName = n;
            time = t;
            floatParameter = fp;
            intParameter = ip;
            stringParameter = sp;
        }

        public string ToJson()
        {
            string str = "";
            str = string.Format("{0},{1}"
                , StringHelper.KeyValueTojson("eventName", StringHelper.AddDoubleQuotation(eventName))
                , StringHelper.KeyValueTojson("time", time));
            
            if( floatParameter != 0 )
            {
                str += "," + StringHelper.KeyValueTojson("parameter", floatParameter);
            }
            else if (intParameter != 0)
            {
                str += "," + StringHelper.KeyValueTojson("parameter", intParameter);
            }
            else if (stringParameter != "")
            {
                str += "," + StringHelper.KeyValueTojson("parameter", StringHelper.AddDoubleQuotation(stringParameter));
            }

            return StringHelper.AddBrace(str);
        }
    }

}