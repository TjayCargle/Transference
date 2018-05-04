using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DatabaseManager : MonoBehaviour {
 
	// Use this for initialization
	void Start () {
 
	}
	
	// Update is called once per frame
	void Update () {
		
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
        string path = "Assets/Resources/skills.csv";

      //  if(test.GetComponent<InventoryScript>())
        {

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);

        string lines = "";
        reader.ReadLine();
        while (lines != null)
        {
            lines = reader.ReadLine();
            Debug.Log(lines);
            if (lines != null)
            {
                string[] parsed = lines.Split(',');
 
                SkillScript skill = ScriptableObject.CreateInstance<SkillScript>();
                skill.NAME = parsed[1];
                skill.ELEMENT = (Element)Enum.Parse(typeof(Element), parsed[2]);
                skill.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[3]);
                skill.DESC = parsed[4];
                skill.ACCURACY = Int32.Parse(parsed[5]);
                skill.DMG = Int32.Parse(parsed[6]);
                skill.CRIT_RATE = Int32.Parse(parsed[7]);
                    skill.TILES = new System.Collections.Generic.List<Vector2>();
                    skill.TYPE = 4;
                    int count = Int32.Parse(parsed[8]);
                    int index = 9;

                    for(int i = 0; i < count; i++)
                    {
                        Vector2 v = new Vector2();
                        v.x = Int32.Parse(parsed[index]);
                        index++;
                        v.y = Int32.Parse(parsed[index]);
                        index++;
                        skill.TILES.Add(v);
                    }
            }
           

        }
     
        reader.Close();
        }
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
                    string[] parsed = lines.Split(',');
                    if(Int32.Parse(parsed[0]) == id)
                    {

                    SkillScript skill = ScriptableObject.CreateInstance<SkillScript>();
                    skill.NAME = parsed[1];
                    skill.ELEMENT = (Element)Enum.Parse(typeof(Element), parsed[2]);
                    skill.ETYPE = (EType)Enum.Parse(typeof(EType), parsed[3]);
                    skill.DESC = parsed[4];
                    skill.ACCURACY = Int32.Parse(parsed[5]);
                    skill.DMG = Int32.Parse(parsed[6]);
                    skill.CRIT_RATE = Int32.Parse(parsed[7]);
                    skill.TILES = new System.Collections.Generic.List<Vector2>();
                    skill.TYPE = 4;
                    int count = Int32.Parse(parsed[8]);
                    int index = 9;

                    for (int i = 0; i < count; i++)
                    {
                        Vector2 v = new Vector2();
                        v.x = Int32.Parse(parsed[index]);
                        index++;
                        v.y = Int32.Parse(parsed[index]);
                        index++;
                        skill.TILES.Add(v);
                    }
                    livingObject.GetComponent<InventoryScript>().SKILLS.Add(skill);
                    livingObject.GetComponent<InventoryScript>().USEABLES.Add(skill);
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
