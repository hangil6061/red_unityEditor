using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Red.Utill;

namespace Red
{
    public struct SpriteData : ToJson
    {
        public IntRect frame;
        public bool rotated;
        public bool trimmed;
        public IntRect spriteSourceSize;
        public Int2 sourceSize;

        public SpriteData(int x, int y, int w, int h)
        {
            frame = new IntRect(x, y, w, h);
            rotated = false;
            trimmed = false;
            spriteSourceSize = new IntRect(0, 0, w, h);
            sourceSize = new Int2(w, h);
        }

        public string ToJson()
        {
            return StringHelper.AddBrace(string.Format("{0},{1},{2},{3},{4}"
                , StringHelper.KeyValueTojson("frame", frame)
                , StringHelper.KeyValueTojson("rotated", rotated.ToString().ToLower())
                , StringHelper.KeyValueTojson("trimmed", trimmed.ToString().ToLower())
                , StringHelper.KeyValueTojson("spriteSourceSize", spriteSourceSize)
                , StringHelper.KeyValueTojson("sourceSize", sourceSize)));
        }
    }

    public struct AtlasMeta : ToJson
    {
        public string app;
        public string version;
        public string image;
        public Int2 size;
        public float scale;

        public AtlasMeta(string app, string version, string img, int w, int h, float scale)
        {
            this.app = app;
            this.version = version;
            this.image = img;
            this.size = new Int2(w, h);
            this.scale = scale;
        }

        public string ToJson()
        {
            return StringHelper.AddBrace(string.Format("{0},{1},{2},{3}"
                , StringHelper.KeyValueTojson("app", StringHelper.AddDoubleQuotation(app))
                 , StringHelper.KeyValueTojson("version", StringHelper.AddDoubleQuotation(version))
                , StringHelper.KeyValueTojson("image", StringHelper.AddDoubleQuotation(image))
                , StringHelper.KeyValueTojson("size", size)
                , StringHelper.KeyValueTojson("scale", scale)));
        }
    }

    public static class SpriteSheetToJspm
    {
        public static string ToJson( Sprite[] spriteList)
        {
            var texHeight = spriteList[0].texture.height;
            string frameList = "";
            for (int i = 0; i < spriteList.Length; i++)
            {
                Rect data = spriteList[i].rect;
                frameList += StringHelper.KeyValueTojson(spriteList[i].name, new SpriteData((int)data.x, (texHeight - (int)data.y) - (int)data.height, (int)data.width, (int)data.height));
                if (i == spriteList.Length - 1) continue;
                frameList += ",\n";
            }
            frameList = StringHelper.AddBrace(frameList);

            Texture tex = spriteList[0].texture;
            var meta = new AtlasMeta(Config.app, Config.version, tex.name + ".png", tex.width, tex.height, 1);
            return StringHelper.AddBrace(string.Format("{0},\n{1}", StringHelper.KeyValueTojson("frames", frameList)
                , StringHelper.KeyValueTojson("meta", meta.ToJson())));
        }
    }


    public static class AtlasToJson
    {
        public static string ToJson(UIAtlas atlas)
        {
            List<UISpriteData> spriteList = atlas.spriteList;

            string frameList = "";
            for (int i = 0; i < spriteList.Count; i++)
            {
                var data = spriteList[i];
                frameList += StringHelper.KeyValueTojson(data.name, new SpriteData(data.x, data.y, data.width, data.height));
                if (i == spriteList.Count - 1) continue;
                frameList += ",\n";
            }
            frameList = StringHelper.AddBrace(frameList);

            var meta = new AtlasMeta(Config.app, Config.version, atlas.name + ".png", atlas.texture.width, atlas.texture.height, 1);

            return StringHelper.AddBrace(string.Format("{0},\n{1}", StringHelper.KeyValueTojson("frames", frameList)
                , StringHelper.KeyValueTojson("meta", meta.ToJson())));
        }

        //{
        //    "frames":
        //    {
        //        "s_block_start_normal":
        //        {
        //            "frame":{"x":0,"y":0,"w":120,"h":84},
        //            "rotated":false,
        //            "trimmed":false,
        //            "spriteSourceSize":{"x":0,"y":0,"w":120,"h":84},
        //            "sourceSize":{"w":120,"h":84}
        //        }
        //    },
        //    "meta":
        //    {
        //        "app":"https://www.leshylabs.com/apps/sstool/",
        //        "version":"Leshy SpriteSheet Tool v0.8.4",
        //        "image":"spritesheet.png",
        //        "size":{"w":120,"h":84},
        //        "scale":1
        //    }
        //}
    }
}