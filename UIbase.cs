using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ui基类
/// </summary>
public class UIbase {

    //实体
    public GameObject m_go;

    public virtual void DoCreate(string path)
    {
        InsGo(path);
        GoShow(true);
    }
    /// <summary>
    /// 实例化
    /// </summary>
    /// <param name="path">面板名字</param>
    public virtual void InsGo(string path)
    {
        m_go = GameObject.Instantiate(Resources.Load<GameObject>(path));
        m_go.transform.SetParent(UIMgr.Ins.m_uiroot.transform,false);
        m_go.transform.localPosition = Vector3.zero;
        m_go.transform.localScale = Vector3.one;
    }
    public virtual void GoShow(bool path)
    {
        if (m_go)
        {
            m_go.SetActive(path);
        }
    }
    public virtual void Destory()
    {
        GameObject.Destroy(m_go);
;    }
}
