using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public static class UnityExtends
{

    public static T AddMissingComponent<T>(this GameObject g) where T : Component
    {
         T t = g.GetComponent<T>();
        if(t==null)
          t=g.AddComponent<T>();
        return t;
    }


    public static void ToggleGroupSetValue(this ToggleGroup group,int num,bool isCheck)
    {
        foreach(Transform t in group.transform)
        {
            if (t.gameObject.name.StartsWith("" + num))
            {
                t.gameObject.gameObject.GetComponent<Toggle>().isOn = isCheck;
              
            }
            else
            {
                t.gameObject.gameObject.GetComponent<Toggle>().isOn = !isCheck;
            }
        }
    }
    public static int ToggleGroupGetValue(this ToggleGroup group)
    {
        IEnumerable<Toggle> togs = group.ActiveToggles();
        foreach (Toggle t in togs)
        {
            int k = int.Parse(t.gameObject.name.Substring(0, 1));
            return k;
        }
        return -1;

    }

    public static void LayerCullingShow(this Camera cam, int layerMask)
    {
        cam.cullingMask |= layerMask;
    }

    public static void LayerCullingShow(this Camera cam, string layer)
    {
        LayerCullingShow(cam, 1 << LayerMask.NameToLayer(layer));
    }

    public static void LayerCullingHide(this Camera cam, int layerMask)
    {
        cam.cullingMask &= ~layerMask;
    }

    public static void LayerCullingHide(this Camera cam, string layer)
    {
        LayerCullingHide(cam, 1 << LayerMask.NameToLayer(layer));
    }

    public static Color RGBInt(this Color c,int r,int g,int b,int a)
    {
        return new Color(r/255.0f,g/255.0f,b/255.0f,a/255.0f);
    }

    public static Color RGBInt(this Color c, int r, int g, int b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
    }


    public static Vector4 CalculateImageWorldPosRation(this Image img,Canvas cans) {
        Vector4 v4 = Vector4.zero;
        RectTransform rt = img.rectTransform;
        Vector3[] posReal = new Vector3[4];
        Vector2 contentSize = cans.gameObject.GetComponent<RectTransform>().rect.size;
        Vector2 _scale = cans.gameObject.GetComponent<RectTransform>().localScale;
        rt.GetWorldCorners(posReal);
        bool isS = img.gameObject.activeInHierarchy;
       

        v4.x = posReal[0].x/contentSize.x/_scale.x;
        v4.y = posReal[0].y/contentSize.y/_scale.y;
        v4.z = (posReal[3].x - posReal[0].x) / contentSize.x / _scale.x;  //宽度
        v4.w= (posReal[1].y - posReal[0].y) / contentSize.y/ _scale.y;

        return v4;
    }
}
