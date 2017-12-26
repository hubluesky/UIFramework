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
        ModelManager.Instance.CreateModel<SamplesModel1>();
        ModelManager.Instance.CreateModel<SamplesModel2>();

        ViewManager.Instance.InitCanvasLayers(canvas, typeof(ViewLayer));

        ViewConfigAsset configAsset = Resources.Load<ViewConfigAsset>("Configs/ViewConfigAsset");
        ViewConfigManager.Instance.AddConfigs(configAsset.configs);

        ViewConfig hallConfig = ViewConfigManager.Instance.GetViewConfig(ViewModulesName.RoleInfo.ToString());
        ViewManager.Instance.CreateView<TransientView>(hallConfig);

        Debug.Log("Load view config asset, Create model and create view");
    }

    public void ShowRoleInfo() {
        View roleInfoView = ViewManager.Instance.GetView(ViewModulesName.RoleInfo.ToString());
        ViewManager.Instance.LoadViewAsset(roleInfoView);
    }

    public void UpdateData() {
        SamplesModel1 model = ModelManager.Instance.GetModel<SamplesModel1>();
        model.headIcon = sprite1;
        model.username = "hubluesky";
        model.goldCount = 520520;
        model.tab1Text1 = "my name is ..";
        model.tab1Text2 = "poling";

        model.tab2Icon1 = sprite2;
        model.tab2Icon2 = sprite3;
    }

    public void Update2Data() {
        SamplesModel1 model = ModelManager.Instance.GetModel<SamplesModel1>();
        model.headIcon = sprite3;
        model.username = "sky blue hu";
        model.goldCount = 12580;
        model.tab1Text1 = "ni hao ..";
        model.tab1Text2 = "poling";

        model.tab2Icon1 = sprite1;
        model.tab2Icon2 = sprite3;
    }
}
