using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public GameObject[] DontDestory;
    public List<ETCButton> attack;
    public ETCJoystick Joy;
    public GameObject uiroot;

    void Start()
    {

        //场景跳转
        for (int i = 0; i < DontDestory.Length; i++)
        {
            GameObject.DontDestroyOnLoad(DontDestory[i]);
        }
        //场景跳转时要处理的
        ScenceAyn.LoadSceneAsyn("Laby", () =>
            {
                //遥感管理赋值
                JoyStickMgr.Ins.m_joyGO = DontDestory[0]; //所有按钮
                JoyStickMgr.Ins.m_joystick = Joy;
                JoyStickMgr.Ins.m_skillButton = attack;
                Debug.Log(JoyStickMgr.Ins.m_skillButton.Count+"--------------");

                //配置表解析
                GameData.Ins.InitByRoleName("Teddy");
                GameData.Ins.InitTaskDate();
                World.Ins.Init();

            }
        );
       
    }

    public void onImagePath(string ret)
    {
        if (!ret.Equals("Fail"))
        {

            return;
        }
        Debug.LogError("打开失败");
    }


}
