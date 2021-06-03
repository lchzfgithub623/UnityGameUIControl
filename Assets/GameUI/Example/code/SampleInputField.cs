using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightFramework.UI;
using UnityEngine.UI;
using System;
using LightFramework;

public class SampleInputField : UIBaseControl {

    private GameUIInputField UIInput;

    protected override void OnInit()
    {

        base.OnInit();
        SetCtrl("UIInputField", ref UIInput);

        UIInput.AddChangedListener(delegate (string s) {
            SampleTest.prompt.ShowText("onValueChanged ->: " + s);
        });

        UIInput.AddEndEditListener(delegate (string s) {
            SampleTest.prompt.ShowText("onEndEdit ->: " + s);
        });
        UIInput.AddValidateListener(delegate (string text, int charIndex, char addedChar) {
            SampleTest.prompt.ShowText("onValidate ->: " + text + ",charIndex:" + charIndex + ",addedChar:" + addedChar);
            return addedChar;
        });


        InputInputCharacterEvent();
        UIDropdownContentTypeEvent();
        UIDropdownLineTypeEvent();
        UIDropdownInputTypeEvent();
        UIDropdownKeyboardTypeEvent();
        UIDropdownCharactorValidateEvent();

        SliderCaretBlinkRateEvent();
        SliderCaretWidthEvent();


    }

    private void InputInputCharacterEvent()
    {
        var input = GetCtrl<GameUIInputField>("InputInputCharacter");
        input.text = UIInput.characterLimit.ToString();

        input.AddChangedListener(delegate (string str) {
            int num = 0;
            int.TryParse(str, out num);
            UIInput.characterLimit = num;
        });
    }

    private void UIDropdownContentTypeEvent()
    {
        var dropdown = GetCtrl<GameUIDropdown>("UIDropdownContentType");
        dropdown.ClearOptions();
        var lineDrop = GetCtrl<GameUIDropdown>("UIDropdownLineType");
        var inputDrop = GetCtrl<GameUIDropdown>("UIDropdownInputType");
        var keyboardDrop = GetCtrl<GameUIDropdown>("UIDropdownKeyboardType");
        var validateDrop = GetCtrl<GameUIDropdown>("UIDropdownCharactorValidate");
        List<InputField.ContentType> list = new List<InputField.ContentType>();
        list.Add(InputField.ContentType.Standard);
        list.Add(InputField.ContentType.Autocorrected);
        list.Add(InputField.ContentType.IntegerNumber);
        list.Add(InputField.ContentType.DecimalNumber);
        list.Add(InputField.ContentType.Alphanumeric);
        list.Add(InputField.ContentType.Name);
        list.Add(InputField.ContentType.EmailAddress);
        list.Add(InputField.ContentType.Password);
        list.Add(InputField.ContentType.Pin);
        list.Add(InputField.ContentType.Custom);
        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
        for (int i = 0; i < list.Count; i++)
        {
            optionList.Add(new Dropdown.OptionData("ContentType." + list[i]));
        }
        dropdown.AddOptions(optionList);

        var val = UIInput.contentType;
        var index = list.FindIndex(t => t == val);
        dropdown.value = index;
        Func<InputField.ContentType, bool> canShow = delegate (InputField.ContentType cy) {
            return cy == InputField.ContentType.Standard || cy == InputField.ContentType.Autocorrected
                || cy == InputField.ContentType.Custom;
        };
        lineDrop.SetActive(canShow(val));
        inputDrop.SetActive(val == InputField.ContentType.Custom);
        keyboardDrop.SetActive(val == InputField.ContentType.Custom);
        validateDrop.SetActive(val == InputField.ContentType.Custom);
        dropdown.AddValueChangedListener(delegate (int i) {
            val = list[i];
            UIInput.contentType = val;
            lineDrop.SetActive(canShow(val));
            inputDrop.SetActive(val == InputField.ContentType.Custom);
            keyboardDrop.SetActive(val == InputField.ContentType.Custom);
            validateDrop.SetActive(val == InputField.ContentType.Custom);
        });
    }

    private void UIDropdownLineTypeEvent()
    {
        var dropdown = GetCtrl<GameUIDropdown>("UIDropdownLineType");
        dropdown.ClearOptions();
        List<InputField.LineType> list = new List<InputField.LineType>();
        list.Add(InputField.LineType.SingleLine);
        list.Add(InputField.LineType.MultiLineSubmit);
        list.Add(InputField.LineType.MultiLineNewline);
        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
        for (int i = 0; i < list.Count; i++)
        {
            optionList.Add(new Dropdown.OptionData("LineType." + list[i]));
        }
        dropdown.AddOptions(optionList);

        var val = UIInput.lineType;
        var index = list.FindIndex(t => t == val);
        dropdown.value = index;

        dropdown.AddValueChangedListener(delegate (int i) {
            val = list[i];
            UIInput.lineType = val;
        });
    }

