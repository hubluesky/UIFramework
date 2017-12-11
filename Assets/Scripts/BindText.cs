using UnityEngine;
using UnityEngine.UI;

public class BindText : BindWidget {
    public Text text { get; protected set; }

    public BindText(Model model, string propertyName, string widgetName, System.Action<Model, string, BindWidget> action)
        : base(model, propertyName, widgetName, action) {
    }

    public override void SetWidgetTransform(Transform transform) {
        text = transform.GetComponent<Text>();
        if (text == null)
            Debug.LogWarningFormat("Bind text {0} form {1} transform failed!", widgetName, transform.name);
    }

    public override bool CheckUpdateWidget() {
        return text != null && text.isActiveAndEnabled;
    }

    protected override void UpdateWidget(Model model, string propertyName, BindWidget widget) {
        text.text = model.GetStringProperty(propertyName);
        Debug.LogFormat("UpdateWidget Text: {0}", text.text);
    }
}