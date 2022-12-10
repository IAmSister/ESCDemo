using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 信息基类  共性属性
/// </summary>
public class object_info
{
    public int ID;        //1 or 2
    public string m_name;
    public Vector3 m_pos;
    public string m_res;
    public MonsterType m_type;
}
/// <summary>
/// 玩家数据
/// </summary>
public class player_info : object_info
{
    public int m_level;
    public float m_HP;
    public float m_hpMax;
    public float m_MP;
    public float m_mpMax;
    //技能列表
    public List<SkillXml> skillList;

}
/// <summary>
/// npc数据
/// </summary>
public class npc_info : object_info
{
    public int m_plotId = 0; //0是不响应

    //传入响应  npc信息
    public npc_info(int plot, object_info info)
    {
        ID = info.ID;
        m_name = info.m_name;
        m_pos = info.m_pos;
        m_res = info.m_res;
        m_type = MonsterType.NPC;
    }
}
/// <summary>
/// 怪物信息
/// </summary>
public class monster_info : object_info
{
    //类型 和信息
    public monster_info(MonsterType type, object_info info)
    {
        ID = info.ID;
        m_name = info.m_name;
        m_pos = info.m_pos;
        m_res = info.m_res;
        m_type = type;
    }

}