using UnityEngine;

public abstract class BindWidget {
    public Model model { get; protected set; }
    public string propertyName { get; protected set; }
    public string widgetName { get; protected set; }
    public System.Action<Model, string, BindWidget> triggerAction { get; protected set; }
    public bool refresh { get; set; }

    public BindWidget(Model model, string propertyName, string widgetName, System.Action<Model, string, BindWidget> action) {
        this.model = model;
        this.propertyName = propertyName;
        this.widgetName = widgetName;
        this.triggerAction = action != null ? action : UpdateWidget;
        refresh = true;
        BindPropertyChanged();
    }

    ~BindWidget() {
        UnbindPropertyChanged();
    }

    public abstract bool CheckUpdateWidget();

    protected abstract void UpdateWidget(Model model, string propertyName, BindWidget widget);

    public abstract void SetWidgetTransform(Transform transform);

    public void RefreshWidget() {
        refresh = false;
        triggerAction(model, propertyName, this);
    }

    private void OnPropertyChanged(string propertyName) {
        if (this.propertyName == propertyName) {
            if (CheckUpdateWidget()) {
                RefreshWidget();
            } else {
                refresh = true;
            }
        }
    }

    public void BindPropertyChanged() {
        model.propertyChanged += OnPropertyChanged;
    }

    public void UnbindPropertyChanged() {
        model.propertyChanged -= OnPropertyChanged;
    }
}