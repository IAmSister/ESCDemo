using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battlesys : UIbase
{
    private Button m_gatherBtn; 
    private Slider m_gatherSlider;
    private int m_gatherInsid;

    /// <summary>
    /// 监听
    /// </summary>
    /// <param name="path"></param>
    public override void DoCreate(string path)
    {
        base.DoCreate(path);
        //监听客户端  刷新btN
        MsgCenter.Ins.AddListen("ClientMsg", ReFreshBtn);
       // MsgCenter.Ins.AddListen("ClientMsg", GoShow);
        MsgCenter.Ins.AddListen("ServerMsg", ServerNotify);
    }
    
   /// <summary>
   /// 销毁
   /// </summary>
    public override void Destory()
    {
        base.Destory();
        MsgCenter.Ins.RemoveListen("ClientMsg", ReFreshBtn);
    }
    #region  两个监听方法  消息
    private void ServerNotify(Notification obj)
    {
        if (obj.msg.Equals("gather_callback"))
        {
            m_gatherSlider.gameObject.SetActive(true); //显示slider
        }
    }
    public override void GoShow(bool active)
    {
        base.GoShow(active);
        
        m_gatherBtn = m_go.transform.Find("gather_btn").GetComponent<Button>();
        m_gatherBtn.onClick.AddListener(() => {
            //交互服务器
            Notification notify = new Notification();
            notify.Refresh("gather", 1);
            MsgCenter.Ins.SendMsg("ServerMsg", notify);
        });
        m_gatherSlider = m_go.transform.Find("gather_slider").GetComponent<Slider>();
        m_gatherBtn.gameObject.SetActive(false);
        m_gatherSlider.gameObject.SetActive(false);
    }
    private void ReFreshBtn(Notification obj)
    {
        Debug.Log("刷新btn 显示Slider");
        if (obj.msg.Equals("gather_trigger"))
        {
            m_gatherInsid = (int)obj.date[0];
            m_gatherBtn.gameObject.SetActive(true);
        }
        
    }
    #endregion  


}
public class MinimapSys : UIbase
{
    public MapController m_map;

    public override void DoCreate(string path)
    {
        base.DoCreate(path);
        Transform map = m_go.transform.Find("minimap/map");
        m_map = map.gameObject.AddComponent<MapController>();

    }

    public override void GoShow(bool active)
    {
        base.GoShow(active);
    }

    public override void Destory()
    {
        GameObject.Destroy(m_map);
        base.Destory();
    }
}
