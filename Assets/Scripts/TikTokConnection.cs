using System.Collections;
using TikTokLiveSharp.Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TikTokLiveSharp.Events.Objects;
using TikTokLiveSharp.Events;
using TikTokLiveSharp.Models;
using TikTokLiveUnity;
using TikTokLiveUnity.Utils;
using System.Collections.Generic;


/// <summary>
/// Example-Script for TikTokLiveUnity
/// </summary>
public class TikTokConnection : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// Text displaying Host connected to
    /// </summary>
    [SerializeField]
    [Tooltip("Text displaying Host connected to")]
    private TMP_Text txtStatusHostId;
    /// <summary>
    /// InputField for Host to connect to
    /// </summary>
    [SerializeField]
    [Tooltip("InputField for Host to connect to")]
    private TMP_InputField ifHostId;
    /// <summary>
    /// Connect-Button
    /// </summary>
    [SerializeField]
    [Tooltip("Connect-Button")]
    private Button btnConnect;

    /// <summary>
    /// Prefab for Row to display Gift
    /// </summary>
    [SerializeField]
    [Tooltip("Prefab for Row to display Gift")]
    //private GiftRow giftRowPrefab;

    /// <summary>
    /// ShortHand for TikTokLiveManager-Access
    /// </summary>
    private TikTokLiveManager mgr => TikTokLiveManager.Instance;

    /// <summary>
    /// SpawnerPlayer Logic
    /// </summary>
    private PlayerSpawner ps;

    [SerializeField]
    private Image tempImg;

    public List<long> IDPlayerOnStage;
    public List<PlayerScript> PlayersOnStage;

    public GameObject boss;
    public BossTemplate actualBoss;
    [SerializeField]
    BossTemplate[] listOfBosses;
    [SerializeField]
    BossTemplate playerBoss;

    public TouretScript[] touretsList;

    public GameObject fireworks;

    [SerializeField]
    GiftRow giftRowPrefab;

    [SerializeField]
    AudioClip vicSound;

        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Initializes this Object
        /// </summary>
        private IEnumerator Start()
        {

            tempImg = this.GetComponent<Image>();

            if (ps = GameObject.FindWithTag("PlayerSpawner").GetComponent<PlayerSpawner>())
            {
                Debug.Log("Le spawn Player a été trouvé");
            }
            btnConnect.onClick.AddListener(OnClick_Connect);
            mgr.OnConnected += ConnectStatusChange;
            mgr.OnDisconnected += ConnectStatusChange;
            mgr.OnJoin += OnJoin;
            mgr.OnLike += OnLike;
            //mgr.OnComment += OnComment;
            mgr.OnGift += OnGift;
            for (int i = 0; i < 3; i++)
                yield return null; // Wait 3 frames in case Auto-Connect is enabled
            UpdateStatus();
            this.changeBoss();
        StartCoroutine(UpdateLeaderboardUI());
            }
        /// <summary>
        /// Deinitializes this Object
        /// </summary>
        private void OnDestroy()
        {
            btnConnect.onClick.RemoveListener(OnClick_Connect);
            if (!TikTokLiveManager.Exists)
                return;
            mgr.OnConnected -= ConnectStatusChange;
            mgr.OnDisconnected -= ConnectStatusChange;
            mgr.OnJoin -= OnJoin;
            mgr.OnLike -= OnLike;
            //mgr.OnComment -= OnComment;
            mgr.OnGift -= OnGift;
        }
        #endregion

        #region Private
        /// <summary>
        /// Handler for Connect-Button
        /// </summary>
        private void OnClick_Connect()
        {
            bool connected = mgr.Connected;
            bool connecting = mgr.Connecting;
            if (connecting)
            {
                Debug.Log("Se connecte...");
            }
        if (connected || connecting)
            mgr.DisconnectFromLivestreamAsync();
        else mgr.ConnectToStreamAsync(ifHostId.text, Debug.LogException);
            UpdateStatus();
            Invoke(nameof(UpdateStatus), .5f);

        }

    public TextMeshProUGUI leaderboardText; // Assurez-vous de lier cet élément dans l'inspecteur Unity
    public float timeToUpdateLeaderBoard = 2.5f;

    // Fonction pour mettre à jour l'interface utilisateur
    private IEnumerator UpdateLeaderboardUI()
    {
        Debug.Log("Actualisation du classemetn");
        string leaderboardContent = "Leaderboard\n";
        this.trierListesJoueurs();
        foreach (PlayerScript player in PlayersOnStage)
        {
            leaderboardContent += player.getName() + " - " + player.getNbLike() + "\n";
        }
        leaderboardText.text = leaderboardContent;
        yield return new WaitForSeconds(timeToUpdateLeaderBoard);
        StartCoroutine(UpdateLeaderboardUI());
    }


    public void trierListesJoueurs()
    {
        PlayersOnStage.Sort((x, y) => y.getNbLike().CompareTo(x.getNbLike()));
    }

