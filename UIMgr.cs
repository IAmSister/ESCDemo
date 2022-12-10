using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr:Singlton<UIMgr>
{
    public GameObject m_uiroot;
    public GameObject m_hudroot;

    public Dictionary<string, UIbase> m_uiDic;

    public void Init(GameObject root,GameObject hud)
    {
        m_uiroot = root;
        m_hudroot = hud;
        m_uiDic = new Dictionary<string, UIbase>();
        m_uiDic.Add("Lobby", new Lobbysys());
        m_uiDic.Add("Battle", new Battlesys());
        m_uiDic.Add("minimap", new MinimapSys());
        m_uiDic.Add("taskPanel", new TaskSys());

        Opend("Lobby");
        Opend("Battle");
        Opend("minimap");
        Opend("taskPanel");

    }
    /// <summary>
    /// 打开面板
    /// </summary>
    /// <param name="paneName"></param>
    private void Opend(string paneName)
    {
        UIbase ui;
        //有对应版面
        if (m_uiDic.TryGetValue(paneName,out ui))
        {
            ui.DoCreate(paneName);
        }
    }
    /// <summary>
    /// 销毁面板
    /// </summary>
    /// <param name="PanelName"></param>
    public void Close(string PanelName)
    {
        UIbase ui;
        //有对应版面
        if (m_uiDic.TryGetValue(PanelName, out ui))
        {
            ui.Destory();
        }
    }
}

