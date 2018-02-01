using UnityEngine;
using VBM;

public class Main : MonoBehaviour {
	public static IconsSettings iconsSettings { get; private set; }

	void Awake() {
		iconsSettings = GetComponent<IconsSettings>();
		Initialized();
	}

	public void Initialized() {
		InitModels();

		Canvas canvas = CanvasUtility.CreateRootCanvas();
		ViewManager.Instance.InitCanvasLayers(canvas, typeof(ViewLayer));

		ViewConfigAsset configAsset = Resources.Load<ViewConfigAsset>("Configs/ViewConfigAsset");
		ViewConfigManager.Instance.AddConfigs(configAsset.configs);

		InitViews();

		TransientView view = ViewManager.Instance.GetView(ViewModulesName.Login) as TransientView;
		view.hideDestroyAsset = true;
		view.Show();
	}

	public void InitModels() {
		LoginModel loginModel = ModelManager.Instance.CreateModel<LoginModel>();
		loginModel.versionName = "v1.0.0 Beta";
		loginModel.rememberAccount = true;

		DialogBoxModel dialogModel = ModelManager.Instance.CreateModel<DialogBoxModel>();
		dialogModel.closeButton = false;
		dialogModel.confirmButton = false;
		dialogModel.cancelButton = false;

		SettingsModel settingsModel = ModelManager.Instance.CreateModel<SettingsModel>();
		settingsModel.music = false;
		settingsModel.effects = true;
		settingsModel.volume = 70;

		HallModel hallModel = ModelManager.Instance.CreateModel<HallModel>();
		hallModel.ore = 4650;
		hallModel.wood = 6000;
		hallModel.food = 7500;
		hallModel.god = 9500;
		hallModel.refine = 150;
		hallModel.forge = 50;

		AchievementsModel achievementsModel = ModelManager.Instance.CreateModel<AchievementsModel>();
		achievementsModel.medalList = new ListModel();
		AddAchievementItemModel(achievementsModel, iconsSettings.attack, "attack", "attack desc", RewardType.Ore, 1500, 10, 15);
		AddAchievementItemModel(achievementsModel, iconsSettings.defend, "defend", "defend desc", RewardType.Wood, 1500, 3, 10);
		AddAchievementItemModel(achievementsModel, iconsSettings.archer, "archer", "archer desc", RewardType.Food, 1500, 10, 10);
		AddAchievementItemModel(achievementsModel, iconsSettings.fire, "fire", "fire desc", RewardType.God, 1500, -1, 10);
		AddAchievementItemModel(achievementsModel, iconsSettings.crown, "crown", "crown desc", RewardType.Refine, 10, 6, 12);
		AddAchievementItemModel(achievementsModel, iconsSettings.bag, "bag", "bag desc", RewardType.Forge, 10, 12, 12);
		
		ShopModel shopModel = ModelManager.Instance.CreateModel<ShopModel>();
		shopModel.shopList = new ListModel();
		AddShopItemModel(shopModel, iconsSettings.ore, "20 ores", "Stack of ores", "1,88 €");
		AddShopItemModel(shopModel, iconsSettings.god, "560 gods", "Pile of Gods", "1,88 €");
		AddShopItemModel(shopModel, iconsSettings.wood, "360 gods", "Pile of Woods", "2,88 €");
		AddShopItemModel(shopModel, iconsSettings.food, "60 foods", "Slice of Meat", "3,88 €");
		AddShopItemModel(shopModel, iconsSettings.refine, "20 refines", "refine skill", "4,88 €");
		AddShopItemModel(shopModel, iconsSettings.forge, "20 forges", "forges skill", "4,88 €");
	}

	static void AddAchievementItemModel(AchievementsModel model, Sprite icon, string name, string desc, RewardType rewardType, int rewardValue, int curProgress, int maxProgress) {
		AchievementItemModel itemModel = new AchievementItemModel();
		itemModel.icon = icon;
		itemModel.name = name;
		itemModel.desc = desc;
		itemModel.rewardType = rewardType;
		itemModel.rewardValue = rewardValue;
		itemModel.maxProgress = maxProgress;
		itemModel.curProgress = curProgress;
		model.medalList.Add(itemModel);
	}

	static void AddShopItemModel(ShopModel model, Sprite icon, string amount, string product, string cost) {
		ShopItemModel itemModel = new ShopItemModel();
		itemModel.icon = icon;
		itemModel.amount = amount;
		itemModel.product = product;
		itemModel.cost = cost;
		model.shopList.Add(itemModel);
	}

	public void InitViews() {
		foreach (var value in System.Enum.GetValues(typeof(ViewModulesName))) {
			ViewConfig viewConfig = ViewConfigManager.Instance.GetViewConfig(value.ToString());
			if (viewConfig != null)
				ViewManager.Instance.CreateView<TransientView>(viewConfig);
		}
	}
}