using UnityEngine;

public interface ISkill
{
    public void Play();
}

public interface ITargetSkill
{
    public bool IsValidTarget(Character target);
    public void SetTarget(Character target);        
}

public abstract class Skill : MonoBehaviour, ISkill
{
    public Character Caster;
    public abstract void Play();
}

public struct SkillInfo 
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Sprite Icon { get; set; }
}

[CreateAssetMenu(fileName = "SkillConfig", menuName = "Skills/SkillConfig")]
public class SkillConfiguredBuilder : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public Skill SkillPrefab;
    
    public SkillInfo GetInfo()
    {
        var skill = new SkillInfo();
        skill.Name = Name;
        skill.Description = Description;
        skill.Icon = Icon;
        return skill;
    }
    
    public ISkill Build(Character caster)
    {
        var skill = ConfigureSkill(caster);
        skill.Caster = caster;
        return skill;
    }
    
    protected Skill ConfigureSkill(Character caster)
    {
        return GameObject.Instantiate(SkillPrefab, caster.transform);
    }
}

