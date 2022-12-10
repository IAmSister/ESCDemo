using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgCenter : Singlton<MsgCenter>
{
    Dictionary<string, Action<Notification>> m_MsgDicts = new Dictionary<string, Action<Notification>>();
    public void AddListen(string id,Action<Notification> action)
    {
        if (m_MsgDicts.ContainsKey(id))
        {
            m_MsgDicts[id] += action;       
        }
        else
        {
            m_MsgDicts.Add(id, action);
        }
    }
    public void RemoveListen(string id, Action<Notification> action)
    {
        if (m_MsgDicts.ContainsKey(id))
        {
            m_MsgDicts[id] -= action;
            if (m_MsgDicts[id]==null)
            {
                m_MsgDicts.Remove(id);
            }
        }
      
    }
    public void SendMsg(string id,Notification notification)
    {
        if (m_MsgDicts.ContainsKey(id))
        {
            m_MsgDicts[id](notification);
        }
    }


}
