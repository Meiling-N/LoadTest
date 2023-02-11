using System.IO;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    const int cLoopKaisuForAverage = 100;
    const int cLoadNum             = 10000;

    Stack<Sprite> spriteStack = new Stack<Sprite>();
    Stack<Image>  imageStack  = new Stack<Image>();
    Sprite surviveTmp;

    int loopCount     = 0;
    float averageTime = 0;

    void Update()
    {
        //計測開始
        var startTime = Time.realtimeSinceStartup;
        for(int i = 0; i < cLoadNum; i++) {
            loadA();
        }
        var endTime   = Time.realtimeSinceStartup;

        //かかった時間を出す
        var passedTime = endTime - startTime;
        averageTime   += passedTime;
        Debug.Log("passed: " + passedTime);
        
        //後処理
        destruct();
        loopCount++;
        if(loopCount == cLoopKaisuForAverage) {
            Debug.Log("averageTime: " + averageTime / (float)cLoopKaisuForAverage);
            Destroy(this.gameObject);
        }
    }

    void destruct() {
        spriteStack.Clear();
        imageStack.Clear();
        surviveTmp = null;
    }






    //毎回Resources.Load
    void loadA() {
        spriteStack.Push(Resources.Load<Sprite>("128"));
        //imageStack.Push(Resources.Load<Image>("128"));
    }

    //作った奴を使う,create
    void loadB() {
        string path = "512";
        if(spriteStack.Count == 0) {
            surviveTmp = Resources.Load<Sprite>(path);
            spriteStack.Push(surviveTmp);
        } else {
            spriteStack.Push(Sprite.Create(surviveTmp.texture, surviveTmp.rect, surviveTmp.pivot));
        }
    }

    //作った奴を使う,instantiate
    void loadC() {
        string path = "32";
        if (spriteStack.Count == 0) {
            surviveTmp = Resources.Load<Sprite>(path);
            spriteStack.Push(surviveTmp);
        } else {
            spriteStack.Push(Instantiate(surviveTmp));
        }
    }

    //毎回外部ファイルを読み込み
    //C#の System.Drawing.Imageが標準では入ってなさそう
    void loadD() {
        var path = System.IO.Path.Combine(Application.dataPath, "Gaibu\\32.png");
        var tex = readImageFile(path);
        spriteStack.Push(Sprite.Create(tex, new Rect(Vector2.zero, tex.texelSize), Vector2.zero));
    }

    Texture2D readImageFile(string path) {
        byte[] val = null;
        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
            BinaryReader bin = new BinaryReader(fileStream);
            val = bin.ReadBytes((int)bin.BaseStream.Length);
            bin.Close();
        }
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(val);
        return texture;
    }

}
