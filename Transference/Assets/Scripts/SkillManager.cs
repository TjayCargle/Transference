
using UnityEngine;

public class SkillManager : MonoBehaviour {
   static string[] skillName = { "water attack", "fire attack", "Ice Attack", "Electric Attack", "Phys Attack","Neutral Attack"};
    static int[] skillAccuraccy = { 100, 100, 100, 100, 90, 90, 90, 100 };
    static int[] skillDmg = { 25, 25, 25, 25, 30, 30, 30, 20 };
    static float[] skillCritRate = { 10.0f, 10.0f, 10.0f, 10.0f, 20.0f, 20.0f, 20.0f, 0.5f };
    static Element[] skillAffinity = { Element.Water, Element.Fire, Element.Ice, Element.Electric, Element.Phys, Element.Neutral };

   public static SkillScript CreateSkill(int index)
    {
        SkillScript newSkill = new SkillScript();
        newSkill.name = skillName[index];
        newSkill.accuraccy = skillAccuraccy[index];
        newSkill.dmg = skillDmg[index];
        newSkill.critRate = skillCritRate[index];
        newSkill.affinity = skillAffinity[index];
        return newSkill;
    }
}
