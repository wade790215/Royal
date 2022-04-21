using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class LoginPage : UIPage
{
	public LoginPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
	{
		
	}

	public InputField accInput;
	public Text accText;
	public InputField pwdInput;
	public Text pwdText;
	public Button loginButton;
	public Slider slider;


	protected override string uiPath => "LoginPage";

	protected override void OnAwake()
	{
		accInput = transform.Find("AccInput").GetComponent<InputField>();
		accText = transform.Find("AccInput/AccText").GetComponent<Text>();
		pwdInput = transform.Find("PwdInput").GetComponent<InputField>();
		pwdText = transform.Find("PwdInput/PwdText").GetComponent<Text>();
		loginButton = transform.Find("LoginButton").GetComponent<Button>();
		slider = transform.Find("Slider").GetComponent<Slider>();

		OnStart();
	}
}