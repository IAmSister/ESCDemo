using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ң�п���
/// </summary>
public class JoyStickMgr : Singlton<JoyStickMgr>
{
    public GameObject m_joyGO;//joy
    public ETCJoystick m_joystick; //��ߴ�ť
    public List<ETCButton> m_skillButton=new List<ETCButton>(); //���С��ť
    HostPlayer m_target; //���Կ������

    //����ң���Ƿ񱻼��� 
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
        //ң�п������
       this. m_target = m_target;
        m_joystick.cameraLookAt = m_target.m_go.transform;
        m_joystick.cameraTransform = camera.transform;
        SetJoytick();
    }

    private void SetJoytick()
    {
        //ң���¼�����
        if (m_joystick && m_target.m_go)
        {
            //�ĸ�λ��  ��������ƶ��ű�����
            m_joystick.OnPressLeft.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
            m_joystick.OnPressRight.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
            m_joystick.OnPressUp.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
            m_joystick.OnPressDown.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
        }

        //���ܰ�ť����

        if (m_skillButton.Count != 0 && m_target.m_go)
        {
            foreach (var item in m_skillButton)
            {
                item.onPressed.AddListener(() => m_target.JoyButtonHandler(item.name));
            }
        }
    }
}
