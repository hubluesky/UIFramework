using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    public RectTransform uiRoleInfo;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;

    public Canvas canvas;
    private SamplesModel1 model;
    // private View view;

    public void StartMBV() {
        model = new SamplesModel1();
        // view = new View();
        // model.BindView(view);
        Debug.Log("Create model and bind view");
    }

    public void ShowRoleInfo() {
		RectTransform ui = Instantiate(uiRoleInfo);
        ui.gameObject.SetActive(false);
        ui.SetParent(canvas.transform, false);
        // view.SetTransform(ui);
        // view.Show();
    }

	public void UpdateData() {
        model.SetHeadIcon(sprite1);
        model.SetUsername("hubluesky");
        model.SetGoldCount(3665);
        model.SetTab1Text1("my name is ..");
        model.SetTab1Text2("poling");

        model.SetTab2Icon1(sprite2);
        model.SetTab2Icon2(sprite3);
    }

	public void Update2Data() {
        model.SetHeadIcon(sprite3);
        model.SetUsername("abscefs");
        model.SetGoldCount(556689);
        model.SetTab1Text1("ni hao ..");
        model.SetTab1Text2("poling");

        model.SetTab2Icon1(sprite1);
        model.SetTab2Icon2(sprite3);
	}
}
