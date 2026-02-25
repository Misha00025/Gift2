using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillUICard : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI Text;
    public Button Button;
    
    private SkillInfo info;

    public void SetupSkill(SkillInfo skill)
    {
        info = skill;
        Image.sprite = skill.Icon;
        Text.SetText(skill.Name);
    }
}
