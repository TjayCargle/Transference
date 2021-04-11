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
    TextAsset objectFile;

    [SerializeField]
    TextAsset optionsFile;

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

    [SerializeField]
    string[] objLines;


    [SerializeField]
    string[] optionLines;

    private Dictionary<int, string> skillDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> weaponDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> armorDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> itemDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> enemyDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> actorDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> hazardDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> mapDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> objDictionary = new Dictionary<int, string>();
    private Dictionary<string, string> optionsDictionary = new Dictionary<string, string>();
    public bool isSetup = false;
    MapData data = new MapData();
    SceneContainer scene = new SceneContainer();
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
                    if (line.Length > 0)
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
            if (objDictionary.Count == 0)
            {
                if (objectFile)
                {

                    file = objectFile.text;
                    objLines = file.Split('\n');

                    for (int i = 1; i < objLines.Length; i++)
                    {
                        string line = objLines[i];
                        if (line[0] != '-')
                        {
                            string[] parsed = line.Split(',');
                            objDictionary.Add(Int32.Parse(parsed[0]), line);
                        }
                    }
                }
            }


            if (optionsDictionary.Count == 0)
            {
                if (optionsFile)
                {

                    file = optionsFile.text;
                    optionLines = file.Split('\n');

                    for (int i = 1; i < optionLines.Length; i++)
                    {
                        string line = optionLines[i];
                        if (line[0] != '-')
                        {
                            string[] parsed = line.Split(',');
                            optionsDictionary.Add(parsed[0], line);
                        }
                    }
                }
            }


            data.unOccupiedIndexes = new List<int>();
            data.events = new List<EventPair>();

            data.doorIndexes = new List<int>();
            data.roomNames = new List<string>();
            data.roomIndexes = new List<int>();
            data.enemyIndexes = new List<int>();
            data.glyphIndexes = new List<int>();
            data.glyphIds = new List<int>();
            data.shopIndexes = new List<int>();
            data.startIndexes = new List<int>();
            data.objMapIndexes = new List<int>();
            data.objIds = new List<int>();
            data.EnemyIds = new List<int>();
            data.specialiles = new List<TileType>();
            data.specialExtra = new List<int>();
            data.tilesInShadow = new List<int>();
            data.specialTileIndexes = new List<int>();

            scene.speakerNames = new List<string>();
            scene.speakertext = new List<string>();
            scene.speakerFace = new List<Sprite>();
            scene.eventIndexs = new List<int>();
            scene.sceneEvents = new List<SceneEventContainer>();

            if (PlayerPrefs.HasKey("gameSettings") == false)
            {
                if (optionLines.Length > 0)
                {
                    string gs = "";
                    for (int i = 0; i < optionLines.Length; i++)
                    {
                        string line = optionLines[i];
                        if (line != String.Empty)
                        {

                            if (line[0] != '-')
                            {
                                // string[] parsed = line.Split(',');
                                gs += line + "|";
                            }
                        }
                    }
                    // Debug.Log(gs);
                    PlayerPrefs.SetString("gameSettings", gs);
                }

                isSetup = true;
            }
        }
    }
    private void OnEnable()
    {
        Setup();
    }
    public void Start()
    {
        Setup();
    }
    static void WriteString()
    {
        //  string path = "Assets/Resources/skills.csv";

        //Write some text to the test.txt file
        // StreamWriter writer = new StreamWriter(path, true);
        // writer.WriteLine("Test");
        //writer.Close();

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
                    skill.AUGMENTS = new List<Augment>();
                    //   skill.DESC = parsed[4];
                    skill.ELEMENT = (Element)Enum.Parse(typeof(Element), parsed[2]);
                    skill.SUBTYPE = (SubSkillType)Enum.Parse(typeof(SubSkillType), parsed[3]);

                    //Debug.Log(id + " " + skill.NAME + " " +skill.ELEMENT);
                    skill.TYPE = 4;
                    skill.USER = livingObject;
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
                                    buff.RTYPE = (RangeType)Enum.Parse(typeof(RangeType), parsed[7]);

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
                                    //buff.TILES = new System.Collections.Generic.List<Vector2>();
                                    buff.OPPORTUNITY = Element.Support;
                                    Modification mod = new Modification();
                                    switch (buff.BUFF)
                                    {
                                        case BuffType.attack:
                                            mod.affectedStat = ModifiedStat.Atk;
                                            break;
                                        case BuffType.Spd:
                                            mod.affectedStat = ModifiedStat.Spd;
                                            break;
                                        case BuffType.Def:
                                            mod.affectedStat = ModifiedStat.Def;
                                            break;
                                        case BuffType.Res:
                                            mod.affectedStat = ModifiedStat.Res;
                                            break;
                                        case BuffType.Dex:
                                            mod.affectedStat = ModifiedStat.Dex;
                                            break;
                                        case BuffType.none:
                                            break;
                                        case BuffType.Str:
                                            mod.affectedStat = ModifiedStat.Str;
                                            break;
                                        case BuffType.Mag:
                                            mod.affectedStat = ModifiedStat.Mag;
                                            break;
                                        case BuffType.all:
                                            mod.affectedStat = ModifiedStat.all;
                                            break;
                                    }
                                    mod.editValue = (float)Double.Parse(parsed[10]);

                                    buff.BUFFEDSTAT = mod.affectedStat;
                                    buff.BUFFVAL = mod.editValue;

                                    //for (int i = 0; i < count; i++)
                                    //{
                                    //    Vector2 v = new Vector2();
                                    //    v.x = Int32.Parse(parsed[index]);
                                    //    index++;
                                    //    v.y = Int32.Parse(parsed[index]);
                                    //    index++;
                                    //    buff.TILES.Add(v);
                                    //}

                                    livingObject.INVENTORY.CSKILLS.Add(buff);
                                    livingObject.INVENTORY.USEABLES.Add(buff);
                                    livingObject.INVENTORY.SKILLS.Add(buff);

                                    //          if (equip == true)
                                    {
                                        if (livingObject.MAGICAL_SLOTS.CanAdd())
                                            livingObject.MAGICAL_SLOTS.SKILLS.Add(buff);
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
                                   // int index = 15;
                                   // int count = Int32.Parse(parsed[14]);
                                    CommandSkill support = ScriptableObject.CreateInstance<CommandSkill>();
                                    skill.Transfer(support);
                                    support.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                                    support.COST = Int32.Parse(parsed[5]);
                                    support.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                                    support.RTYPE = (RangeType)Enum.Parse(typeof(RangeType), parsed[7]);
                                    support.ACCURACY = Int32.Parse(parsed[10]);

                                    support.DAMAGE = (DMG)Enum.Parse(typeof(DMG), parsed[11]);
                                    support.HITS = Int32.Parse(parsed[12]);
                                    support.CRIT_RATE = Int32.Parse(parsed[13]);
                                    //support.TILES = new System.Collections.Generic.List<Vector2>();
                                    support.OPPORTUNITY = Element.Buff;

                                    //for (int i = 0; i < count; i++)
                                    //{
                                    //    Vector2 v = new Vector2();
                                    //    v.x = Int32.Parse(parsed[index]);
                                    //    index++;
                                    //    v.y = Int32.Parse(parsed[index]);
                                    //    index++;
                                    //    support.TILES.Add(v);
                                    //}
                                    switch (support.EFFECT)
                                    {
                                        case SideEffect.heal:
                                            {
                                                support.DESC = "Heals " + support.DAMAGE + " amount of health to target";
                                            }
                                            break;

                                        case SideEffect.swap:
                                            {
                                                support.DESC = "Swaps positions with target";
                                            }
                                            break;
                                    }

                                    livingObject.GetComponent<InventoryScript>().CSKILLS.Add(support);
                                    livingObject.GetComponent<InventoryScript>().USEABLES.Add(support);
                                    livingObject.GetComponent<InventoryScript>().SKILLS.Add(support);

                                    //   if (equip == true)
                                    {
                                        if (livingObject.MAGICAL_SLOTS.CanAdd())
                                            livingObject.MAGICAL_SLOTS.SKILLS.Add(support);
                                    }
                                }
                                break;
                            case Element.Passive:
                                {

                                    int index = 10;
                                    int count = Int32.Parse(parsed[9]);

                                    ComboSkill combo = ScriptableObject.CreateInstance<ComboSkill>();
                                    skill.Transfer(combo);

                                    combo.GAIN = Int32.Parse(parsed[5]);
                                    combo.FIRST = (Element)Enum.Parse(typeof(Element), parsed[6]);
                                    combo.SECOND = (Element)Enum.Parse(typeof(Element), parsed[7]);
                                    combo.THIRD = (Element)Enum.Parse(typeof(Element), parsed[8]);


                                    if (count > 0)
                                    {

                                        for (int i = 0; i < count; i++)
                                        {
                                            SkillEventContainer cont = new SkillEventContainer();
                                            SkillEvent se = (SkillEvent)Enum.Parse(typeof(SkillEvent), parsed[index]);
                                            index++;
                                            SkillReaction sr = (SkillReaction)Enum.Parse(typeof(SkillReaction), parsed[index]);
                                            index++;
                                            cont.theSkill = combo;
                                            cont.theEvent = se;
                                            cont.theReaction = sr;
                                            combo.SPECIAL_EVENTS.Add(cont);
                                        }
                                    }

                                    livingObject.INVENTORY.USEABLES.Add(combo);
                                    livingObject.INVENTORY.COMBOS.Add(combo);
                                    livingObject.INVENTORY.SKILLS.Add(combo);

                                    //    if (equip == true)
                                    {
                                        if (livingObject.COMBO_SLOTS.CanAdd())
                                        {
                                            livingObject.COMBO_SLOTS.SKILLS.Add(combo);
                                            livingObject.UpdateBuffsAndDebuffs();
                                        }
                                    }
                                    combo.UpdateDesc();
                                    return combo;
                                }
                                break;
                            case Element.Opp:
                                {

                                    OppSkill opp = ScriptableObject.CreateInstance<OppSkill>();
                                    skill.Transfer(opp);
                                    opp.TRIGGERS.Add((Element)Enum.Parse(typeof(Element), parsed[4]));
                                    opp.REACTION = ((Element)Enum.Parse(typeof(Element), parsed[5]));

                                    opp.UpdateDesc();

                                    livingObject.GetComponent<InventoryScript>().USEABLES.Add(opp);
                                    livingObject.GetComponent<InventoryScript>().OPPS.Add(opp);
                                    livingObject.GetComponent<InventoryScript>().SKILLS.Add(opp);
                                    // if (equip == true)
                                    {
                                        //if (livingObject.OPP_SLOTS.CanAdd())
                                        //    livingObject.OPP_SLOTS.SKILLS.Add(opp);
                                    }

                                    return opp;
                                }
                                break;
                            case Element.Ailment:
                                {

                                    int count = Int32.Parse(parsed[14]);
                                    CommandSkill ailment = ScriptableObject.CreateInstance<CommandSkill>();
                                    skill.Transfer(ailment);

                                    ailment.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                                    ailment.COST = Int32.Parse(parsed[5]);
                                    ailment.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                                    ailment.RTYPE = (RangeType)Enum.Parse(typeof(RangeType), parsed[7]);

                                    ailment.ACCURACY = Int32.Parse(parsed[10]);

                                    ailment.HITS = 1;//Int32.Parse(parsed[11]);
                                    ailment.OPPORTUNITY = (Element)(id % 8);
                                    if (count > 0)
                                    {

                                        int index = 15;
                                        for (int i = 0; i < count; i++)
                                        {
                                            SkillEventContainer cont = new SkillEventContainer();
                                            SkillEvent se = (SkillEvent)Enum.Parse(typeof(SkillEvent), parsed[index]);
                                            index++;
                                            SkillReaction sr = (SkillReaction)Enum.Parse(typeof(SkillReaction), parsed[index]);
                                            index++;
                                            cont.theSkill = ailment;
                                            cont.theEvent = se;
                                            cont.theReaction = sr;
                                            ailment.SPECIAL_EVENTS.Add(cont);
                                        }
                                    }


                                    livingObject.GetComponent<InventoryScript>().CSKILLS.Add(ailment);
                                    livingObject.GetComponent<InventoryScript>().USEABLES.Add(ailment);
                                    livingObject.GetComponent<InventoryScript>().SKILLS.Add(ailment);
                                    //if (equip == true)
                                    //{
                                    //    if (livingObject.BATTLE_SLOTS.CanAdd())
                                    //        livingObject.BATTLE_SLOTS.SKILLS.Add(ailment);
                                    //}
                                    if (livingObject.MAGICAL_SLOTS.CanAdd())
                                        livingObject.MAGICAL_SLOTS.SKILLS.Add(ailment);
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
                                    auto.ACT = (SkillEvent)Enum.Parse(typeof(SkillEvent), parsed[6]);
                                    auto.REACT = (SkillReaction)Enum.Parse(typeof(SkillReaction), parsed[7]);

                                    //     auto.NEXT = Int32.Parse(parsed[9]);
                                    //   auto.NEXTCOUNT = Int32.Parse(parsed[10]);
                                    auto.VAL = Int32.Parse(parsed[8]);
                                    auto.OWNER = livingObject;
                                    livingObject.GetComponent<InventoryScript>().USEABLES.Add(auto);
                                    livingObject.GetComponent<InventoryScript>().AUTOS.Add(auto);
                                    livingObject.GetComponent<InventoryScript>().SKILLS.Add(auto);
                                    if (equip == true)
                                    {

                                    }
                                    if (livingObject.AUTO_SLOTS.CanAdd())
                                        livingObject.AUTO_SLOTS.SKILLS.Add(auto);
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
                                    int count = Int32.Parse(parsed[14]);
                                    CommandSkill command = ScriptableObject.CreateInstance<CommandSkill>();
                                    skill.Transfer(command);
                                    command.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                                    command.COST = Int32.Parse(parsed[5]);
                                    command.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                                    command.RTYPE = (RangeType)Enum.Parse(typeof(RangeType), parsed[7]);
                                    command.ACCURACY = Int32.Parse(parsed[10]);

                                    command.DAMAGE = (DMG)Enum.Parse(typeof(DMG), parsed[11]);
                                    command.HITS = Int32.Parse(parsed[12]);
                                    command.CRIT_RATE = Int32.Parse(parsed[13]);
                                    //  command.TILES = new System.Collections.Generic.List<Vector2>();
                                    command.OPPORTUNITY = (Element)(id % 8);


                                    //for (int i = 0; i < count; i++)
                                    //{
                                    //    Vector2 v = new Vector2();
                                    //    v.x = Int32.Parse(parsed[index]);
                                    //    index++;
                                    //    v.y = Int32.Parse(parsed[index]);
                                    //    index++;
                                    //    command.TILES.Add(v);
                                    //}
                                    if (command.SUBTYPE == SubSkillType.RngAtk)
                                    {
                                        command.MIN_HIT = Int32.Parse(parsed[8]);
                                        // index++;
                                        command.MAX_HIT = Int32.Parse(parsed[9]);

                                    }
                                    //if (command.RTYPE == RangeType.single)
                                    //{
                                    //    command.DESC = "Deals " + command.DAMAGE + " " + command.ETYPE + "  " + skill.ELEMENT + " damage to";
                                    //    if (count > 1)
                                    //    {
                                    //        command.DESC += " enemies in row";
                                    //    }
                                    //    else
                                    //    {
                                    //        command.DESC += " a single enemy";
                                    //    }
                                    //    if (command.HITS > 1)
                                    //    {
                                    //        command.DESC += " " + command.HITS + " times";
                                    //    }
                                    //}
                                    //if (command.RTYPE == RangeType.area)
                                    //{
                                    //    command.DESC = "Deals " + command.DAMAGE + " " + command.ETYPE + "  " + skill.ELEMENT + " damage to all enemies in range";

                                    //}
                                    //if (command.RTYPE == RangeType.multi)
                                    //{
                                    //    command.DESC = "Deals " + command.DAMAGE + " " + command.ETYPE + "  " + skill.ELEMENT + " damage to an enemy in range";

                                    //}
                                    //// if (command.HITS == 1)
                                    ////    command.DESC = "Deals " + command.DAMAGE + " " + skill.ELEMENT + " based " + command.ETYPE + " damage to " + command.TILES.Count + " enemy";
                                    ////else
                                    ////{

                                    ////    command.DESC = "Deals " + command.DAMAGE + " " + skill.ELEMENT + " based " + command.ETYPE + " damage to enemy " + command.HITS + " times";

                                    ////}
                                    //if (command.SUBTYPE == SubSkillType.RngAtk)
                                    //{
                                    //    command.DESC = "Deals " + command.DAMAGE + " " + command.ETYPE + "  " + skill.ELEMENT + " damage to enemy " + command.MIN_HIT + "-" + command.MAX_HIT + " times";
                                    //}
                                    //if (command.EFFECT != SideEffect.none)
                                    //{
                                    //    if (command.EFFECT < SideEffect.reduceStr)
                                    //    {
                                    //        command.DESC += " with a chance of " + command.EFFECT.ToString();
                                    //    }
                                    //    else
                                    //    {
                                    //        command.DESC += " with a chance to debuff " + Common.GetSideEffectText(command.EFFECT);
                                    //        command.BUFFEDSTAT = Common.GetSideEffectMod(command.EFFECT);
                                    //        command.BUFFVAL = -1 * command.CRIT_RATE;
                                    //        command.EFFECT = SideEffect.debuff;
                                    //    }
                                    //}
                                    if (count > 0)
                                    {

                                        int index = 15;
                                        for (int i = 0; i < count; i++)
                                        {
                                            SkillEventContainer cont = new SkillEventContainer();
                                            SkillEvent se = (SkillEvent)Enum.Parse(typeof(SkillEvent), parsed[index]);
                                            index++;
                                            SkillReaction sr = (SkillReaction)Enum.Parse(typeof(SkillReaction), parsed[index]);
                                            index++;
                                            cont.theSkill = command;
                                            cont.theEvent = se;
                                            cont.theReaction = sr;
                                            command.SPECIAL_EVENTS.Add(cont);
                                        }
                                    }
                                    command.UpdateDesc();

                                    livingObject.INVENTORY.CSKILLS.Add(command);

                                    livingObject.INVENTORY.USEABLES.Add(command);
                                    livingObject.INVENTORY.SKILLS.Add(command);
                                    if (equip == true)
                                    {
                                    }
                                    if (command.ETYPE == EType.physical)
                                    {

                                        if (livingObject.PHYSICAL_SLOTS.CanAdd())
                                            livingObject.PHYSICAL_SLOTS.SKILLS.Add(command);
                                    }
                                    else if (command.ETYPE == EType.mental)
                                    {
                                        if (livingObject.MAGICAL_SLOTS.CanAdd())
                                            livingObject.MAGICAL_SLOTS.SKILLS.Add(command);
                                    }
                                    else
                                    {

                                        if (livingObject.MAGICAL_SLOTS.CanAdd())
                                            livingObject.MAGICAL_SLOTS.SKILLS.Add(command);
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
                            buff.RTYPE = (RangeType)Enum.Parse(typeof(RangeType), parsed[7]);


                            buff.ACCURACY = 100;
                            buff.OPPORTUNITY = Element.Support;
                            buff.HITS = Int32.Parse(parsed[11]);
                            buff.BUFF = (BuffType)Enum.Parse(typeof(BuffType), parsed[12]);
                            //  buff.TILES = new System.Collections.Generic.List<Vector2>();

                            Modification mod = new Modification();
                            switch (buff.BUFF)
                            {
                                case BuffType.attack:
                                    mod.affectedStat = ModifiedStat.Atk;
                                    break;
                                case BuffType.Spd:
                                    mod.affectedStat = ModifiedStat.Spd;
                                    break;
                                case BuffType.Def:
                                    mod.affectedStat = ModifiedStat.Def;
                                    break;
                                case BuffType.Res:
                                    mod.affectedStat = ModifiedStat.Res;
                                    break;
                                case BuffType.Dex:
                                    mod.affectedStat = ModifiedStat.Dex;
                                    break;
                                case BuffType.none:
                                    break;
                                case BuffType.Str:
                                    mod.affectedStat = ModifiedStat.Str;
                                    break;
                                case BuffType.Mag:
                                    mod.affectedStat = ModifiedStat.Mag;
                                    break;
                                case BuffType.all:
                                    mod.affectedStat = ModifiedStat.all;
                                    break;
                            }
                            mod.editValue = (float)Double.Parse(parsed[10]);

                            buff.BUFFEDSTAT = mod.affectedStat;
                            buff.BUFFVAL = mod.editValue;

                            //for (int i = 0; i < count; i++)
                            //{
                            //    Vector2 v = new Vector2();
                            //    v.x = Int32.Parse(parsed[index]);
                            //    index++;
                            //    v.y = Int32.Parse(parsed[index]);
                            //    index++;
                            //    buff.TILES.Add(v);
                            //}

                            buff.UpdateDesc();
                            return buff;
                        }
                        break;
                    case Element.Support:
                        {
                            //int index = 15;
                            //int count = Int32.Parse(parsed[14]);
                            CommandSkill support = ScriptableObject.CreateInstance<CommandSkill>();
                            skill.Transfer(support);
                            support.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                            support.COST = Int32.Parse(parsed[5]);
                            support.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                            support.RTYPE = (RangeType)Enum.Parse(typeof(RangeType), parsed[7]);
                            support.ACCURACY = Int32.Parse(parsed[8]);

                            support.DAMAGE = (DMG)Enum.Parse(typeof(DMG), parsed[9]);
                            support.HITS = Int32.Parse(parsed[10]);
                            support.CRIT_RATE = Int32.Parse(parsed[11]);
                            //support.TILES = new System.Collections.Generic.List<Vector2>();
                            support.OPPORTUNITY = Element.Buff;

                            //for (int i = 0; i < count; i++)
                            //{
                            //    Vector2 v = new Vector2();
                            //    v.x = Int32.Parse(parsed[index]);
                            //    index++;
                            //    v.y = Int32.Parse(parsed[index]);
                            //    index++;
                            //    support.TILES.Add(v);
                            //}

                            support.UpdateDesc();
                        }
                        break;
                    case Element.Passive:
                        {
                            int index = 10;
                            int count = Int32.Parse(parsed[9]);

                            ComboSkill combo = ScriptableObject.CreateInstance<ComboSkill>();
                            skill.Transfer(combo);

                            combo.GAIN = Int32.Parse(parsed[5]);
                            combo.FIRST = (Element)Enum.Parse(typeof(Element), parsed[6]);
                            combo.SECOND = (Element)Enum.Parse(typeof(Element), parsed[7]);
                            combo.THIRD = (Element)Enum.Parse(typeof(Element), parsed[8]);


                            if (count > 0)
                            {

                                for (int i = 0; i < count; i++)
                                {
                                    SkillEventContainer cont = new SkillEventContainer();
                                    SkillEvent se = (SkillEvent)Enum.Parse(typeof(SkillEvent), parsed[index]);
                                    index++;
                                    SkillReaction sr = (SkillReaction)Enum.Parse(typeof(SkillReaction), parsed[index]);
                                    index++;
                                    cont.theSkill = combo;
                                    cont.theEvent = se;
                                    cont.theReaction = sr;
                                    combo.SPECIAL_EVENTS.Add(cont);
                                }
                            }


                            combo.UpdateDesc();
                            return combo;
                        }


                        break;
                    case Element.Opp:
                        {

                            OppSkill opp = ScriptableObject.CreateInstance<OppSkill>();
                            skill.Transfer(opp);
                            opp.TRIGGERS.Add((Element)Enum.Parse(typeof(Element), parsed[4]));
                            opp.REACTION = ((Element)Enum.Parse(typeof(Element), parsed[5]));

                            opp.UpdateDesc();
                            return opp;
                        }
                        break;
                    case Element.Ailment:
                        {
                            int count = Int32.Parse(parsed[14]);
                            CommandSkill ailment = ScriptableObject.CreateInstance<CommandSkill>();
                            skill.Transfer(ailment);

                            ailment.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                            ailment.COST = Int32.Parse(parsed[5]);
                            ailment.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                            ailment.RTYPE = (RangeType)Enum.Parse(typeof(RangeType), parsed[7]);

                            ailment.ACCURACY = Int32.Parse(parsed[10]);

                            ailment.HITS = 1;//Int32.Parse(parsed[11]);
                            ailment.OPPORTUNITY  = (Element)(id % 8);


                            if (count > 0)
                            {

                                int index = 15;
                                for (int i = 0; i < count; i++)
                                {
                                    SkillEventContainer cont = new SkillEventContainer();
                                    SkillEvent se = (SkillEvent)Enum.Parse(typeof(SkillEvent), parsed[index]);
                                    index++;
                                    SkillReaction sr = (SkillReaction)Enum.Parse(typeof(SkillReaction), parsed[index]);
                                    index++;
                                    cont.theSkill = ailment;
                                    cont.theEvent = se;
                                    cont.theReaction = sr;
                                    ailment.SPECIAL_EVENTS.Add(cont);
                                }
                            }
                            // ailment.TILES = new System.Collections.Generic.List<Vector2>();



                            //for (int i = 0; i < count; i++)
                            //{
                            //    Vector2 v = new Vector2();
                            //    v.x = Int32.Parse(parsed[index]);
                            //    index++;
                            //    v.y = Int32.Parse(parsed[index]);
                            //    index++;
                            //    ailment.TILES.Add(v);
                            //}


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
                            auto.ACT = (SkillEvent)Enum.Parse(typeof(SkillEvent), parsed[6]);
                            auto.REACT = (SkillReaction)Enum.Parse(typeof(SkillReaction), parsed[7]);

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


                            int count = Int32.Parse(parsed[14]);
                            CommandSkill command = ScriptableObject.CreateInstance<CommandSkill>();
                            skill.Transfer(command);
                            command.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[4]);
                            command.COST = Int32.Parse(parsed[5]);
                            command.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[6]);
                            command.RTYPE = (RangeType)Enum.Parse(typeof(RangeType), parsed[7]);
                            command.ACCURACY = Int32.Parse(parsed[10]);

                            command.DAMAGE = (DMG)Enum.Parse(typeof(DMG), parsed[11]);
                            command.HITS = Int32.Parse(parsed[12]);
                            command.CRIT_RATE = Int32.Parse(parsed[13]);
                            //  command.TILES = new System.Collections.Generic.List<Vector2>();
                            command.OPPORTUNITY = (Element)(id % 8);


                            //for (int i = 0; i < count; i++)
                            //{
                            //    Vector2 v = new Vector2();
                            //    v.x = Int32.Parse(parsed[index]);
                            //    index++;
                            //    v.y = Int32.Parse(parsed[index]);
                            //    index++;
                            //    command.TILES.Add(v);
                            //}
                            if (command.SUBTYPE == SubSkillType.RngAtk)
                            {
                                command.MIN_HIT = Int32.Parse(parsed[8]);
                                //  index++;
                                command.MAX_HIT = Int32.Parse(parsed[9]);

                            }

                            if (count > 0)
                            {

                                int index = 15;
                                for (int i = 0; i < count; i++)
                                {
                                    SkillEventContainer cont = new SkillEventContainer();
                                    SkillEvent se = (SkillEvent)Enum.Parse(typeof(SkillEvent), parsed[index]);
                                    index++;
                                    SkillReaction sr = (SkillReaction)Enum.Parse(typeof(SkillReaction), parsed[index]);
                                    index++;
                                    cont.theSkill = command;
                                    cont.theEvent = se;
                                    cont.theReaction = sr;
                                    command.SPECIAL_EVENTS.Add(cont);
                                }
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
                weapon.ATTACK = DMG.tiny;//(DMG)Enum.Parse(typeof(DMG),parsed[3]);
                weapon.ATTACK_TYPE = EType.mental;//(EType)Enum.Parse(typeof(EType), parsed[4]);
                weapon.ELEMENT = (Element)Enum.Parse(typeof(Element), parsed[5]);
                weapon.ATKRANGE = (RangeType)Enum.Parse(typeof(RangeType), parsed[6]);
                //weapon.DIST = Int32.Parse(parsed[6]);
                //weapon.Range = Int32.Parse(parsed[7]);
                //  Common.SetWeaponDistRange(weapon);
                weapon.ACCURACY = Int32.Parse(parsed[7]);
                weapon.CHANCE = Single.Parse(parsed[8]);
                weapon.SEVENT = (SkillEvent)Enum.Parse(typeof(SkillEvent), parsed[9]);
                weapon.SREACTION = (SkillReaction)Enum.Parse(typeof(SkillReaction), parsed[10]);

                weapon.TYPE = 0;

                //if (weapon.Range == 1)
                //{
                //    weapon.DESC += " Hits a tile " + weapon.DIST + " space away.";
                //}
                //else if (weapon.Range == weapon.DIST)
                //{
                //    weapon.DESC += " Hits tiles up to " + weapon.DIST + " spaces away.";
                //}
                switch (weapon.ATKRANGE)
                {
                    case RangeType.adjacent:
                        {
                            weapon.COST = 8;
                        }
                        break;
                    case RangeType.pinWheel:
                        {
                            weapon.COST = 12;
                        }
                        break;
                    default:
                        {
                            weapon.COST = 10;
                        }
                        break;
                }

                weapon.UpdateDesc();
                if (livingObject)
                {

                    if (livingObject.INVENTORY)
                    {
                        livingObject.INVENTORY.WEAPONS.Add(weapon);
                        livingObject.INVENTORY.USEABLES.Add(weapon);
                        if (!livingObject.WEAPON)
                        {
                            livingObject.WEAPON.Equip(weapon);
                        }
                        if (!livingObject.WEAPON.EQUIPPED)
                        {
                            livingObject.WEAPON.Equip(weapon);
                        }
                    }
                    weapon.USER = livingObject;
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
                list.Add((EHitType)Enum.Parse(typeof(EHitType), parsed[13]));

                // armor.DESC = "Defense: " + armor.DEFENSE + " Resistance: " + armor.RESISTANCE + " Speed: " + armor.SPEED;
                armor.UpdateDesc();//DESC = "Def +" + armor.DEFENSE + ", Res+" + armor.RESISTANCE + " Spd +" + armor.SPEED;
                //for (int i = 0; i < list.Count; i++)
                //{
                //    if (list[i] != EHitType.normal)
                //    {
                //        if (list[i] < EHitType.normal)
                //        {
                //            armor.DESC += ", " + list[i].ToString() + " " + (Element)i;
                //        }
                //        else
                //        {
                //            armor.DESC += ", " + list[i].ToString() + " to " + (Element)i;
                //        }
                //    }
                //}
                armor.HITLIST = list;
                armor.MAX_HEALTH = 40.0f;
                armor.HEALTH = armor.MAX_HEALTH;
                if (livingObject)
                {

                    if (livingObject.INVENTORY)
                    {


                        if (!livingObject.ARMOR)
                        {
                            //livingObject.ARMOR.Equip(armor);
                            Debug.Log("no armor");
                        }
                        if (!livingObject.DEFAULT_ARMOR)
                        {

                            livingObject.DEFAULT_ARMOR = armor;
                            livingObject.ARMOR.Equip(armor);
                        }
                        else
                        {
                            livingObject.INVENTORY.ARMOR.Add(armor);
                            livingObject.INVENTORY.USEABLES.Add(armor);
                        }
                    }
                    armor.USER = livingObject;
                }
                return armor;
            }




            // reader.Close();
        }
        return null;
    }



    public ItemScript GetItem(int id, LivingObject livingObject)
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
                item.RTYPE = (RangeType)Enum.Parse(typeof(RangeType), parsed[2]);
                item.ITYPE = (ItemType)Enum.Parse(typeof(ItemType), parsed[3]);
                item.TTYPE = (TargetType)Enum.Parse(typeof(TargetType), parsed[4]);
                item.VALUE = (float)Double.Parse(parsed[5]);
                item.ELEMENT = (Element)Enum.Parse(typeof(Element), parsed[6]);
                item.STAT = (ModifiedStat)Enum.Parse(typeof(ModifiedStat), parsed[7]);
                item.EFFECT = (SideEffect)Enum.Parse(typeof(SideEffect), parsed[8]);
                if (parsed.Length == 10)
                    item.PDMG = (DMG)Enum.Parse(typeof(DMG), parsed[9]);
                // item.RTYPE = RangeType.adjacent;

                if (item.ELEMENT == Element.none)
                {
                    item.ELEMENT = Element.Support;
                }
                item.UpdateDesc();
                if (livingObject)
                {

                    if (livingObject.INVENTORY)
                    {
                        livingObject.INVENTORY.ITEMS.Add(item);
                        livingObject.INVENTORY.USEABLES.Add(item);
                    }
                    item.USER = livingObject;
                }
                return item;
            }
        }


        return null;
        // reader.Close();

    }

    public ItemScript GenerateScroll(LivingObject enemy, LivingObject reciever)
    {



        string lines = "";
        if (enemy != null)
        {
            string[] parsed = lines.Split(',');
            if (reciever != null)
            {
                ItemScript item = ScriptableObject.CreateInstance<ItemScript>();
                item.INDEX = 101;
                item.NAME = enemy.NAME + " Scroll";

                item.ITYPE = ItemType.summon;
                item.TTYPE = TargetType.adjecent;
                item.VALUE = enemy.id;
                item.ELEMENT = Element.Support;
                item.STAT = ModifiedStat.none;
                item.EFFECT = SideEffect.none;

                item.RTYPE = RangeType.adjacent;
                string details = "";
                details += "" + (enemy.BASE_STATS as StatScript).ToString() + ",";

                details += "" + enemy.INVENTORY.WEAPONS.Count + "";
                for (int i = 0; i < enemy.INVENTORY.WEAPONS.Count; i++)
                {
                    details += "," + enemy.INVENTORY.WEAPONS[i].INDEX + "," + enemy.INVENTORY.WEAPONS[i].LEVEL;
                }

                details += "," + enemy.INVENTORY.ARMOR.Count + "";
                for (int i = 0; i < enemy.INVENTORY.ARMOR.Count; i++)
                {
                    details += "," + enemy.INVENTORY.ARMOR[i].INDEX + "," + enemy.INVENTORY.ARMOR[i].LEVEL;
                }
                details += "," + enemy.INVENTORY.CSKILLS.Count + "";
                for (int i = 0; i < enemy.INVENTORY.CSKILLS.Count; i++)
                {
                    details += "," + enemy.INVENTORY.CSKILLS[i].INDEX + "," + enemy.INVENTORY.CSKILLS[i].LEVEL;
                }
                details += "," + enemy.INVENTORY.COMBOS.Count + "";
                for (int i = 0; i < enemy.INVENTORY.COMBOS.Count; i++)
                {
                    details += "," + enemy.INVENTORY.COMBOS[i].INDEX + "," + enemy.INVENTORY.COMBOS[i].LEVEL;
                }

                details += "," + enemy.INVENTORY.AUTOS.Count + "";
                for (int i = 0; i < enemy.INVENTORY.AUTOS.Count; i++)
                {
                    details += "," + enemy.INVENTORY.AUTOS[i].INDEX + "," + enemy.INVENTORY.AUTOS[i].LEVEL;
                }

                details += "," + enemy.INVENTORY.OPPS.Count + "";
                for (int i = 0; i < enemy.INVENTORY.OPPS.Count; i++)
                {
                    details += "," + enemy.INVENTORY.OPPS[i].INDEX + "," + enemy.INVENTORY.OPPS[i].LEVEL;
                }
                item.DEATS = details;
                item.UpdateDesc();

                if (reciever)
                {

                    if (reciever.INVENTORY)
                    {
                        reciever.INVENTORY.ITEMS.Add(item);
                        reciever.INVENTORY.USEABLES.Add(item);
                    }
                    item.USER = reciever;
                }
                return item;
            }
        }


        return null;
        // reader.Close();

    }

    public string GenerateSaveString(LivingObject someObject)
    {



        string lines = "";
        if (someObject != null)
        {
            string[] parsed = lines.Split(',');
           // if (reciever != null)
            {
            
                string details = "";
                details += someObject.FullName + ",";
                details += someObject.FACTION + ",";
                details += someObject.id + ",";
                details += someObject.ACTIONS + ",";
                details += someObject.currentTile.listindex + ",";
                details += "" + (someObject.BASE_STATS).ToString() + ",";
                details += "" + (someObject.STATS).ToString() + ",";

                details += "" + someObject.INVENTORY.WEAPONS.Count + "";
                for (int i = 0; i < someObject.INVENTORY.WEAPONS.Count; i++)
                {
                    details += "," + someObject.INVENTORY.WEAPONS[i].GetDataString();
                    
                    int augCount = someObject.INVENTORY.WEAPONS[i].AUGMENTS.Count;
                    details += "," + augCount;
                    for (int j = 0; j < augCount; j++)
                    {
                        details += "," + someObject.INVENTORY.WEAPONS[i].AUGMENTS[j];
                       
                    }
                }

                details += "," + someObject.INVENTORY.ARMOR.Count + "";
                for (int i = 0; i < someObject.INVENTORY.ARMOR.Count; i++)
                {
                    details += "," + someObject.INVENTORY.ARMOR[i].GetDataString();
                  
                    int augCount = someObject.INVENTORY.ARMOR[i].AUGMENTS.Count;
                    details += "," + augCount;
                    for (int j = 0; j < augCount; j++)
                    {
                        details += "," + someObject.INVENTORY.ARMOR[i].AUGMENTS[j];
                    }

                }

                details += "," + someObject.INVENTORY.CSKILLS.Count + "";
                for (int i = 0; i < someObject.INVENTORY.CSKILLS.Count; i++)
                {
                    details += "," + someObject.INVENTORY.CSKILLS[i].GetDataString();
                    
                    int augCount = someObject.INVENTORY.CSKILLS[i].AUGMENTS.Count;
                    details += "," + augCount;
                    for (int j = 0; j < augCount; j++)
                    {
                        details += "," + someObject.INVENTORY.CSKILLS[i].AUGMENTS[j];
                    }
                }
                details += "," + someObject.INVENTORY.COMBOS.Count + "";
                for (int i = 0; i < someObject.INVENTORY.COMBOS.Count; i++)
                {
                    details += "," + someObject.INVENTORY.COMBOS[i].INDEX + "," + someObject.INVENTORY.COMBOS[i].LEVEL;

                    int augCount = someObject.INVENTORY.COMBOS[i].AUGMENTS.Count;
                    details += "," + augCount;
                    for (int j = 0; j < augCount; j++)
                    {
                        details += "," + someObject.INVENTORY.COMBOS[i].AUGMENTS[j];
                    }
                }

                details += "," + someObject.INVENTORY.AUTOS.Count + "";
                for (int i = 0; i < someObject.INVENTORY.AUTOS.Count; i++)
                {
                    details += "," + someObject.INVENTORY.AUTOS[i].INDEX + "," + someObject.INVENTORY.AUTOS[i].LEVEL;

                    int augCount = someObject.INVENTORY.AUTOS[i].AUGMENTS.Count;
                    details += "," + augCount;
                    for (int j = 0; j < augCount; j++)
                    {
                        details += "," + someObject.INVENTORY.AUTOS[i].AUGMENTS[j];
                    }
                }

                details += "," + someObject.INVENTORY.OPPS.Count + "";
                for (int i = 0; i < someObject.INVENTORY.OPPS.Count; i++)
                {
                    details += "," + someObject.INVENTORY.OPPS[i].INDEX + "," + someObject.INVENTORY.OPPS[i].LEVEL;
                }

                details += "," + someObject.INVENTORY.ITEMS.Count + "";
                for (int i = 0; i < someObject.INVENTORY.ITEMS.Count; i++)
                {
                    details += "," + someObject.INVENTORY.ITEMS[i].INDEX + "," + someObject.INVENTORY.ITEMS[i].LEVEL;
                }

                details += "," + someObject.INVENTORY.EFFECTS.Count + "";
                for (int i = 0; i < someObject.INVENTORY.EFFECTS.Count; i++)
                {
                    details += "," + someObject.INVENTORY.EFFECTS[i].EFFECT + "," + someObject.INVENTORY.EFFECTS[i].TURNS;
                }
                //item.DEATS = details;
                // item.UpdateDesc();

                return details;
            }
        }


        return null;
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
                    newHazard.id = id;
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

                    newHazard.HTYPE = (HazardType)id;

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
                    newEnemy.id = id;
                    newEnemy.BASE_STATS.Reset(true);
                    newEnemy.STATS.Reset(true);
                    newEnemy.DEAD = false;
                    int fileIndex = 1;
                    newEnemy.FullName = parsed[fileIndex];
                    fileIndex++;

                    string[] factionName = newEnemy.FullName.Split(' ');
                    if (factionName.Length > 0)
                    {
                        if (factionName[0].ToLower() == "fairy")
                        {
                            newEnemy.FACTION = Faction.fairy;
                        }
                        else
                        {
                            newEnemy.FACTION = Faction.enemy;
                        }
                    }
                    newEnemy.BASE_STATS.LEVEL = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    newEnemy.BASE_STATS.MAX_HEALTH = Int32.Parse(parsed[fileIndex]);
                    newEnemy.STATS.HEALTH = newEnemy.BASE_STATS.MAX_HEALTH;
                    fileIndex++;
                    newEnemy.BASE_STATS.MAX_MANA = Int32.Parse(parsed[fileIndex]);
                    newEnemy.STATS.MANA = newEnemy.BASE_STATS.MAX_MANA;
                    fileIndex++;
                    newEnemy.BASE_STATS.MAX_FATIGUE = Int32.Parse(parsed[fileIndex]);
                    newEnemy.STATS.FATIGUE = 0;
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
                    newEnemy.BASE_STATS.DEX = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    newEnemy.BASE_STATS.MOVE_DIST = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofskills = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofweapons = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofarmors = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofItems = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;

                    EPCluster cluster = EPCluster.natural;
                    if (Enum.TryParse<EPCluster>(parsed[fileIndex], out cluster))
                    {
                        newEnemy.personality = Common.GetRandomType(cluster);
                    }
                    else
                    {
                        Enum.TryParse<EPType>(parsed[fileIndex], out newEnemy.personality);
                    }

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

                    for (int i = 0; i < numofItems; i++)
                    {
                        GetItem(Int32.Parse(parsed[fileIndex]), newEnemy);
                        fileIndex++;
                    }
                }
            }




            // reader.Close();
        }
    }
    public LivingObject GetLiving(int id, LivingObject newEnemy)
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
                    newEnemy.STATS.HEALTH = newEnemy.BASE_STATS.MAX_HEALTH;
                    fileIndex++;
                    newEnemy.BASE_STATS.MAX_MANA = Int32.Parse(parsed[fileIndex]);
                    newEnemy.STATS.MANA = newEnemy.BASE_STATS.MAX_MANA;
                    fileIndex++;
                    newEnemy.BASE_STATS.MAX_FATIGUE = Int32.Parse(parsed[fileIndex]);
                    newEnemy.STATS.FATIGUE = 0;
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
                    newEnemy.BASE_STATS.DEX = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    newEnemy.BASE_STATS.MOVE_DIST = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofskills = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofweapons = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofarmors = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    int numofitems = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    //skip personality
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
                    for (int i = 0; i < numofitems; i++)
                    {
                        GetItem(Int32.Parse(parsed[fileIndex]), newEnemy);
                        fileIndex++;
                    }
                }
            }
        }
        return newEnemy;
    }

    public GridObject GetObject(int id, GridObject newObject)
    {


        if (!newObject.GetComponent<BaseStats>())
        {
            newObject.BASE_STATS = newObject.gameObject.AddComponent<BaseStats>();
        }

        if (!newObject.GetComponent<ModifiedStats>())
        {
            newObject.STATS = newObject.gameObject.AddComponent<ModifiedStats>();
        }
        BaseStats baseStats = newObject.GetComponent<BaseStats>();
        ModifiedStats modStats = newObject.GetComponent<ModifiedStats>();
        string lines = "";
        if (objDictionary.TryGetValue(id, out lines))
        {
            string[] parsed = lines.Split(',');
            if (Int32.Parse(parsed[0]) == id)
            {
                baseStats.Reset(true);
                modStats.Reset(true);
                newObject.id = id;
                int fileIndex = 1;
                newObject.FullName = parsed[fileIndex];
                newObject.name = newObject.FullName;

                fileIndex++;
                baseStats.MAX_HEALTH = Int32.Parse(parsed[fileIndex]);
                modStats.HEALTH = baseStats.MAX_HEALTH;
                fileIndex++;
                baseStats.DEFENSE = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                baseStats.RESIESTANCE = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                baseStats.SPEED = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                baseStats.DEX = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                newObject.FACTION = (Faction)Enum.Parse(typeof(Faction), parsed[fileIndex]);
                baseStats.MOVE_DIST = 0;

            }
        }
        return newObject;
    }

    public InteractableObject GetInteractable(int id, InteractableObject newObject)
    {


        if (!newObject.GetComponent<BaseStats>())
        {
            newObject.BASE_STATS = newObject.gameObject.AddComponent<BaseStats>();
        }

        if (!newObject.GetComponent<ModifiedStats>())
        {
            newObject.STATS = newObject.gameObject.AddComponent<ModifiedStats>();
        }
        BaseStats baseStats = newObject.GetComponent<BaseStats>();
        ModifiedStats modStats = newObject.GetComponent<ModifiedStats>();
        string lines = "";
        if (objDictionary.TryGetValue(id, out lines))
        {
            string[] parsed = lines.Split(',');
            if (Int32.Parse(parsed[0]) == id)
            {
                baseStats.Reset(true);
                modStats.Reset(true);
                newObject.id = id;
                int fileIndex = 1;
                newObject.FullName = parsed[fileIndex];
                newObject.name = newObject.FullName;

                fileIndex++;
                baseStats.MAX_HEALTH = Int32.Parse(parsed[fileIndex]);
                modStats.HEALTH = baseStats.MAX_HEALTH;
                fileIndex++;
                baseStats.DEFENSE = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                baseStats.RESIESTANCE = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                baseStats.SPEED = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                baseStats.DEX = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                newObject.FACTION = (Faction)Enum.Parse(typeof(Faction), parsed[fileIndex]);
                baseStats.MOVE_DIST = 0;
                fileIndex++;
                newObject.reaction = (Interaction)Enum.Parse(typeof(Interaction), parsed[fileIndex]);
                fileIndex++;
                newObject.range = (RangeType)Enum.Parse(typeof(RangeType), parsed[fileIndex]);
                fileIndex++;
                newObject.onlyOnce = Boolean.Parse(parsed[fileIndex]);
                fileIndex++;
                newObject.targetTileIndex = Int32.Parse(parsed[fileIndex]);
                fileIndex++;
                newObject.useTargetIndex = Boolean.Parse(parsed[fileIndex]);

            }
        }
        return newObject;
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
                    living.id = id;
                    living.STATS.Reset(true);
                    living.FullName = parsed[fileIndex];
                    fileIndex++;
                    living.BASE_STATS.LEVEL = Int32.Parse(parsed[fileIndex]);
                    fileIndex++;
                    living.BASE_STATS.MAX_HEALTH = Int32.Parse(parsed[fileIndex]);
                    living.STATS.HEALTH = living.BASE_STATS.MAX_HEALTH;
                    fileIndex++;
                    living.BASE_STATS.MAX_MANA = Int32.Parse(parsed[fileIndex]);
                    living.STATS.MANA = living.BASE_STATS.MAX_MANA;
                    fileIndex++;
                    living.BASE_STATS.MAX_FATIGUE = Int32.Parse(parsed[fileIndex]);
                    living.STATS.FATIGUE = 0;
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
                    living.BASE_STATS.DEX = Int32.Parse(parsed[fileIndex]);
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
        map.startIndexes = new List<int>();
        map.objMapIndexes = new List<int>();
        map.objIds = new List<int>();
        map.enemyIds = new List<int>();
        map.unOccupiedIndexes = new List<int>();
        map.hazardIds = new List<int>();
        map.specialiles = new List<TileType>();
        map.specialExtra = new List<int>();
        map.tileIndexes = new List<int>();
        map.tilesInShadow = new List<int>();
        string lines = "";
        if (mapDictionary.TryGetValue(id, out lines))
        {
            string[] parsed = lines.Split(',');
            if (Int32.Parse(parsed[0]) == id)
            {
                map.mapIndex = id;
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

                //        int numOfDoors = Int32.Parse(parsed[fileIndex]);
                //        fileIndex++;
                //        for (int i = 0; i < numOfDoors; i++)
                //        {
                //            map.doorIndexes.Add(Int32.Parse(parsed[fileIndex]));
                //            fileIndex++;
                //        }

                //        for (int i = 0; i < numOfDoors; i++)
                //        {
                //            map.roomNames.Add(parsed[fileIndex]);
                //            fileIndex++;
                //        }


                //        for (int i = 0; i < numOfDoors; i++)
                //        {
                //            map.roomIndexes.Add(Int32.Parse(parsed[fileIndex]));
                //            fileIndex++;
                //        }


                //        for (int i = 0; i < numOfDoors; i++)
                //        {
                //            map.startIndexes.Add(Int32.Parse(parsed[fileIndex]));
                //            fileIndex++;
                //        }

                //        int numOfEnemies = Int32.Parse(parsed[fileIndex]);
                //        fileIndex++;
                //        for (int i = 0; i < numOfEnemies; i++)
                //        {
                //            map.enemyIndexes.Add(Int32.Parse(parsed[fileIndex]));
                //            fileIndex++;
                //        }

                //        int numOfHazards = Int32.Parse(parsed[fileIndex]);
                //        fileIndex++;
                //        for (int i = 0; i < numOfHazards; i++)
                //        {
                //            map.hazardIndexes.Add(Int32.Parse(parsed[fileIndex]));
                //            fileIndex++;
                //        }

                //        int numOfShops = Int32.Parse(parsed[fileIndex]);
                //        fileIndex++;
                //        for (int i = 0; i < numOfShops; i++)
                //        {
                //            map.shopIndexes.Add(Int32.Parse(parsed[fileIndex]));
                //            fileIndex++;
                //        }

                //        int numOfObjs = Int32.Parse(parsed[fileIndex]);
                //        fileIndex++;
                //        for (int i = 0; i < numOfObjs; i++)
                //        {
                //            map.objMapIndexes.Add(Int32.Parse(parsed[fileIndex]));
                //            fileIndex++;
                //        }


                //        for (int i = 0; i < numOfObjs; i++)
                //        {
                //            map.objIds.Add(Int32.Parse(parsed[fileIndex]));
                //            fileIndex++;
                //        }
            }
        }

        return map;
    }

    public MapData GetMapData(string name)
    {
        string shrtname = name;
        string[] subs = shrtname.Split(' ');
        shrtname = "";
        for (int i = 0; i < subs.Length; i++)
        {
            shrtname += subs[i];
        }

        data.mapName = name;
        data.unOccupiedIndexes.Clear();
        data.events.Clear();
        data.eventMap = false;
        data.doorIndexes.Clear();
        data.roomNames.Clear();
        data.roomIndexes.Clear();
        data.enemyIndexes.Clear();
        data.glyphIndexes.Clear();
        data.glyphIds.Clear();
        data.shopIndexes.Clear();
        data.startIndexes.Clear();
        data.objMapIndexes.Clear();
        data.objIds.Clear();
        data.yElevation = -1;
        data.xElevation = -1;
        data.yMinRestriction = -1;
        data.yMaxRestriction = -1;
        data.xMinRestriction = -1;
        data.xMaxRestriction = -1;
        data.revealCount = 0;

        data.specialExtra.Clear();
        data.tilesInShadow.Clear();
        data.specialiles.Clear();
        data.specialTileIndexes.Clear();
        TextAsset asset = Resources.Load("maps/" + shrtname) as TextAsset;
        if (asset)
        {
            string file = asset.text;
            string[] mapLines = file.Split('\n');



            for (int i = 0; i < mapLines.Length; i++)
            {
                string line = mapLines[i];
                if (line.Length > 0)
                    if (line[0] != '-')
                    {
                        string[] parsed = line.Split(',');

                        switch (parsed[0])
                        {

                            case "r":
                                for (int j = 0; j < parsed.Length - 1; j++)
                                {
                                    data.unOccupiedIndexes.Add(Int32.Parse(parsed[j + 1]));
                                }
                                break;
                            case "n":
                                data.eventMap = bool.Parse(parsed[1]);
                                break;
                            case "i":
                                {
                                    data.mapIndex = Int32.Parse(parsed[1]);
                                }
                                break;
                            case "t":
                                {
                                    int textureNum = Int32.Parse(parsed[1]);
                                    data.texture = Resources.LoadAll<Texture>("Textures/")[textureNum];
                                }
                                break;
                            case "w":
                                {
                                    data.width = Int32.Parse(parsed[1]);
                                }
                                break;
                            case "h":
                                {
                                    data.height = Int32.Parse(parsed[1]);
                                }
                                break;
                            case "d":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.doorIndexes.Add(Int32.Parse(parsed[j + 1]));
                                    }
                                }
                                break;
                            case "m":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.roomNames.Add(parsed[j + 1]);
                                    }
                                }
                                break;
                            case "l":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.roomIndexes.Add(Int32.Parse(parsed[j + 1]));
                                    }
                                }
                                break;
                            case "x":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.startIndexes.Add(Int32.Parse(parsed[j + 1]));
                                    }
                                }
                                break;
                            case "e":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.enemyIndexes.Add(Int32.Parse(parsed[j + 1]));
                                    }
                                }
                                break;
                            case "ei":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.EnemyIds.Add(Int32.Parse(parsed[j + 1]));
                                    }
                                }
                                break;
                            case "ex":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.specialExtra.Add(Int32.Parse(parsed[j + 1]));

                                    }
                                }
                                break;
                            case "st":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.tilesInShadow.Add(Int32.Parse(parsed[j + 1]));

                                    }
                                }
                                break;
                            case "g":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.glyphIndexes.Add(Int32.Parse(parsed[j + 1]));
                                    }
                                }
                                break;
                            case "gi":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.glyphIds.Add(Int32.Parse(parsed[j + 1]));
                                    }
                                }
                                break;
                            case "gr":
                                {
                                    data.revealCount = Int32.Parse(parsed[1]);
                                    //for (int j = 0; j < parsed.Length - 1; j++)
                                    //{
                                    //    data.glyphIds.Add(Int32.Parse(parsed[j + 1]));
                                    //}
                                }
                                break;
                            case "s":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.shopIndexes.Add(Int32.Parse(parsed[j + 1]));
                                    }
                                }
                                break;
                            case "o":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.objMapIndexes.Add(Int32.Parse(parsed[j + 1]));
                                    }
                                }
                                break;

                            case "oi":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.objIds.Add(Int32.Parse(parsed[j + 1]));
                                    }
                                }
                                break;
                            case "yE":
                                {
                                    data.yElevation = float.Parse(parsed[1]);
                                }
                                break;
                            case "xE":
                                {
                                    data.xElevation = float.Parse(parsed[1]);
                                }
                                break;
                            case "yIr":
                                {
                                    data.yMinRestriction = Int32.Parse(parsed[1]);
                                }
                                break;
                            case "yAr":
                                {
                                    data.yMaxRestriction = Int32.Parse(parsed[1]);
                                }
                                break;
                            case "xIr":
                                {
                                    data.xMinRestriction = Int32.Parse(parsed[1]);
                                }
                                break;
                            case "xAr":
                                {
                                    data.xMaxRestriction = Int32.Parse(parsed[1]);
                                }
                                break;
                            case "stt":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.specialiles.Add((TileType)Enum.Parse(typeof(TileType), parsed[j + 1]));
                                    }
                                }
                                break;
                            case "sti":
                                {
                                    for (int j = 0; j < parsed.Length - 1; j++)
                                    {
                                        data.specialTileIndexes.Add(Int32.Parse(parsed[j + 1]));
                                    }
                                }
                                break;
                        }

                    }
            }

        }
        return data;
    }
    public MapData GetBlankMap()
    {
        data.mapName = "";
        data.unOccupiedIndexes.Clear();
        data.events.Clear();
        data.eventMap = false;
        data.doorIndexes.Clear();
        data.roomNames.Clear();
        data.roomIndexes.Clear();
        data.enemyIndexes.Clear();
        data.glyphIndexes.Clear();
        data.glyphIds.Clear();
        data.shopIndexes.Clear();
        data.startIndexes.Clear();
        data.objMapIndexes.Clear();
        data.objIds.Clear();
        data.yElevation = -1;
        data.xElevation = -1;
        data.yMinRestriction = -1;
        data.yMaxRestriction = -1;
        data.xMinRestriction = -1;
        data.xMaxRestriction = -1;
        data.revealCount = 0;

        data.specialExtra.Clear();
        data.tilesInShadow.Clear();
        data.specialiles.Clear();
        data.specialTileIndexes.Clear();
        return data;
    }
    public SceneContainer AddInterruptToScene(SceneContainer scene, int interrupt, SceneEvent sevent, int edata)
    {     
        scene.eventIndexs.Add(interrupt);
        SceneEventContainer SEC = new SceneEventContainer();
        SEC.intercept = interrupt;
        SEC.scene = sevent;
        SEC.data = edata;
        scene.sceneEvents.Add(SEC);

        return scene;
    }
    public SceneContainer AddDataToScene(SceneContainer scene, string name, string text, Sprite face)
    {
        scene.speakerNames.Add(name);
        scene.speakertext.Add(text);
        scene.speakerFace.Add(face);

        return scene;
    }
    public SceneContainer GenerateScene(string name, string text, Sprite face)
    {
        scene.speakertext.Clear();
        scene.speakerNames.Clear();
        scene.speakerFace.Clear();
        scene.sceneEvents.Clear();
        scene.eventIndexs.Clear();
        scene.index = 0;

        scene.speakerNames.Add(name);
        scene.speakertext.Add(text);
        scene.speakerFace.Add(face);

        return scene;
    }
    public SceneContainer GetSceneData(string name)
    {
        scene.soundTrack = -1;
        scene.speakertext.Clear();
        scene.speakerNames.Clear();
        scene.speakerFace.Clear();
        scene.sceneEvents.Clear();
        scene.eventIndexs.Clear();
        TextAsset asset = Resources.Load("Dialogue/" + name) as TextAsset;
        if (asset)
        {
            string file = asset.text;
            string[] mapLines = file.Split('\n');



            for (int i = 0; i < mapLines.Length; i++)
            {
                string line = mapLines[i];
                if (line != String.Empty)
                    if (line[0] != '-')
                    {


                        string[] parsed = line.Split(',');

                        switch (parsed[0])
                        {

                            case "a":
                                {
                                    if (parsed[1] == "Jax?")
                                    {
                                        string actorName = "Jax Drix";
                                        scene.speakerNames.Add("Jax?");
                                        string shrtName = Common.GetShortName(actorName);
                                        if (parsed.Length > 3)
                                        {
                                            scene.speakerFace.Add(Resources.LoadAll<Sprite>("" + shrtName + "/Face/")[Int32.Parse(parsed[3])]);

                                        }
                                        else
                                        {
                                            scene.speakerFace.Add(Resources.LoadAll<Sprite>("" + shrtName + "/Face/")[0]);

                                        }
                                    }
                                    else
                                    {

                                        int actorNum = Int32.Parse(parsed[1]);
                                        string lines = "";
                                        if (actorDictionary.TryGetValue(actorNum, out lines))
                                        {
                                            string[] actorParse = lines.Split(',');
                                            string actorName = actorParse[1];
                                            scene.speakerNames.Add(actorName);
                                            string shrtName = Common.GetShortName(actorName);
                                            if (parsed.Length > 3)
                                            {
                                                scene.speakerFace.Add(Resources.LoadAll<Sprite>("" + shrtName + "/Face/")[Int32.Parse(parsed[3])]);

                                            }
                                            else
                                            {

                                                scene.speakerFace.Add(Resources.LoadAll<Sprite>("" + shrtName + "/Face/")[0]);

                                            }
                                        }
                                    }

                                    parsed[2] = parsed[2].Replace(';', ',');
                                    scene.speakertext.Add(parsed[2]);
                                }
                                break;
                            case "e":
                                {
                                    int enemyNum = Int32.Parse(parsed[1]);
                                    string lines = "";
                                    if (enemyDictionary.TryGetValue(enemyNum, out lines))
                                    {

                                        string[] actorParse = lines.Split(',');
                                        string actorName = actorParse[1];
                                        scene.speakerNames.Add(actorName);
                                        string shrtName = Common.GetShortName(actorName);
                                        if (parsed.Length > 3)
                                        {
                                            scene.speakerFace.Add(Resources.LoadAll<Sprite>("" + shrtName + "/Face/")[Int32.Parse(parsed[3])]);

                                        }
                                        else
                                        {
                                            scene.speakerFace.Add(Resources.LoadAll<Sprite>("" + shrtName + "/Face/")[0]);

                                        }
                                    }
                                    parsed[2] = parsed[2].Replace(';', ',');
                                    scene.speakertext.Add(parsed[2]);
                                }

                                break;

                            case "s":
                                {
                                    scene.soundTrack = Int32.Parse(parsed[1]);
                                }
                                break;
                            case "i":
                                {
                                    //text number, event, event data
                                    int interrupt = Int32.Parse(parsed[1]);
                                    SceneEvent sevent = (SceneEvent)Enum.Parse(typeof(SceneEvent), parsed[2]);
                                    int edata = Int32.Parse(parsed[3]);
                                    scene.eventIndexs.Add(interrupt);
                                    SceneEventContainer SEC = new SceneEventContainer();
                                    SEC.intercept = interrupt;
                                    SEC.scene = sevent;
                                    SEC.data = edata;
                                    scene.sceneEvents.Add(SEC);
                                }
                                break;
                        }

                    }
            }

        }
        return scene;
    }
}
