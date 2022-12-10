using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#region ����ί�е��¼�����
public class GameEventManager : Singlton<GameEventManager>
{
    Dictionary<string, Action<Notification>> dic = new Dictionary<string, Action<Notification>>();
    public void AddListen(string id, Action<Notification> action)
    {
        if (dic.ContainsKey(id))
        {
            dic[id] += action;
        }
        else
        {
            dic.Add(id, action);
        }
    }
    public void RemoveListen(string id, Action<Notification> action)
    {
        if (dic.ContainsKey(id))
        {
            dic[id] -= action;
            if (dic[id]==null)
            {
                dic.Remove(id);
            }
        } 
    }
    public void BroadCast(string id, Notification notification)
    {
        if (dic.ContainsKey(id))
        {
            dic[id](notification);
        }
    }
}
/// <summary>
/// ��Ϣ����ö��
/// </summary>
public class Notification
{
    //��Ϣ����
    public string msg;
    //��Ϣ
    public object[] date;
    
    public void Refresh(string msg,params object[] data)
    {
        this.msg = msg;
        this.date = data;
    }

    public void Clear()
    {
        msg = string.Empty;
        date = null;
    }
}
#endregion