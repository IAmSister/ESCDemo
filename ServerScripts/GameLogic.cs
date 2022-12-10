using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��������һ������� 
public class ServerPlayerInfo
{
    //λ�� ����ħ�� Ѫ�� ������Ϣ  ������Ϣ
    public Vector3 m_pos;
    public float m_mp;
    public float m_hp;
    public Dictionary<int, ServerSkillInfo> m_skills;
    public List<ServerTaskInfo> m_tasks;
}
/// <summary>
/// ������Ϣ  id,�������� �ȼ�
/// </summary>
public class ServerTaskInfo
{
    public int id;
    public string name;
    public int limitLev;
}
/// <summary>
///  ������Ϣ  ��Χ ���� ���� buff
/// </summary>
public class ServerSkillInfo
{
    public float range;
    public float mp;
    public float check_atk;
    public float buff_group;
    public float lock_target;
    public float skill_cd;
    public float atk_value;
}
//ϵͳ����
public enum SysType
{
    battle, 
    task,

}
//��������Ϣ����
public class ServerActionBase
{
    //����
    public SysType type;
    //���
    public ServerPlayer m_player;

    public ServerActionBase(ServerPlayer m_player)
    {
        this.m_player = m_player;
    }
}
/// <summary>
/// ��������¼  ��ʼ������ ��������ʵ���� �߼�
/// </summary>
public class GameLogic : MonoBehaviour
{
    //ʵ��Ψһid ÿһ���ͻ���
    public Dictionary<int, ServerPlayer> m_allplayer = new Dictionary<int, ServerPlayer>();

    public GameLogic()
    {
        //������ʱ������ͻ��˷������ӵ���Ϣ
        MsgCenter.Ins.AddListen("TOSERVER", ToServer);
        //ʵ��������
        IntiWorld();
    }

    private void ToServer(Notification notify)
    {
        switch (notify.msg)
        {
            case "PlayerMove":
                int insid = (int)notify.date[0];
                Vector3 pos = (Vector3)notify.date[1];
                m_allplayer[insid].RefreshPos(pos);
                break;
            case "BattleSkill":
                break;
            default:
                break;
        }
    }

    private void IntiWorld()
    {
        //�������ɸ��ͻ���
        ServerPlayer player;
        for (int i = 0; i < 10; i++)
        {
            player = new ServerPlayer();
            player.m_insid = i + 1;
            player.RefreshPos(Vector3.zero);
            //�������������ݿ��������ϢTo�ͻ���
            player.SendMsg2Client("TOCLIENT", "RefreshPlayer", player.m_info);
        }
    }


}


//�������ϵ�ÿ�����
public class ServerPlayer
{
    public int m_insid;    //���id
    public ServerPlayerInfo m_info; //�����Ϣ
    //����   ��Ӧ�ķ���
    public Dictionary<SysType, ServerActionBase> m_allAction = new Dictionary<SysType, ServerActionBase>();
    Notification notify = new Notification(); 

    public ServerPlayer()
    {
        IntiPlayerInfo();
    }

    public void IntiPlayerInfo()
    {
        m_info = new ServerPlayerInfo();
        //�����ȡ�����Ϣ
        //������ߵ�ʱ�򷢸����
    }
    /// <summary>
    /// ˢ��λ��
    /// </summary>
    /// <param name="pos"></param>
    public void RefreshPos(Vector3 pos)
    {
        m_info.m_pos = pos;
    }
    /// <summary>
    /// ˢ������ key = 1:hp  2;mp  3:���  4:��ʯ 5:��ȯ  .........
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void RefreshBaseData(int key, float value)
    {
        switch (key)
        {
            case 1:
                m_info.m_hp += value;
                break;
            case 2:
                m_info.m_mp += value;
                break;
            default:
                break;
        }

        SendMsg2Client("TOCLIENT", "RefreshBaseInfo", m_insid, m_info);
    }
    //����������Ϣ���ͻ���
    public void SendMsg2Client(string typecode, string msgcode, params object[] para)
    {
        notify.Refresh(msgcode, para);
        MsgCenter.Ins.SendMsg(typecode, notify);
        notify.Clear();
    }
}
