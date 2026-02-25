using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;

public class TestInputs : MonoBehaviour
{
    public Player Player;
    public CharacterMover CharacterMover;
    public List<ItemConfig> Resources;
    public int Count = 100;
    
    public CollectableItem ItemToSpawn;
    public Transform DraggableObject;
    
    private Item _currentResource;
    
    void Start()
    {
        var collector = FindAnyObjectByType<ItemsCollector>();
        collector?.Initialize(Player.ResourcesStorage);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _currentResource = Resources[0].Build();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _currentResource = Resources[1].Build();
            
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
