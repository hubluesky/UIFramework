using System.Collections.Generic;
using UnityEngine;

public class SamplesModel1 : Model {
    private Dictionary<string, object> variableMap = new Dictionary<string, object>();

    public void BindView(View view) {
        view.rootNodeBind.BindImage(this, "headIcon", "HeadImage");
        view.rootNodeBind.BindText(this, "username", "UsernameText");
        view.rootNodeBind.BindText(this, "goldCount", "GoldText");

        NodeBind content1Bind = view.rootNodeBind.CreateChild("Content1");
        content1Bind.BindText(this, "tab1Text1", "Text1");
        content1Bind.BindText(this, "tab1Text2", "Text2");

        NodeBind content2Bind = view.rootNodeBind.CreateChild("Content2");
        content2Bind.BindImage(this, "tab2Icon1", "Image1");
        content2Bind.BindImage(this, "tab2Icon2", "Image2");
    }

    public void SetHeadIcon(Sprite sprite) {
        AddVariable("headIcon", sprite);
    }
    public void SetUsername(string name) {
        AddVariable("username", name);
    }
    public void SetGoldCount(int count) {
        AddVariable("goldCount", count);
    }

    public void SetTab1Text1(string text) {
        AddVariable("tab1Text1", text);
    }

    public void SetTab1Text2(string text) {
        AddVariable("tab1Text2", text);
    }

    public void SetTab2Icon1(Sprite sprite) {
        AddVariable("tab2Icon1", sprite);
    }

    public void SetTab2Icon2(Sprite sprite) {
        AddVariable("tab2Icon2", sprite);
    }

    protected void AddVariable(string key, object value) {
        if (variableMap.ContainsKey(key)) {
            if (variableMap[key].Equals(value))
                return;
            variableMap[key] = value;
        } else {
            variableMap.Add(key, value);
        }
        NotifyPropertyChanged(key);
    }

    public override string GetStringProperty(string propertyName) {
        if (variableMap.ContainsKey(propertyName))
            return variableMap[propertyName].ToString();
        return null;
    }

    public override Sprite GetSpriteProperty(string propertyName) {
        if (variableMap.ContainsKey(propertyName))
            return variableMap[propertyName] as Sprite;
        return null;
    }
}
