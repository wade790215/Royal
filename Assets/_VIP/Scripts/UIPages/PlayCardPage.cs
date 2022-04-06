using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class PlayCardPage : UIPage
{
	public Image playCardsArea;
	public Transform cardTrans;
	public Transform startPos;
	public Transform endPos;


	protected override string uiPath => "PlayCardPage";

	protected override void OnAwake()
	{
		playCardsArea = transform.Find("PlayCardsArea").GetComponent<Image>();
		cardTrans = transform.Find("PlayCardsArea/CardTrans").GetComponent<Transform>();
		startPos = transform.Find("StartPos").GetComponent<Transform>();
		endPos = transform.Find("EndPos").GetComponent<Transform>();

		OnStart();
	}
}