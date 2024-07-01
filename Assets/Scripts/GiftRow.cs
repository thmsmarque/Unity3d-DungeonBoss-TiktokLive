using System;
using System.Collections;
using System.Collections.Generic;
using TikTokLiveSharp.Events.Objects;
using TikTokLiveUnity;
using TikTokLiveUnity.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


    /// <summary>
    /// Displays Gift (with Updates) in ExampleScene
    /// </summary>
    public class GiftRow : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Gift being Displayed
        /// </summary>
        public TikTokGift Gift { get; private set; }


        /// <summary>
        /// Text displaying Amount sent
        /// </summary>
        [SerializeField]
        [Tooltip("Text displaying Amount sent")]
        private TMP_Text txtAmount;

        #endregion

        [SerializeField]
        ArrayList giftList = new ArrayList();

        TikTokConnection gm;

        #region Methods
        /// <summary>
        /// Initializes GiftRow
        /// </summary>
        /// <param name="gift">Gift to Display</param>
        public void Init(TikTokGift gift)
        {
            Gift = gift;
        initList();
            gm = GameObject.FindGameObjectWithTag("Manager").GetComponent<TikTokConnection>();
            Gift.OnAmountChanged += AmountChanged;
            Gift.OnStreakFinished += StreakFinished;
            Debug.Log(Gift.Sender.UniqueId + "sent a " + Gift.Gift.Name + "  " + Gift.Amount);
            //txtUserName.text = $"{Gift.Sender.UniqueId} sent a {Gift.Gift.Name}!";
            //txtAmount.text = $"{Gift.Amount}x";
            //RequestImage(imgUserProfile, Gift.Sender.AvatarThumbnail);
            //RequestImage(imgGiftIcon, Gift.Gift.Image);
            // Run Streak-End for non-streakable gifts
            if (gift.StreakFinished)
                StreakFinished(gift, gift.Amount);
        GiftChecked();
        }
        /// <summary>
        /// Deinitializes GiftRow
        /// </summary>
        private void OnDestroy()
        {
            gameObject.SetActive(false);
            if (Gift == null)
                return;
            Gift.OnAmountChanged -= AmountChanged;
            Gift.OnStreakFinished -= StreakFinished;
        }
        /// <summary>
        /// Updates Gift-Amount if Amount Changed
        /// </summary>
        /// <param name="gift">Gift for Event</param>
        /// <param name="newAmount">New Amount</param>
        private void AmountChanged(TikTokGift gift, long change, long newAmount)
        {
            txtAmount.text = $"{newAmount}x";

            this.GiftChecked();
        }
        /// <summary>
        /// Called when GiftStreaks Ends. Starts Destruction-Timer
        /// </summary>
        /// <param name="gift">Gift for Event</param>
        /// <param name="finalAmount">Final Amount for Streak</param>
        private void StreakFinished(TikTokGift gift, long finalAmount)
        {
            //AmountChanged(gift, 0, finalAmount);
            //Destroy(gameObject, 2f);
        }
        /// <summary>
        /// Requests Image from TikTokLive-Manager
        /// </summary>
        /// <param name="img">UI-Image used for display</param>
        /// <param name="picture">Data for Image</param>
        private static void RequestImage(Image img, Picture picture)
        {
            Dispatcher.RunOnMainThread(() =>
            {
                if (TikTokLiveManager.Exists)
                    TikTokLiveManager.Instance.RequestSprite(picture, spr =>
                    {
                        if (img != null && img.gameObject != null && img.gameObject.activeInHierarchy)
                            img.sprite = spr;
                    });
            });
        }
        #endregion

        void initList()
        {
            //Rose giftList[0] vaut 1
            giftList.Add("Rose");
            
            //Rose giftList[1] vaut 30
            giftList.Add("Doughnut");

            //Rose giftList[2] vaut 5
            giftList.Add("Finger Heart");

            //Rose giftList[3] vaut 10
            giftList.Add("Watermelon");
        }

        void GiftChecked()
        {
            if(Gift.Gift.Name == (string)giftList[0])
            {
                RoseAction();
            }

            if (Gift.Gift.Name == (string)giftList[1])
            {
                DoughnutAction();
            }

            if (Gift.Gift.Name == (string)giftList[2])
            {
                IceCreamAction();
            }

            if (Gift.Gift.Name == (string)giftList[3])
            {
                WatermelonAction();
            }

        }

        void RoseAction()
        {
            //Debug.Log("Rose envoyée");
            gm.sendLevelsTourets(1);
        }

        void IceCreamAction()
        {
            //Debug.Log("FingerHeart envoyée");
            gm.sendLevelsTourets(7);
        }

        void DoughnutAction()
        {
            //Debug.Log("Donut envoyée");
            gm.sendLevelsTourets(50);
        }

        void WatermelonAction()
        {
            //Debug.Log("Watermelon envoyée");
            gm.sendLevelsTourets(15);
        }
    }

