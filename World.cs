using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������  ��� ��� ������ڵ�  ����ʵ����������
/// </summary>
public class World : Singlton<World>
{

    //ʵ����id ����
    public Dictionary<int, ObjectBase> dic = new Dictionary<int, ObjectBase>();
    //��ǰ���
    public HostPlayer m_plyer;
    //npc���ڵ�
    private GameObject npcroot;
    public Camera m_mian;//���

    public float xlength;
    public float ylength;

    public void Init()
    {
        //���õذ��С
        GameObject plan = GameObject.Find("Plane");
        Vector3 leng = plan.GetComponent<MeshFilter>().mesh.bounds.size;
        xlength = leng.x * plan.transform.lossyScale.x;
        ylength = leng.z + plan.transform.lossyScale.z;
        Debug.Log("��ǰ��ͼMeshFiler");

        m_mian = GameObject.Find("Main Camera").GetComponent<Camera>();
        npcroot = GameObject.Find("NPC_Root");  //?????????
        //��ʼ�����ڵ�
        UIMgr.Ins.Init(GameObject.Find("UiRoot"), GameObject.Find("HUD"));

        //�����Ϣ  ���и�ֵ
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

        //���һ�������
        m_plyer = new HostPlayer(info);
        m_plyer.CreatObj(MonsterType.Null);
        JoyStickMgr.Ins.SetJoyArg(m_mian, m_plyer);
        JoyStickMgr.Ins.JoyActive = true;
        //��ȡ��Ϣ��ʵ������
        CreateIns();

        //�ƶ�����
        MsgCenter.Ins.AddListen("AutoMove", (notify) =>
        {
            this.AutoMoveByInsId((int)notify.date[0], (Vector3)notify.date[1]);
        });
        //buf ����
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
        //�Ƴ�
        MsgCenter.Ins.AddListen("ReMoveBuff", (notify) =>
        {

        });
    }
    /// <summary>
    /// ���ݹ�������ɹ���
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
   /// �������Ͳ�ͬ���ز�ͬ�Ĺ���
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
            Debug.Log("����ʧ��!!!!");
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
                    //��ʵ���ƶ�
                    tmp.Current.Value.AutoMove(pos, pos);
                }
            }
        }

    }

}
