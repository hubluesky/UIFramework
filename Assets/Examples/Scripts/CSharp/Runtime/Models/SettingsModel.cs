using UnityEngine;
using VBM;

public class SettingsModel : DictionaryModel {
    public bool music {
        get { 
            Debug.Log("music: " + GetProperty<bool>("music"));
            return GetProperty<bool>("music"); }
        set { SetProperty("music", value); }
    }

    public bool effects {
        get { 
            Debug.Log("effects: " + GetProperty<bool>("effects"));
            return GetProperty<bool>("effects"); }
        set { SetProperty("effects", value); }
    }

    public float volume {
        get { return GetProperty<float>("volume"); }
        set { SetProperty("volume", value); }
    }

    public void FAQ() {
        Debug.Log("Click FAQ button action.");
    }

    public void Credits() {
        Debug.Log("Click Credits button action.");
    }
}