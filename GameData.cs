using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO ;
using Newtonsoft.Json;

/// <summary>
/// ���ݹ�����   һ��Ҫ��һ�����ݿ� Ȼ����⿪�ŵĽӿڷ��� ��ɾ�Ĳ�
/// </summary>
public class GameData : Singlton<GameData>
{
    #region  ģ��������Ϊ�� ��ǰģ�����м���
    public Dictionary<string, List<SkillXml>> dic = new Dictionary<string, List<SkillXml>>();
  /// <summary>
  /// ��ȡ��Ӧ������
  /// </summary>
  /// <param name="roleName"></param>
    public void InitByRoleName(string roleName)
    {
        if (File.Exists("Assets/"+roleName+".txt"))
        {
            string str = File.ReadAllText("Assets/" + roleName + ".txt");
            List<SkillXml> list = JsonConvert.DeserializeObject<List<SkillXml>>(str);
           //����Ϊ��
            dic.Add(roleName, list);
        }
    }
    //��ȡ���м��ܷ���
    public List<SkillXml> GetSkillByRoleName(string roleName)
    {
        if (dic.ContainsKey(roleName))
        {
            return dic[roleName];
        }
        return null;
    }
    #endregion
    #region  ��������  ����idΪ�� ��
    public Dictionary<int, TaskDate> allTask = new Dictionary<int, TaskDate>();
  
    //���Ӧ���ǵ�����ѡ��ý�ɫ�Ժ� ���������ظ����ǵ�
    public void InitTaskDate()
    {
       //����
        TaskDate task = new TaskDate();
        task.taskId = 1;
        task.taskName = "����1";
        allTask.Add(task.taskId, task);
        Debug.Log("��������" + allTask.Count);
    }
    public TaskDate GetTasksById(int id)
    {
        Debug.Log("��ǰ��������" + allTask.Count);
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
/// ���ܴ洢��ʽ
/// </summary>
public class SkillXml
{
    public string name;
    public Dictionary<string, List<string>> skills = new Dictionary<string, List<string>>();
}
/// <summary>
/// ����洢��ʽ
/// </summary>
public class TaskDate
{
    public int taskId;
    public string taskName;
}
#endregion