using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public float xMap, yMap;
    public float xoffset, yoffset;

    private Transform player;
    Dictionary<MonsterType, Transform> monsterdic = new Dictionary<MonsterType, Transform>();

    List<ObjectBase> otherGoPos = new List<ObjectBase>();
    Vector3 playerpos = new Vector3(0, 0, 0);
    List<Vector3> otherpos = new List<Vector3>();

    private void Awake()
    {
        xMap = this.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        yMap = this.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        xoffset = xMap / World.Ins.xlength;
        yoffset = xMap / World.Ins.ylength;

        player = transform.Find("player");
        monsterdic.Add(MonsterType.Gather, transform.Find("gather"));
        monsterdic.Add(MonsterType.Normal, transform.Find("monster"));
        monsterdic.Add(MonsterType.NPC, transform.Find("npc"));
    }

    void Update()
    {
        if (World.Ins.dic.Count != otherGoPos.Count)
        {
            otherGoPos.Clear();
            otherpos.Clear();
            foreach (var item in World.Ins.dic)
            {
                otherGoPos.Add(item.Value);
                otherpos.Add(new Vector3(0, 0, 0));
            }
        }
        if (player && World.Ins.m_plyer.m_go)
        {
            playerpos.Set(World.Ins.m_plyer.m_go.transform.position.x * xoffset, World.Ins.m_plyer.m_go.transform.position.z * yoffset, 0);
            //Debug.Log($"×îÖÕÎ»ÖÃ  x:{World.Instance.m_plyer.m_go.transform.position.x * xoffset}  y:{World.Instance.m_plyer.m_go.transform.position.z * yoffset}");
            player.localPosition = playerpos;
        }
        if (otherGoPos != null && otherGoPos.Count > 0)
        {
            for (int i = 0; i < otherGoPos.Count; i++)
            {
                otherpos[i] = new Vector3(otherGoPos[i].m_go.transform.position.x * xoffset, otherGoPos[i].m_go.transform.position.z * yoffset, 0);
                monsterdic[otherGoPos[i].monsterType].transform.localPosition = otherpos[i];
            }
        }

    }

    private void OnDestroy()
    {

        CancelInvoke("UpdateMap");
    }
}
