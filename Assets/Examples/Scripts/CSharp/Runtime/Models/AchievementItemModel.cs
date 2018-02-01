using UnityEngine;
using VBM;

public enum RewardType {
    Ore,
    Wood,
    Food,
    God,
    Refine,
    Forge,
}

public class AchievementItemModel : DictionaryModel {
    public string name {
        get { return GetProperty<string>("name"); }
        set { SetProperty("name", value); }
    }

    public string desc {
        get { return GetProperty<string>("desc"); }
        set { SetProperty("desc", value); }
    }

    public Sprite icon {
        get { return GetProperty<Sprite>("icon"); }
        set { SetProperty("icon", value); }
    }

    public RewardType rewardType {
        get { return GetProperty<RewardType>("rewardType"); }
        set { SetProperty("rewardType", value); }
    }

    public int rewardValue {
        get { return GetProperty<int>("rewardValue"); }
        set { SetProperty("rewardValue", value); }
    }

    public int curProgress {
        get { return GetProperty<int>("curProgress"); }
        set {
            SetProperty("curProgress", value);
            progress = maxProgress == 0 ? 0 : (float) value / maxProgress;
            progressText = string.Format("{0}/{1}", value, maxProgress);
            claimReward = value == maxProgress;
            completed = value == -1;
        }
    }

    public int maxProgress {
        get { return GetProperty<int>("maxProgress"); }
        set { SetProperty("maxProgress", value); }
    }

    public float progress {
        get { return GetProperty<float>("progress"); }
        set { SetProperty("progress", value); }
    }

    public string progressText {
        get { return GetProperty<string>("progressText"); }
        set { SetProperty("progressText", value); }
    }

    public bool claimReward {
        get { return GetProperty<bool>("claimReward"); }
        set { SetProperty("claimReward", value); }
    }

    public bool completed {
        get { return GetProperty<bool>("completed"); }
        set { SetProperty("completed", value); }
    }

    public void ClaimReward() {
        HallModel hallModel = ModelManager.Instance.GetModel<HallModel>();
        switch (rewardType) {
            case RewardType.Ore:
                hallModel.ore += rewardValue;
                break;
            case RewardType.Wood:
                hallModel.wood += rewardValue;
                break;
            case RewardType.Food:
                hallModel.food += rewardValue;
                break;
            case RewardType.God:
                hallModel.god += rewardValue;
                break;
            case RewardType.Refine:
                hallModel.refine += rewardValue;
                break;
            case RewardType.Forge:
                hallModel.forge += rewardValue;
                break;
        }
        Debug.LogFormat("Add reward {0} {1}", rewardType, rewardValue);
        curProgress = -1;
    }
}