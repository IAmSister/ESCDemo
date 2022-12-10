using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Լ����Ƶ�player
public class HostPlayer : PlayerObj
{
    Player player;
    public HostPlayer(player_info info) : base(info)
    {
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

        player = m_go.AddComponent<Player>();
        player.InitData();

        //���Ŷ�����ʱ�������������Ϣ
        MsgCenter.Ins.AddListen("atkActionPlay", (notify) => {
            if (notify.msg.Equals("ByServer"))
            {
                //����������id ����Ҳ����ż���
                int skillid = (int)notify.date[0];
                player.SetData(skillid.ToString());
                player.Play();
            }

        });
        //��ʼ�����
        //Player.Init("Teddy");
    }
    
    Notification notify = new Notification();
    /// <summary>
    /// ң���ƶ�
    /// </summary>
    /// <param name="h"></param>
    /// <param name="v"></param>
    public void JoystickHandlerMoving(float h,float v)
    {
        //����ƶ� 
        if (Mathf.Abs(h) > 0.05f || (Mathf.Abs(v) > 0.05f))
        {
            MoveByTranslate(new Vector3(m_go.transform.position.x + h, m_go.transform.position.y, m_go.transform.position.z + v), Vector3.forward * Time.deltaTime * 1);
            notify.Refresh("Player", m_go.transform.position);
            MsgCenter.Ins.SendMsg("MovePos", notify);
        }
    }

    //�����ͷ� ���� �ɷ��¼�  ֪ͨ������
    public void JoyButtonHandler(string btnName)
    {
        List<Skill_Base> componentList;
        switch (btnName)
        {
            case "attack":
                //player.SetData("1");
                //player.play();
                Notification m_notify = new Notification();
                m_notify.Refresh("atkOther", 1, 2, 1);
                MsgCenter.Ins.SendMsg("ByClent_Battle", m_notify);
                // ���� List��ֵ����
                break;

        }
    }
}


//һ���н�ɫ���ݵĹ������NPC
public class OtherPlayer : PlayerObj
{
    public OtherPlayer(player_info info) : base(info)
    {
        m_insId = info.ID;
        m_modelPath = info.m_res;
    }
    public override void CreatObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }
}

