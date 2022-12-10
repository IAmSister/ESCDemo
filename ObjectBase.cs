using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��������
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
    //����
    public GameObject m_go;
    public Vector3 m_local_pos; //λ��
    public Animator animator;
    public UIPate m_pate;//Ѫ��
    public MonsterType monsterType;
    public int m_insId;
    public string m_modelPath;//ģ��·��

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
    //��������ʱ���ʼ��
    public virtual void OnCreat()
    {
       
    }
    public virtual void SetPos(Vector3 pos)
    {
        m_local_pos = pos;
    }
    //����λ�� �ж�
    public virtual void MoveByTranslate(Vector3 look,Vector3 move)
    {
        m_go.transform.LookAt(look);
        m_go.transform.Translate(move);
    }
    //�Զ��巽�� �������ƶ�
    public virtual void AutoMove(Vector3 look,Vector3 move)
    {
        //��ʾ·����  ֪ͨ��ͼ��ʾ·����
        MoveByTranslate(look, move);
    }
    public virtual void Destory()
    {
        //Ѫ��
        if (m_pate)
        {
            //����Ѫ��
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
