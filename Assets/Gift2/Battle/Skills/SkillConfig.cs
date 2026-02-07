using UnityEngine;

[CreateAssetMenu(fileName = "SkillConfig", menuName = "Skills/SkillConfig")]
public class SkillConfig : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    
    public SkillInfo GetInfo()
    {
        var skill = new SkillInfo();
        skill.Name = Name;
        skill.Description = Description;
        skill.Icon = Icon;
        return skill;
    }
}
