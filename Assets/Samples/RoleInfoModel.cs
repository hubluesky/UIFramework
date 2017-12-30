using System.Collections.Generic;
using UnityEngine;
using VBM;

public class RoleInfoModel : DictionaryModel {
    public Sprite headIcon {
        get { return GetProperty<Sprite>("headIcon"); }
        set { SetProperty("headIcon", value); }
    }

    public string username {
        get { return GetProperty<string>("username"); }
        set { SetProperty("username", value); }
    }

    public int goldCount {
        get { return GetProperty<int>("goldCount"); }
        set { SetProperty("goldCount", value); }
    }

    public string tab1Text1 {
        get { return GetProperty<string>("tab1Text1"); }
        set { SetProperty("tab1Text1", value); }
    }

    public string tab1Text2 {
        get { return GetProperty<string>("tab1Text2"); }
        set { SetProperty("tab1Text2", value); }
    }

    public Sprite tab2Icon1 {
        get { return GetProperty<Sprite>("tab2Icon1"); }
        set { SetProperty("tab2Icon1", value); }
    }

    public Sprite tab2Icon2 {
        get { return GetProperty<Sprite>("tab2Icon2"); }
        set { SetProperty("tab2Icon2", value); }
    }

    public void AddFriend() {
        Debug.Log("Add friend function.");
    }
}