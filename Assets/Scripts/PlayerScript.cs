using System.Collections;
using System.Collections.Generic;
using TikTokLiveSharp.Events;
using TikTokLiveSharp.Events.Objects;
using TikTokLiveUnity;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    Material profilePicture;
    public string name;
    public long idPlayer;
    public int numberOfLikes;

    public bool winTest = false;
     
   
    public void setPlayer(Material image, string name, long id)
    {

        this.name = name;
        profilePicture = image;
        this.idPlayer = id;

        profilePicture = image;
        this.GetComponentInChildren<MeshRenderer>().sharedMaterial = image;
       
    }

    public Material getImage()
    {
        return profilePicture;
    }

    private void Update()
    {
        GetComponent<Animator>().SetBool("VictoryCelebration", winTest);
    }

    public long getIdPlayer()
    {
        return idPlayer;
    }

    public string getName()
    {
        return name;
    }

    public int getNbLike()
    {
        return numberOfLikes;
    }

    public void aLike(int likes)
    {
        numberOfLikes += likes;
    }


    public IEnumerator CelebVictory()
    {
        System.Random r = new System.Random();
        Jump();
        yield return new WaitForSeconds((float)r.NextDouble() * (3-1) + 1);
        winTest = true;
        yield return new WaitForSeconds(1);
        winTest = false;
        yield return new WaitForSeconds(1);
    }

    public float jumpForce = 2;
    public void Jump()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
