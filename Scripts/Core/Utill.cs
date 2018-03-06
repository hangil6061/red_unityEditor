using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Red.Utill
{
    public struct IntRect : ToJson
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public IntRect(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public string ToJson()
        {
            return StringHelper.AddBrace( string.Format("{0},{1},{2},{3}"
                , StringHelper.KeyValueTojson("x", x), StringHelper.KeyValueTojson("y", y)
                , StringHelper.KeyValueTojson("w", w), StringHelper.KeyValueTojson("h", h)));
        }
    }

    public struct IntBorder : ToJson
    {
        public int l;
        public int t;
        public int r;
        public int b;

        public IntBorder(int l, int b, int r, int t)
        {
            this.l = l;           
            this.r = r;
            this.b = b;
            this.t = t;
        }

        public IntBorder(Vector4 bor)
        {
            this.l = (int)bor.x;
            this.b = (int)bor.y;
            this.r = (int)bor.z;
            this.t = (int)bor.w;
        }

        public string ToJson()
        {
            return StringHelper.AddBrace(string.Format("{0},{1},{2},{3}"
                , StringHelper.KeyValueTojson("l", l), StringHelper.KeyValueTojson("t", t)
                , StringHelper.KeyValueTojson("r", r), StringHelper.KeyValueTojson("b", b)));
        }
    }

    public struct Int2 : ToJson
    {
        public int x;
        public int y;

        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Int2(float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
        }
        
        public string ToJson()
        {
            return StringHelper.AddBrace( string.Format("{0},{1}", StringHelper.KeyValueTojson("x", x), StringHelper.KeyValueTojson("y", y)));
        }
    }

    public struct Float2 : ToJson
    {
        public float x;
        public float y;

        public Float2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public string ToJson()
        {
            return StringHelper.AddBrace(string.Format("{0},{1}", StringHelper.KeyValueTojson("x", x), StringHelper.KeyValueTojson("y", y)));
        }
    }

    public static class StringHelper
    {
        public static string AddMark(string str, string front, string back)
        {
            return front + str + back;
        }

        public static string AddDoubleQuotation(string str)
        {
            return AddMark(str, "\"", "\"");
        }

        public static string AddNewLine(string str)
        {
            return AddMark(str, "\n", "\n");
        }

        public static string AddBrace(string str)
        {
            return AddMark(str, "{", "}");
        }

        public static string AddBracket(string str)
        {
            return AddMark(str, "[", "]");
        }

        public static string KeyValueTojson(string key, object value )
        {
            string vStr = value is ToJson ? (value as ToJson).ToJson() : value.ToString();
            return string.Format("{0}:{1}", AddDoubleQuotation(key), vStr);
        }        
    }

    public static class Utill
    {
        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }
    }

    public static class FileIO
    {
        public static void WriteData(string strData, string fileName, bool isNewCreate = false, string forder = "/save/", bool isOpen = true)
        {
            string path = /*Application.dataPath +*/ forder;
            string filePath = path + fileName;
            if( !isNewCreate )
            {
                filePath = GetUniqueFileNameWithPath(path, fileName);
            }
            

            FileStream f = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(f, System.Text.Encoding.UTF8 );
            writer.WriteLine(strData);
            writer.Close();

            if (isOpen)
            {
                Process.Start(path);
            }
        }

        public static void CopyWritePng(Texture2D tex, string fileName, string forder = "/save/", bool isOpen = true)
        {
            if (tex == null) return;
            Texture2D newTex = CopyTexture2D(tex);
            WritePng(newTex, fileName, forder, isOpen);
            Object.DestroyImmediate(newTex);
        }

        public static Texture2D CopyTexture2D( Texture2D target )
        {
            Texture2D tex = new Texture2D(target.width, target.height, target.format, false);
            tex.LoadRawTextureData(target.GetRawTextureData());
            tex.Apply();        
            return tex;
        }

        public static void WritePng( Texture2D tex, string fileName, string forder = "/save/", bool isOpen = true)
        {
            string path = /*Application.dataPath +*/ forder;
            //string filePath = GetUniqueFileNameWithPath(path, fileName);

            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(path + "/" + fileName, bytes);
        }

        public static string GetUniqueFileNameWithPath(string dirPath, string fileN)
        {
            string fileName = fileN;

            int indexOfDot = fileName.LastIndexOf(".");
            string strName = fileName.Substring(0, indexOfDot);
            string strExt = fileName.Substring(indexOfDot + 1);

            bool bExist = true;
            int fileCount = 0;

            while (bExist)
            {
                if (File.Exists(Path.Combine(dirPath, fileName)))
                {
                    fileCount++;
                    fileName = strName + "(" + fileCount + ")." + strExt;
                }
                else
                {
                    bExist = false;
                }
            }
            return Path.Combine(dirPath, fileName);
        }
    }

    public interface ToJson
    {
        string ToJson();
    }
}