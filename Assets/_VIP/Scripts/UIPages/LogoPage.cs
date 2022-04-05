using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class LogoPage : UIPage
{
	public Slider progess; //指定widgetID的物件


	protected override string uiPath => "LogoPage"; //名稱要與Adressables一樣

	protected override void OnAwake()
	{
		progess = transform.Find("Progess").GetComponent<Slider>();

		OnStart();
	}
}