using VBM;

public class AchievementsModel : DictionaryModel {
    public ListModel medalList {
        get { return GetProperty<ListModel>("medalList"); }
        set { SetProperty("medalList", value); }
    }
}