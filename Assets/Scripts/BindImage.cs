using UnityEngine;
using UnityEngine.UI;

public class BindImage : BindWidget {
    public Image image;

    public BindImage(Model model, string propertyName, string widgetName, System.Action<Model, string, BindWidget> action)
        : base(model, propertyName, widgetName, action) {
    }

    public override void SetWidgetTransform(Transform transform) {
        image = transform.GetComponent<Image>();
        if (image == null)
            Debug.LogWarningFormat("Bind image {0} form {1} transform failed!", widgetName, transform.name);
    }

    public override bool CheckUpdateWidget() {
        return image != null && image.isActiveAndEnabled;
    }

    protected override void UpdateWidget(Model model, string propertyName, BindWidget widget) {
        image.sprite = model.GetSpriteProperty(propertyName);
        Debug.LogFormat("UpdateWidget Image: {0}", image.sprite == null ? "" : image.sprite.name);
    }
}