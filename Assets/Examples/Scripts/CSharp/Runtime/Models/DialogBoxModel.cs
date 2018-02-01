using VBM;

public class DialogBoxModel : DictionaryModel {
    public event System.Action closeAction;
    public event System.Action confirmAction;
    public event System.Action cancelAction;

    public enum Buttons {
        Close = 1,
        Confirm = 2,
        Cancel = 4,
        All = Close | Confirm | Cancel,
    }

    public string title {
        get { return GetProperty<string>("title"); }
        set { SetProperty("title", value); }
    }

    public string content {
        get { return GetProperty<string>("content"); }
        set { SetProperty("content", value); }
    }

    public bool closeButton {
        get { return GetProperty<bool>("closeButton"); }
        set { SetProperty("closeButton", value); }
    }

    public bool confirmButton {
        get { return GetProperty<bool>("confirmButton"); }
        set { SetProperty("confirmButton", value); }
    }

    public bool cancelButton {
        get { return GetProperty<bool>("cancelButton"); }
        set { SetProperty("cancelButton", value); }
    }

    public void CloseAction() {
        if (closeAction != null)
            closeAction();
        closeAction = null;
        ViewManager.Instance.HideView(ViewModulesName.DialogBox);
    }

    public void ConfirmAction() {
        if (confirmAction != null)
            confirmAction();
        confirmAction = null;
        ViewManager.Instance.HideView(ViewModulesName.DialogBox);
    }

    public void CancelAction() {
        if (cancelAction != null)
            cancelAction();
        cancelAction = null;
        ViewManager.Instance.HideView(ViewModulesName.DialogBox);
    }

    public static void ShowDialog(string title, string content, Buttons buttons, System.Action closeAction = null, System.Action confirmAction = null, System.Action cancelAction = null) {
        DialogBoxModel dialog = ModelManager.Instance.GetModel<DialogBoxModel>();
        dialog.title = title;
        dialog.content = content;
        dialog.closeButton = (buttons & Buttons.Close) != 0;
        dialog.confirmButton = (buttons & Buttons.Confirm) != 0;
        dialog.cancelButton = (buttons & Buttons.Cancel) != 0;
        if (closeAction != null)
            dialog.closeAction += closeAction;
        if (confirmAction != null)
            dialog.confirmAction += confirmAction;
        if (cancelAction != null)
            dialog.cancelAction += cancelAction;
        ViewManager.Instance.ShowView(ViewModulesName.DialogBox);
    }
}