using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SummonsSelector : MonoBehaviour
{
    public List<CharacterConfig> AvailableCharacters = new();
    
    [Header("Panels")]
    public SummonPanel MainSummonPanel;
    public SummonPanel LeftSupportSummonPanel;
    public SummonPanel RightSupportSummonPanel;
    
    void Awake()
    {
        if (AvailableCharacters.Count == 0) return;
        
        SetupPanel(MainSummonPanel);
        SetupPanel(LeftSupportSummonPanel);
        SetupPanel(RightSupportSummonPanel);
    }
    
    private void SetupPanel(SummonPanel panel)
    {
        panel.SetupCharacter(AvailableCharacters[0]);
        panel.MoveToLeftButton?.onClick.AddListener(() => MoveLeft(panel));
        panel.MoveToRightButton?.onClick.AddListener(() => MoveRight(panel));
    }
    
    private void MoveLeft(SummonPanel panel)
    {
        var index = AvailableCharacters.FindIndex(e => e == panel.Current);
        if (index == -1) return;
        
        if (index == 0)
        {
            panel.SetupCharacter(AvailableCharacters[AvailableCharacters.Count -1]);
        }
        else
        {
            panel.SetupCharacter(AvailableCharacters[index-1]);
        }
    }
    
    private void MoveRight(SummonPanel panel)
    {
        var index = AvailableCharacters.FindIndex(e => e == panel.Current);
        if (index == -1) return;
        
        if (index == (AvailableCharacters.Count - 1))
        {
            panel.SetupCharacter(AvailableCharacters[0]);
        }
        else
        {
            panel.SetupCharacter(AvailableCharacters[index+1]);
        }
    }
    
    public void SubmitSummons()
    {
        var supports = new List<CharacterConfig>() {LeftSupportSummonPanel.Current, RightSupportSummonPanel.Current};
        BattleInitializer.StartupData.RegisterData(MainSummonPanel.Current, supports);
        SceneManager.LoadScene("BattleScene");
    }
}
