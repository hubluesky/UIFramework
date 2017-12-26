using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VBM;

public class Main : MonoBehaviour {
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Canvas canvas;

    public void StartMBV() {
        ModelManager.Instance.CreateModel<RoleInfoModel>();
        ModelManager.Instance.CreateModel<RankingModel>();

        ViewManager.Instance.InitCanvasLayers(canvas, typeof(ViewLayer));

        ViewConfigAsset configAsset = Resources.Load<ViewConfigAsset>("Configs/ViewConfigAsset");
        ViewConfigManager.Instance.AddConfigs(configAsset.configs);

        ViewConfig roleInfoConfig = ViewConfigManager.Instance.GetViewConfig(ViewModulesName.RoleInfo.ToString());
        ViewConfig rankingConfig = ViewConfigManager.Instance.GetViewConfig(ViewModulesName.Ranking.ToString());
        ViewManager.Instance.CreateView<TransientView>(roleInfoConfig);
        ViewManager.Instance.CreateView<TransientView>(rankingConfig);

        Debug.Log("Load view config asset, Create model and create view");
    }

    public void ShowRoleInfo() {
        View view = ViewManager.Instance.GetView(ViewModulesName.RoleInfo.ToString());
        if (view.transform == null)
            ViewManager.Instance.LoadViewAsset(view);
    }

    public void UpdateRoleInfoData() {
        RoleInfoModel model = ModelManager.Instance.GetModel<RoleInfoModel>();
        model.headIcon = sprite1;
        model.username = "hubluesky";
        model.goldCount = 520520;
        model.tab1Text1 = "my name is ..";
        model.tab1Text2 = "poling";

        model.tab2Icon1 = sprite2;
        model.tab2Icon2 = sprite3;
    }

    public void UpdateRoleInfoData2() {
        RoleInfoModel model = ModelManager.Instance.GetModel<RoleInfoModel>();
        model.headIcon = sprite3;
        model.username = "sky blue hu";
        model.goldCount = 12580;
        model.tab1Text1 = "ni hao ..";
        model.tab1Text2 = "poling";

        model.tab2Icon1 = sprite1;
        model.tab2Icon2 = sprite3;
    }

    public void ShowRanking() {
        View view = ViewManager.Instance.GetView(ViewModulesName.Ranking.ToString());
        if (view.transform == null)
            ViewManager.Instance.LoadViewAsset(view);
    }

    public void UpdateRankData() {
        RankingModel model = ModelManager.Instance.GetModel<RankingModel>();
        model.updateTime = 6;
        ListModel listModel = new ListModel();
        model.rankList = listModel;
        listModel.Add(new RankItemModel(sprite1, "hubluesky", 1));
        listModel.Add(new RankItemModel(sprite2, "sky", 2));
        listModel.Add(new RankItemModel(sprite3, "fool", 3));
    }

    public void UpdateRankData2() {
        RankingModel model = ModelManager.Instance.GetModel<RankingModel>();
        model.updateTime = 12;
        RankItemModel itemModel = model.rankList[1] as RankItemModel;
        itemModel.headIcon = sprite1;
        itemModel.username = "sky sky";
    }
}
