using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class MainUIContrller : MonoBehaviour
{


	public int totalItem = 0;
	public int successItem = 0;
	public Slider scoreSlider;
	public Text scoreText;
	public RectTransform grid;
	public int Columns=14;
	public int Rows = 14;
	public RawImage RawShowImage;
	public RawImage ViewRawImagPic;
	public GameObject gridItemPrefab;
	public RectTransform SuccessPanel;
	public RectTransform SetingPanel;
	public RectTransform ViewRawPicPanel;
	public RectTransform CameraPanel;

	public List<RawImage> listItems=new List<RawImage>();
	public List<Texture> listTextures=new List<Texture>();

	public List<Texture> BigIamges = new List<Texture> ();

	public Text currentDifficult;

	public AudioSource adu1;
	public AudioSource takePhotoSound;

	public RawImage VideoTexture;

	#region sigleton
	private static MainUIContrller _instance;
	public static MainUIContrller Instance{
		get{ 
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(MainUIContrller)) as MainUIContrller;
			}
			return _instance;
		}
	}


	#endregion


	#region mono 
	void Start ()
	{
		gridItemPrefab.gameObject.SetActive (false);
		grid.gameObject.SetActive (false);
		SuccessPanel.gameObject.SetActive (false);
		//InitGrid ();
		adu1=this.gameObject.GetComponent<AudioSource>();

	}
	

	void Update () 
	{
	
	}

	#endregion



	#region  OnClick Event

	public List<int> randomList=new List<int>();
	public void OnClickStartGame()
	{
		SuccessPanel.gameObject.SetActive (false);
		Debug.Log ("开始游戏----"+Time.time);
		int _max = Columns * Rows;
		randomList.Clear ();
		while (randomList.Count < _max) 
		{
			int _ra = Random.Range (0, _max);
			if (!randomList.Contains (_ra)) {
				randomList.Add (_ra);
			}
		}
		for (int i = 0; i < listItems.Count; i++)
		{
			if (i < randomList.Count) 
			{
				int _n = randomList [i];
				if (_n < listTextures.Count&&_n>=0) {
					listItems [i].texture = listTextures [_n];

					listItems [i].gameObject.GetComponent<PicItem> ().Num = _n;
				} else {
					Debug.Log ("_n=========="+_n);
				}
			} else {
				Debug.Log ("i=========="+i);
			}
		}
	}

	public void OnClickSlicePic()
	{
		Debug.Log ("开始切图----"+Time.time);
		grid.gameObject.SetActive (true);
		InitGrid ();
		SlicePic ();
		for (int i = 0; i < listTextures.Count; i++) 
		{
			listItems[i].texture=listTextures[i];
			listItems [i].gameObject.GetComponent<PicItem> ().Num = i;
		}
		RawShowImage.enabled = false;
		SuccessPanel.gameObject.SetActive (false);
	}


	int CurrentShowIndex=0;
	public void OnClickNextImage()
	{
		CurrentShowIndex++;
		if (CurrentShowIndex >= BigIamges.Count) {
			CurrentShowIndex = 0;
		}
		RawShowImage.enabled = true;
		RawShowImage.texture=BigIamges[CurrentShowIndex];
		grid.gameObject.SetActive (false);
	}


	public void OnClickSettingButon()
	{
		if (!SetingPanel.gameObject.activeInHierarchy) 
		{
			SetingPanel.gameObject.SetActive (true);
			SetingPanel.localScale = new Vector3 (1f, 0f, 1f);
			SetingPanel.DOScaleY (1.0f, 0.5f);
		} 
		else
		{
			SetingPanel.DOScaleY (0.0f, 0.5f).OnComplete(delegate {
				SetingPanel.gameObject.SetActive (false);
			});
		}
	}

	public void OnSliderValueChange(float k){
		int _k = Mathf.RoundToInt (k);
		Columns = Rows = _k;
		currentDifficult.text = _k+"";
	}

	public void OnClickViwRawPic(){
		if (!ViewRawPicPanel.gameObject.activeInHierarchy) 
		{
			ViewRawPicPanel.gameObject.SetActive (true);
			ViewRawPicPanel.localScale = new Vector3 (1f, 0f, 1f);
			ViewRawPicPanel.DOScaleY (1.0f, 0.5f);
		} 
		else
		{
			ViewRawPicPanel.DOScaleY (0.0f, 0.5f).OnComplete(delegate {
				ViewRawPicPanel.gameObject.SetActive (false);
			});
		}
	}


	public void OnClickCamera(){
		if (!CameraPanel.gameObject.activeInHierarchy) 
		{
			CameraPanel.gameObject.SetActive (true);
			CameraPanel.localScale = new Vector3 (1f, 0f, 1f);
			CameraPanel.DOScaleY (1.0f, 0.5f);
			startVideo (VideoTexture);//
			//startVideo(RawShowImage);

		} 
		else
		{
			StopVideo ();
			CameraPanel.DOScaleY (0.0f, 0.5f).OnComplete(delegate {
				CameraPanel.gameObject.SetActive (false);
			});
		}

	}
	public void OnClickTakePhoto(){
		if(webCamtex!=null)
		{

			Texture2D texture = new Texture2D (webCamtex.width, webCamtex.height);  
			texture.SetPixels (webCamtex.GetPixels ());
			texture.Apply ();      
			RawShowImage.texture = texture;
			takePhotoSound.Play ();
		}
	}

	#endregion

	#region  video
	public string deviceName = "";
	public WebCamTexture webCamtex = null;
	bool startVideo (RawImage m)
	{
		Application.RequestUserAuthorization (UserAuthorization.WebCam);
		if (Application.HasUserAuthorization (UserAuthorization.WebCam)) {

			WebCamDevice[] devies = WebCamTexture.devices;
			if (devies == null || devies.Length == 0) {
				return false;
			}
			deviceName = devies [0].name;
			webCamtex = new WebCamTexture (deviceName, 300, 300, 2);
			webCamtex.Play ();
			m.texture = webCamtex;	 
			return true;
		}
		return false;
	}


	public void StopVideo()
	{
		if (webCamtex!=null&&webCamtex.isPlaying) {
			webCamtex.Stop ();
		}
		if (webCamtex != null)
		{
			webCamtex = null;
		}
	}


	private void TakePhtoto(){
		
	}
	#endregion




	#region  internal function

	private void InitGrid()
	{
		
		Vector2 _gridSize = grid.rect.size;
		float _offsetWidth = _gridSize.x / Columns;
		float _offsetHeight = _gridSize.y / Rows;
		GiveBackAll (grid);
		listItems.Clear ();
		for (int i = 0; i < Rows; i++)
		{
			for (int j = 0; j < Columns; j++) 
			{
				GameObject _go =getAGird();
				_go.transform.SetParent (grid.transform,false);
				_go.name =(i * Columns + j)+"_Item";
				listItems.Add (_go.GetComponentInChildren<RawImage> ());   
			}
			GridLayoutGroup _group= grid.gameObject.GetComponent<GridLayoutGroup> ();
			_group.cellSize = new Vector2 (_offsetWidth, _offsetHeight);
			_group.constraintCount = Columns;
		}
	}


	private void SlicePic(){
		scoreText.text = "0%";
		scoreSlider.value = 0;
		for (int i = 0; i < listTextures.Count; i++) {
			Destroy (listTextures[i]);
		}
		listTextures.Clear ();
	    
		ViewRawImagPic.texture = RawShowImage.texture;
		if (RawShowImage.texture != null)
		{
			Texture t = RawShowImage.texture;
			int width = t.width;  //实际宽度
			int height = t.height; //实际高度
			Debug.Log (width + "---" + height);
			Texture2D t2d = t as Texture2D;
			if (t2d != null)
			{
				Debug.Log (t2d.width + "   " + t2d.height);
				int _rows_offset = height / Rows;
				int _column_offset = width / Columns;

				for (int i = 0; i < Rows; i++) {
					for (int j = 0; j < Columns; j++) {
						int _startx = i * _column_offset;
						int _endx = (i + 1) * _column_offset;
						int _starty = j * _rows_offset;
						int _endy = (j + 1) * _rows_offset;

						Texture2D newt2d = new Texture2D (_column_offset, _rows_offset);
						for (int z = _startx; z < _endx; z++) 
						{

							for (int w = _starty; w < _endy; w++)
							{
								if (z < width && w < height) {
									Color c = t2d.GetPixel (z, w);
									newt2d.SetPixel (z, w, c);
								}

							}
						}
						newt2d.Apply ();
						if (newt2d != null) 
						{
							int _num = i * Columns + j;
							listTextures.Add (newt2d as Texture);
						}


					}
				}
			}
		}
	}


	public void CaculateState()
	{
		successItem = 0;
		for (int i = 0; i < listItems.Count; i++)
		{
			GameObject _g = listItems [i].gameObject.transform.parent.gameObject;
			int _parent_num=int.Parse(_g.name.Split('_')[0]);
			PicItem _myPicitem = listItems [i].gameObject.GetComponent<PicItem> ();
			int _myNum = _myPicitem.Num;
			if (_myNum == _parent_num) {
				successItem++;
			}

		}
		totalItem = listItems.Count;
		float _score = (float)successItem / (float)totalItem;
		_score = Mathf.RoundToInt (_score * 100) / 100.0f;
		scoreSlider.value = _score;
		scoreText.text=_score*100+"%";
		if (_score >= 1.0f) {
			SuccessPanel.gameObject.SetActive (true);
		}
		adu1.Play ();

	}


	void OnGUI1(){
		if (GUILayout.Button ("ddddddddddddd")) {
			CaculateState ();

		}
	}
	#endregion


	#region 资源池

	private List<GameObject> listpool=new List<GameObject>();

	public GameObject getAGird(){
		if (listpool.Count > 0) {
			GameObject _g=listpool[0];
			listpool.Remove (_g);
			_g.SetActive (true);
			return _g;
		} else {
			GameObject _g= GameObject.Instantiate (gridItemPrefab, gridItemPrefab.transform.position, Quaternion.identity) as GameObject;
			_g.SetActive (true);
			return _g;
		}
	}

	public void GiveBackAll(RectTransform parent){
		foreach (Transform t in parent) {
			t.gameObject.SetActive (false);
			listpool.Add (t.gameObject);
		}
	}

	#endregion

} 


