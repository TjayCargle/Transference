using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;

public class TestMessage : MonoBehaviour
{
    public DialogManager DialogManager;

    public GameObject[] Example;

    private void Awake()
    {
        var dialogTexts = new List<DialogData>();

        //dialogTexts.Add(new DialogData("/size:up/Hi, /size:init/my name is Li.", "Li"));

        //dialogTexts.Add(new DialogData("I am Sa. Popped out to let you know Asset can show other characters.", "Sa"));
        
        //dialogTexts.Add(new DialogData("This Asset, The D'Dialog System has many features.", "Li"));

        //dialogTexts.Add(new DialogData("/hide:1/You can easily change text /color:red/color, /color:white/and /size:up//size:up/size/size:init/ like this.", "Li", () => Show_Example(0)));

        //dialogTexts.Add(new DialogData("Just put the command in the string!/reveal:1//move:-850.0/", "Li", () => Show_Example(1)));

        //dialogTexts.Add(new DialogData("/move:-50/You can also change the character's sprite /emote:Sad/like this, /click//emote:Happy/Smile.", "Sa",  () => Show_Example(2)));

        //dialogTexts.Add(new DialogData("If you need an emphasis effect, /wait:0.5/wait... /click/or click command.", "Sa", () => Show_Example(3)));

        //dialogTexts.Add(new DialogData("/move:0.0/Text can be /select:1//move:0//speed:down/slow... /speed:init//speed:up/or fast.", "Li", () => Show_Example(4)));

        //dialogTexts.Add(new DialogData("You don't even need to click on the window like this.../speed:0.1/ tada!/close/", "Li", () => Show_Example(5)));

        //dialogTexts.Add(new DialogData("/speed:0.1/AND YOU CAN'T SKIP THIS SENTENCE./speed:init//hide:1/", "Li", () => Show_Example(6), false));

        //dialogTexts.Add(new DialogData("And here we go, the haha sound! /click//sound:haha/haha.", "Li", () => Hide_Example(6), false));

        //dialogTexts.Add(new DialogData("That's it! Please check the documents. Good luck to you.", "Li"));


        DatabaseManager dm = Common.GetDatabase();
        dm.Setup();
        SceneContainer scene = dm.GetSceneData("JaxFindZeff");
        for (int i = 0; i < scene.speakertext.Count; i++)
        {
            //Debug.Log(scene.speakerFace);
            dialogTexts.Add(new DialogData(scene.speakertext[i], scene.speakerFace[i], scene.speakerNames[i]));
        }

        DialogManager.Show(dialogTexts);
    }

    private void Show_Example(int index)
    {
        for (int i = 0; i < Example.Length; i++)
        {
            Example[i].SetActive(false);
        }
        Example[index].SetActive(true);
    }

    private void Hide_Example(int index)
    {
        Example[index].SetActive(false);
    }
}
