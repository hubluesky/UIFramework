using VBM;

public class HallModel : DictionaryModel {
    public static readonly int maxOre = 10000;
    public static readonly int maxWood = 10000;
    public static readonly int maxFood = 10000;
    public static readonly int maxGod = 10000;
    public static readonly int maxRefine = 250;
    public static readonly int maxForge = 250;

    public int ore {
        get { return GetProperty<int>("ore"); }
        set {
            int newValue = System.Math.Min(value, maxOre);
            SetProperty("ore", newValue);
            oreRate = (float) newValue / maxOre;
        }
    }

    public int wood {
        get { return GetProperty<int>("wood"); }
        set {
            int newValue = System.Math.Min(value, maxWood);
            SetProperty("wood", newValue);
            woodRate = (float) newValue / maxWood;
        }
    }

    public int food {
        get { return GetProperty<int>("food"); }
        set {
            int newValue = System.Math.Min(value, maxFood);
            SetProperty("food", newValue);
            foodRate = (float) newValue / maxFood;
        }
    }

    public int god {
        get { return GetProperty<int>("god"); }
        set {
            int newValue = System.Math.Min(value, maxGod);
            SetProperty("god", newValue);
            godRate = (float) newValue / maxGod;
        }
    }

    public float oreRate {
        get { return GetProperty<float>("oreRate"); }
        set { SetProperty("oreRate", value); }
    }

    public float woodRate {
        get { return GetProperty<float>("woodRate"); }
        set { SetProperty("woodRate", value); }
    }

    public float foodRate {
        get { return GetProperty<float>("foodRate"); }
        set { SetProperty("foodRate", value); }
    }

    public float godRate {
        get { return GetProperty<float>("godRate"); }
        set { SetProperty("godRate", value); }
    }

    public int refine {
        get { return GetProperty<int>("refine"); }
        set { SetProperty("refine", value); }
    }

    public int forge {
        get { return GetProperty<int>("forge"); }
        set { SetProperty("forge", value); }
    }

    public void AddRefine() {
        refine = System.Math.Min(refine + 5, maxRefine);
    }

    public void AddForge() {
        forge = System.Math.Min(forge + 5, maxForge);
    }

    public void Logout() {
        ViewManager.Instance.ShowView(ViewModulesName.Login);
    }

    public void OpenSettings() {
        ViewManager.Instance.ShowView(ViewModulesName.Settings);
    }

    public void OpenShop() {
        ViewManager.Instance.ShowView(ViewModulesName.Shop);
    }

    public void OpenMap() {
        ViewManager.Instance.ShowView(ViewModulesName.Achievements);
    }
}