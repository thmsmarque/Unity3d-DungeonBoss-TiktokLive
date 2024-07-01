using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossScript : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameOfBoss;

    public float health = 100;
    public float maxHealth = 100;

    public bool monsterOnBoard;

    public Image healthBar;

    public GameObject Manager;

    public GameObject actualMonster;

    [SerializeField]
    Transform posSpawnBoss;

    [SerializeField]
    Transform[] smokesSpawner;
    [SerializeField]
    ParticleSystem smokePrefab;
    [SerializeField]
    AudioSource audioSource;

    private void Start()
    {
        //smokesSpawner = GameObject.Find("SmokeSpawner").GetComponentsInChildren<Transform>();
        //Manager = GameObject.FindGameObjectWithTag("Manager");
        //healthBar = this.GetComponentInChildren<Image>().GetComponentInChildren<Image>();
    }

    /// <summary>
    /// Met en place le monstre
    /// </summary>
    /// <param name="bt"></param>
    public void setMonster(BossTemplate bt)
    {
        this.health = bt.health;
        this.maxHealth = bt.maxHealth;
        this.nameOfBoss.text = bt.name;
        actualMonster = Instantiate(bt.bodyGO, posSpawnBoss);
        monsterOnBoard = true;
        audioSource.PlayOneShot(bt.bossAudio);

    }


    private void Update()
    {
        healthBar.fillAmount = health / maxHealth;
        
    }

    /// <summary>
    /// Inflige des dégats au boss
    /// </summary>
    /// <param name="amountDmg">Les dégats à infliger</param>
    public void takeDamage(float amountDmg)
    {
        Debug.Log("Dégats prit");
        health -= amountDmg;
        health = Mathf.Clamp(health, 0f, maxHealth);

        System.Random r = new System.Random();

        if (health <= 0 && monsterOnBoard)
        {
            monsterOnBoard = false;
            Manager.GetComponent<TikTokConnection>().vic();
            GameObject.Find("BossSpawn").GetComponentInChildren<Animator>().SetBool("Dead", true);
            StartCoroutine(destroyMonster(6));
        }
    }

    IEnumerator destroyMonster(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(actualMonster);
        actualMonster.SetActive(false);
    }
}
