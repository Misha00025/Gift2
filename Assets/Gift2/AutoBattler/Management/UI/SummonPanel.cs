using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonPanel : MonoBehaviour
{
    public Image IconImage;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI DescriptionText;
    
    public Button MoveToLeftButton;
    public Button MoveToRightButton;
    
    public CharacterConfig Current {get; private set;}

    public void SetupCharacter(CharacterConfig config)
    {
        IconImage.sprite = config.Icon;
        NameText.SetText(config.Name);
        DescriptionText?.SetText(config.Description);
        Current = config;
    }
}
