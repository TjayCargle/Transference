﻿
using UnityEngine;

public class SkillManager : MonoBehaviour {
  // static string[] skillName = { "water attack", "pyro attack", "Ice Attack", "Electric Attack", "Phys Attack","Neutral Attack"};
  //  static int[] skillAccuraccy = { 100, 100, 100, 100, 90, 90, 90, 100 };
  //  static int[] skillDmg = { 25, 25, 25, 25, 30, 30, 30, 20 };
 //   static float[] skillCritRate = { 10.0f, 10.0f, 10.0f, 10.0f, 20.0f, 20.0f, 20.0f, 0.5f };
  //  static Element[] skillAffinity = { Element.Water, Element.Pyro, Element.Ice, Element.Electric, Element.Phys, Element.Neutral };

   public static SkillScript CreateSkill(LivingObject invokingObject, int index)
    {
        SkillScript newSkill = new SkillScript();//invokingObject.gameObject.AddComponent<SkillScript>();

        return newSkill;
    }
}
