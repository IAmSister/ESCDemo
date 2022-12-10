using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//服务器玩家基本数据 
public class ServerPlayerInfo
{
    //位置 法抗魔抗 血量 技能信息  任务信息
    public Vector3 m_pos;
    public float m_mp;
    public float m_hp;
    public Dictionary<int, ServerSkillInfo> m_skills;
    public List<ServerTaskInfo> m_tasks;
}
/// <summary>
/// 任务信息  id,任务名字 等级
/// </summary>
public class ServerTaskInfo
{
    public int id;
    public string name;
    public int limitLev;
}
/// <summary>
///  技能信息  范围 法抗 攻击 buff
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
//系统类型
public enum SysType
{
    battle, 
    task,

}
//服务器消息基类
public class ServerActionBase
{
    //类型
    public SysType type;
    //玩家
    public ServerPlayer m_player;

    public ServerActionBase(ServerPlayer m_player)
    {
        this.m_player = m_player;
    }
}
/// <summary>
/// 服务器登录  初始化世界 缓存所有实例化 逻辑
/// </summary>
public class GameLogic : MonoBehaviour
{
    //实例唯一id 每一个客户端
    public Dictionary<int, ServerPlayer> m_allplayer = new Dictionary<int, ServerPlayer>();

    public GameLogic()
    {
        //启动的时候监听客户端发来链接的消息
        MsgCenter.Ins.AddListen("TOSERVER", ToServer);
        //实例化场景
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
        //创建若干个客户端
        ServerPlayer player;
        for (int i = 0; i < 10; i++)
        {
            player = new ServerPlayer();
            player.m_insid = i + 1;
            player.RefreshPos(Vector3.zero);
            //服务器发送数据库存的玩家信息To客户端
            player.SendMsg2Client("TOCLIENT", "RefreshPlayer", player.m_info);
        }
    }


}


//服务器上的每个玩家
public class ServerPlayer
{
    public int m_insid;    //玩家id
    public ServerPlayerInfo m_info; //玩家信息
    //类型   对应的方法
    public Dictionary<SysType, ServerActionBase> m_allAction = new Dictionary<SysType, ServerActionBase>();
    Notification notify = new Notification(); 

    public ServerPlayer()
    {
        IntiPlayerInfo();
    }

    public void IntiPlayerInfo()
    {
        m_info = new ServerPlayerInfo();
        //读表获取玩家信息
        //玩家上线的时候发给玩家
    }
    /// <summary>
    /// 刷新位置
    /// </summary>
    /// <param name="pos"></param>
    public void RefreshPos(Vector3 pos)
    {
        m_info.m_pos = pos;
    }
    /// <summary>
    /// 刷新数据 key = 1:hp  2;mp  3:金币  4:钻石 5:点券  .........
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
    //服务器发消息给客户端
    public void SendMsg2Client(string typecode, string msgcode, params object[] para)
    {
        notify.Refresh(msgcode, para);
        MsgCenter.Ins.SendMsg(typecode, notify);
        notify.Clear();
    }
}
