using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField]
    private SkillScript skill;

    [SerializeField]
    TextAsset skillFile;

    [SerializeField]
    TextAsset weaponFile;

    [SerializeField]
    TextAsset armorFile;

    [SerializeField]
    TextAsset accFile;

    [SerializeField]
    string[] skillLines;

    [SerializeField]
    string[] weaponLines;

    [SerializeField]
    string[] armorLines;

    [SerializeField]
    string[] accLines;
    void Start()
    {
        string file = skillFile.text;
        skillLines = file.Split('\n');

        file = weaponFile.text;
        weaponLines = file.Split('\n');

        file = armorFile.text;
        armorLines = file.Split('\n');

        file = accFile.text;
        accLines = file.Split('\n');
    }
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

    public void LearnSkill(int id, LivingObject livingObject, bool equip = false)
    {
        if (skill == null)
        {
            skill = ScriptableObject.CreateInstance<SkillScript>();
        }

        //  test =  Resources.Load("skills.csv") as TextAsset;



        int total = skillLines.Length;
        int currIndex = 1;
        // string path =  "Assets/Resources/skills.csv";

        if (livingObject.GetComponent<InventoryScript>())
        {

            //Read the text from directly from the test.txt file
            //  StreamReader reader = new StreamReader(path);

            string lines = "";
            // reader.ReadLine();
            while (currIndex < total)
            {
                lines = skillLines[currIndex];
                currIndex++;

                if (lines != null)
                {
                    if (lines == "")
                    {
                        continue;
                    }
                    if (lines[0] == '-')
                    {
                        continue;
                    }
                    string[] parsed = lines.Split(',');
                    if (Int32.Parse(parsed[0]) == id)
                    {
                        skill.OWNER = livingObject;
                        skill.NAME = parsed[1];
                        skill.DESC = parsed[2];
                        skill.ELEMENT = (Element)Enum.Parse(typeof(Element), parsed[3]);
                        //Debug.Log(id + " " + skill.NAME + " " +skill.ELEMENT);
                        skill.TYPE = 4;


                        if (skill.ELEMENT == Element.Auto)
                        {
                            AutoSkill auto = ScriptableObject.CreateInstance<AutoSkill>();
                            skill.Transfer(auto);

                            auto.CHANCE = (float)Double.Parse(parsed[4]);
                            auto.ACT = (AutoAct)Enum.Parse(typeof(AutoAct), parsed[5]);
                            auto.REACT = (AutoReact)Enum.Parse(typeof(AutoReact), parsed[6]);

                            auto.NEXT = Int32.Parse(parsed[7]);
                            auto.NEXTCOUNT = Int32.Parse(parsed[8]);

                            livingObject.GetComponent<InventoryScript>().USEABLES.Add(auto);
                            livingObject.GetComponent<InventoryScript>().AUTOS.Add(auto);
                            livingObject.GetComponent<InventoryScript>().SKILLS.Add(auto);
                            if (equip == true)
                            {
                                if (livingObject.AUTO_SLOTS.CanAdd())
                                    livingObject.AUTO_SLOTS.SKILLS.Add(auto);
                            }
                        }
                        else if (skill.ELEMENT == Element.Opp)
                        {
                            OppSkill opp = ScriptableObject.CreateInstance<OppSkill>();
                            skill.Transfer(opp);
                            opp.TRIGGER = (Element)Enum.Parse(typeof(Element), parsed[4]);
                            opp.MOD = (float)Double.Parse(parsed[5]);

                            livingObject.GetComponent<InventoryScript>().USEABLES.Add(opp);
                            livingObject.GetComponent<InventoryScript>().OPPS.Add(opp);
                            livingObject.GetComponent<InventoryScript>().SKILLS.Add(opp);
                            if (equip == true)
                            {
                                if (livingObject.OPP_SLOTS.CanAdd())
                                    livingObject.OPP_SLOTS.SKILLS.Add(opp);
                            }
                        }
                        else if (skill.ELEMENT == Element.Buff)
                        {
                            int index = 15;
                            int count = Int32.Parse(parsed[14]);

                            CommandSkill buff = ScriptableObject.CreateInstance<CommandSkill>();
                            skill.Transfer(buff);
                            buff.NEXT = Int32.Parse(parsed[8]);
                            buff.NEXTCOUNT = Int32.Parse(parsed[9]);
                            buff.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                            buff.DAMAGE = (DMG)Enum.Parse(typeof(DMG), parsed[11]);
                            buff.HITS = Int32.Parse(parsed[12]);
                            buff.BUFF = (BuffType)Enum.Parse(typeof(BuffType), parsed[13]);
                            buff.TILES = new System.Collections.Generic.List<Vector2>();

                            Modification mod = new Modification();
                            switch (buff.BUFF)
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
                            mod.editValue = (float)Double.Parse(parsed[10]) * buff.ACCURACY;

                            buff.BUFFEDSTAT = mod.affectedStat;
                            buff.BUFFVAL = mod.editValue;

                            for (int i = 0; i < count; i++)
                            {
                                Vector2 v = new Vector2();
                                v.x = Int32.Parse(parsed[index]);
                                index++;
                                v.y = Int32.Parse(parsed[index]);
                                index++;
                                buff.TILES.Add(v);
                            }
                            livingObject.GetComponent<InventoryScript>().CSKILLS.Add(buff);
                            livingObject.GetComponent<InventoryScript>().USEABLES.Add(buff);
                            livingObject.GetComponent<InventoryScript>().SKILLS.Add(buff);
                            if (equip == true)
                            {
                                if (livingObject.BATTLE_SLOTS.CanAdd())
                                    livingObject.BATTLE_SLOTS.SKILLS.Add(buff);
                            }
                        }
                        else if (skill.ELEMENT == Element.Passive)
                        {
                            int index = 9;
                            int count = Int32.Parse(parsed[8]);

                            PassiveSkill passive = ScriptableObject.CreateInstance<PassiveSkill>();
                            skill.Transfer(passive);
                            passive.ModElements = new List<Element>();
                            passive.PERCENT = (float)Double.Parse(parsed[4]);
                            passive.ModValues = new List<float>();
                            if (count > 0)
                            {

                                for (int i = 0; i < count; i++)
                                {
                                    Modification mod = new Modification();
                                    mod.affectedStat = (ModifiedStat)Enum.Parse(typeof(ModifiedStat), parsed[7]);
                                    mod.affectedElement = (Element)Enum.Parse(typeof(Element), parsed[index]);
                                    mod.editValue = (float)Double.Parse(parsed[5]) * passive.PERCENT;
                                    index++;
                                    passive.ModStat = mod.affectedStat;
                                    passive.ModValues.Add(mod.editValue);
                                    passive.ModElements.Add(mod.affectedElement);
                                }
                            }
                            else
                            {
                                Modification mod = new Modification();
                                mod.affectedStat = (ModifiedStat)Enum.Parse(typeof(ModifiedStat), parsed[7]);
                                mod.editValue = (float)Double.Parse(parsed[5]) * passive.PERCENT;
                                passive.ModStat = mod.affectedStat;
                                passive.ModValues.Add(mod.editValue);
                                passive.ModElements.Add(mod.affectedElement);
                            }
                            livingObject.GetComponent<InventoryScript>().USEABLES.Add(passive);
                            livingObject.GetComponent<InventoryScript>().PASSIVES.Add(passive);
                            livingObject.GetComponent<InventoryScript>().SKILLS.Add(passive);
                            livingObject.ApplyPassives();

                            if (equip == true)
                            {
                                if (livingObject.PASSIVE_SLOTS.CanAdd())
                                    livingObject.PASSIVE_SLOTS.SKILLS.Add(passive);
                            }
                        }
                        else
                        {
                            int index = 15;
                            int count = Int32.Parse(parsed[14]);
                            CommandSkill command = ScriptableObject.CreateInstance<CommandSkill>();
                            skill.Transfer(command);
                            command.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                            command.NEXT = Int32.Parse(parsed[8]);
                            command.NEXTCOUNT = Int32.Parse(parsed[9]);

                            command.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                            command.COST = Int32.Parse(parsed[5]);
                            command.RTYPE = (RanngeType)Enum.Parse(typeof(RanngeType), parsed[7]);
                            command.ACCURACY = Int32.Parse(parsed[10]);

                            command.DAMAGE = (DMG)Enum.Parse(typeof(DMG), parsed[11]);
                            command.HITS = Int32.Parse(parsed[12]);
                            command.CRIT_RATE = Int32.Parse(parsed[13]);
                            command.TILES = new System.Collections.Generic.List<Vector2>();


                            for (int i = 0; i < count; i++)
                            {
                                Vector2 v = new Vector2();
                                v.x = Int32.Parse(parsed[index]);
                                index++;
                                v.y = Int32.Parse(parsed[index]);
                                index++;
                                command.TILES.Add(v);
                            }
                            command.DESC = "Deals " + skill.ELEMENT + " based " + command.ETYPE + " damage to " + command.TILES.Count + " " + skill.DESC;


                            livingObject.GetComponent<InventoryScript>().CSKILLS.Add(command);
                            livingObject.GetComponent<InventoryScript>().USEABLES.Add(command);
                            livingObject.GetComponent<InventoryScript>().SKILLS.Add(command);
                            if (equip == true)
                            {
                                if (livingObject.BATTLE_SLOTS.CanAdd())
                                    livingObject.BATTLE_SLOTS.SKILLS.Add(command);
                            }
                        }
                        skill.OWNER = livingObject;

                    }
                }


            }

            //reader.Close();
        }
    }

    public void GetWeapon(int id, LivingObject livingObject)
    {
        // string path = "Assets/Resources/weapons.csv";

        int total = weaponLines.Length;
        int currIndex = 1;
        // string path =  "Assets/Resources/skills.csv";

        if (livingObject.GetComponent<InventoryScript>())
        {

            //Read the text from directly from the test.txt file
            //  StreamReader reader = new StreamReader(path);

            string lines = "";
            // reader.ReadLine();
            while (currIndex < total)
            {
                lines = weaponLines[currIndex];
                currIndex++;
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

            //   reader.Close();
        }
    }

    public void GetArmor(int id, LivingObject livingObject)
    {
        //  string path = "Assets/Resources/armor.csv";

        int total = armorLines.Length;
        int currIndex = 1;
        // string path =  "Assets/Resources/skills.csv";

        if (livingObject.GetComponent<InventoryScript>())
        {

            //Read the text from directly from the test.txt file
            //  StreamReader reader = new StreamReader(path);

            string lines = "";
            // reader.ReadLine();
            while (currIndex < total)
            {
                lines = armorLines[currIndex];
                currIndex++;

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

            // reader.Close();
        }
    }
}
