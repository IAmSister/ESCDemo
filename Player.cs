using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public RuntimeAnimatorController controller;
    public AnimatorOverrideController overrideController;
    public AudioSource audioSource;
    public Transform effectsparent;
   public Dictionary<string, List<Skill_Base>> skillsList = new Dictionary<string, List<Skill_Base>>();


    private Skill_Ani _Anim;
    private Skill_Aud _Aduio;
    private Skill_Effect _Effect;
    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    /// <summary>
    /// ��ʼ������ ���������
    /// </summary>
    public void InitData()
    {
      
        overrideController = new AnimatorOverrideController();
        controller = Resources.Load<RuntimeAnimatorController>("Player");
        overrideController.runtimeAnimatorController = controller;
        anim.runtimeAnimatorController = overrideController;
        audioSource = gameObject.AddComponent<AudioSource>();
        effectsparent = transform.Find("effectsparent");
    }
    /// <summary>
    /// �������
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// ���Ŷ���ͬʱ ������Ч
    /// </summary>
    public void Play()
    {
        _Anim.Play();
    }
    /// <summary>
    /// ���������ؼ��ؼ��� 
    /// </summary>
    /// <param name="skillName">�������</param>
    public void SetData(string skillName)
    {
        //������м���
        List<SkillXml> skillList = GameData.Ins.GetSkillByRoleName("Teddy");
        foreach (var item in skillList)
        {
            if (item.name == skillName)
            {
                foreach (var ite in item.skills)
                {
                    foreach (var it in ite.Value)
                    {
                        if (ite.Key.Equals("����"))
                        {
                            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/GameDate/Anim/" + it + ".anim");
                            if (_Anim == null) _Anim = new Skill_Ani(this);
                            _Anim.SetAnimClip(clip);
                            _Anim.Play();
                        }
                        else if (ite.Key.Equals("��Ч"))
                        {
                            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/GameDate/Audio/" + it + ".mp3");
                            if (_Aduio == null) _Aduio = new Skill_Aud(this);
                            _Aduio.SetSourClip(clip);
                            _Aduio.Play();
                        }
                        else if (ite.Key.Equals("��Ч"))
                        {
                            GameObject clip = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameDate/Effect/Skill/" + it + ".prefab");
                            if (_Effect == null) _Effect = new Skill_Effect(this);
                            _Effect.SetGameClip(clip);
                            _Effect.Play();
                        }
                    }
                }
            }
        }

    }
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Player Init(string path)
    {
        if (path != null)
        {
            string str = "Assets/aaa/" + path + ".prefab";
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(str);
            if (obj != null)
            {
                Player player = Instantiate(obj).GetComponent<Player>();
                player.overrideController = new AnimatorOverrideController();
                player.controller = Resources.Load<RuntimeAnimatorController>("Player");
                player.overrideController.runtimeAnimatorController = player.controller;
                player.anim.runtimeAnimatorController = player.overrideController;
                player.audioSource = player.gameObject.AddComponent<AudioSource>();
                player.effectsparent = player.transform.Find("effectsparent");
                player.gameObject.name = path;
                player.LoadAllSkill();
                return player;
            }
        }
        return null;
    }
    /// <summary>
    /// ����
    /// </summary>
    private void LoadAllSkill()
    {
        if (File.Exists("Assets/" + gameObject.name + ".txt"))
        {
            string str = File.ReadAllText("Assets/" + gameObject.name + ".txt");
            List<SkillXml> skills = JsonConvert.DeserializeObject<List<SkillXml>>(str);
            foreach (var item in skills)
            {
                skillsList.Add(item.name, new List<Skill_Base>());
                foreach (var ite in item.skills)
                {
                    foreach (var it in ite.Value)
                    {
                        if (ite.Key.Equals("����"))
                        {
                            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/GameDate/Anim/" + it + ".anim");
                            Skill_Ani _Anim = new Skill_Ani(this);
                            _Anim.SetAnimClip(clip);
                            skillsList[item.name].Add(_Anim);
                        }
                        else if (ite.Key.Equals("��Ч"))
                        {
                            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/GameDate/Audio/" + it + ".mp3");
                            Skill_Aud _Anim = new Skill_Aud(this);
                            _Anim.SetSourClip(clip);
                            skillsList[item.name].Add(_Anim);
                        }
                        else if (ite.Key.Equals("��Ч"))
                        {
                            GameObject clip = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameDate/Effect/Skill/" + it + ".prefab");
                            Skill_Effect _Anim = new Skill_Effect(this);
                            _Anim.SetGameClip(clip);
                            skillsList[item.name].Add(_Anim);
                        }
                    }
                }
            }
        }
    } 
    /// <summary>
    /// ��Ӽ��� ����return û�����
    /// </summary>
    /// <param name="skillName"></param>
    /// <returns></returns>

    public List<Skill_Base> AddNeWsKILL(string skillName)
    {
        if (skillsList.ContainsKey(skillName))
        {
            return skillsList[skillName];
        }
        skillsList.Add(skillName, new List<Skill_Base>());
        return new List<Skill_Base>();
    }
    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public List<Skill_Base> GetSkill(string name)
    {
        if (skillsList.ContainsKey(name))
        {
            return skillsList[name];
        }
        return null;
    }
    /// <summary>
    /// ɾ������
    /// </summary>
    /// <param name="name"></param>
    public void RevSkill(string name)
    {
        if (skillsList.ContainsKey(name))
        {
            skillsList.Remove(name);
        }
    }
    private void OnDestroy()
    {
        return;
        //���ٵ�ʱ��д�������Ϣ
        List<SkillXml> skills = new List<SkillXml>();
        foreach (var item in skillsList)
        {
            SkillXml skillXml = new SkillXml();
            skillXml.name = item.Key;
            foreach (var ite in item.Value)
            {
                if (ite is Skill_Ani)
                {
                    if (!skillXml.skills.ContainsKey("����"))
                    {
                        skillXml.skills.Add("����", new List<string>());
                    }
                    skillXml.skills["����"].Add(ite.name);
                }
                else if (ite is Skill_Aud)
                {
                    if (!skillXml.skills.ContainsKey("��Ч"))
                    {
                        skillXml.skills.Add("��Ч", new List<string>());
                    }
                    skillXml.skills["��Ч"].Add(ite.name);
                }
                else if (ite is Skill_Effect)
                {
                    if (!skillXml.skills.ContainsKey("��Ч"))
                    {
                        skillXml.skills.Add("��Ч", new List<string>());
                    }
                    skillXml.skills["��Ч"].Add(ite.name);
                }

            }
            skills.Add(skillXml);
        }
        string str = JsonConvert.SerializeObject(skills);
        File.WriteAllText("Assets/" + gameObject.name + ".txt", str);
    }

}
