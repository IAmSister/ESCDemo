using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//数据库
public static class LocalProps
{
    public static Dictionary<long, SPlayer> players = new Dictionary<long, SPlayer>();

}
/// <summary>
/// 玩家技能
/// </summary>
public class SkillProp
{
    public float range;
    //范围、攻击力、时长、触发节点
}
/// <summary>
/// 组件类型
/// </summary>
public enum ComponentType : byte
{
    nil = 0,
    task,
    battle,

}
/// <summary>
/// 组件发送消息
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
/// 任务组件
/// </summary>
public class TaskComponent : SComponent
{
    public List<int> Tasks;
    //

    public override void Init()
    {



        //监听服务器采集操作  处理任务逻辑
        MsgCenter.Ins.AddListen("GatherAction", (notify) =>
        {
            // 判断一下 采集物是否是任务   修改任务进度  通知  Client的View去更新 
            Debug.Log("处理采集进度");
        });
    }


}
/// <summary>
/// 战斗组件
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
        //==实际逻辑为下方逻辑
        //float tmp = p1.skills[skillid].range;
        //判断攻击距离  修改伤害数据   攻击减防御  可能再加上各种系数
        //if (tmp >= Vector3.Distance(p1.m_pos,p2.m_pos))
        //{
        //    p2.PropOperation(1, -10);
        //    S2CMsg("atkover", true);
        //    S2CMsg("shouji", p2.m_insid);
        //}
        //===临时消息
        S2CMsg("atkActionPlay", skillid);
        //S2CMsg("shouji", p2.m_insid);
    }
}


//服务器玩家
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
/// 服务器初始化
/// </summary>
public class ServerInit : MonoBehaviour
{
    //玩家位置
    public Vector3 m_playerPos;
    //其他玩家的位置
    public Dictionary<int, Vector3> m_otherPosDic;

    private void Awake()
    {
        //监听自己移动 其他玩家移动
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
        //监听采集任务
        MsgCenter.Ins.AddListen("ServerMsg", (notify) =>
        {
            //采集
            if (notify.msg.Equals("gather"))
            {
                Debug.Log($"点击采集按钮  Insid：{(int)notify.date[0]}");
                int insid = (int)notify.date[0];
                //采集回复消息，采集ID，采集数量
                notify.Refresh("gather_callback", insid, 2);
               
                MsgCenter.Ins.SendMsg("ServerMsg", notify);

                //广播采集 客户端尽享采集
                MsgCenter.Ins.SendMsg("GatherAction", notify);

            }
            //接收采集任务 
            if (notify.msg.Equals("AcceptTask"))
            {

                int taskid = (int)notify.date[0];
                //比对服务器数据路 玩家任务列表
                foreach (var item in LocalProps.players)
                {
                    if (item.Key == 1)
                    {
                        //添加任务组件
                        item.Value.components.Add(ComponentType.task, new TaskComponent());
                        item.Value.components[ComponentType.task].Init();
                    }
                }

            }
        });

        //任务配置表解析 S端

        //解析配置表 初始化角色信息
        SPlayer splayer = new SPlayer();
        splayer.InitPlayer();
        splayer.m_insid = 1;
        splayer.Hp = 100;
        //添加战斗组件
        splayer.components.Add(ComponentType.battle, new BattleComponent());
        //添加任务组件 一开始没有 当接受任务的时候 再去添加
        //splayer.components.Add(ComponentType.task, new TaskComponent());

        LocalProps.players.Add(splayer.m_insid, splayer);
        // 初始化 完所有角色，给所有角色相关组件注册回调
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
