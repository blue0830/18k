using UnityEngine;
using System.Collections;

public class LoginPanel : MonoBehaviour {

    public UIInput username;
    public UIInput password;

    public GameObject registerbtn;
    public GameObject loginbtn;
    public GameObject forgetbtn;
    public GameObject kefubtn;


    public UIToggle savePassword;

    public bool IsInputPswd = false;
	// Use this for initialization
	void Start () {
        EventDelegate.Add(savePassword.onChange, OnToggleChange);
        EventDelegate.Add(password.onChange, OnInputPswd);
     

        username.value = PlayerPrefs.GetString("UserName", "");

        if (PlayerPrefs.GetInt("savePassword", 0)==0)
        {
            savePassword.value = false;
        }
        else
        {
            savePassword.value = true;

        }

        if (!PlayerPrefs.HasKey("Password"))
        {
            savePassword.value = false;
            PlayerPrefs.SetInt("savePassword", 0);
        }

    }

	public void Reset()
	{
		username.value = PlayerPrefs.GetString("UserName", "");
	}

    void OnInputPswd()
    {
        if(password.isSelected)
            IsInputPswd = true;
    }
	
	// Update is called once per frame
	void Update () {
	    

	}


    void OnToggleChange()
    {
        if (!savePassword.value)
        {
            password.value = "";
            PlayerPrefs.DeleteKey("Password");
            PlayerPrefs.SetInt("savePassword", 0);
        }
        else
        {
            string temp = PlayerPrefs.GetString("Password", "");
            if (!string.IsNullOrEmpty(temp))
            {
                password.value = "666666";
            }
               
        }

    }
}
