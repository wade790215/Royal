using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class LogoPage : UIPage
{
	public LogoPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
	{
		
	}

	public Slider progess;


	protected override string uiPath => "LogoPage";

	protected override void OnAwake()
	{
		progess = transform.Find("Progess").GetComponent<Slider>();

		OnStart();
	}
}