    private void UIDropdownInputTypeEvent()
    {
        var dropdown = GetCtrl<GameUIDropdown>("UIDropdownInputType");
        dropdown.ClearOptions();
        List<InputField.InputType> list = new List<InputField.InputType>();
        list.Add(InputField.InputType.Standard);
        list.Add(InputField.InputType.AutoCorrect);
        list.Add(InputField.InputType.Password);
        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
        for (int i = 0; i < list.Count; i++)
        {
            optionList.Add(new Dropdown.OptionData("InputType." + list[i]));
        }
        dropdown.AddOptions(optionList);

        var val = UIInput.inputType;
        var index = list.FindIndex(t => t == val);
        dropdown.value = index;

        dropdown.AddValueChangedListener(delegate (int i) {
            val = list[i];
            UIInput.inputType = val;
        });
    }

    private void UIDropdownKeyboardTypeEvent()
    {
        var dropdown = GetCtrl<GameUIDropdown>("UIDropdownKeyboardType");
        dropdown.ClearOptions();
        List<TouchScreenKeyboardType> list = new List<TouchScreenKeyboardType>();
        list.Add(TouchScreenKeyboardType.Default);
        list.Add(TouchScreenKeyboardType.ASCIICapable);
        list.Add(TouchScreenKeyboardType.NumbersAndPunctuation);
        list.Add(TouchScreenKeyboardType.URL);
        list.Add(TouchScreenKeyboardType.NumberPad);
        list.Add(TouchScreenKeyboardType.PhonePad);
        list.Add(TouchScreenKeyboardType.NamePhonePad);
        list.Add(TouchScreenKeyboardType.EmailAddress);
        list.Add(TouchScreenKeyboardType.Search);

        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
        for (int i = 0; i < list.Count; i++)
        {
            optionList.Add(new Dropdown.OptionData("TouchScreenKeyboardType." + list[i]));
        }
        dropdown.AddOptions(optionList);

        var val = UIInput.keyboardType;
        var index = list.FindIndex(t => t == val);
        dropdown.value = index;

        dropdown.AddValueChangedListener(delegate (int i) {
            val = list[i];
            UIInput.keyboardType = val;
        });
    }

    private void UIDropdownCharactorValidateEvent()
    {
        var dropdown = GetCtrl<GameUIDropdown>("UIDropdownCharactorValidate");
        dropdown.ClearOptions();
        List<InputField.CharacterValidation> list = new List<InputField.CharacterValidation>();
        list.Add(InputField.CharacterValidation.None);
        list.Add(InputField.CharacterValidation.Integer);
        list.Add(InputField.CharacterValidation.Decimal);
        list.Add(InputField.CharacterValidation.Alphanumeric);
        list.Add(InputField.CharacterValidation.Name);
        list.Add(InputField.CharacterValidation.EmailAddress);

        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
        for (int i = 0; i < list.Count; i++)
        {
            optionList.Add(new Dropdown.OptionData("CharacterValidation." + list[i]));
        }
        dropdown.AddOptions(optionList);

        var val = UIInput.characterValidation;
        var index = list.FindIndex(t => t == val);
        dropdown.value = index;

        dropdown.AddValueChangedListener(delegate (int i) {
            val = list[i];
            UIInput.characterValidation = val;
        });
    }

    private void SliderCaretBlinkRateEvent()
    {
        var slider = GetCtrl<GameUISlider>("SliderCaretBlinkRate");
        var txt = GetCtrl<GameUIText>("TxtCaretBlinkRate");
        txt.FormatStr = ".caretBlinkRate = <color=green>{0}</color>";
        var val = UIInput.caretBlinkRate;
        slider.value = val;
        txt.SetTextFormatValues(val);

        slider.AddValueChangedListener(delegate (float f) {
            val = f;
            UIInput.ActivateInputField();
            UIInput.caretBlinkRate = val;
            txt.SetTextFormatValues(val);
        });
    }

    private void SliderCaretWidthEvent()
    {
        var slider = GetCtrl<GameUISlider>("SliderCaretWidth");
        var txt = GetCtrl<GameUIText>("TxtCaretWidth");
        txt.FormatStr = ".caretWidth = <color=green>{0}</color>";
        var val = UIInput.caretWidth;
        slider.maxValue = 5;
        slider.value = val;
        txt.SetTextFormatValues(val);

        slider.AddValueChangedListener(delegate (float f) {
            val = Mathf.FloorToInt(f);
            UIInput.ActivateInputField();
            UIInput.caretWidth = val;
            txt.SetTextFormatValues(val);
        });
    }
}
