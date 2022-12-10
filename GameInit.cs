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

        //������ת
        for (int i = 0; i < DontDestory.Length; i++)
        {
            GameObject.DontDestroyOnLoad(DontDestory[i]);
        }
        //������תʱҪ�����
        ScenceAyn.LoadSceneAsyn("Laby", () =>
            {
                //ң�й���ֵ
                JoyStickMgr.Ins.m_joyGO = DontDestory[0]; //���а�ť
                JoyStickMgr.Ins.m_joystick = Joy;
                JoyStickMgr.Ins.m_skillButton = attack;
                Debug.Log(JoyStickMgr.Ins.m_skillButton.Count+"--------------");

                //���ñ����
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
        Debug.LogError("��ʧ��");
    }


}
