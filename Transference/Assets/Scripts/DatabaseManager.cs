using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    private SkillScript skill;

    [SerializeField]
    TextAsset skillFile;

    [SerializeField]
    TextAsset weaponFile;

    [SerializeField]
    TextAsset armorFile;

    [SerializeField]
    TextAsset enemyFile;

    [SerializeField]
    TextAsset itemFile;

    [SerializeField]
    TextAsset actorFile;

    [SerializeField]
    TextAsset hazardFile;


    [SerializeField]
    TextAsset mapFile;

    [SerializeField]
    string[] skillLines;

    [SerializeField]
    string[] weaponLines;

    [SerializeField]
    string[] armorLines;

    [SerializeField]
    string[] enemyLines;

    [SerializeField]
    string[] itemLines;

    [SerializeField]
    string[] actorLines;

    [SerializeField]
    string[] hazardLines;

    [SerializeField]
    string[] mapLines;

    private Dictionary<int, string> skillDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> weaponDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> armorDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> itemDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> enemyDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> actorDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> hazardDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> mapDictionary = new Dictionary<int, string>();
    public bool isSetup = false;
    public void Setup()
    {
        if (!isSetup)
        {

            string file = skillFile.text;
            skillLines = file.Split('\n');
            if (skillDictionary.Count == 0)
            {

                for (int i = 0; i < skillLines.Length; i++)
                {
                    string line = skillLines[i];
                    if (line[0] != '-')
                    {
                        string[] parsed = line.Split(',');

                        skillDictionary.Add(Int32.Parse(parsed[0]), skillLines[i]);
                    }
                }
            }

            file = weaponFile.text;
            weaponLines = file.Split('\n');
            if (weaponDictionary.Count == 0)
            {
                for (int i = 1; i < weaponLines.Length; i++)
                {
                    string line = weaponLines[i];
                    if (line[0] != '-')
                    {
                        string[] parsed = line.Split(',');
                        weaponDictionary.Add(Int32.Parse(parsed[0]), line);
                    }
                }
            }
            file = armorFile.text;
            armorLines = file.Split('\n');
            if (armorDictionary.Count == 0)
            {

                for (int i = 1; i < armorLines.Length; i++)
                {
                    string line = armorLines[i];
                    if (line[0] != '-')
                    {
                        string[] parsed = line.Split(',');
                        armorDictionary.Add(Int32.Parse(parsed[0]), line);
                    }
                }
            }

            file = enemyFile.text;
            enemyLines = file.Split('\n');
            if (enemyDictionary.Count == 0)
            {
                for (int i = 1; i < enemyLines.Length; i++)
                {
                    string line = enemyLines[i];
                    if (line[0] != '-')
                    {
                        string[] parsed = line.Split(',');
                        enemyDictionary.Add(Int32.Parse(parsed[0]), line);
                    }
                }
            }

            if (itemDictionary.Count == 0)
            {
                file = itemFile.text;
                itemLines = file.Split('\n');

                for (int i = 1; i < itemLines.Length; i++)
                {
                    string line = itemLines[i];
                    if (line[0] != '-')
                    {
                        string[] parsed = line.Split(',');
                        itemDictionary.Add(Int32.Parse(parsed[0]), line);
                    }
                }
            }

            if (actorDictionary.Count == 0)
            {
                if (actorFile)
                {

                    file = actorFile.text;
                    actorLines = file.Split('\n');

                    for (int i = 1; i < actorLines.Length; i++)
                    {
                        string line = actorLines[i];
                        if (line[0] != '-')
                        {
                            string[] parsed = line.Split(',');
                            actorDictionary.Add(Int32.Parse(parsed[0]), line);
                        }
                    }
                }
            }

            if (hazardDictionary.Count == 0)
            {
                if (hazardFile)
                {

                    file = hazardFile.text;
                    hazardLines = file.Split('\n');

                    for (int i = 1; i < hazardLines.Length; i++)
                    {
                        string line = hazardLines[i];
                        if (line[0] != '-')
                        {
                            string[] parsed = line.Split(',');
                            hazardDictionary.Add(Int32.Parse(parsed[0]), line);
                        }
                    }
                }
            }

            if (mapDictionary.Count == 0)
            {
                if (mapFile)
                {

                    file = mapFile.text;
                    mapLines = file.Split('\n');

                    for (int i = 1; i < mapLines.Length; i++)
                    {
                        string line = mapLines[i];
                        if (line[0] != '-')
                        {
                            string[] parsed = line.Split(',');
                            mapDictionary.Add(Int32.Parse(parsed[0]), line);
                        }
                    }
                }
            }
            isSetup = true;
        }
    }
    public void Start()
    {
        Setup();
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


    public SkillScript LearnSkill(int id, LivingObject livingObject, bool equip = false)
    {
        if (skill == null)
        {
            skill = ScriptableObject.CreateInstance<SkillScript>();
        }

        if (livingObject.GetComponent<InventoryScript>())
        {
            string lines = "";
            if (skillDictionary.TryGetValue(id, out lines))
            {
                string[] parsed = lines.Split(',');
                if (Int32.Parse(parsed[0]) == id)
                {
                    skill.OWNER = livingObject;
                    skill.INDEX = id;
                    skill.OWNER = livingObject;
                    skill.NAME = parsed[1];
                    //   skill.DESC = parsed[4];
                    skill.ELEMENT = (Element)Enum.Parse(typeof(Element), parsed[2]);
                    skill.SUBTYPE = (SubSkillType)Enum.Parse(typeof(SubSkillType), parsed[3]);
                
                    //Debug.Log(id + " " + skill.NAME + " " +skill.ELEMENT);
                    skill.TYPE = 4;
                    if (!livingObject.INVENTORY.ContainsSkillName(skill.NAME))
                    {
                        switch (skill.ELEMENT)
                        {

                            case Element.Buff:
                                {

                                    int index = 14;
                                    int count = Int32.Parse(parsed[13]);
                                    CommandSkill buff = ScriptableObject.CreateInstance<CommandSkill>();
                                    skill.Transfer(buff);
                                    //buff.FRIEND = Int32.Parse(parsed[1]);
                                    //buff.FRIEND_NEXT = Int32.Parse(parsed[2]);
                                    buff.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                                    buff.COST = Int32.Parse(parsed[5]);
                                    buff.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                                    buff.RTYPE = (RanngeType)Enum.Parse(typeof(RanngeType), parsed[7]);

                                    //buff.NEXT = Int32.Parse(parsed[10]);
                                    buff.ACCURACY = 100;
                                    //if (buff.NEXT >= 0)
                                    //{
                                    //    buff.NEXT = buff.INDEX + 1;
                                    //}
                                    //buff.NEXTCOUNT = Int32.Parse(parsed[11]);
                                    //if (buff.NEXTCOUNT > 0)
                                    //{
                                    //    buff.NEXTCOUNT = 2;
                                    //}
                                    // buff.BUFFVAL = (float)Double.Parse(parsed[11]);
                                    buff.HITS = Int32.Parse(parsed[11]);
                                    buff.BUFF = (BuffType)Enum.Parse(typeof(BuffType), parsed[12]);
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
                                        case BuffType.skill:
                                            mod.affectedStat = ModifiedStat.Skill;
                                            break;
                                        case BuffType.none:
                                            break;
                                        case BuffType.str:
                                            mod.affectedStat = ModifiedStat.Str;
                                            break;
                                        case BuffType.mag:
                                            mod.affectedStat = ModifiedStat.Mag;
                                            break;
                                        case BuffType.all:
                                            mod.affectedStat = ModifiedStat.all;
                                            break;
                                    }
                                    mod.editValue = (float)Double.Parse(parsed[10]);

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
                                    if (buff.SUBTYPE == SubSkillType.Buff)
                                    {
                                        buff.DESC = "Increases " + buff.BUFFEDSTAT + " of yourself or ally by " + buff.BUFFVAL + "% for 3 turns";
                                    }
                                    else if (buff.SUBTYPE == SubSkillType.Debuff)
                                    {
                                        buff.DESC = "Decreases " + buff.BUFFEDSTAT + " of enemy by " + buff.BUFFVAL + "% for 3 turns";
                                    }
                                    else
                                    {
                                        buff.DESC = "You shouldnt see this";
                                    }
                                    buff.UpdateDesc();
                                    return buff;
                                }
                                break;
                            case Element.Support:
                                {
                                    int index = 15;
                                    int count = Int32.Parse(parsed[14]);
                                    CommandSkill support = ScriptableObject.CreateInstance<CommandSkill>();
                                    skill.Transfer(support);
                                    support.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                                    support.COST = Int32.Parse(parsed[5]);
                                    support.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                                    support.RTYPE = (RanngeType)Enum.Parse(typeof(RanngeType), parsed[7]);
                                    support.ACCURACY = Int32.Parse(parsed[10]);

                                    support.DAMAGE = (DMG)Enum.Parse(typeof(DMG), parsed[11]);
                                    support.HITS = Int32.Parse(parsed[12]);
                                    support.CRIT_RATE = Int32.Parse(parsed[13]);
                                    support.TILES = new System.Collections.Generic.List<Vector2>();


                                    for (int i = 0; i < count; i++)
                                    {
                                        Vector2 v = new Vector2();
                                        v.x = Int32.Parse(parsed[index]);
                                        index++;
                                        v.y = Int32.Parse(parsed[index]);
                                        index++;
                                        support.TILES.Add(v);
                                    }
                                    switch (support.EFFECT)
                                    {
                                        case SideEffect.heal:
                                            {
                                                support.DESC = "Heals " + support.DAMAGE + " amount of health to target";
                                            }
                                            break;
                                    }

                                    livingObject.GetComponent<InventoryScript>().CSKILLS.Add(support);
                                    livingObject.GetComponent<InventoryScript>().USEABLES.Add(support);
                                    livingObject.GetComponent<InventoryScript>().SKILLS.Add(support);
                                    if (equip == true)
                                    {
                                        if (livingObject.BATTLE_SLOTS.CanAdd())
                                            livingObject.BATTLE_SLOTS.SKILLS.Add(support);
                                    }
                                }
                                break;
                            case Element.Passive:
                                {

                                    int index = 10;
                                    int count = Int32.Parse(parsed[9]);

                                    PassiveSkill passive = ScriptableObject.CreateInstance<PassiveSkill>();
                                    skill.Transfer(passive);
                                    passive.ModElements = new List<Element>();
                                    passive.ModValues = new List<float>();
                                    passive.DESC = parsed[4];
                                    passive.PERCENT = (float)Double.Parse(parsed[5]);
                                    if (count > 0)
                                    {

                                        for (int i = 0; i < count; i++)
                                        {
                                            Modification mod = new Modification();
                                            mod.affectedStat = (ModifiedStat)Enum.Parse(typeof(ModifiedStat), parsed[8]);
                                            mod.affectedElement = (Element)Enum.Parse(typeof(Element), parsed[index]);
                                            mod.editValue = (float)Double.Parse(parsed[6]) * passive.PERCENT;
                                            index++;
                                            passive.ModStat = mod.affectedStat;
                                            passive.ModValues.Add(mod.editValue);
                                            passive.ModElements.Add(mod.affectedElement);
                                        }
                                    }
                                    else
                                    {
                                        Modification mod = new Modification();
                                        mod.affectedStat = (ModifiedStat)Enum.Parse(typeof(ModifiedStat), parsed[8]);
                                        mod.editValue = (float)Double.Parse(parsed[6]) * passive.PERCENT;
                                        passive.ModStat = mod.affectedStat;
                                        passive.ModValues.Add(mod.editValue);
                                        passive.ModElements.Add(mod.affectedElement);
                                    }
                                    livingObject.INVENTORY.USEABLES.Add(passive);
                                    livingObject.INVENTORY.PASSIVES.Add(passive);
                                    livingObject.INVENTORY.SKILLS.Add(passive);


                                    if (equip == true)
                                    {
                                        if (livingObject.PASSIVE_SLOTS.CanAdd())
                                        {
                                            livingObject.PASSIVE_SLOTS.SKILLS.Add(passive);
                                            livingObject.ApplyPassives();
                                        }
                                    }
                                    passive.UpdateDesc();
                                    return passive;
                                }
                                break;
                            case Element.Opp:
                                {

                                    OppSkill opp = ScriptableObject.CreateInstance<OppSkill>();
                                    skill.Transfer(opp);
                                    opp.TRIGGER = (Element)Enum.Parse(typeof(Element), parsed[4]);
                                    opp.MOD = (float)Double.Parse(parsed[5]);
                                    opp.DESC = "Allows a free attack after an ally hits with a " + opp.TRIGGER + " attack.";

                                    livingObject.GetComponent<InventoryScript>().USEABLES.Add(opp);
                                    livingObject.GetComponent<InventoryScript>().OPPS.Add(opp);
                                    livingObject.GetComponent<InventoryScript>().SKILLS.Add(opp);
                                    if (equip == true)
                                    {
                                        if (livingObject.OPP_SLOTS.CanAdd())
                                            livingObject.OPP_SLOTS.SKILLS.Add(opp);
                                    }
                                    return opp;
                                }
                                break;
                            case Element.Ailment:
                                {

                                    int index = 13;
                                    int count = Int32.Parse(parsed[12]);
                                    CommandSkill ailment = ScriptableObject.CreateInstance<CommandSkill>();
                                    skill.Transfer(ailment);

                                    ailment.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                                    ailment.COST = Int32.Parse(parsed[5]);
                                    ailment.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                                    ailment.RTYPE = (RanngeType)Enum.Parse(typeof(RanngeType), parsed[7]);

                                    ailment.ACCURACY = 100;

                                    ailment.HITS = 1;//Int32.Parse(parsed[11]);

                                    ailment.TILES = new System.Collections.Generic.List<Vector2>();



                                    for (int i = 0; i < count; i++)
                                    {
                                        Vector2 v = new Vector2();
                                        v.x = Int32.Parse(parsed[index]);
                                        index++;
                                        v.y = Int32.Parse(parsed[index]);
                                        index++;
                                        ailment.TILES.Add(v);
                                    }
                                    livingObject.GetComponent<InventoryScript>().CSKILLS.Add(ailment);
                                    livingObject.GetComponent<InventoryScript>().USEABLES.Add(ailment);
                                    livingObject.GetComponent<InventoryScript>().SKILLS.Add(ailment);
                                    if (equip == true)
                                    {
                                        if (livingObject.BATTLE_SLOTS.CanAdd())
                                            livingObject.BATTLE_SLOTS.SKILLS.Add(ailment);
                                    }

                                    //ailment.DESC = "Has an " + ailment.ACCURACY + "% chance to inflict enemy with " + ailment.EFFECT;
                                    ailment.UpdateDesc();
                                    return ailment;
                                }
                                break;
                            case Element.Auto:
                                {

                                    AutoSkill auto = ScriptableObject.CreateInstance<AutoSkill>();
                                    skill.Transfer(auto);
                                    auto.DESC = parsed[4];
                                    auto.CHANCE = (float)Double.Parse(parsed[5]);
                                    auto.ACT = (AutoAct)Enum.Parse(typeof(AutoAct), parsed[6]);
                                    auto.REACT = (AutoReact)Enum.Parse(typeof(AutoReact), parsed[7]);

                                    //     auto.NEXT = Int32.Parse(parsed[9]);
                                    //   auto.NEXTCOUNT = Int32.Parse(parsed[10]);
                                    auto.VAL = Int32.Parse(parsed[8]);
                                    auto.OWNER = livingObject;
                                    livingObject.GetComponent<InventoryScript>().USEABLES.Add(auto);
                                    livingObject.GetComponent<InventoryScript>().AUTOS.Add(auto);
                                    livingObject.GetComponent<InventoryScript>().SKILLS.Add(auto);
                                    if (equip == true)
                                    {
                                        if (livingObject.AUTO_SLOTS.CanAdd())
                                            livingObject.AUTO_SLOTS.SKILLS.Add(auto);
                                    }
                                    auto.UpdateDesc();
                                    return auto;
                                }
                                break;
                            case Element.none:
                                break;
                            default:
                                {

                                    {
                                        //command.FRIEND = Int32.Parse(parsed[1]);
                                        //command.FRIEND_NEXT = Int32.Parse(parsed[2]);

                                        //command.NEXT = Int32.Parse(parsed[10]);
                                        //if (command.NEXT >= 0)
                                        //  {
                                        //    command.NEXT = command.INDEX + 1;
                                        //}
                                        // command.NEXTCOUNT = Int32.Parse(parsed[11]);
                                        //if (command.NEXTCOUNT > 0)
                                        //{
                                        //    command.NEXTCOUNT = 2;
                                        //}
                                    }
                                    int index = 15;
                                    int count = Int32.Parse(parsed[14]);
                                    CommandSkill command = ScriptableObject.CreateInstance<CommandSkill>();
                                    skill.Transfer(command);
                                    command.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                                    command.COST = Int32.Parse(parsed[5]);
                                    command.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
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
                                    if (command.SUBTYPE == SubSkillType.RngAtk)
                                    {
                                        command.MIN_HIT = Int32.Parse(parsed[index]);
                                        index++;
                                        command.MAX_HIT = Int32.Parse(parsed[index]);

                                    }
                                    if (command.RTYPE == RanngeType.single)
                                    {
                                        command.DESC = "Deals " + command.DAMAGE + " " + command.ETYPE + "  " + skill.ELEMENT + " damage to";
                                        if (count > 1)
                                        {
                                            command.DESC += " enemies in row";
                                        }
                                        else
                                        {
                                            command.DESC += " a single enemy";
                                        }
                                        if (command.HITS > 1)
                                        {
                                            command.DESC += " " + command.HITS + " times";
                                        }
                                    }
                                    if (command.RTYPE == RanngeType.area)
                                    {
                                        command.DESC = "Deals " + command.DAMAGE + " " + command.ETYPE + "  " + skill.ELEMENT + " damage to all enemies in range";

                                    }
                                    if (command.RTYPE == RanngeType.multi)
                                    {
                                        command.DESC = "Deals " + command.DAMAGE + " " + command.ETYPE + "  " + skill.ELEMENT + " damage to an enemy in range";

                                    }
                                    // if (command.HITS == 1)
                                    //    command.DESC = "Deals " + command.DAMAGE + " " + skill.ELEMENT + " based " + command.ETYPE + " damage to " + command.TILES.Count + " enemy";
                                    //else
                                    //{

                                    //    command.DESC = "Deals " + command.DAMAGE + " " + skill.ELEMENT + " based " + command.ETYPE + " damage to enemy " + command.HITS + " times";

                                    //}
                                    if (command.SUBTYPE == SubSkillType.RngAtk)
                                    {
                                        command.DESC = "Deals " + command.DAMAGE + " " + command.ETYPE + "  " + skill.ELEMENT + " damage to enemy " + command.MIN_HIT + "-" + command.MAX_HIT + " times";
                                    }
                                    if (command.EFFECT != SideEffect.none)
                                    {
                                        if (command.EFFECT < SideEffect.reduceStr)
                                        {
                                            command.DESC += " with a chance of " + command.EFFECT.ToString();
                                        }
                                        else
                                        {
                                            command.DESC += " with a chance to debuff " + Common.GetSideEffectText(command.EFFECT);
                                            command.BUFFEDSTAT = Common.GetSideEffectMod(command.EFFECT);
                                            command.BUFFVAL = -1 * command.CRIT_RATE;
                                            command.EFFECT = SideEffect.debuff;
                                        }
                                    }
                                    command.UpdateDesc();
                                    livingObject.GetComponent<InventoryScript>().CSKILLS.Add(command);
                                    livingObject.GetComponent<InventoryScript>().USEABLES.Add(command);
                                    livingObject.GetComponent<InventoryScript>().SKILLS.Add(command);
                                    if (equip == true)
                                    {
                                        if (livingObject.BATTLE_SLOTS.CanAdd())
                                            livingObject.BATTLE_SLOTS.SKILLS.Add(command);
                                    }
                                    return command;
                                }
                                break;
                        }



                    }
                }



            }

            //reader.Close();
        }
        return skill;
    }
    public SkillScript GetSkill(int id)
    {
        if (skill == null)
        {
            skill = ScriptableObject.CreateInstance<SkillScript>();
        }


        string lines = "";
        if (skillDictionary.TryGetValue(id, out lines))
        {
            string[] parsed = lines.Split(',');
            if (Int32.Parse(parsed[0]) == id)
            {

                skill.INDEX = id;
                skill.AUGMENTS = new List<Augment>();
                skill.NAME = parsed[1];
                //   skill.DESC = parsed[4];
                skill.ELEMENT = (Element)Enum.Parse(typeof(Element), parsed[2]);
                skill.SUBTYPE = (SubSkillType)Enum.Parse(typeof(SubSkillType), parsed[3]);
                //skill.AUGMENTS = ScriptableObject.CreateInstance<AugmentScript>();
                //Debug.Log(id + " " + skill.NAME + " " +skill.ELEMENT);
                skill.TYPE = 4;

                switch (skill.ELEMENT)
                {

                    case Element.Buff:
                        {

                            int index = 14;
                            int count = Int32.Parse(parsed[13]);
                            CommandSkill buff = ScriptableObject.CreateInstance<CommandSkill>();
                            skill.Transfer(buff);

                            buff.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                            buff.COST = Int32.Parse(parsed[5]);
                            buff.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                            buff.RTYPE = (RanngeType)Enum.Parse(typeof(RanngeType), parsed[7]);


                            buff.ACCURACY = 100;

                            buff.HITS = Int32.Parse(parsed[11]);
                            buff.BUFF = (BuffType)Enum.Parse(typeof(BuffType), parsed[12]);
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
                                case BuffType.skill:
                                    mod.affectedStat = ModifiedStat.Skill;
                                    break;
                                case BuffType.none:
                                    break;
                                case BuffType.str:
                                    mod.affectedStat = ModifiedStat.Str;
                                    break;
                                case BuffType.mag:
                                    mod.affectedStat = ModifiedStat.Mag;
                                    break;
                                case BuffType.all:
                                    mod.affectedStat = ModifiedStat.all;
                                    break;
                            }
                            mod.editValue = (float)Double.Parse(parsed[10]);

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

                            buff.UpdateDesc();
                            return buff;
                        }
                        break;
                    case Element.Support:
                        {
                            int index = 15;
                            int count = Int32.Parse(parsed[14]);
                            CommandSkill support = ScriptableObject.CreateInstance<CommandSkill>();
                            skill.Transfer(support);
                            support.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                            support.COST = Int32.Parse(parsed[5]);
                            support.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                            support.RTYPE = (RanngeType)Enum.Parse(typeof(RanngeType), parsed[7]);
                            support.ACCURACY = Int32.Parse(parsed[10]);

                            support.DAMAGE = (DMG)Enum.Parse(typeof(DMG), parsed[11]);
                            support.HITS = Int32.Parse(parsed[12]);
                            support.CRIT_RATE = Int32.Parse(parsed[13]);
                            support.TILES = new System.Collections.Generic.List<Vector2>();


                            for (int i = 0; i < count; i++)
                            {
                                Vector2 v = new Vector2();
                                v.x = Int32.Parse(parsed[index]);
                                index++;
                                v.y = Int32.Parse(parsed[index]);
                                index++;
                                support.TILES.Add(v);
                            }

                            support.UpdateDesc();
                        }
                        break;
                    case Element.Passive:
                        {

                            int index = 10;
                            int count = Int32.Parse(parsed[9]);

                            PassiveSkill passive = ScriptableObject.CreateInstance<PassiveSkill>();
                            skill.Transfer(passive);
                            passive.ModElements = new List<Element>();
                            passive.ModValues = new List<float>();
                            passive.DESC = parsed[4];
                            passive.PERCENT = (float)Double.Parse(parsed[5]);
                            if (count > 0)
                            {

                                for (int i = 0; i < count; i++)
                                {
                                    Modification mod = new Modification();
                                    mod.affectedStat = (ModifiedStat)Enum.Parse(typeof(ModifiedStat), parsed[8]);
                                    mod.affectedElement = (Element)Enum.Parse(typeof(Element), parsed[index]);
                                    mod.editValue = (float)Double.Parse(parsed[6]) * passive.PERCENT;
                                    index++;
                                    passive.ModStat = mod.affectedStat;
                                    passive.ModValues.Add(mod.editValue);
                                    passive.ModElements.Add(mod.affectedElement);
                                }
                            }
                            else
                            {
                                Modification mod = new Modification();
                                mod.affectedStat = (ModifiedStat)Enum.Parse(typeof(ModifiedStat), parsed[8]);
                                mod.editValue = (float)Double.Parse(parsed[6]) * passive.PERCENT;
                                passive.ModStat = mod.affectedStat;
                                passive.ModValues.Add(mod.editValue);
                                passive.ModElements.Add(mod.affectedElement);
                            }
                            passive.UpdateDesc();
                            return passive;
                        }
                        break;
                    case Element.Opp:
                        {

                            OppSkill opp = ScriptableObject.CreateInstance<OppSkill>();
                            skill.Transfer(opp);
                            opp.TRIGGER = (Element)Enum.Parse(typeof(Element), parsed[4]);
                            opp.MOD = (float)Double.Parse(parsed[5]);

                            opp.DESC = "Allows a free attack after an ally hits with a " + opp.TRIGGER + " attack.";
                            return opp;
                        }
                        break;
                    case Element.Ailment:
                        {

                            int index = 13;
                            int count = Int32.Parse(parsed[12]);
                            CommandSkill ailment = ScriptableObject.CreateInstance<CommandSkill>();
                            skill.Transfer(ailment);

                            ailment.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                            ailment.COST = Int32.Parse(parsed[5]);
                            ailment.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                            ailment.RTYPE = (RanngeType)Enum.Parse(typeof(RanngeType), parsed[7]);

                            ailment.ACCURACY = 100;

                            ailment.HITS = 1;//Int32.Parse(parsed[11]);

                            ailment.TILES = new System.Collections.Generic.List<Vector2>();



                            for (int i = 0; i < count; i++)
                            {
                                Vector2 v = new Vector2();
                                v.x = Int32.Parse(parsed[index]);
                                index++;
                                v.y = Int32.Parse(parsed[index]);
                                index++;
                                ailment.TILES.Add(v);
                            }


                            ailment.UpdateDesc();
                            return ailment;
                        }
                        break;
                    case Element.Auto:
                        {

                            AutoSkill auto = ScriptableObject.CreateInstance<AutoSkill>();
                            skill.Transfer(auto);
                            auto.DESC = parsed[4];
                            auto.CHANCE = (float)Double.Parse(parsed[5]);
                            auto.ACT = (AutoAct)Enum.Parse(typeof(AutoAct), parsed[6]);
                            auto.REACT = (AutoReact)Enum.Parse(typeof(AutoReact), parsed[7]);

                            //     auto.NEXT = Int32.Parse(parsed[9]);
                            //   auto.NEXTCOUNT = Int32.Parse(parsed[10]);
                            auto.VAL = Int32.Parse(parsed[8]);
                            auto.UpdateDesc();
                            return auto;
                        }
                        break;
                    case Element.none:
                        break;
                    default:
                        {


                            int index = 15;
                            int count = Int32.Parse(parsed[14]);
                            CommandSkill command = ScriptableObject.CreateInstance<CommandSkill>();
                            skill.Transfer(command);
                            command.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                            command.COST = Int32.Parse(parsed[5]);
                            command.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
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
                            if (command.SUBTYPE == SubSkillType.RngAtk)
                            {
                                command.MIN_HIT = Int32.Parse(parsed[index]);
                                index++;
                                command.MAX_HIT = Int32.Parse(parsed[index]);

                            }

                            command.UpdateDesc();

                            return command;
                        }
                        break;
                }






            }


        }
        return skill;
    }
    public WeaponScript GetWeapon(int id, LivingObject livingObject)
    {
        // string path = "Assets/Resources/weapons.csv";

        // string path =  "Assets/Resources/skills.csv";



        string lines = "";
        if (weaponDictionary.TryGetValue(id, out lines))
        {
            string[] parsed = lines.Split(',');
            if (Int32.Parse(parsed[0]) == id)
            {

                WeaponScript weapon = ScriptableObject.CreateInstance<WeaponScript>();
                weapon.INDEX = id;
                weapon.NAME = parsed[1];
                //  weapon.DESC = parsed[2];
                weapon.AUGMENTS = new List<Augment>();
                weapon.ATTACK = Int32.Parse(parsed[3]);
                weapon.ATTACK_TYPE = (EType)Enum.Parse(typeof(EType), parsed[4]);
                weapon.AFINITY = (Element)Enum.Parse(typeof(Element), parsed[5]);
                weapon.DIST = Int32.Parse(parsed[6]);
                weapon.Range = Int32.Parse(parsed[7]);
                weapon.ACCURACY = Int32.Parse(parsed[8]);
                weapon.CRIT = Int32.Parse(parsed[9]);
                weapon.BOOST = (ModifiedStat)Enum.Parse(typeof(ModifiedStat), parsed[10]);
                weapon.BOOSTVAL = Int32.Parse(parsed[11]);
                weapon.TYPE = 0;

                if(weapon.BOOST != ModifiedStat.none)
                {
                    weapon.DESC = "" + weapon.BOOST.ToString() + " +" + weapon.BOOSTVAL + "."; 
                }
                weapon.DESC += "Deals " + weapon.ATTACK_TYPE + " " + weapon.AFINITY + " based dmg.";

                if (weapon.Range == 1)
                {
                    weapon.DESC += " Hits a tile " + weapon.DIST + " space away.";
                }
                else if (weapon.Range == weapon.DIST)
                {
                    weapon.DESC += " Hits tiles up to " + weapon.DIST + " spaces away.";
                }
                if (livingObject)
                {

                    if (livingObject.GetComponent<InventoryScript>())
                    {
                        livingObject.INVENTORY.WEAPONS.Add(weapon);
                        livingObject.INVENTORY.USEABLES.Add(weapon);
                        if (!livingObject.WEAPON)
                        {
                            livingObject.WEAPON.Equip(weapon);
                        }
                        if (livingObject.WEAPON.NAME.Equals("default"))
                        {
                            livingObject.WEAPON.Equip(weapon);
                        }
                    }
                }

                return weapon;
            }





            //   reader.Close();
        }
        return null;
    }

    public ArmorScript GetArmor(int id, LivingObject livingObject)
    {
        //  string path = "Assets/Resources/armor.csv";


        // string path =  "Assets/Resources/skills.csv";


        //Read the text from directly from the test.txt file
        //  StreamReader reader = new StreamReader(path);

        string lines = "";
        if (armorDictionary.TryGetValue(id, out lines))
        {
            string[] parsed = lines.Split(',');
            if (Int32.Parse(parsed[0]) == id)
            {

                ArmorScript armor = ScriptableObject.CreateInstance<ArmorScript>();
                armor.NAME = parsed[1];
                armor.INDEX = id;
                armor.AUGMENTS = new List<Augment>();
                //  armor.DESC = parsed[2];
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

               // armor.DESC = "Defense: " + armor.DEFENSE + " Resistance: " + armor.RESISTANCE + " Speed: " + armor.SPEED;
                armor.DESC = "Def +" + armor.DEFENSE + ", Res+" + armor.RESISTANCE + " Spd +" + armor.SPEED;
                for (int i = 0; i < list.Count; i++)
                {
                    if(list[i] != EHitType.normal)
                    {
                        if(list[i] < EHitType.normal)
                        {
                            armor.DESC += ", " + list[i].ToString() + " " + (Element)i;
                        }
                        else
                        {
                            armor.DESC += ", " + list[i].ToString() + " to " + (Element)i;
                        }
                    }
                }
                armor.HITLIST = list;
                armor.MAX_HEALTH = 40.0f;
                armor.HEALTH = armor.MAX_HEALTH;
                if (livingObject)
                {

                    if (livingObject.GetComponent<InventoryScript>())
                    {

                        livingObject.INVENTORY.ARMOR.Add(armor);
                        livingObject.INVENTORY.USEABLES.Add(armor);
                        if (!livingObject.ARMOR)
                        {
                            livingObject.ARMOR.Equip(armor);
                        }
                        if (livingObject.ARMOR.NAME.Equals("default"))
                        {
                            livingObject.ARMOR.Equip(armor);
                        }
                    }
                }
                return armor;
            }




            // reader.Close();
        }
        return null;
    }

    public void GetItem(int id, LivingObject livingObject)
    {



        string lines = "";
        if (itemDictionary.TryGetValue(id, out lines))
        {
            string[] parsed = lines.Split(',');
            if (Int32.Parse(parsed[0]) == id)
            {
                ItemScript item = ScriptableObject.CreateInstance<ItemScript>();
                item.INDEX = id;
                item.NAME = parsed[1];
                item.DESC = parsed[2];
                item.ITYPE = (ItemType)Enum.Parse(typeof(ItemType), parsed[3]);
                item.TTYPE = (TargetType)Enum.Parse(typeof(TargetType), parsed[4]);
                item.VALUE = (float)Double.Parse(parsed[5]);
                item.ELEMENT = (Element)Enum.Parse(typeof(Element), parsed[6]);
                item.STAT = (ModifiedStat)Enum.Parse(typeof(ModifiedStat), parsed[7]);
                item.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[8]);
                if (livingObject)
                {

                    if (livingObject.INVENTORY)
                    {
                        livingObject.INVENTORY.ITEMS.Add(item);
                        livingObject.INVENTORY.USEABLES.Add(item);
                    }
                }
            }
        }



        // reader.Close();

    }

    public void GetHazard(int id, HazardScript newHazard)
    {


        if (newHazard.GetComponent<InventoryScript>())
        {

            string lines = "";
            if (hazardDictionary.TryGetValue(id, out lines))
            {
                string[] parsed = lines.Split(',');
                if (Int32.Parse(parsed[0]) == id)
                {
                    int fileIndex = 1;
                    newHazard.FullName = parsed[fileIndex];
                    fileIndex++;
                    newHazard.BASE_STATS.LEVEL = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofskills = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofweapons = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int glyphAtk = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    glyphAtk = newHazard.REWARD;
                    if (newHazard)
                    {
                        LearnSkill(glyphAtk, newHazard, true);
                        fileIndex++;
                    }

                    else
                    {
                        GetWeapon(glyphAtk, newHazard);
                        fileIndex++;
                    }


                }
            }


        }
    }

    public void GetEnemy(int id, EnemyScript newEnemy)
    {


        if (newEnemy.GetComponent<InventoryScript>())
        {

            string lines = "";
            if (enemyDictionary.TryGetValue(id, out lines))
            {
                string[] parsed = lines.Split(',');
                if (Int32.Parse(parsed[0]) == id)
                {
                    newEnemy.BASE_STATS.Reset(true);
                    newEnemy.STATS.Reset(true);
                    newEnemy.DEAD = false;
                    int fileIndex = 1;
                    newEnemy.FullName = parsed[fileIndex];
                    fileIndex++;
                    newEnemy.BASE_STATS.LEVEL = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    newEnemy.BASE_STATS.MAX_HEALTH = Int32.Parse(parsed[fileIndex]);
                    newEnemy.BASE_STATS.HEALTH = newEnemy.BASE_STATS.MAX_HEALTH;
                    fileIndex++;
                    newEnemy.BASE_STATS.MAX_MANA = Int32.Parse(parsed[fileIndex]);
                    newEnemy.BASE_STATS.MANA = newEnemy.BASE_STATS.MAX_MANA;
                    fileIndex++;
                    newEnemy.BASE_STATS.MAX_FATIGUE = Int32.Parse(parsed[fileIndex]);
                    newEnemy.BASE_STATS.FATIGUE = 0;
                    fileIndex++;
                    newEnemy.BASE_STATS.STRENGTH = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    newEnemy.BASE_STATS.MAGIC = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    newEnemy.BASE_STATS.DEFENSE = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    newEnemy.BASE_STATS.RESIESTANCE = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    newEnemy.BASE_STATS.SPEED = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    newEnemy.BASE_STATS.SKILL = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    newEnemy.BASE_STATS.MOVE_DIST = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofskills = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofweapons = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofarmors = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;



                    for (int i = 0; i < numofskills; i++)
                    {
                        LearnSkill(Int32.Parse(parsed[fileIndex]), newEnemy, true);
                        fileIndex++;
                    }

                    for (int i = 0; i < numofweapons; i++)
                    {
                        GetWeapon(Int32.Parse(parsed[fileIndex]), newEnemy);
                        fileIndex++;
                    }

                    for (int i = 0; i < numofarmors; i++)
                    {
                        GetArmor(Int32.Parse(parsed[fileIndex]), newEnemy);
                        fileIndex++;
                    }
                }
            }




            // reader.Close();
        }
    }

    public void GetActor(int id, LivingObject living)
    {


        if (living.GetComponent<InventoryScript>())
        {

            string lines = "";
            if (actorDictionary.TryGetValue(id, out lines))
            {
                string[] parsed = lines.Split(',');
                if (Int32.Parse(parsed[0]) == id)
                {
                    int fileIndex = 1;
                    living.FullName = parsed[fileIndex];
                    fileIndex++;
                    living.BASE_STATS.LEVEL = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    living.BASE_STATS.MAX_HEALTH = Int32.Parse(parsed[fileIndex]);
                    living.BASE_STATS.HEALTH = living.BASE_STATS.MAX_HEALTH;
                    fileIndex++;
                    living.BASE_STATS.MAX_MANA = Int32.Parse(parsed[fileIndex]);
                    living.BASE_STATS.MANA = living.BASE_STATS.MAX_MANA;
                    fileIndex++;
                    living.BASE_STATS.MAX_FATIGUE = Int32.Parse(parsed[fileIndex]);
                    living.BASE_STATS.FATIGUE = 0;
                    fileIndex++;
                    living.BASE_STATS.STRENGTH = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    living.BASE_STATS.MAGIC = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    living.BASE_STATS.DEFENSE = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    living.BASE_STATS.RESIESTANCE = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    living.BASE_STATS.SPEED = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    living.BASE_STATS.SKILL = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    living.BASE_STATS.MOVE_DIST = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofskills = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofweapons = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofarmors = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofitems = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    for (int i = 0; i < numofskills; i++)
                    {
                        LearnSkill(Int32.Parse(parsed[fileIndex]), living, true);
                        fileIndex++;
                    }

                    for (int i = 0; i < numofweapons; i++)
                    {
                        GetWeapon(Int32.Parse(parsed[fileIndex]), living);
                        fileIndex++;
                    }

                    for (int i = 0; i < numofarmors; i++)
                    {
                        GetArmor(Int32.Parse(parsed[fileIndex]), living);
                        fileIndex++;
                    }

                    for (int i = 0; i < numofitems; i++)
                    {
                        GetItem(Int32.Parse(parsed[fileIndex]), living);
                        fileIndex++;
                    }
                }
            }




            // reader.Close();
        }
    }

    public MapDetail GetMap(int id)
    {
        MapDetail map = new MapDetail();
        map.doorIndexes = new List<int>();
        map.roomNames = new List<string>();
        map.roomIndexes = new List<int>();
        map.enemyIndexes = new List<int>();
        map.hazardIndexes = new List<int>();
        map.shopIndexes = new List<int>();
        string lines = "";
        if (mapDictionary.TryGetValue(id, out lines))
        {
            string[] parsed = lines.Split(',');
            if (Int32.Parse(parsed[0]) == id)
            {
                int fileIndex = 1;
                map.mapName = parsed[fileIndex];
                fileIndex++;
                int tex = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                map.texture = Resources.LoadAll<Texture>("Textures/")[tex];
                map.width = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                map.height = Int32.Parse(parsed[fileIndex]);
                fileIndex++;

                int numOfDoors = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                for (int i = 0; i < numOfDoors; i++)
                {
                    map.doorIndexes.Add(Int32.Parse(parsed[fileIndex]));
                    fileIndex++;
                }

                for (int i = 0; i < numOfDoors; i++)
                {
                    map.roomNames.Add(parsed[fileIndex]);
                    fileIndex++;
                }

                for (int i = 0; i < numOfDoors; i++)
                {
                    map.roomIndexes.Add(Int32.Parse(parsed[fileIndex]));
                    fileIndex++;
                }

                int numOfEnemies = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                for (int i = 0; i < numOfEnemies; i++)
                {
                    map.enemyIndexes.Add(Int32.Parse(parsed[fileIndex]));
                    fileIndex++;
                }

                int numOfHazards = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                for (int i = 0; i < numOfHazards; i++)
                {
                    map.hazardIndexes.Add(Int32.Parse(parsed[fileIndex]));
                    fileIndex++;
                }

                int numOfShops = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                for (int i = 0; i < numOfShops; i++)
                {
                    map.shopIndexes.Add(Int32.Parse(parsed[fileIndex]));
                    fileIndex++;
                }
            }
        }

        return map;
    }
}
