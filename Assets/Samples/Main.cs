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
        ViewManager.Instance.InitCanvasLayers(canvas, typeof(ViewLayer));

        ViewConfigAsset configAsset = Resources.Load<ViewConfigAsset>("Configs/ViewConfigAsset");
        ViewConfigManager.Instance.AddConfigs(configAsset.configs);
        
        ModelManager.Instance.CreateModel<RoleInfoModel>();
        ModelManager.Instance.CreateModel<RankingModel>();

        ViewConfig roleInfoConfig = ViewConfigManager.Instance.GetViewConfig(ViewModulesName.RoleInfo.ToString());
        ViewConfig rankingConfig = ViewConfigManager.Instance.GetViewConfig(ViewModulesName.Ranking.ToString());
        ViewManager.Instance.CreateView<TransientView>(roleInfoConfig);
        ViewManager.Instance.CreateView<TransientView>(rankingConfig);

        Debug.Log("Load view config asset, Create model and create view");
    }

    public void ShowRoleInfo() {
        View view = ViewManager.Instance.GetView(ViewModulesName.RoleInfo.ToString());
        view.Show();
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
        view.Show();
    }

    public void UpdateRankData() {
        RankingModel model = ModelManager.Instance.GetModel<RankingModel>();
        model.updateTime = 6;
        ListModel listModel = new ListModel();
        model.rankList = listModel;
        listModel.Add(new RankItemModel(sprite1, "hubluesky", 2));
        listModel.Add(new RankItemModel(sprite2, "sky", 1));
        listModel.Add(new RankItemModel(sprite3, "fool", 5));
        listModel.Add(new RankItemModel(sprite2, "blue", 4));
        listModel.Add(new RankItemModel(sprite1, "hu", 3));
    }

    public void UpdateRankData2() {
        RankingModel model = ModelManager.Instance.GetModel<RankingModel>();
        model.updateTime = 12;
        RankItemModel itemModel = model.rankList[1] as RankItemModel;
        itemModel.headIcon = sprite1;
        itemModel.username = "sky sky";
    }

    public void SortRankData() {
        RankingModel model = ModelManager.Instance.GetModel<RankingModel>();
        model.rankList.Sort((x, y) => {
            RankItemModel rx = x as RankItemModel;
            RankItemModel ry = y as RankItemModel;
            return rx.rank - ry.rank;
        });

        model.rankList.Swap(1, 3);
        model.rankList.Swap(4, 0);
    }
}
