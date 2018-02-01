using UnityEngine;
using VBM;

public class ShopItemModel : DictionaryModel {
    public Sprite icon {
        get { return GetProperty<Sprite>("icon"); }
        set { SetProperty("icon", value); }
    }

    public string amount {
        get { return GetProperty<string>("amount"); }
        set { SetProperty("amount", value); }
    }

    public string product {
        get { return GetProperty<string>("product"); }
        set { SetProperty("product", value); }
    }

    public string cost {
        get { return GetProperty<string>("cost"); }
        set { SetProperty("cost", value); }
    }
}