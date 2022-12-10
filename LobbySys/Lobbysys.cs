using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 玩家ui场景
/// </summary>
public class Lobbysys : UIbase
{
    private Image m_head;
    
    private List<Image> m_buffs;

    //初始化的时候
    public override void DoCreate(string path)
    {
        m_buffs = new List<Image>();
        base.DoCreate(path);
    }
    //是否显示
    public override void GoShow(bool active)
    {
        base.GoShow(active);
        m_head = m_go.transform.Find("head").GetComponent<Image>();
        Transform buffgo = m_go.transform.Find("bufflayout").transform;
        for (int i = 0; i < buffgo.childCount; i++)
        {
            m_buffs.Add(buffgo.GetChild(i).GetComponent<Image>());
        }
    }
    public override void Destory()
    {
        base.Destory();
    }
}
