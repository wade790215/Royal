using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class LoginPage
{
	public void OnStart()
	{
		KBEngine.Event.registerOut(KBEngine.EventOutTypes.onLoginBaseapp, this, "OnLoginBaseapp");
		slider.onValueChanged.AddListener((value) =>
		{
			string s = new string('x',6).Replace("x",value.ToString());
			accText.text = s;
			pwdText.text = s;
		});

		loginButton.onClick.AddListener(() =>
		{
			//fireIn傳事件給Server , fireOut Server傳給Client
			KBEngine.Event.fireIn(KBEngine.EventInTypes.login , accText.text,pwdText.text,Encoding.ASCII.GetBytes("Wade"));
		});
	}

	public void OnLoginBaseapp()
    {
		ShowPageAsync<MainPage>();
    }

}
