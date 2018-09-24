using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class LoginXMPP : MonoBehaviour {

    public string username, domain, password, status;

    public InputField usernameText, domainText, passwordText, statusText;
    public string license = string.Empty;
    private void LoadLicense()
    {
        byte[] lic = File.ReadAllBytes(Application.streamingAssetsPath + "/license.txt");
        license = Encoding.ASCII.GetString(lic);
        Matrix.License.LicenseManager.SetLicense(license);
        Debug.LogWarning("License error:" + Matrix.License.LicenseManager.LicenseError);
        Debug.LogWarning("License raw:" + Matrix.License.LicenseManager.RawLicense);
    }

    public void Start()
    {
        LoadLicense();
       

        /// Load test Data
        /// usernameText.text = "hhoria";
        /// domainText.text = "localhost";
        /// passwordText.text = "qwertyp@1U";
        /// statusText.text = "Online !!!";
    }

    public void SetUsername()
    {
        username = usernameText.text;
    }
    public void SetDomain()
    {
        domain = domainText.text;
    }
    public void SetPassword()
    {
        password = passwordText.text;
    }    
    public void SetStatus()
    {
        status = statusText.text;
    }
}
