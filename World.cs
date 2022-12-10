using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景管理  玩家 相机 怪物跟节点  所有实例化出来的
/// </summary>
public class World : Singlton<World>
{

    //实例化id 类型
    public Dictionary<int, ObjectBase> dic = new Dictionary<int, ObjectBase>();
    //当前玩家
    public HostPlayer m_plyer;
    //npc根节点
    private GameObject npcroot;
    public Camera m_mian;//相机

    public float xlength;
    public float ylength;

    public void Init()
    {
        //设置地板大小
        GameObject plan = GameObject.Find("Plane");
        Vector3 leng = plan.GetComponent<MeshFilter>().mesh.bounds.size;
        xlength = leng.x * plan.transform.lossyScale.x;
        ylength = leng.z + plan.transform.lossyScale.z;
        Debug.Log("当前地图MeshFiler");

        m_mian = GameObject.Find("Main Camera").GetComponent<Camera>();
        npcroot = GameObject.Find("NPC_Root");  //?????????
        //初始化根节点
        UIMgr.Ins.Init(GameObject.Find("UiRoot"), GameObject.Find("HUD"));

        //玩家信息  进行赋值
        player_info info = new player_info();
        info.ID = 0;
        info.m_name = "tony";
        info.m_level = 9;
        info.m_pos = Vector3.zero;
        info.m_res = "Teddy";
        info.m_HP = 2000;
        info.m_MP = 1000;
        info.m_hpMax = 2000;
        info.m_mpMax = 2000;

        //添加一个玩家类
        m_plyer = new HostPlayer(info);
        m_plyer.CreatObj(MonsterType.Null);
        JoyStickMgr.Ins.SetJoyArg(m_mian, m_plyer);
        JoyStickMgr.Ins.JoyActive = true;
        //获取信息并实例怪物
        CreateIns();

        //移动监听
        MsgCenter.Ins.AddListen("AutoMove", (notify) =>
        {
            this.AutoMoveByInsId((int)notify.date[0], (Vector3)notify.date[1]);
        });
        //buf 监听
        MsgCenter.Ins.AddListen("AddBuff", (notify) =>
        {
            int insid = (int)notify.date[0];
            int buffid = (int)notify.date[1];
            ObjectBase p = dic[insid];
            if (p is PlayerObj)
            {
                BuffSystem.Instance.AddBuff(p as PlayerObj, buffid);
            }
        });
        //移除
        MsgCenter.Ins.AddListen("ReMoveBuff", (notify) =>
        {

        });
    }
    /// <summary>
    /// 根据怪物表生成怪物
    /// </summary>
    private void CreateIns()
    {
        JsonData data = MonsterCfg.Instance.GetJsonDate();
        object_info info;

        for (int i = 0; i < data.datas.Count; i++)
        {
            info = new object_info();
            info.ID = dic.Count + 1;
            info.m_name = string.Format("{0}({1})", data.datas[i].name, info.ID);
            info.m_res = data.datas[i].name;
            info.m_pos = new Vector3(data.datas[i].x, data.datas[i].y, data.datas[i].z);
            info.m_type = data.datas[i].type;
            CreateObj(info);
        }
    }
    ObjectBase monster = null;
   /// <summary>
   /// 根据类型不同加载不同的怪物
   /// </summary>
   /// <param name="info"></param>
    private void CreateObj(object_info info)
    {
      //  Debug.Log("shwengchengguaiqu");
        monster = null;
        if (info != null)
        {
            if (info.m_type == MonsterType.Normal)
            {
                monster = new Normal(info);
            }
            else if (info.m_type == MonsterType.Gather)
            {
                monster = new Gather(info);
            }
            else if (info.m_type == MonsterType.NPC)
            {
                 monster = new NPCObj(1, info);
            }
        }
        if (monster != null)
        {
            monster.CreatObj(info.m_type);
            monster.m_go.transform.SetParent(npcroot.transform, false);
            dic.Add(info.ID, monster);
        }
        else
        {
            Debug.Log("生成失败!!!!");
        }
    }


    public void AutoMoveByInsId(int target, Vector3 pos)
    {
        using (var tmp = dic.GetEnumerator())
        {
            while (tmp.MoveNext())
            {
                if (target == tmp.Current.Key)
                {
                    //让实例移动
                    tmp.Current.Value.AutoMove(pos, pos);
                }
            }
        }

    }

}
