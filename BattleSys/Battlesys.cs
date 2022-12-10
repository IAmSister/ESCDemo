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
    /// ����
    /// </summary>
    /// <param name="path"></param>
    public override void DoCreate(string path)
    {
        base.DoCreate(path);
        //�����ͻ���  ˢ��btN
        MsgCenter.Ins.AddListen("ClientMsg", ReFreshBtn);
       // MsgCenter.Ins.AddListen("ClientMsg", GoShow);
        MsgCenter.Ins.AddListen("ServerMsg", ServerNotify);
    }
    
   /// <summary>
   /// ����
   /// </summary>
    public override void Destory()
    {
        base.Destory();
        MsgCenter.Ins.RemoveListen("ClientMsg", ReFreshBtn);
    }
    #region  ������������  ��Ϣ
    private void ServerNotify(Notification obj)
    {
        if (obj.msg.Equals("gather_callback"))
        {
            m_gatherSlider.gameObject.SetActive(true); //��ʾslider
        }
    }
    public override void GoShow(bool active)
    {
        base.GoShow(active);
        
        m_gatherBtn = m_go.transform.Find("gather_btn").GetComponent<Button>();
        m_gatherBtn.onClick.AddListener(() => {
            //����������
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
        Debug.Log("ˢ��btn ��ʾSlider");
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
