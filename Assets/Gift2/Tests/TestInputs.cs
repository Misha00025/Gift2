using System;
using System.Collections.Generic;
using Gift2;
using Gift2.Core;
using HeneGames.DialogueSystem;
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
    public QuestDialer QuestDialer;
    public CharacterMover CharacterMover;
    public List<KeyItem> Resources;
    public Joystick Joystick;
    private Interactor _interactor;
    
    private bool BlockReadInputs => DialogueUI.instance.gameObject.activeSelf;
    
    void Start()
    {
        var collector = FindAnyObjectByType<ItemsCollector>();
        collector?.SetStorage(Player.ResourcesStorage);
        _interactor = FindAnyObjectByType<Interactor>();
    }
    
    void Update()
    {
        if (BlockReadInputs) return;

        // if (Input.GetKeyDown(KeyCode.Space))
        //     Use();
            
        HandleMoving();
    }
    
    private void HandleMoving()
    {
        if (BlockReadInputs) return;
        
        var xDirection = 0f; // Input.GetAxisRaw("Horizontal");
        var yDirection = 0f; // Input.GetAxisRaw("Vertical");
        if (Joystick != null)
        {
            xDirection += Joystick.Horizontal;
            yDirection += Joystick.Vertical;
        }
        var direction = new Vector2(xDirection, yDirection);
        
        CharacterMover.Move(direction);
    }
    
    public void Use()
    {
        if (DialogueUI.instance.gameObject.activeSelf)
        {
            DialogueUI.instance.NextSentenceSoft();
            return;
        }
    
        if (BlockReadInputs) return;
        
        var obj = _interactor.GetSelectedObject();
        
        obj?.Use();
    }
}
