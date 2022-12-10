using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���ݿ�
public static class LocalProps
{
    public static Dictionary<long, SPlayer> players = new Dictionary<long, SPlayer>();

}
/// <summary>
/// ��Ҽ���
/// </summary>
public class SkillProp
{
    public float range;
    //��Χ����������ʱ���������ڵ�
}
/// <summary>
/// �������
/// </summary>
public enum ComponentType : byte
{
    nil = 0,
    task,
    battle,

}
/// <summary>
/// ���������Ϣ
/// </summary>
public class SComponent
{
    public Func<long, SPlayer> GetPlayerById;

    Notification m_notify;
    public virtual void S2CMsg(string cmd, object value)
    {
        if (m_notify == null)
        {
            m_notify = new Notification();
        }
        m_notify.Refresh("ByServer", value);
        MsgCenter.Ins.SendMsg(cmd, m_notify);
    }

    public virtual void Init()
    { }
}
/// <summary>
/// �������
/// </summary>
public class TaskComponent : SComponent
{
    public List<int> Tasks;
    //

    public override void Init()
    {



        //�����������ɼ�����  ���������߼�
        MsgCenter.Ins.AddListen("GatherAction", (notify) =>
        {
            // �ж�һ�� �ɼ����Ƿ�������   �޸��������  ֪ͨ  Client��Viewȥ���� 
            Debug.Log("����ɼ�����");
        });
    }


}
/// <summary>
/// ս�����
/// </summary>
public class BattleComponent : SComponent
{
    public override void Init()
    {
        MsgCenter.Ins.AddListen("ByClent_Battle", (notify) =>
        {
            if (notify.msg.Equals("atkOther"))
            {
                int atkId = (int)notify.date[0];
                int targetID = (int)notify.date[1];
                int skillID = (int)notify.date[2];

                AtkPlayer(atkId, targetID, skillID);
            }

        });
    }

    private void AtkPlayer(long atk, long target, int skillid)
    {
        SPlayer p1 = GetPlayerById(atk);
        SPlayer p2 = GetPlayerById(target);
        //==ʵ���߼�Ϊ�·��߼�
        //float tmp = p1.skills[skillid].range;
        //�жϹ�������  �޸��˺�����   ����������  �����ټ��ϸ���ϵ��
        //if (tmp >= Vector3.Distance(p1.m_pos,p2.m_pos))
        //{
        //    p2.PropOperation(1, -10);
        //    S2CMsg("atkover", true);
        //    S2CMsg("shouji", p2.m_insid);
        //}
        //===��ʱ��Ϣ
        S2CMsg("atkActionPlay", skillid);
        //S2CMsg("shouji", p2.m_insid);
    }
}


//���������
public class SPlayer
{
    public long m_insid;

    public Vector3 m_pos;
    public float Hp;
    public float Mp;
    public float Atk;
    //
    public List<int> buffs;
    public List<SkillProp> skills;

    public Dictionary<ComponentType, SComponent> components;


    public void InitPlayer()
    {
        buffs = new List<int>();
        skills = new List<SkillProp>();
        components = new Dictionary<ComponentType, SComponent>();
    }

    public void PropOperation(int type, float value)
    {
        switch (type)
        {
            case 1:
                Hp += value;
                break;
            case 2:
                Mp += value;
                break;
        }
        Notification m_notify = new Notification();
        m_notify.Refresh("ByServer", type, value);
        MsgCenter.Ins.SendMsg("propchange", m_notify);
    }
}
/// <summary>
/// ��������ʼ��
/// </summary>
public class ServerInit : MonoBehaviour
{
    //���λ��
    public Vector3 m_playerPos;
    //������ҵ�λ��
    public Dictionary<int, Vector3> m_otherPosDic;

    private void Awake()
    {
        //�����Լ��ƶ� ��������ƶ�
        MsgCenter.Ins.AddListen("MovePos", (notify) =>
        {
            if (notify.msg.Equals("Player"))
            {
                m_playerPos = (Vector3)notify.date[0];
            }
            else if (notify.msg.Equals("Other"))
            {
                if (m_otherPosDic == null)
                {
                    m_otherPosDic = new Dictionary<int, Vector3>();
                }
                int insid = (int)notify.date[0];
                Vector3 pos = (Vector3)notify.date[1];
                if (!m_otherPosDic.ContainsKey(insid))
                {
                    m_otherPosDic.Add(insid, pos);
                }
                else
                {
                    m_otherPosDic[insid] = pos;
                }
            }

        });
        //�����ɼ�����
        MsgCenter.Ins.AddListen("ServerMsg", (notify) =>
        {
            //�ɼ�
            if (notify.msg.Equals("gather"))
            {
                Debug.Log($"����ɼ���ť  Insid��{(int)notify.date[0]}");
                int insid = (int)notify.date[0];
                //�ɼ��ظ���Ϣ���ɼ�ID���ɼ�����
                notify.Refresh("gather_callback", insid, 2);
               
                MsgCenter.Ins.SendMsg("ServerMsg", notify);

                //�㲥�ɼ� �ͻ��˾���ɼ�
                MsgCenter.Ins.SendMsg("GatherAction", notify);

            }
            //���ղɼ����� 
            if (notify.msg.Equals("AcceptTask"))
            {

                int taskid = (int)notify.date[0];
                //�ȶԷ���������· ��������б�
                foreach (var item in LocalProps.players)
                {
                    if (item.Key == 1)
                    {
                        //����������
                        item.Value.components.Add(ComponentType.task, new TaskComponent());
                        item.Value.components[ComponentType.task].Init();
                    }
                }

            }
        });

        //�������ñ���� S��

        //�������ñ� ��ʼ����ɫ��Ϣ
        SPlayer splayer = new SPlayer();
        splayer.InitPlayer();
        splayer.m_insid = 1;
        splayer.Hp = 100;
        //���ս�����
        splayer.components.Add(ComponentType.battle, new BattleComponent());
        //���������� һ��ʼû�� �����������ʱ�� ��ȥ���
        //splayer.components.Add(ComponentType.task, new TaskComponent());

        LocalProps.players.Add(splayer.m_insid, splayer);
        // ��ʼ�� �����н�ɫ�������н�ɫ������ע��ص�
        if (LocalProps.players == null) return;
        foreach (var item in LocalProps.players)
        {
            foreach (var item1 in item.Value.components)
            {
                item1.Value.GetPlayerById = GetPlayer;
                item1.Value.Init();
            }
        }
    }
    private SPlayer GetPlayer(long id)
    {
        using (var tmp = LocalProps.players.GetEnumerator())
        {
            while (tmp.MoveNext())
            {
                if (tmp.Current.Key == id)
                {
                    return tmp.Current.Value;
                }
            }
        }
        return null;
    }


}
