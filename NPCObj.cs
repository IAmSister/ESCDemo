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

        //范围检测
        StaticCircleCheck check = m_go.AddComponent<StaticCircleCheck>();
        // 可以去添加对话组  距离检测组件  距离出发
        //对话组件
        check.m_call = (isenter) =>
        {
            Debug.Log("进入检测范围");
            //展示talk 对话组件

            //{  显示对话内容   走配置表   }

        };

    }

}