/// <summary>
/// Handler for Connection-Events. Updates StatusPanel
/// </summary>
private void ConnectStatusChange(TikTokLiveClient sender, bool e) => UpdateStatus();

        /// <summary>
        /// Handler for Gift-Event
        /// </summary>
        private void OnGift(TikTokLiveClient sender, TikTokGift gift)
        {
        //Debug.Log("Ce cadeau a été envoyé :" + gift.Gift.Name);
        giftRowPrefab.Init(gift);
        }
        /// <summary>
        /// Handler for Join-Event
        /// </summary>
        private void OnJoin(TikTokLiveClient sender, Join join)
        {

        }
        /// <summary>
        /// Handler for Like-Event
        /// </summary>
        private void OnLike(TikTokLiveClient sender, Like like)
        {
        Debug.Log("Un like a été envoyé");
            //Invocation du joueur sur le plateau
            if(!IDPlayerOnStage.Contains(like.Sender.Id))
            {
                //Si le joueur n'est pas in-game
                InvocationPlayer(like.Sender.Id, like.Sender.AvatarThumbnail, tempImg, like.Sender.NickName);
            }else
            {
            //Le joueur est déjà inGame
            Debug.Log("Dégats provoqué par un joueur : " + like.Sender.NickName + " likes : " + like.Count);
                sendDamageToActualBoss(5f * like.Count);
                getPlayerFromId(like.Sender.Id).aLike((int)like.Count);
                
            }

        }



        /// <summary>
        /// Handler for Comment-Event
        /// </summary>
        private void OnComment(TikTokLiveClient sender, Chat e)
        {
            if (e.Message.Equals("!join"))
            {
                //InvocationPlayer();
            }
        }
        /// <summary>
        /// Requests Image from TikTokLive-Manager
        /// </summary>
        /// <param name="img">UI-Image used for display</param>
        /// <param name="picture">Data for Image</param>
        private void RequestImage(Image img, Picture picture)
        {
            Dispatcher.RunOnMainThread(() =>
            {
                mgr.RequestSprite(picture, spr =>
                {
                    if (img != null && img.gameObject != null && img.gameObject.activeInHierarchy)
                    {
                        img.sprite = spr;
                        Debug.Log("Image récupérée");
                    } else
                    {
                        Debug.Log("Image non récupérée");
                    }
                });
            });
        }

        /// <summary>
        /// Updates Status-Panel based on ConnectionState
        /// </summary>
        private void UpdateStatus()
        {
            bool connected = mgr.Connected;
            bool connecting = mgr.Connecting;
            if (connected || connecting)
                txtStatusHostId.text = mgr.HostName;
            ifHostId.gameObject.SetActive(!connected && !connecting);
            btnConnect.GetComponentInChildren<TMP_Text>().text = connected ? "Disconnect" : connecting ? "Cancel" : "Connect";
            if (connected)
            {
                btnConnect.transform.parent.gameObject.SetActive(false);
            }
        }
        #endregion
        #endregion

        /// <summary> Sert à l'invocation des players</summary>
        void InvocationPlayer(long id, Picture picture,Image img, string nickName)
        {
            if (!IDPlayerOnStage.Contains(id))
            {

                Dispatcher.RunOnMainThread(() =>
                {
                    TikTokLiveManager.Instance.RequestSprite(picture, spr =>
                    {
                        img.sprite = spr;

                        Material mtl = new Material(Shader.Find("Standard"));
                        mtl.mainTexture = img.sprite.texture;
                        ps.SpawnPlayer(nickName, mtl,id);
                    });
                });
                
                IDPlayerOnStage.Add(id);
            }
        }

        /// <summary>
        /// Ajoute les joueurs dans la liste
        /// </summary>
        /// <param name="pl"></param>
        public void addPlayerInList(PlayerScript pl)
        {
        this.PlayersOnStage.Add(pl);
        }

        /// <summary>
        /// Inflige des dégats au boss
        /// </summary>
        /// <param name="dmg"></param>
        public void sendDamageToActualBoss(float dmg)
        {
            boss.GetComponent<BossScript>().takeDamage(dmg);
        }

        /// <summary>
        /// Appelle la fonction setMonster de BossScript (utiliser changeBoss())
        /// </summary>
        void initBoss()
        {
            boss.GetComponent<BossScript>().setMonster(actualBoss);
        }

        /// <summary>
        /// Choisi aléatoirement un nouveau boss
        /// </summary>
        public void changeBoss()
        {
            Debug.Log("Changement de boss...");
            System.Random r = new System.Random();
            if(r.Next(0,3) != 1 || PlayersOnStage.Count==0)
                {
                    //On fait apparaître un boss
                    actualBoss = listOfBosses[r.Next(0,listOfBosses.Length)];
                    this.initBoss();
            }else
                    //On met un joueur à la place du boss
                {
            PlayerScript t = PlayersOnStage[r.Next(0,PlayersOnStage.Count-1)];
            playerBoss.setNameAndProfile(t.name, t.getImage());
            actualBoss = playerBoss;
            this.initBoss();
                }
    }

        /// <summary>
        /// Retourne le joueur sur le plateau avec l'id correspondant
        /// </summary>
        /// <param name="id">id du joueur à chercher</param>
        /// <returns></returns>
        PlayerScript getPlayerFromId(long id)
        {
            PlayerScript res = new PlayerScript();
            
            foreach(PlayerScript pl in PlayersOnStage)
            {
            if (pl.getIdPlayer() == id) return pl;
            }
        return res;
        }

        /// <summary>
        /// Lance la cinématique de victoire
        /// </summary>
        public void vic()
        {
        StartCoroutine(Victory());
        }
        
        /// <summary>
        /// Lance l'évenement de victoire
        /// </summary>
        /// <returns></returns>
        IEnumerator Victory()
    {
        this.GetComponent<AudioSource>().PlayOneShot(vicSound);
        fireworks.SetActive(true);
        foreach (PlayerScript pl in PlayersOnStage)
        {
            pl.CelebVictory();
        }
        yield return new WaitForSeconds(12);
        fireworks.SetActive(false);
        changeBoss();
    }

    /// <summary>
    /// Envoi des levels à une tourelle au hasard
    /// </summary>
    /// <param name="lvToSend">Nombre de niveaux à envoyer</param>
    public void sendLevelsTourets(int lvToSend)
    {
        System.Random r = new System.Random();
        touretsList[r.Next(0, touretsList.Length)].upgradeLevel(lvToSend);
    }

            
}

