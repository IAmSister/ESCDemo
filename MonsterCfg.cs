using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class JsonData
{
    public List<MonsterData> datas = new List<MonsterData>();
}
[Serializable]
public class MonsterData
{
    public string name;
    public float x;
    public float y;
    public float z;
    public MonsterType type;
    public MonsterData(string name,float x,float y,float z, MonsterType type)
    {
        this.name = name;
        this.x = x;
        this.y = y;
        this.z = z;
        this.type = type;
    }
}
public class MonsterCfg
{
    static MonsterCfg _instance;
    public static MonsterCfg Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = new MonsterCfg();
                _instance.Init();
            }
            return _instance;
        }
    }
    public JsonData data;
    string path;
    void Init()
    {
       //∂¡»°π÷ŒÔ±Ì
        path = Application.streamingAssetsPath + @"/monster.json";
        string json = File.ReadAllText(path);
        data = JsonUtility.FromJson<JsonData>(json);
    }
    public JsonData GetJsonDate()
    {
        return data;
    }
}
