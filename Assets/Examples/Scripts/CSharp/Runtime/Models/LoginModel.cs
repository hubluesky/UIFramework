using VBM;
public class LoginModel : DictionaryModel {
    public string accountName {
        get { return GetProperty<string>("accountName"); }
        set { SetProperty("accountName", value); }
    }

    public string password {
        get { return GetProperty<string>("password"); }
        set { SetProperty("password", value); }
    }

    public bool rememberAccount {
        get { return GetProperty<bool>("rememberAccount"); }
        set { SetProperty("rememberAccount", value); }
    }

    public string versionName {
        get { return GetProperty<string>("versionName"); }
        set { SetProperty("versionName", value); }
    }

    public void Login() {
        if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(password)) {
            DialogBoxModel.ShowDialog("Login failed", "Input account or password is error!", DialogBoxModel.Buttons.Close | DialogBoxModel.Buttons.Confirm);
            return;
        }
        ViewManager.Instance.HideView(ViewModulesName.Login);
        ViewManager.Instance.ShowView(ViewModulesName.Hall);
        if (!rememberAccount) {
            accountName = null;
            password = null;
        }
    }

    public void OpenSettings() {
        ViewManager.Instance.ShowView(ViewModulesName.Settings);
    }
}