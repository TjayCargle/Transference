/*
The MIT License

Copyright (c) 2020 DoublSB
https://github.com/DoublSB/UnityDialogAsset

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
namespace Doublsb.Dialog
{
    public class DialogManager : MonoBehaviour
    {
        //================================================
        //Public Variable
        //================================================
        [Header("Game Objects")]
        public GameObject Printer;
        public GameObject Characters;

        [Header("UI Objects")]
        public TextMeshProUGUI Printer_Text;

        [Header("Audio Objects")]
        public AudioSource SEAudio;
        public AudioSource CallAudio;

        [Header("Preference")]
        public float Delay = 0.1f;

        [Header("Selector")]
        public GameObject Selector;
        public GameObject SelectorItem;
        public Text SelectorItemText;

        //  [HideInInspector]
        public State state;

        [HideInInspector]
        public string Result;

        [Header("Extras")]
        public bool autoAdvance = true;
        public GameObject speaker1Nameplate = null;
        public GameObject speaker2Nameplate = null;


        //================================================
        //Private Method
        //================================================
        private Character _current_Character;
        private Character _first_Character;
        private Character _second_Character;
        private DialogData _current_Data;

        private float _currentDelay;
        private float _lastDelay;
        private float _autoDelay = 0.5f;
        private Coroutine _textingRoutine;
        private Coroutine _printingRoutine;
        private ManagerScript _manager;
        //================================================
        //Public Method
        //================================================
  
        public Character FIRST
        {
            get { return _first_Character; }
        }

        public Character SECOND
        {
            get { return _second_Character; }
        }

        #region Show & Hide
        public void Show(DialogData Data)
        {
            _current_Data = Data;
            _find_character(Data.Character);

            if (_current_Character != null)
            {
                _emote("normal");
                _current_Character.gameObject.SetActive(true);
                _current_Character.myImage.color = Color.white;
            }
            if (_first_Character != null && _second_Character != null)
            {
                if (_current_Character != _second_Character)
                {
                    _second_Character.myImage.color = Common.dark;
                    _current_Character.isFirst = true;
               

                }

                if (_current_Character != _first_Character)
                {
                    _first_Character.myImage.color = Common.dark;
                    _current_Character.isFirst = false;
                    if (speaker1Nameplate != null)
                        speaker1Nameplate.SetActive(true);
                }

            }

            _textingRoutine = StartCoroutine(Activate());
        }

        public void Show(List<DialogData> Data)
        {
            Printer.SetActive(true);
            Characters.SetActive(true);
            StopCoroutine("Activate_List");
            _select("1");
            _move("0");
            _select("0");
            _move("0");
            _hide("0");
            _hide("1");
            StartCoroutine(Activate_List(Data));

        }

        public void Click_Window()
        {
            switch (state)
            {
                case State.Active:
                    StartCoroutine(_skip()); break;

                case State.Wait:
                    if (_current_Data.SelectList.Count <= 0) Hide(); break;
            }
        }

        public void Hide()
        {
            if (_textingRoutine != null)
                StopCoroutine(_textingRoutine);

            if (_printingRoutine != null)
                StopCoroutine(_printingRoutine);
            if (autoAdvance == false)
            {

                Printer.SetActive(false);
                Characters.SetActive(false);
                Selector.SetActive(false);
            }

            state = State.Deactivate;

            if (_current_Data.Callback != null)
            {
                _current_Data.Callback.Invoke();
                _current_Data.Callback = null;
            }

        }
        #endregion

        #region Selector

        public void Select(int index)
        {
            Result = _current_Data.SelectList.GetByIndex(index).Key;
            Hide();
        }

        #endregion

        #region Sound
        private void Awake()
        {
            _initialize();
        }
        public void Play_ChatSE()
        {
            if (_current_Character != null)
            {
                SEAudio.clip = _current_Character.ChatSE[UnityEngine.Random.Range(0, _current_Character.ChatSE.Length)];
                SEAudio.Play();
            }
        }

        public void Play_CallSE(string SEname)
        {
            if (_current_Character != null)
            {
                var FindSE = Array.Find(_current_Character.CallSE, (SE) => SE.name == SEname);

                CallAudio.clip = FindSE;
                CallAudio.Play();
            }
        }

        #endregion

        #region Speed

        public void Set_Speed(string speed)
        {
            switch (speed)
            {
                case "up":
                    _currentDelay -= 0.25f;
                    if (_currentDelay <= 0) _currentDelay = 0.001f;
                    break;

                case "down":
                    _currentDelay += 0.25f;
                    break;

                case "init":
                    _currentDelay = Delay;
                    break;

                default:
                    _currentDelay = float.Parse(speed);
                    break;
            }

            _lastDelay = _currentDelay;
        }

        public void Set_AutoSpeed(string speed)
        {
            switch (speed)
            {
                case "up":
                    _autoDelay -= 0.25f;
                    if (_currentDelay <= 0) _currentDelay = 0.001f;
                    break;

                case "down":
                    _autoDelay += 0.25f;
                    break;

                case "init":
                    _autoDelay = 0.5f;
                    break;

                default:
                    _autoDelay = float.Parse(speed);
                    break;
            }

            _lastDelay = _currentDelay;
        }

        #endregion

        //================================================
        //Private Method
        //================================================

        private void _find_character(string name)
        {
            if (name != string.Empty)
            {
                Transform Child = Characters.transform.Find(name);
                if (Child != null) _current_Character = Child.GetComponent<Character>();
            }
        }

        private void _initialize()
        {
            _currentDelay = Delay;
            _lastDelay = 0.1f;
            Printer_Text.text = string.Empty;

            Printer.SetActive(true);
           // Characters.SetActive(_current_Character != null);
            Character[] chars = Characters.GetComponentsInChildren<Character>();

            if (chars.Length == 2)
            {
                _first_Character = chars[0];
                _second_Character = chars[1];
                _current_Character = _first_Character;
                _current_Character.isFirst = true;
            }

            if (_current_Character != null)
            {
                _current_Character.gameObject.SetActive(true);
            }
            
            if(_manager == null)
            {
                _manager = GameObject.FindObjectOfType<ManagerScript>();
            }
        }

        private void _init_selector()
        {
            _clear_selector();

            if (_current_Data.SelectList.Count > 0)
            {
                Selector.SetActive(true);

                for (int i = 0; i < _current_Data.SelectList.Count; i++)
                {
                    _add_selectorItem(i);
                }
            }

            else Selector.SetActive(false);
        }

        private void _clear_selector()
        {
            for (int i = 1; i < Selector.transform.childCount; i++)
            {
                Destroy(Selector.transform.GetChild(i).gameObject);
            }
        }

        private void _add_selectorItem(int index)
        {
            SelectorItemText.text = _current_Data.SelectList.GetByIndex(index).Value;

            var NewItem = Instantiate(SelectorItem, Selector.transform);
            NewItem.GetComponent<Button>().onClick.AddListener(() => Select(index));
            NewItem.SetActive(true);
        }

        #region Show Text

        private IEnumerator Activate_List(List<DialogData> DataList)
        {
            state = State.Active;

            foreach (var Data in DataList)
            {
                Show(Data);
                _init_selector();

                while (state != State.Deactivate) { yield return null; }
            }
            Printer.SetActive(false);
            Characters.SetActive(false);
            Selector.SetActive(false);
        }

        private void OnDisable()
        {
            StopCoroutine("Activate_List");
        }

        private IEnumerator Activate()
        {


            state = State.Active;
            if (_current_Data.Emote != null)
            {
                _current_Data.Commands.Insert(0, new DialogCommand(Command.emote, "-1"));
            }

            foreach (var item in _current_Data.Commands)
            {
                switch (item.Command)
                {
                    case Command.print:
                        yield return _printingRoutine = StartCoroutine(_print(item.Context));
                        break;

                    case Command.color:
                        _current_Data.Format.Color = item.Context;
                        break;

                    case Command.emote:
                        if (_current_Data.Emote != null)
                        {
                            _emote(_current_Data.Emote);
                        }
                        else
                        {
                            _emote(item.Context);
                        }
                        yield return new WaitForSeconds(_currentDelay * 25);
                        break;

                    case Command.size:
                        _current_Data.Format.Resize(item.Context);
                        break;

                    case Command.sound:
                        Play_CallSE(item.Context);
                        break;

                    case Command.speed:
                        Set_Speed(item.Context);
                        break;

                    case Command.click:
                        if (autoAdvance == false)
                            yield return _waitInput();
                        break;

                    case Command.close:
                        Hide();
                        yield break;

                    case Command.wait:
                        yield return new WaitForSeconds(float.Parse(item.Context));
                        break;
                    case Command.reveal:
                        _reveal(item.Context);
                        yield return new WaitForSeconds(_autoDelay);
                        break;
                    case Command.hide:
                        _hide(item.Context);
                        yield return new WaitForSeconds(_autoDelay);
                        break;
                    case Command.select:
                        _select(item.Context);
                        yield return new WaitForSeconds(_autoDelay);
                        break;
                    case Command.swap:
                        _swap(item.Context);
                        yield return new WaitForSeconds(_autoDelay);
                        break;
                    case Command.move:
                        _move(item.Context);
                        yield return new WaitForSeconds(_autoDelay);
                        break;
                    case Command.autoDelay:
                        Set_AutoSpeed(item.Context);
                        yield return new WaitForSeconds(_autoDelay);
                        break;
                    case Command.setAsFirst:
                        {
                            if(Characters != null)
                            {
                                Characters.transform.GetChild(0).name = item.Context;
                            }
                        }
                        break;
                    case Command.setAsSecond:
                        {
                            if (Characters != null)
                            {
                                Characters.transform.GetChild(1).name = item.Context;
                            }
                        }
                        break;
                }
            }

            state = State.Wait;
            if (autoAdvance == true)
            {
                yield return new WaitForSeconds(_currentDelay * 50);
                if (_current_Data.SelectList.Count <= 0)
                {
                    Hide();
                    if (_manager != null)
                    {
                        _manager.NextScene();
                    }
                }
            }
        }

        private IEnumerator _waitInput()
        {
            while (!Input.GetMouseButtonDown(0)) yield return null;
            _currentDelay = _lastDelay;
        }

        private IEnumerator _print(string Text)
        {
            _current_Data.PrintText += _current_Data.Format.OpenTagger;

            for (int i = 0; i < Text.Length; i++)
            {
                _current_Data.PrintText += Text[i];
                Printer_Text.text = _current_Data.PrintText + _current_Data.Format.CloseTagger;

                if (Text[i] != ' ') Play_ChatSE();
                if (_currentDelay != 0) yield return new WaitForSeconds(_currentDelay);
            }

            _current_Data.PrintText += _current_Data.Format.CloseTagger;
        }

        public void _emote(string Text)
        {
            _current_Character.GetComponent<Image>().sprite = _current_Character.Emotion.Data[Text];
        }

        public void _emote(Sprite newSprite)
        {
            _current_Character.GetComponent<Image>().sprite = newSprite;
        }

        //Additions from TJay
        public void _reveal(string Text)
        {
            int index = 0;
            Int32.TryParse(Text, out index);

            if (index == 1)
            {
                if (_second_Character != null)
                {
                    _second_Character.gameObject.SetActive(true);
                    if (speaker2Nameplate != null)
                        speaker2Nameplate.SetActive(true);
                   // if (person2Name != null)
                        //person2Name.text = _second_Character.name;
                }
            }
            else
            {
                if (_first_Character != null)
                {
                    _first_Character.gameObject.SetActive(true);
                }
            }
        }
        public void _hide(string Text)
        {
            int index = 0;
            Int32.TryParse(Text, out index);

            if (index == 1)
            {
                if (_second_Character != null)
                {
                    _second_Character.gameObject.SetActive(false);
                    if (speaker2Nameplate != null)
                        speaker2Nameplate.SetActive(false);
                }
            }
            else
            {
                if (_first_Character != null)
                {
                    _first_Character.gameObject.SetActive(false);
                }
            }
        }

        public void _select(string Text)
        {
            int index = 0;
            Int32.TryParse(Text, out index);

            if (index == 1)
            {
                if (_second_Character != null)
                {
                    _current_Character = _second_Character;
                }
            }
            else
            {
                if (_first_Character != null)
                {
                    _current_Character = _first_Character;
                }
            }
        }

        public void _swap(string Text)
        {
            if (_current_Character != null)
            {
                if (_first_Character != null && _second_Character != null)
                {
                    if (_current_Character == _second_Character)
                    {
                        _current_Character = _first_Character;
                        _second_Character.gameObject.SetActive(false);
                        _first_Character.gameObject.SetActive(true);
                        _current_Character.isFirst = true;
                    }

                    else if (_current_Character == _first_Character)
                    {
                        _current_Character = _second_Character;
                        _first_Character.gameObject.SetActive(false);
                        _second_Character.gameObject.SetActive(true);
                        _current_Character.isFirst = false;
                    }
                    else
                    {
                        _current_Character = _first_Character;
                        _second_Character.gameObject.SetActive(false);
                        _first_Character.gameObject.SetActive(true);
                        _current_Character.isFirst = true;
                    }
                }


            }



        }
        public void _move(string Text)
        {
            float dir = 0.0f;

            Single.TryParse(Text, out dir);
            RectTransform rect = _current_Character.GetComponent<RectTransform>();

            LeanTween.move(rect, new Vector3(dir, 0.0f, 0.0f), 0.2f).setOnComplete(x => { });
            //LeanTween.moveX(_current_Character.GetComponent<RectTransform>(), dir, 0.2f);
        }
        private IEnumerator _skip()
        {
            if (_current_Data.isSkippable)
            {
                _currentDelay = 0;
                while (state != State.Wait) yield return null;
                _currentDelay = Delay;
            }
        }

        #endregion

    }
}