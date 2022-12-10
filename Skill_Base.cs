using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Base 
{
    public string name = string.Empty;
    public virtual void Play() { }
    public virtual void Init() { }
    public virtual void Stop() { }
}
public class Skill_Ani: Skill_Base
{
    Player player;
    Animator anim;
    public AnimationClip animClip;
    AnimatorOverrideController controller;

    public Skill_Ani(Player _player)
    {
        player = _player;
        anim = player.gameObject.GetComponent<Animator>();
        controller = player.overrideController;
    }
    public override void Init()
    {
        controller["Attack4"] = animClip;
    }
    public void SetAnimClip(AnimationClip _animClip)
    {
        animClip = _animClip;
        name = animClip.name;
        controller["Attack4"] = animClip;
    }
    public override void Play()
    {
        base.Play();
        anim.StopPlayback();
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        controller["Attack4"] = animClip;
        anim.Play("Start", 0, 0);
        //if (stateInfo.IsName("Idlel"))
        //{
        //    //anim.SetTrigger("Play");
           
        //}
    }
    public override void Stop()
    {
        base.Stop();
        anim.StartPlayback();
    }
}
public class Skill_Aud:Skill_Base
{
    Player player;
    AudioClip audioClip;
    AudioSource audioSource;
    public Skill_Aud(Player _player)
    {
        player = _player;
        audioSource = player.gameObject.GetComponent<AudioSource>();
    }
    public void SetSourClip(AudioClip _audirClip)
    {
        audioClip = _audirClip;
        name = audioClip.name;
        audioSource.clip = audioClip;
    }
    public override void Init()
    {
        base.Init();
        audioSource.clip = audioClip;

    }
    public override void Play()
    {
        base.Play();
        audioSource.Play();
    }
    public override void Stop()
    {
        base.Stop();
        audioSource.Stop();
    }
}
public class Skill_Effect: Skill_Base
{
    public GameObject gameClip;
    Player player;
    ParticleSystem particleSystem;//Á£×ÓÊÂ¼þ
    GameObject obj;
    public Skill_Effect(Player _player)
    {
        player = _player;
    }
    public void SetGameClip(GameObject _audioClip)
    {
        gameClip = _audioClip;
        if (gameClip.GetComponent<ParticleSystem>())
        {
            obj = GameObject.Instantiate(gameClip, player.effectsparent);
            particleSystem = obj.GetComponent<ParticleSystem>();
            particleSystem.Stop();
        }
        name = _audioClip.name;
    }
    public override void Init()
    {
        base.Init();
        if (gameClip.GetComponent<ParticleSystem>())
        {
            particleSystem = obj.GetComponent<ParticleSystem>();
            particleSystem.Stop();
        }
    }
    public override void Play()
    {
        base.Play();
        if (particleSystem!=null)
        {
            particleSystem.Play();
        }
    }
    public override void Stop()
    {
        base.Play();
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }
    }
}
