using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Red.Utill;
using System;

namespace Red
{

    [System.Serializable]
    public class UIData : ToJson
    {
        public string name;
        public GameObject go;
        public Int2 position;
        public Int2 localPosition;
        public Float2 scale = new Float2(1, 1);
        public Float2 pivot = new Float2(0.5f, 0.5f);
        public UIComponent component;
        public List<UIData> children = new List<UIData>();
        
        public UIData(Transform root, Transform tr)
        {
            Vector2 piv = new Vector2(0.5f, 0.5f);
            if (tr is RectTransform)
            {
                piv = (tr as RectTransform).pivot;
            }
            Vector2 pos = root.InverseTransformPoint(tr.position);

            name = tr.name;
            pivot = new Float2(piv.x, piv.y);
            scale = new Float2(tr.localScale.x, tr.localScale.y);
            position = new Int2(pos.x, pos.y);
            localPosition = new Int2(tr.localPosition.x * 1, tr.localPosition.y * 1);
            go = tr.gameObject;

            Component comp = tr.GetComponent<SpriteRenderer>();
            if (comp != null)
            {
                SpriteRenderer c = comp as SpriteRenderer;
                if( c.sprite != null)
                {
                    component = new UIComponentSprite(c.sprite.name, Utill.Utill.ColorToHex(c.color), c.color.a
                       , comp.GetComponent<UIInteractive>() != null ? true : false);
                }
                else
                {
                    component = new UIComponentSprite( "null", Utill.Utill.ColorToHex(c.color), c.color.a
                    , comp.GetComponent<UIInteractive>() != null ? true : false);
                    (component as UIComponentSprite).width = (tr as RectTransform).sizeDelta.x;               
                    (component as UIComponentSprite).height = (tr as RectTransform).sizeDelta.y;
                }
            }

            comp = tr.GetComponent<Image>();
            if (comp != null)
            {
                Image image = comp as Image;
                Button button = tr.GetComponent<Button>();
                
                if( button != null )
                {
                    string normal = "";
                    string over = "";
                    string push = "";
                    string disabled = "";
                    int transition = 0;
                    bool isBorder = false;
                    IntBorder border = new IntBorder();
                    int width = 0;
                    int height = 0;

                    if( image.type ==  Image.Type.Sliced )
                    {
                        isBorder = true;
                        float rate = image.sprite.pixelsPerUnit / 100f;
                        Vector4 bor = image.sprite.border;
                        bor /= rate;
                        border = new IntBorder(bor);
                        width = (int)image.rectTransform.sizeDelta.x;
                        height = (int)image.rectTransform.sizeDelta.y;
                    }
                    

                    if(button.transition == Selectable.Transition.SpriteSwap)
                    {
                        transition = 2;
                        normal = image.sprite.name;
                        over = button.spriteState.highlightedSprite != null ? button.spriteState.highlightedSprite.name : normal;
                        push = button.spriteState.pressedSprite != null ? button.spriteState.pressedSprite.name : normal;
                        disabled = button.spriteState.disabledSprite != null ? button.spriteState.disabledSprite.name : "";
                        component = new UIComponentButton(transition, normal, over, push, disabled);

                        if( isBorder )
                        {
                            UIComponentButton componentButton = component as UIComponentButton;
                            componentButton.isBorder = isBorder;
                            componentButton.border = border;
                            componentButton.width = width;
                            componentButton.height = height;
                        }
                    }
                    else if (button.transition == Selectable.Transition.ColorTint)
                    {
                        transition = 1;
                        normal = "0x" + Utill.Utill.ColorToHex(button.colors.normalColor);
                        over = "0x" + Utill.Utill.ColorToHex(button.colors.highlightedColor);
                        push = "0x" + Utill.Utill.ColorToHex(button.colors.pressedColor);
                        disabled = "0x" + Utill.Utill.ColorToHex(button.colors.disabledColor);
                        component = new UIComponentButton(transition, normal, over, push, disabled);
                        UIComponentButton componentButton = component as UIComponentButton;
                        componentButton.spriteName = image.sprite.name;

                        if (isBorder)
                        {                            
                            componentButton.isBorder = isBorder;
                            componentButton.border = border;
                            componentButton.width = width;
                            componentButton.height = height;
                        }
                    }   
                }   
                else if (image.sprite != null && image.type == Image.Type.Sliced )
                {
                    float rate = image.sprite.pixelsPerUnit / 100f;
                    Vector4 bor = image.sprite.border;
                    bor /= rate;

                    component = new UIComponentSpriteBorder(image.sprite.name
                        , new IntBorder(bor)
                        , image.rectTransform.sizeDelta.x
                        , image.rectTransform.sizeDelta.y
                        , Utill.Utill.ColorToHex(image.color)
                        , image.color.a
                        , comp.GetComponent<UIInteractive>() != null ? true : false
                        );
                }
                else if(image.sprite != null && image.type == Image.Type.Tiled )
                {
                    float rate = image.sprite.pixelsPerUnit / 100f;

                    component = new UIComponentSpriteTiled(image.sprite.name
                        , image.rectTransform.sizeDelta.x
                        , image.rectTransform.sizeDelta.y
                        , Utill.Utill.ColorToHex(image.color)
                        , image.color.a
                        , comp.GetComponent<UIInteractive>() != null ? true : false
                        );
                }
                else
                {                    
                    //component = new UIComponentSprite(image.sprite.name, Utill.Utill.ColorToHex(image.color), image.color.a
                    //    , comp.GetComponent<UIInteractive>() != null ? true : false);

                    if (image.sprite != null)
                    {
                        component = new UIComponentSprite(image.sprite.name, Utill.Utill.ColorToHex(image.color), image.color.a
                           , comp.GetComponent<UIInteractive>() != null ? true : false);
                    }
                    else
                    {
                        component = new UIComponentSprite("null", Utill.Utill.ColorToHex(image.color), image.color.a
                        , comp.GetComponent<UIInteractive>() != null ? true : false);
                        (component as UIComponentSprite).width = (tr as RectTransform).sizeDelta.x;
                        (component as UIComponentSprite).height = (tr as RectTransform).sizeDelta.y;
                    }

                }
            }

            comp = tr.GetComponent<Text>();
            if( comp != null )
            {
                Text text = comp as Text;
                int width = 0;

                if (text.horizontalOverflow == HorizontalWrapMode.Wrap)
                {
                    width = (int)text.rectTransform.sizeDelta.x;
                }

                bool isNormal = tr.GetComponent<UINormalFont>() == null ? false : true;

                string align = text.alignment.ToString().Replace("Upper", "").Replace("Middle", "").Replace("Lower", "");
                component = new UIComponentText(text.font.name, text.text.Replace("\n", "\\n").Replace("\r", ""), text.fontSize
                    , Utill.Utill.ColorToHex( text.color ), align.ToLower(), width, isNormal);

                
            }

            comp = tr.GetComponent<UIInputField>();
            if( comp != null )
            {
                UIInputField input = comp as UIInputField;
                RectTransform rtr = comp.GetComponent<RectTransform>();
                component = new UIComponentInput( input.placeholder, (int)rtr.sizeDelta.x, (int)rtr.sizeDelta.y
                    , input.fontSize, Utill.Utill.ColorToHex(input.fontColor), Utill.Utill.ColorToHex(input.placeholderColor)
                    , input.isTextArea, input.isPassword, input.isNumberOnly);
            }

            comp = tr.GetComponent<UIScroll>();
            if( comp != null )
            {
                UIScroll scroll = comp as UIScroll;
                component = new UIComponentScroll( scroll.scrollArea.name, scroll.scrollMask.name,
                    scroll.scrollbar_bar.name, scroll.scrollbar_bg.name );
            }

            comp = tr.GetComponent<UIMask>();
            if (comp != null)
            {
                RectTransform rtr = comp.GetComponent<RectTransform>();
                component = new UIComponentMask((int)rtr.sizeDelta.x, (int)rtr.sizeDelta.y );
            }

            comp = tr.GetComponent<UIToggleGroup>();
            if( comp != null )
            {
                UIToggleGroup tg = comp as UIToggleGroup;
                component = new UIComponentToggleGroup(tg.toggles, tg.defaultIndex);
            }

            int count = tr.childCount;
            for (int i = 0; i < count; i++)
            {
                children.Add(new UIData(root, tr.transform.GetChild(i)));
            }
        }

       

