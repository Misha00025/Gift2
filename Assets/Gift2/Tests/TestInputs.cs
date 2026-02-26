using System;
using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;

public class TestInputs : MonoBehaviour
{
    [Serializable]
    public struct KeyItem
    {
        public KeyCode Key;
        public ItemConfig Item;
    }

    public Player Player;
    public CharacterMover CharacterMover;
    public List<KeyItem> Resources;
    public int Count = 100;
    
    public CollectableItem ItemToSpawn;
    public Transform DraggableObject;
    
    private Item _currentResource;
    
    void Start()
    {
        var collector = FindAnyObjectByType<ItemsCollector>();
        collector?.SetStorage(Player.ResourcesStorage);
    }
    
    void Update()
    {
        foreach (var keyItem in Resources)
            if (Input.GetKeyDown(keyItem.Key))
                _currentResource = keyItem.Item.Build();

            
        if (Input.GetKeyDown(KeyCode.Q))
            Player.ResourcesStorage.Add(_currentResource, Count);
        if (Input.GetKeyDown(KeyCode.E))
            Player.ResourcesStorage.Remove(_currentResource, Count);
            
        if (Input.GetKey(KeyCode.Space))
            TrySpawn();
        if (Input.GetKey(KeyCode.LeftShift))
            DragObject();
            
        HandleMoving();
    }
    
    private void HandleMoving()
    {
        var xDirection = Input.GetAxisRaw("Horizontal");
        var yDirection = Input.GetAxisRaw("Vertical");
        var direction = new Vector2(xDirection, yDirection);
        
        CharacterMover.Move(direction);
    }
    
    private void DragObject()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        DraggableObject.transform.position = position;
    }
    
    private void TrySpawn()
    {
        var item = Instantiate(ItemToSpawn);
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        item.transform.position = position;
    }
}
