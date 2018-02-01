using VBM;

public class ShopModel : DictionaryModel {
    public ListModel shopList {
        get { return GetProperty<ListModel>("shopList"); }
        set { SetProperty("shopList", value); }
    }
}