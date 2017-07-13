using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class TipManager : MonoBehaviour {

	#region sigleton

	private static TipManager _instance;
	public static  TipManager Instance{
		get{ 

			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(TipManager)) as TipManager;
			}
			return _instance;
		}
	}
	#endregion

	#region  var

	[SerializeField] private Text tipContentText;
	[SerializeField] private RectTransform myRect;

	#endregion


	#region

	void Start(){
		
	}
	#endregion
	public void show(string txt){
		myRect.gameObject.SetActive (true);
		myRect.DOScaleY (1, 0.2f);
		tipContentText.text = txt;
	}


	public void OnClickClose(){
		myRect.DOScaleY (0.0f, 0.2f).OnComplete (delegate() {
			myRect.gameObject.SetActive(false);
		});
	}

}
