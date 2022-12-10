using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : ObjectBase
{
    public monster_info m_info;

    public Monster(MonsterType type, monster_info info)
    {
        info.m_type = type;
        m_info = info;
        m_insId = info.ID;
        m_modelPath = info.m_res;
    }

    public override void OnCreat()
    {
        base.OnCreat();
    }
}

//正常怪物
public class Normal : Monster
{
    public Normal(monster_info info)
        : base(MonsterType.Normal, info)
    {
    }

    public Normal(object_info info) :
        base(MonsterType.Normal, new monster_info(MonsterType.Normal, info))
    {
    }

    public override void CreatObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }

    public override void OnCreat()
    {
        base.OnCreat();

        //状态机 

        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();

        m_pate.m_name.gameObject.SetActive(true);
        m_pate.m_hp.gameObject.SetActive(true);
        m_pate.m_mp.gameObject.SetActive(true);
        m_pate.m_gather.gameObject.SetActive(false);
    }
}

//采集物
public class Gather : Monster
{
    public Gather(monster_info info)
        : base(MonsterType.Gather, info)
    {
        MsgCenter.Ins.AddListen("ServerMsg", ServerNotify);
    }
    public Gather(object_info info) :
        base(MonsterType.Gather, new monster_info(MonsterType.Gather, info))
    {
       
    }

    public override void CreatObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }

    public override void OnCreat()
    {
        base.OnCreat();
        
        StaticCircleCheck check = m_go.AddComponent<StaticCircleCheck>();
        check.m_taget = World.Ins.m_plyer.m_go;
        check.m_call = (isenter) =>
        {
            
            Debug.Log(string.Format("主角触发了我,我是{0}", m_info.m_res));
            Notification notify = new Notification();
            //采集物消息
            notify.Refresh("gather_trigger", m_info.ID);
            //给客户端发消息 碰到采集物了
            MsgCenter.Ins.SendMsg("ClientMsg", notify); 
            //打开显示图片
            Debug.Log("点击按钮触发显示按钮");
        };

      
        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();

        m_pate.m_name.gameObject.SetActive(false);
        m_pate.m_hp.gameObject.SetActive(false);
        m_pate.m_mp.gameObject.SetActive(false);
        m_pate.m_gather.gameObject.SetActive(true);

    }

    private void ServerNotify(Notification obj)
    {

        if (obj.msg.Equals("gather_callback"))
        {
            int insID = (int)obj.date[0];
            //if (insID == m_insID)//逻辑上是需要判断是不是当前的采集物品
            //{
            m_pate.SetData((int)obj.date[1]);
            //}
        }
    }

    public void RefreshGatherCount(int cnt)
    {
        if (m_pate && m_pate.m_gathers.Count > 0)
        {
            for (int i = 0; i < m_pate.m_gathers.Count; i++)
            {
                m_pate.m_gathers[i].gameObject.SetActive(cnt <= i + 1);
            }
        }
    }

}

public class Biaoche : Monster
{
    public Biaoche(monster_info info)
        : base(MonsterType.BiaoChe, info)
    {
    }
    public Biaoche(object_info info) :
       base(MonsterType.BiaoChe, new monster_info(MonsterType.BiaoChe, info))
    {
    }
    public override void CreatObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }

    public override void OnCreat()
    {
        base.OnCreat();
        StaticCircleCheck check = m_go.AddComponent<StaticCircleCheck>();
        //FollowComponent   类  组件     FSM   HFSM  行为树
        check.m_call = (isenter) =>
        {
            Debug.Log("进入检测范围");
            //follow跟随

            //{  ObjectBase    MoveByTranslate  (  transform.find  "player"  // 不断更新.设置 目标点为玩家坐标)   }

        };
    }
}
