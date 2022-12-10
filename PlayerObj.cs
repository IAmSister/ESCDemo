using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自己控制的player
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

        //播放动画的时候给服务器发消息
        MsgCenter.Ins.AddListen("atkActionPlay", (notify) => {
            if (notify.msg.Equals("ByServer"))
            {
                //服务器返回id 玩家找并播放技能
                int skillid = (int)notify.date[0];
                player.SetData(skillid.ToString());
                player.Play();
            }

        });
        //初始化玩家
        //Player.Init("Teddy");
    }
    
    Notification notify = new Notification();
    /// <summary>
    /// 遥感移动
    /// </summary>
    /// <param name="h"></param>
    /// <param name="v"></param>
    public void JoystickHandlerMoving(float h,float v)
    {
        //玩家移动 
        if (Mathf.Abs(h) > 0.05f || (Mathf.Abs(v) > 0.05f))
        {
            MoveByTranslate(new Vector3(m_go.transform.position.x + h, m_go.transform.position.y, m_go.transform.position.z + v), Vector3.forward * Time.deltaTime * 1);
            notify.Refresh("Player", m_go.transform.position);
            MsgCenter.Ins.SendMsg("MovePos", notify);
        }
    }

    //技能释放 方法 派发事件  通知服务器
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
                // 遍历 List赋值播放
                break;

        }
    }
}


//一个有角色数据的怪物或者NPC
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

