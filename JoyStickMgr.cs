using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遥感控制
/// </summary>
public class JoyStickMgr : Singlton<JoyStickMgr>
{
    public GameObject m_joyGO;//joy
    public ETCJoystick m_joystick; //左边大按钮
    public List<ETCButton> m_skillButton=new List<ETCButton>(); //五个小按钮
    HostPlayer m_target; //可以控制玩家

    //设置遥感是否被激活 
    public bool JoyActive
    {
        set
        {
            if (m_joyGO.activeSelf!=value)
            {
                m_joyGO.SetActive(value);
            }
        }
    }
    public void SetJoyArg(Camera camera , HostPlayer m_target)
    {
        //遥感看向玩家
       this. m_target = m_target;
        m_joystick.cameraLookAt = m_target.m_go.transform;
        m_joystick.cameraTransform = camera.transform;
        SetJoytick();
    }

    private void SetJoytick()
    {
        //遥感事件监听
        if (m_joystick && m_target.m_go)
        {
            //四个位置  调用玩家移动脚本方法
            m_joystick.OnPressLeft.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
            m_joystick.OnPressRight.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
            m_joystick.OnPressUp.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
            m_joystick.OnPressDown.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
        }

        //技能按钮监听

        if (m_skillButton.Count != 0 && m_target.m_go)
        {
            foreach (var item in m_skillButton)
            {
                item.onPressed.AddListener(() => m_target.JoyButtonHandler(item.name));
            }
        }
    }
}
