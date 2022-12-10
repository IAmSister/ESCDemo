using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO ;
using Newtonsoft.Json;

/// <summary>
/// 数据管理类   一定要有一个数据库 然后对外开放的接口方法 增删改查
/// </summary>
public class GameData : Singlton<GameData>
{
    #region  模型数据名为键 后当前模型所有技能
    public Dictionary<string, List<SkillXml>> dic = new Dictionary<string, List<SkillXml>>();
  /// <summary>
  /// 读取对应解析表
  /// </summary>
  /// <param name="roleName"></param>
    public void InitByRoleName(string roleName)
    {
        if (File.Exists("Assets/"+roleName+".txt"))
        {
            string str = File.ReadAllText("Assets/" + roleName + ".txt");
            List<SkillXml> list = JsonConvert.DeserializeObject<List<SkillXml>>(str);
           //名字为键
            dic.Add(roleName, list);
        }
    }
    //获取所有技能方法
    public List<SkillXml> GetSkillByRoleName(string roleName)
    {
        if (dic.ContainsKey(roleName))
        {
            return dic[roleName];
        }
        return null;
    }
    #endregion
    #region  任务数据  任务id为键 后
    public Dictionary<int, TaskDate> allTask = new Dictionary<int, TaskDate>();
  
    //这个应该是等我们选择好角色以后 服务器返回给我们的
    public void InitTaskDate()
    {
       //任务
        TaskDate task = new TaskDate();
        task.taskId = 1;
        task.taskName = "任务1";
        allTask.Add(task.taskId, task);
        Debug.Log("任务数量" + allTask.Count);
    }
    public TaskDate GetTasksById(int id)
    {
        Debug.Log("当前任务数量" + allTask.Count);
        if (allTask.ContainsKey(id))
        {
            return allTask[id];
        }
        return null;
    }
    #endregion
}
#region
/// <summary>
/// 技能存储方式
/// </summary>
public class SkillXml
{
    public string name;
    public Dictionary<string, List<string>> skills = new Dictionary<string, List<string>>();
}
/// <summary>
/// 任务存储方式
/// </summary>
public class TaskDate
{
    public int taskId;
    public string taskName;
}
#endregion