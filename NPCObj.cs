using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObj : ObjectBase
{
    public npc_info m_info;
    public NPCObj(npc_info info)
    {
        m_info = info;
        m_insId = info.ID;
        m_modelPath = info.m_res;
    }

    public NPCObj(int plot, object_info info)
    {
        m_info = new npc_info(plot, info);
        m_insId = info.ID;
        m_modelPath = info.m_res;

    }

    public override void CreatObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }

    public override void OnCreat()
    {
        base.OnCreat();

        //��Χ���
        StaticCircleCheck check = m_go.AddComponent<StaticCircleCheck>();
        // ����ȥ��ӶԻ���  ���������  �������
        //�Ի����
        check.m_call = (isenter) =>
        {
            Debug.Log("�����ⷶΧ");
            //չʾtalk �Ի����

            //{  ��ʾ�Ի�����   �����ñ�   }

        };

    }

}
