using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouretScript : MonoBehaviour
{
    [SerializeField]
    int level;
    [SerializeField]
    float speedAttack;
    [SerializeField]
    bool isOnBoard;
    [SerializeField]
    float damageDealt;

    public float maxSize, minSize;

    public BossScript boss;

    GameObject mesh;

    public ParticleSystem smoke;
    public ParticleSystem maginRing;
    public float damageIncrease;

    // Start is called before the first frame update
    void Start()
    {
        maginRing.Stop();
        damageIncrease = (float)2.0;
        level = 0;
        speedAttack = 5;
        isOnBoard = false;

        boss = GameObject.FindGameObjectWithTag("Monster").GetComponent<BossScript>();
        mesh = this.GetComponentInChildren<Transform>().gameObject;
        //mesh.transform.localScale = new Vector3((float)minSize, (float)minSize, (float)minSize);
        //mesh.gameObject.SetActive(false);

        StartCoroutine(waitForAttacking());
    }

    IEnumerator waitForAttacking()
    {
        yield return new WaitWhile(() => !isOnBoard);
        StartCoroutine(attackBoss());

    }

    /// <summary>
    /// Inflige des dégats au boss toute les speedAttack secondes
    /// </summary>
    /// <returns></returns>
    IEnumerator attackBoss()
    {
        //Debug.Log("Tour attends d'attaquer");
       // Debug.Log("Tour va attaquer mtn");

        if (isOnBoard)
        {
            Debug.Log("Tourelle attaque");
            float damage = (int)((float)level * damageIncrease);
            damageDealt += damage;
            boss.takeDamage(damage);
            smoke.Play();
        }
        //Debug.Log("Tour va se recharger");

        yield return new WaitForSeconds(speedAttack);

        StartCoroutine(attackBoss());

    }

    /// <summary>
    /// Increase level
    /// </summary>
    /// <param name="lv">level to increase</param>
    public void upgradeLevel(int lv)
    {
        StartCoroutine(upLevel(lv));
    }

    IEnumerator upLevel(int lv)
    {
        this.level += lv;
        if (level > 0)
        {
            GetComponentInChildren<Animator>().SetBool("wakeUp", true);
            isOnBoard = true;
            mesh.SetActive(true);
            maginRing.Play();
            speedAttack = Mathf.Clamp(speedAttack - (float)0.05, 1, 10);
            yield return new WaitForSeconds(1);
            maginRing.Stop();
        }
    }

    /// <summary>
    /// Diminue la durée entre chaque attaques
    /// </summary>
    /// <param name="speedAttack">La durée à diminuer</param>
    public void decreaseSpeedAttack(float speedAttack)
    {
        this.speedAttack = Mathf.Clamp(this.speedAttack - speedAttack, (float)0.5, 15);
       
    }

    
}
