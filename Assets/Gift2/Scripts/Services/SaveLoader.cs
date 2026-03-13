using System;
using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Gift2
{
    public class SaveLoader : MonoBehaviour
    {
        [Serializable]
        public struct Purchase {public string Shop; public int Slot; public int Amount;}
        [Serializable]
        public struct Resource {public string Key; public int Amount;}
        
    
        public string FileName = "gift2.save";
        public Player Player;
        
        public List<InteractableQuestDealer> Dealers = new();
        
        public ShopView SpeedShop;
        public ShopView GrowShop;
        public ShopView WeaponShop;
        public List<ShopView> Shops => new() {SpeedShop, GrowShop, WeaponShop};
        
        public UnityEvent OnFirstLoad = new();
        
        [Header("Test")]
        public bool UseTest = false;
        public int TestQuestsCompleted = 3;
        [Serializable]
        public struct TestResource {public string Key; public int Amount;}
        public List<Purchase> Purchases = new();
        public List<TestResource> Resources = new();
        
        private struct PlayerData 
        {
            public bool QuestInProgress;
            public int QuestsCompleted;
            public Vector2 Position;
            public List<Purchase> Purchases;
            public List<Resource> Resources;            
        }
        
        
        
        void Start()
        {
            var data = Load(out var firstLoad);
            if (firstLoad)
                OnFirstLoad.Invoke();
            else
                LoadFromSave(data);
            PostProcess();
        }
        
        private void PostProcess()
        {
            foreach (var shop in Shops)
                shop.CloseShop();
            
        }
        
        private void LoadFromSave(PlayerData data)
        {
            for (int i = 0; i < data.QuestsCompleted; i++)
            {
                if (Dealers.Count > i)
                {
                    var quest = Dealers[i].SilenceAcceptQuest();
                    quest.Complete();
                }
            }
            if (Dealers.Count > data.QuestsCompleted && data.QuestInProgress)
            {
                Dealers[data.QuestsCompleted].SilenceAcceptQuest();
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
            }
            
            foreach (var r in data.Resources)
            {
                Player.ResourcesStorage.Add(r.Key, r.Amount);
            }
            
            Player.Character.transform.position = data.Position;
        }
        
        public void Save()
        {
            var data = new PlayerData();
            data.Purchases = new();
            data.Resources = new();
            
            data.Position = Player.Character.transform.position;
            
            if (Player.QuestsManager.Quests.Count > 0)
                data.QuestInProgress = true;
            
            foreach (var dealer in Dealers)
            {
                if (dealer.Dialer.Completed)
                    data.QuestsCompleted += 1;
            }
            
            foreach (var shop in Shops)
            {
                var c = shop.ShopController;
                for (var i = 0; i < c.Slots.Count; i++)
                {
                    var slot = c.Slots[i];
                    var purchase = new Purchase();
                    purchase.Shop = shop.name;
                    purchase.Slot = i;
                    purchase.Amount = slot.CurrentBuy;
                    data.Purchases.Add(purchase);
                }
            }
            
            foreach (var resource in Player.Resources)
            {
                data.Resources.Add(new(){Key=resource.Item.Key, Amount=resource.Amount});
            }
            
            SaveManager.Save(FileName, data);
        }
        
        private PlayerData Load() => Load(out var _);
        private PlayerData Load(out bool firstLoad)
        {
            var data = new PlayerData();
            firstLoad = true;
            if (UseTest)
            {
                firstLoad = false;
                data.QuestsCompleted = TestQuestsCompleted;
                data.Purchases = Purchases;
                data.Resources = new();
                foreach (var r in Resources)
                {
                    data.Resources.Add(new(){Key=r.Key, Amount=r.Amount});
                }
            }
            else
            {
                var ok = SaveManager.Load(FileName, out data);
                firstLoad = !ok;
                if (data.Purchases == null)
                    data.Purchases = new();
                if (data.Resources == null)
                    data.Resources = new();
            }
            return data;
        }
        
        [ContextMenu("Clear Save")]
        public void Clear()
        {
            SaveManager.Clear(FileName);
        }
    }
}
