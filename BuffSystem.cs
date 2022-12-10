using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    //buf ¼¼ÄÜ
    private Dictionary<int, string> BuffIdToPath = new Dictionary<int, string>
    {
        { 1,"zhuoshao"},
        { 2,"bingdong"},
        { 3,"liuxue"}
    };

    static private BuffSystem _instance;
    static public BuffSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BuffSystem();
            }
            return _instance;
        }
    }

    
    public List<int> buffs = new List<int>();

    /// <summary>
    /// Ìí¼Óbuf
    /// </summary>
    /// <param name="p"></param>
    /// <param name="id"></param>
    public void AddBuff(PlayerObj p, int id)
    {
        if (!buffs.Contains(id))
        {
            buffs.Add(id);
        }

        p.AddBuff(BuffIdToPath[id]);
    }
}