        public string ToJson()
        {
            Int2 pos = position;
            Int2 localPos = localPosition;
            Float2 piv = pivot;
            pos.y *= -1;
            localPos.y *= -1;
            piv.y = 1 - piv.y;

            string str = string.Format("{0},{1},{2},{3},{4},{5}",
                StringHelper.KeyValueTojson("name", StringHelper.AddDoubleQuotation(name)),
                StringHelper.KeyValueTojson("visible", StringHelper.AddDoubleQuotation( go.activeSelf.ToString().ToLower())),
                StringHelper.KeyValueTojson("position", pos),
                StringHelper.KeyValueTojson("localPosition", localPos),
                StringHelper.KeyValueTojson("pivot", piv),
                StringHelper.KeyValueTojson("scale", scale));

            if (component != null)
            {
                str += string.Format(",{0}", StringHelper.KeyValueTojson(component.name, component.ToJson()));
            }

            string cStr = "";
            if (children.Count > 0)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    cStr += children[i].ToJson();
                    if (i == children.Count - 1) continue;
                    cStr += ",\n";
                }
            }

            str += "," + StringHelper.KeyValueTojson("children", StringHelper.AddBracket(cStr));

            return StringHelper.AddBrace(str);
        }
    }

    public class UIComponent : ToJson
    {
        public string name;

        public virtual string ToJson()
        {
            return "";
        }
    }

    public class UIComponentSprite : UIComponent
    {
        public string spriteName;
        public string color;
        public float alpha;
        public bool isInteractive;
        public float width = 0;
        public float height = 0;

        public UIComponentSprite(string name, string color, float alpha, bool isInteractive = false)
        {
            this.name = "sprite";
            spriteName = name;
            this.color = color;
            this.alpha = alpha;
            this.isInteractive = isInteractive;
        }

        public override string ToJson()
        {
            string str = string.Format("{0},{1},{2},{3},{4},{5}"
                , StringHelper.KeyValueTojson("spriteName", StringHelper.AddDoubleQuotation(spriteName))
                , StringHelper.KeyValueTojson("width", width)
                , StringHelper.KeyValueTojson("height", height)
                , StringHelper.KeyValueTojson("color", StringHelper.AddDoubleQuotation("0x" + color))
                , StringHelper.KeyValueTojson("alpha", alpha)
                , StringHelper.KeyValueTojson("isInteractive", isInteractive.ToString().ToLower())
                );

            return StringHelper.AddBrace(str);
        }
    }

    public class UIComponentSpriteBorder : UIComponent
    {
        public string spriteName;
        public IntBorder border;
        public float width = 0;
        public float height = 0;
        public string color;
        public float alpha;
        public bool isInteractive = false;

        public UIComponentSpriteBorder(string name, IntBorder border, float width, float height, string color, float alpha, bool isInteractive = false)
        {
            this.name = "nineSlice";
            spriteName = name;
            this.border = border;
            this.width = width;
            this.height = height;
            this.color = color;
            this.alpha = alpha;
            this.isInteractive = isInteractive;
        }

        public override string ToJson()
        {
            string str = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                , StringHelper.KeyValueTojson("spriteName", StringHelper.AddDoubleQuotation(spriteName))
                , StringHelper.KeyValueTojson("border", border.ToJson())
                , StringHelper.KeyValueTojson("width", width)
                , StringHelper.KeyValueTojson("height", height)
                , StringHelper.KeyValueTojson("color", StringHelper.AddDoubleQuotation("0x" + color))
                , StringHelper.KeyValueTojson("alpha", alpha)
                , StringHelper.KeyValueTojson("isInteractive", isInteractive.ToString().ToLower())
                );           

            return StringHelper.AddBrace(str);
        }
    }

    public class UIComponentSpriteTiled : UIComponent
    {
        public string spriteName;
        public float width = 0;
        public float height = 0;
        public string color;
        public float alpha;
        public bool isInteractive = false;

        public UIComponentSpriteTiled(string name, float width, float height, string color, float alpha, bool isInteractive = false)
        {
            this.name = "tilingSprite";
            spriteName = name;
            this.width = width;
            this.height = height;
            this.color = color;
            this.alpha = alpha;
            this.isInteractive = isInteractive;
        }

        public override string ToJson()
        {
            string str = string.Format("{0},{1},{2},{3},{4},{5}"
                , StringHelper.KeyValueTojson("spriteName", StringHelper.AddDoubleQuotation(spriteName))
                , StringHelper.KeyValueTojson("width", width)
                , StringHelper.KeyValueTojson("height", height)
                , StringHelper.KeyValueTojson("color", StringHelper.AddDoubleQuotation("0x" + color))
                , StringHelper.KeyValueTojson("alpha", alpha)
                , StringHelper.KeyValueTojson("isInteractive", isInteractive.ToString().ToLower())
                );

            return StringHelper.AddBrace(str);
        }
    }

    public class UIComponentButton : UIComponent
    {
        public string spriteName;
        public bool isBorder;
        public IntBorder border;
        public float width = 0;
        public float height = 0;

        public int transition = 0; 
        public string normal;
        public string over;
        public string push;
        public string disabled;

        
        public UIComponentButton(int transition, string normal, string over, string push, string disabled)
        {
            this.name = "button";
            this.transition = transition;
            this.normal = normal;
            this.over = over;
            this.push = push;
            this.disabled = disabled;
        }

        public override string ToJson()
        {
            string str = string.Format("{0},{1},{2},{3},{4}"
                , StringHelper.KeyValueTojson("transition", transition)
                , StringHelper.KeyValueTojson("normal", StringHelper.AddDoubleQuotation(normal))
                , StringHelper.KeyValueTojson("over", StringHelper.AddDoubleQuotation(over))
                , StringHelper.KeyValueTojson("push", StringHelper.AddDoubleQuotation(push))
                , StringHelper.KeyValueTojson("disabled", StringHelper.AddDoubleQuotation(disabled)));

            if(transition == 1)
            {
                str += "," + StringHelper.KeyValueTojson("spriteName", StringHelper.AddDoubleQuotation(spriteName));
            }

            if(isBorder)
            {
                str += "," + StringHelper.KeyValueTojson("border", border.ToJson());
                str += "," + StringHelper.KeyValueTojson("width", width);
                str += "," + StringHelper.KeyValueTojson("height", height);
            }

            return StringHelper.AddBrace(str);
        }
    }

    public class UIComponentText : UIComponent
    {
        public string font;
        public string text;
        public int size;
        public string color;
        public int width = 0;
        public string align = "left";
        public bool isNormal = false;

        public UIComponentText(string font, string text, int size, string color, string align = "left", int width = 0, bool normal = false)
        {
            this.name = "text";
            this.font = font;
            this.text = text;
            this.size = size;
            this.color = color;
            this.align = align;
            this.width = width;
            this.isNormal = normal;
        }

        public override string ToJson()
        {
            string str = string.Format("{0},{1},{2},{3},{4}"
                , StringHelper.KeyValueTojson("font", StringHelper.AddDoubleQuotation(font))
                , StringHelper.KeyValueTojson("text", StringHelper.AddDoubleQuotation(text))
                , StringHelper.KeyValueTojson("size", size)
                , StringHelper.KeyValueTojson("align", StringHelper.AddDoubleQuotation(align))
                , StringHelper.KeyValueTojson("color", StringHelper.AddDoubleQuotation("0x" + color)));      
            str += "," + StringHelper.KeyValueTojson("width", width);
            str += "," + StringHelper.KeyValueTojson("isNormal", isNormal.ToString().ToLower());
            return StringHelper.AddBrace(str);                
        }
    }

    public class UIComponentInput : UIComponent
    {
        public string placeholder;
        public int width = 0;
        public int height = 0;
        public int fontSzie = 18;
        public string fontColor;
        public string placeholderColor;
        public bool isTextArea = false;
        public bool isPassword = false;
        public bool isNumberOnly = false;
        

        public UIComponentInput(string placeholder, int width , int height, int fontSize, string fontColor, string placeholderColor,bool isTextArea = false, bool isPassword = false, bool isNumberOnly = false)
        {
            this.name = "input";
            this.placeholder = placeholder;
            this.width = width;
            this.height = height;
            this.fontSzie = fontSize;
            this.fontColor = fontColor;
            this.placeholderColor = placeholderColor;
            this.isTextArea = isTextArea;
            this.isPassword = isPassword;
            this.isNumberOnly = isNumberOnly;
        }

        public override string ToJson()
        {
            string str = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}"
                , StringHelper.KeyValueTojson("placeholder", StringHelper.AddDoubleQuotation(placeholder))
                , StringHelper.KeyValueTojson("width", width)
                , StringHelper.KeyValueTojson("height", height)
                , StringHelper.KeyValueTojson("fontSzie", fontSzie)
                , StringHelper.KeyValueTojson("fontColor", StringHelper.AddDoubleQuotation(fontColor))
                , StringHelper.KeyValueTojson("placeholderColor", StringHelper.AddDoubleQuotation(placeholderColor))
                , StringHelper.KeyValueTojson("isTextArea", StringHelper.AddDoubleQuotation(isTextArea.ToString().ToLower()))
                , StringHelper.KeyValueTojson("isPassword", StringHelper.AddDoubleQuotation(isPassword.ToString().ToLower()))
                , StringHelper.KeyValueTojson("isNumberOnly", StringHelper.AddDoubleQuotation(isNumberOnly.ToString().ToLower()))
                );

            return StringHelper.AddBrace(str);
        }
    }

    public class UIComponentScroll : UIComponent
    {
        public string area;
        public string mask;
        public string bar;
        public string barBg;

        public UIComponentScroll( string area, string mask, string bar, string barBg )
        {
            name = "scroll";
            this.area = area;
            this.mask = mask;
            this.bar = bar;
            this.barBg = barBg;
        }

        public override string ToJson()
        {
            string str = string.Format("{0},{1},{2},{3}"
                , StringHelper.KeyValueTojson("area", StringHelper.AddDoubleQuotation(area))
                , StringHelper.KeyValueTojson("mask", StringHelper.AddDoubleQuotation(mask))
                , StringHelper.KeyValueTojson("bar", StringHelper.AddDoubleQuotation(bar))
                , StringHelper.KeyValueTojson("barBg", StringHelper.AddDoubleQuotation(barBg))
                );

            return StringHelper.AddBrace(str);
        }
    }

    public class UIComponentMask : UIComponent
    {
        public int width;
        public int height;

        public UIComponentMask(int w, int h)
        {
            name = "mask";
            width = w;
            height = h;
        }

        public override string ToJson()
        {
            string str = string.Format("{0},{1}"
                , StringHelper.KeyValueTojson("width", width)
                , StringHelper.KeyValueTojson("height", height)
                );

            return StringHelper.AddBrace(str);
        }
    }

    public class UIComponentToggleGroup : UIComponent
    {
        public UIToggleButton[] toggles;
        public int defaultIndex = 0;

        public UIComponentToggleGroup(UIToggleButton[] toggles, int defaultIndex)
        {
            name = "toggleGroup";
            this.toggles = toggles;
            this.defaultIndex = defaultIndex;
        }

        public override string ToJson()
        {
            string str = "";
            for( int i = 0; i < toggles.Length; i++ )
            {
                str += StringHelper.AddBrace(string.Format("{0},{1}"
                    , StringHelper.KeyValueTojson("buttonName", StringHelper.AddDoubleQuotation( toggles[i].button.name ))
                    , StringHelper.KeyValueTojson("onImageName", StringHelper.AddDoubleQuotation( toggles[i].onImage.name ))) );

                if (i == toggles.Length - 1) continue;
                str += ",";
            }

            str = string.Format("{0},{1}"
                , StringHelper.KeyValueTojson("defaultIndex", defaultIndex)
                , StringHelper.KeyValueTojson("toggles", StringHelper.AddBracket(str)));

            return StringHelper.AddBrace(str);
        }
    }


    public static class LayoutToJson
    {
        public static string ToJson(Transform root)
        {
            List<UIData> uiDatas = new List<UIData>();
            int count = root.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                uiDatas.Add(new UIData(root, root.transform.GetChild(i)));
            }

            string str = "";
            for (int i = 0; i < uiDatas.Count; i++)
            {
                str += uiDatas[i].ToJson();
                if (i == uiDatas.Count - 1) continue;
                str += ",\n";
            }

            return StringHelper.AddBracket(str);
        }
    }
}