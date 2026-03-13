using System;
using System.Collections.Generic;
using System.Linq;
using Gift2.Core;
using UnityEngine;

namespace Gift2
{
    public class SaveLoader : MonoBehaviour
    {
        [Serializable]
        public struct Purchase {public string Shop; public int Slot; public int Amount;}
        
    
        public string FileName = "gift2.save";
        public Player Player;
        
        public List<InteractableQuestDealer> Dealers = new();
        
        public ShopView SpeedShop;
        public ShopView GrowShop;
        public ShopView WeaponShop;
        
        [Header("Test")]
        public bool UseTest = false;
        public int TestQuestsCompleted = 3;
        [Serializable]
        public struct TestResource {public string Key; public int Amount;}
        public List<Purchase> Purchases = new();
        public List<TestResource> Resources = new();

                
        public List<ShopView> Shops => new() {SpeedShop, GrowShop, WeaponShop};
        
        private struct PlayerData 
        {
            public int QuestsCompleted;
            public List<Purchase> Purchases;
            public Dictionary<string, int> Resources;            
        }
        
        void Start()
        {
            var data = Load();
            for (int i = 0; i < data.QuestsCompleted; i++)
            {
                if (Dealers.Count > i)
                {
                    var quest = Dealers[i].SilenceAcceptQuest();
                    quest.Complete();
                }
            }
            foreach (var shop in Shops)
            {
                var slots = data.Purchases.FindAll(e => e.Shop == shop.name);
                var controller = shop.GetComponent<ShopController>();
                foreach (var slot in slots)
                {
                    for (int i = 0; i < slot.Amount; i++)
                        controller.FreeBuy(slot.Slot);
                }
                shop.CloseShop();
            }
            
            foreach (var r in data.Resources)
            {
                Player.ResourcesStorage.Add(r.Key, r.Value);
            }
        }
        
        public void Save()
        {
            var data = new PlayerData();
            SaveManager.Save(FileName, data);
        }
        
        private PlayerData Load()
        {
            var data = new PlayerData();
            if (UseTest)
            {
                data.QuestsCompleted = TestQuestsCompleted;
                data.Purchases = Purchases;
                data.Resources = new();
                foreach (var r in Resources)
                {
                    data.Resources.Add(r.Key, r.Amount);
                }
            }
            else
            {
                data.Purchases = new();
                data.Resources = new();
            }
            return data;
        }
    }
}
