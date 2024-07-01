using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameObjects/Boss")]
public class BossTemplate : ScriptableObject
{
    public float health;
    public float maxHealth;
    public string name;
    public GameObject bodyGO;
    public AudioClip bossAudio;

    public BossTemplate(float health, float maxhealth, string nam, GameObject go)
    {
        this.health = health;
        this.maxHealth = maxhealth;
        this.name = nam;
        this.bodyGO = go;
    }

    public void setNameAndProfile(string name,Material mat)
    {
        if (name!= null && mat != null)
        {
            this.name = name;
            bodyGO.GetComponent<MeshRenderer>().sharedMaterial = mat;
        }
        
    }
    public void setBoss(float health, float maxhealth, string nam, GameObject go)
    {
        this.health = health;
        this.maxHealth = maxhealth;
        this.name = nam;
        this.bodyGO = go;
    }


}
