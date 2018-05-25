using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    static void WriteString()
    {
        string path = "Assets/Resources/skills.csv";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Test");
        writer.Close();

    }

    void ReadString()
    {
        //    string path = "Assets/Resources/skills.csv";

        //  //  if(test.GetComponent<InventoryScript>())
        //    {

        //    //Read the text from directly from the test.txt file
        //    StreamReader reader = new StreamReader(path);

        //    string lines = "";
        //    reader.ReadLine();
        //    while (lines != null)
        //    {
        //        lines = reader.ReadLine();
        //        Debug.Log(lines);
        //        if (lines != null)
        //        {
        //            string[] parsed = lines.Split(',');

        //            SkillScript skill = ScriptableObject.CreateInstance<SkillScript>();
        //            skill.NAME = parsed[1];
        //            skill.ELEMENT = (Element)Enum.Parse(typeof(Element), parsed[2]);
        //            skill.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[3]);
        //            skill.DESC = parsed[4];
        //            skill.ACCURACY = Int32.Parse(parsed[5]);
        //            skill.DAMAGE = (DMG)Enum.Parse(typeof(DMG), parsed[6]);
        //                skill.CRIT_RATE = Int32.Parse(parsed[7]);
        //                skill.TILES = new System.Collections.Generic.List<Vector2>();
        //                skill.TYPE = 4;
        //                int count = Int32.Parse(parsed[8]);
        //                int index = 9;

        //                for(int i = 0; i < count; i++)
        //                {
        //                    Vector2 v = new Vector2();
        //                    v.x = Int32.Parse(parsed[index]);
        //                    index++;
        //                    v.y = Int32.Parse(parsed[index]);
        //                    index++;
        //                    skill.TILES.Add(v);
        //                }
        //        }


        //    }

        //    reader.Close();
        //    }
    }

    public void LearnSkill(int id, LivingObject livingObject)
    {
        string path = "Assets/Resources/skills.csv";

        if (livingObject.GetComponent<InventoryScript>())
        {

            //Read the text from directly from the test.txt file
            StreamReader reader = new StreamReader(path);

            string lines = "";
            reader.ReadLine();
            while (lines != null)
            {
                lines = reader.ReadLine();

                if (lines != null)
                {
                    if (lines == "")
                    {
                        continue;
                    }
                    string[] parsed = lines.Split(',');
                    if (Int32.Parse(parsed[0]) == id)
                    {

                        SkillScript skill = ScriptableObject.CreateInstance<SkillScript>();
                        skill.NAME = parsed[1];
                        skill.ELEMENT = (Element)Enum.Parse(typeof(Element), parsed[2]);
                        skill.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[3]);
                        skill.RTYPE = (RanngeType)Enum.Parse(typeof(RanngeType), parsed[5]);
                        skill.NEXT = Int32.Parse(parsed[6]);
                        skill.NEXTCOUNT = Int32.Parse(parsed[7]);
                        skill.ACCURACY = Int32.Parse(parsed[8]);
                        int index = 13;
                        skill.TYPE = 4;
                        int count = Int32.Parse(parsed[12]);
                        if (skill.ELEMENT == Element.Buff)
                        {
                            skill.ModValues = new List<float>();
                            skill.DESC = parsed[4];
                            skill.DAMAGE = (DMG)Enum.Parse(typeof(DMG), parsed[9]);
                            skill.HITS = Int32.Parse(parsed[10]);
                            skill.Buff = (BuffType)Enum.Parse(typeof(BuffType), parsed[11]);
                            skill.TILES = new System.Collections.Generic.List<Vector2>();

                            Modification mod = new Modification();
                            switch (skill.Buff)
                            {
                                case BuffType.attack:
                                    mod.affectedStat = ModifiedStat.Atk;
                                    break;
                                case BuffType.speed:
                                    mod.affectedStat = ModifiedStat.Speed;
                                    break;
                                case BuffType.defense:
                                    mod.affectedStat = ModifiedStat.Def;
                                    break;
                                case BuffType.resistance:
                                    mod.affectedStat = ModifiedStat.Res;
                                    break;
                                case BuffType.luck:
                                    mod.affectedStat = ModifiedStat.Luck;
                                    break;
                            }
                            mod.editValue = (float)Double.Parse(parsed[9]) * skill.ACCURACY;

                            skill.ModStat = mod.affectedStat;
                            skill.ModValues.Add(mod.editValue);

                            for (int i = 0; i < count; i++)
                            {
                                Vector2 v = new Vector2();
                                v.x = Int32.Parse(parsed[index]);
                                index++;
                                v.y = Int32.Parse(parsed[index]);
                                index++;
                                skill.TILES.Add(v);
                            }
                        }
                        else if (skill.ELEMENT == Element.Passive)
                        {
                            skill.DESC = parsed[4];
                            skill.ModElements = new List<Element>();

                            skill.ModValues = new List<float>();
                            if (count > 0)
                            {

                                for (int i = 0; i < count; i++)
                                {
                                    Modification mod = new Modification();
                                    mod.affectedStat = (ModifiedStat)Enum.Parse(typeof(ModifiedStat), parsed[11]);
                                    mod.affectedElement = (Element)Enum.Parse(typeof(Element), parsed[index]);
                                    mod.editValue = (float)Double.Parse(parsed[9]) * skill.ACCURACY;
                                    index++;
                                    skill.ModStat = mod.affectedStat;
                                    skill.ModValues.Add(mod.editValue);
                                    skill.ModElements.Add(mod.affectedElement);
                                }
                            }
                            else
                            {
                                Modification mod = new Modification();
                                mod.affectedStat = (ModifiedStat)Enum.Parse(typeof(ModifiedStat), parsed[11]);
                                mod.editValue = (float)Double.Parse(parsed[9]) * skill.ACCURACY;
                                skill.ModStat = mod.affectedStat;
                                skill.ModValues.Add(mod.editValue);
                                skill.ModElements.Add(mod.affectedElement);
                            }
                        }
                        else
                        {
                            skill.DAMAGE = (DMG)Enum.Parse(typeof(DMG), parsed[9]);
                            skill.HITS = Int32.Parse(parsed[10]);
                            skill.CRIT_RATE = Int32.Parse(parsed[11]);
                            skill.TILES = new System.Collections.Generic.List<Vector2>();


                            for (int i = 0; i < count; i++)
                            {
                                Vector2 v = new Vector2();
                                v.x = Int32.Parse(parsed[index]);
                                index++;
                                v.y = Int32.Parse(parsed[index]);
                                index++;
                                skill.TILES.Add(v);
                            }

                            skill.DESC = "Deals " + skill.ELEMENT + " based " + skill.ETYPE + " damage to " + skill.TILES.Count + " " + parsed[4];

                        }
                        skill.OWNER = livingObject;
                        if (skill.ELEMENT == Element.Passive)
                        {
                            livingObject.GetComponent<InventoryScript>().PASSIVES.Add(skill);
                            livingObject.ApplyPassives();

                        }
                        else
                        {
                            livingObject.GetComponent<InventoryScript>().SKILLS.Add(skill);
                            livingObject.GetComponent<InventoryScript>().USEABLES.Add(skill);

                        }
                    }
                }


            }

            reader.Close();
        }
    }

    public void GetWeapon(int id, LivingObject livingObject)
    {
        string path = "Assets/Resources/weapons.csv";

        if (livingObject.GetComponent<InventoryScript>())
        {

            //Read the text from directly from the test.txt file
            StreamReader reader = new StreamReader(path);

            string lines = "";
            reader.ReadLine();
            while (lines != null)
            {
                lines = reader.ReadLine();

                if (lines != null)
                {
                    string[] parsed = lines.Split(',');
                    if (Int32.Parse(parsed[0]) == id)
                    {

                        WeaponScript weapon = ScriptableObject.CreateInstance<WeaponScript>();
                        weapon.NAME = parsed[1];
                        weapon.DESC = parsed[2];
                        weapon.ATTACK = Int32.Parse(parsed[3]);
                        weapon.ATTACK_TYPE = (EType)Enum.Parse(typeof(EType), parsed[4]);
                        weapon.AFINITY = (Element)Enum.Parse(typeof(Element), parsed[5]);
                        weapon.DIST = Int32.Parse(parsed[6]);
                        weapon.Range = Int32.Parse(parsed[7]);
                        weapon.ACCURACY = Int32.Parse(parsed[8]);
                        weapon.LUCK = Int32.Parse(parsed[9]);
                        weapon.TYPE = 0;

                        livingObject.GetComponent<InventoryScript>().WEAPONS.Add(weapon);
                        livingObject.GetComponent<InventoryScript>().USEABLES.Add(weapon);
                    }
                }


            }

            reader.Close();
        }
    }

    public void GetArmor(int id, LivingObject livingObject)
    {
        string path = "Assets/Resources/armor.csv";

        if (livingObject.GetComponent<InventoryScript>())
        {

            //Read the text from directly from the test.txt file
            StreamReader reader = new StreamReader(path);

            string lines = "";
            reader.ReadLine();
            while (lines != null)
            {
                lines = reader.ReadLine();

                if (lines != null)
                {
                    string[] parsed = lines.Split(',');
                    if (Int32.Parse(parsed[0]) == id)
                    {

                        ArmorScript armor = ScriptableObject.CreateInstance<ArmorScript>();
                        armor.NAME = parsed[1];
                        armor.DESC = parsed[2];
                        armor.DEFENSE = Int32.Parse(parsed[3]);
                        armor.RESISTANCE = Int32.Parse(parsed[4]);
                        armor.SPEED = Int32.Parse(parsed[5]);
                        armor.TYPE = 1;
                        List<EHitType> list = new List<EHitType>();
                        list.Add((EHitType)Enum.Parse(typeof(EHitType), parsed[6]));
                        list.Add((EHitType)Enum.Parse(typeof(EHitType), parsed[7]));
                        list.Add((EHitType)Enum.Parse(typeof(EHitType), parsed[8]));
                        list.Add((EHitType)Enum.Parse(typeof(EHitType), parsed[9]));
                        list.Add((EHitType)Enum.Parse(typeof(EHitType), parsed[10]));
                        list.Add((EHitType)Enum.Parse(typeof(EHitType), parsed[11]));
                        list.Add((EHitType)Enum.Parse(typeof(EHitType), parsed[12]));


                        armor.HITLIST = list;


                        livingObject.GetComponent<InventoryScript>().ARMOR.Add(armor);
                        livingObject.GetComponent<InventoryScript>().USEABLES.Add(armor);
                    }
                }


            }

            reader.Close();
        }
    }
}
