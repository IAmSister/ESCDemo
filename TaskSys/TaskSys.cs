using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TaskSys : UIbase
{
    private Text taskText;
    private Button acceptBtn;
    public override void DoCreate(string path)
    {
        base.DoCreate(path);

    }
    public override void GoShow(bool path)
    {
        base.GoShow(path);
        taskText = m_go.transform.Find("TaskText").GetComponent<Text>();
        acceptBtn = m_go.transform.Find("AcceptButton").GetComponent<Button>();

        taskText.text = GameData.Ins.GetTasksById(1).taskName;
        acceptBtn.onClick.AddListener(() =>
        {
            //给服务器发消息
            Notification notification = new Notification();
            notification.Refresh("AcceptTask", 1);
            MsgCenter.Ins.SendMsg("ServerMsg", notification);
        });
    }
    public override void Destory()
    {
        base.Destory();
    }
}
