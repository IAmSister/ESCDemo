using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//物体种类
public enum MonsterType
{
    Null=0,
    Normal,
    Gather,
    BiaoChe,
    NPC
}
public abstract class ObjectBase
{
    //物体
    public GameObject m_go;
    public Vector3 m_local_pos; //位置
    public Animator animator;
    public UIPate m_pate;//血条
    public MonsterType monsterType;
    public int m_insId;
    public string m_modelPath;//模型路径

    public ObjectBase() { }

    public virtual void CreatObj(MonsterType monsterType)
    {
        this.monsterType = monsterType;
        if (!string.IsNullOrEmpty(m_modelPath)&& m_insId>=0)
        {
            m_go = GameObject.Instantiate(Resources.Load<GameObject>(m_modelPath));
            m_go.name = m_insId.ToString();
            m_go.transform.position = m_local_pos;
            if (m_go)
            {
                OnCreat();
            }
        }
    }
    //创建物体时候初始化
    public virtual void OnCreat()
    {
       
    }
    public virtual void SetPos(Vector3 pos)
    {
        m_local_pos = pos;
    }
    //看向位置 行动
    public virtual void MoveByTranslate(Vector3 look,Vector3 move)
    {
        m_go.transform.LookAt(look);
        m_go.transform.Translate(move);
    }
    //自定义方法 依旧是移动
    public virtual void AutoMove(Vector3 look,Vector3 move)
    {
        //显示路径点  通知地图显示路径点
        MoveByTranslate(look, move);
    }
    public virtual void Destory()
    {
        //血条
        if (m_pate)
        {
            //销毁血条
            GameObject.Destroy(m_pate);
        }
        GameObject.Destroy(m_go);
        
        m_local_pos = Vector3.zero;
        m_insId = -1;
        animator = null;
    }
}

public class PlayerObj : ObjectBase
{
    public player_info m_info;

    public PlayerObj(player_info info)
    {
        m_info = info;
    }


    public override void SetPos(Vector3 pos)
    {
        base.SetPos(pos);
    }

    public void SetPos(Vector3 pos, float speed)
    {
       
    }

    public override void OnCreat()
    {
        base.OnCreat();
        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();
        m_pate.m_gather.SetActive(false);
        m_pate.SetData(m_info.m_name, m_info.m_HP / m_info.m_hpMax, m_info.m_MP / m_info.m_mpMax);
    }

    public void AddBuff(string path)
    {
    
    }
}
