using UnityEngine;
using VBM;

public class SamplesModel2 : DictionaryModel {
    public int updateTime {
        get { return GetProperty<int>("updateTime"); }
        set { SetProperty("updateTime", value); }
    }

    public ListModel rankList {
        get { return GetProperty<ListModel>("rankList"); }
        set { SetProperty("rankList", value); }
    }
}