using UnityEngine;
using VBM;

public class RankItemModel : DictionaryModel {

    public RankItemModel(Sprite headIcon, string username, int rank) {
        this.headIcon = headIcon;
        this.username = username;
        this.rank = rank;
    }

    public Sprite headIcon {
        get { return GetProperty<Sprite>("headIcon"); }
        set { SetProperty("headIcon", value); }
    }

    public string username {
        get { return GetProperty<string>("username"); }
        set { SetProperty("username", value); }
    }

    public int rank {
        get { return GetProperty<int>("rank"); }
        set { SetProperty("rank", value); }
    }
}