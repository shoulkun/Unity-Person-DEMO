using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSeed
{
    /// <summary>
    /// 用户输入的Seed
    /// </summary>
    private static string strOutSeed = "";
    /// <summary>
    /// 替换VIO文字后的Seed
    /// </summary>
    private static string normalSeed = "";
    /// <summary>
    /// 文字转Ascii码
    /// </summary>
    private static string asciiSeed = "";
    /// <summary>
    /// 按一定规律打乱Seed
    /// </summary>
    private static string shuffleSeed = "";
    /// <summary>
    /// 将shuffleSeed二次有规律的随机
    /// </summary>
    private static string balanceSeed = "";
    /// <summary>
    /// 最终局内Seed，随机事件，地图歙生成等用
    /// </summary>
    private static string seed = "";

    public static string GetSeed()
    {
        return seed;
    }

    public static void SetSeed(string s)
    {
        strOutSeed = s;
        Debug.Log("strOutSeed:"+strOutSeed);
    }
    
    public static void computeOutResult()
    {

        replaceVIO();
        asciiReplace();
        suffleReplace();
        balanceReplace();
        generate();
    }

    private static void balanceReplace()
    {
        balanceSeed = "";

        for (int i = 0; i < shuffleSeed.Length; i++)
        {
            int a = 0, b = 0, sum = 0, r = 0;
            float n = 0f;
            int.TryParse(""+shuffleSeed[i], out a);

            for (int j = 0; j < shuffleSeed.Length; j++)
            {
                int.TryParse(""+shuffleSeed[j], out b);
                sum +=b+i;
            }
            Random.InitState(sum*a);
            n = Random.value;
            //Debug.Log(n);
            int.TryParse(n.ToString().Substring(n.ToString().Length-1 ,1), out r);
            balanceSeed += r.ToString();
        }

        // int a,b,c,d,e,f,g,h;
        // int.TryParse(""+shuffleSeed[0]+shuffleSeed[3], out a);
        // int.TryParse(""+shuffleSeed[1]+shuffleSeed[2], out b);
        // int.TryParse(""+shuffleSeed[6]+shuffleSeed[9], out c);
        // int.TryParse(""+shuffleSeed[7]+shuffleSeed[8], out d);


        // int.TryParse(""+shuffleSeed[0], out e);
        // int.TryParse(""+shuffleSeed[2], out f);
        // int.TryParse(""+shuffleSeed[6], out g);
        // int.TryParse(""+shuffleSeed[8], out h);

        // balanceSeed += (a/2).ToString()[0];
        // balanceSeed += e/2;
        // balanceSeed += f/2;
        // balanceSeed += (b/2).ToString()[0];
        // balanceSeed += shuffleSeed.Substring(4, 2);
        // balanceSeed += (c/2).ToString()[0];
        // balanceSeed += g/2;
        // balanceSeed += h/2;
        // balanceSeed += (d/2).ToString()[0];
        // balanceSeed += shuffleSeed.Substring(10);

        Debug.Log("balanceSeed:" + balanceSeed);
    }
    private static void generate()
    {
        seed = "";
        for (int i = 0; i < balanceSeed.Length; i++,i++)
        {
            string temp;
            Random.InitState((int)(balanceSeed[i]*balanceSeed[i+1]));
            temp = Random.value.ToString();
            seed += temp.Substring(2);
        }
        Debug.Log("seed:" + seed);
    }
    private static void suffleReplace()
    {
        shuffleSeed = "";
        char[] tempSeed = asciiSeed.ToCharArray();

        //asciiSeed的最后一位和倒数第二位作为随机种子
        int suffleCheckNum = (int)(asciiSeed[asciiSeed.Length - 1 - 2] + asciiSeed[asciiSeed.Length - 1]);
        for (int i = 0; i < asciiSeed.Length; i++)
        {
            char temp = tempSeed[i];

            Random.InitState(suffleCheckNum*2);
            int tIndex = Random.Range(i, asciiSeed.Length);
            
            tempSeed[i] = tempSeed[tIndex];
            tempSeed[tIndex] = temp;
        }
        shuffleSeed = new string(tempSeed);
        Debug.Log("shuffleSeed:"+ shuffleSeed);
    }
    private static void asciiReplace()
    {
        asciiSeed = "";

        for (int i = 0; i < normalSeed.Length; i++)
        {
            asciiSeed += ((int)normalSeed[i]).ToString();
        }
        Debug.Log("asciiSeed:"+asciiSeed);
    }
    /// <summary>
    /// 替换V I O 字符
    /// </summary>
    private static void replaceVIO()
    {
        normalSeed = "";
        string temp = "";

        for (int i = 0; i < strOutSeed.Length; i++)
        {
            char t = strOutSeed[i];
            if(t == 'V')
            {
                t = 'U';
            }
            else if(t == 'I')
            {
                t = '1';
            }
            else if(t == 'O')
            {
                t = '0';
            }
            temp += t;
        }
        normalSeed = temp;
        Debug.Log("normalSeed:"+normalSeed);
    }
}
