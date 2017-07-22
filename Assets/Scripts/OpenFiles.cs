using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;


public enum FileType{
	Dir,Img,Normal
}


public class FileItem{
	public 	string name;
	public  FileType fileType;

}


public class OpenFiles : MonoBehaviour 
{

	public List<FileItem> ShowFileList = new List<FileItem> ();
	List<string> FilePathList = new List<string> ();


	public Stack<string> filePahtStack = new Stack<string> ();
	public GameObject ButtonPrefab;
	public RectTransform content;
	public Text showImageResult;

	public OpenImages openImageWnd;
	void Start () 
	{
		ButtonPrefab.SetActive (false);
		initOpenFilesList ();
		InitFilesUIs ();
	}

	#region  OnclickEvent

	public void OnClickFileButton(GameObject go)
	{
		FileItemUI item = go.GetComponent<FileItemUI> ();
		if (item.fileItm.fileType==FileType.Img)
			return;
		string _path = item.fileItm.name;
		OpenFileList (_path);
		InitFilesUIs ();
		filePahtStack.Push (_path);



	}

	public void OnClickLastDir()
	{
		string str = "";
		if (filePahtStack.Count > 1)
		{
			
			 str= filePahtStack.Pop ();
			str = filePahtStack.Pop ();
			OpenFileList (str);
			InitFilesUIs ();
		} 
		else if (filePahtStack.Count >=0)
		{
			if(filePahtStack.Count>0)
			     str = filePahtStack.Pop ();
			ShowFileList.Clear ();
			initOpenFilesList ();
			InitFilesUIs ();
		}

	}


	#endregion
	public void OnClickOpenImageDialog()
	{
		if (imageList.Count > 0) 
		{
			MainUIContrller.Instance.OnClickOpenImageDialog ();
			openImageWnd.OnOpenImageList (imageList);

		} 
		else {
			TipManager.Instance.show ("没有发现可以加载的图片....");
		}

	}

	#region  

	#endregion



	#region 内部函数





	private bool OpenFileList(string _path)
	{
		ShowFileList.Clear ();
		string[] _showDirList = Directory.GetDirectories (_path);
		string[] _showFileList = Directory.GetFiles (_path);
		if (_showFileList.Length == 0&&_showDirList.Length==0)
			return false;
		
		for (int i = 0; i < _showFileList.Length; i++) 
		{
			FileItem _item = new FileItem ();
			string str=_showFileList[i];
			_item.name = str;
			if (str.EndsWith (".png") || str.EndsWith (".jpg")) 
			{
				_item.fileType = FileType.Img;
				ShowFileList.Add (_item);

			}

		}
		for (int i = 0; i < _showDirList.Length; i++) 
		{
			FileItem _item = new FileItem ();
			string str=_showDirList[i];
			_item.name = str;
			_item.fileType = FileType.Dir;
		    ShowFileList.Add (_item);


		}

		return true;
	}

	private void initOpenFilesList()
	{
		
		if (ShowFileList.Count==0) 
		{
			string[] _showFileList = Directory.GetLogicalDrives ();
			for (int i = 0; i < _showFileList.Length; i++)
			{
				FileItem _item = new FileItem ();
				_item.name = _showFileList [i];
				if (Directory.Exists (_showFileList [i]))
				{
					_item.fileType = FileType.Dir;
					
				} 
				else if (File.Exists (_showFileList [i])) 
				{
					_item.fileType = FileType.Normal;
				}
				ShowFileList.Add (_item);
			}
		}





	}

	public List<string> imageList=new List<string>();
	private void InitFilesUIs()
	{
		ReturnAllResource (content);
		imageList.Clear ();
		if (ShowFileList.Count > 0)
		{
			for (int i = 0; i<ShowFileList.Count; i++) 
			{
				FileItemUI itemUI = getAprefab ().GetComponent<FileItemUI> ();
				FileItem _item=ShowFileList[i];
				itemUI.fileItm = _item;
				if (itemUI != null) 
				{
					itemUI.transform.SetParent (content, false);
					itemUI.textName.text = ShowFileList [i].name;

					if (_item.fileType == FileType.Img)
					{
						imageList.Add (itemUI.textName.text);
						/*
						byte[] bys =	File.ReadAllBytes (_item.name);

						Texture2D t = new Texture2D (300, 300);
						t.LoadImage (bys);
						t.Apply ();
						int _width = 0;
						int _height = 0;
						if (t.width > 500) {
							_width = 500;
							_height =Mathf.RoundToInt( ((float)_width /(float) t.width) * t.height);
						} 
						else 
						{
							_width = t.width;
							_height = t.height;
						}
					
						if (_height > 500) {
							_height = 500;
							_width = Mathf.RoundToInt (((float)_height / t.height) * t.width);
						}

						itemUI.showImagRawImage.gameObject.SetActive (true);
						itemUI.showImagRawImage.texture= t;
						itemUI.textName.gameObject.SetActive (false);
						itemUI.showImagRawImage.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal,_width);
						itemUI.layoutItem.preferredHeight =_height;
						imageCount++;
						*/
					} 
					else
					{
						itemUI.showImagRawImage.gameObject.SetActive (false);
						itemUI.textName.gameObject.SetActive (true);
						itemUI.layoutItem.preferredHeight=100;
						
					}

                    
				}
			}
		}
		if (imageList.Count == 0)
		{
			showImageResult.text = "本目录，未发现图片";
		} 
		else
		{
			showImageResult.text = "发现图片 "+imageList.Count+" 张，点击加载";
		}
	}




	#endregion


	#region  资源池

	public List<GameObject> listObj=new List<GameObject>();

	private GameObject getAprefab(){
		GameObject _go;
		if (listObj.Count > 0) {
			_go = listObj [0];
			listObj.RemoveAt (0);
		} else {
			_go = GameObject.Instantiate (ButtonPrefab) as GameObject;

		}
		_go.SetActive (true);
		return _go;
	}

	private void ReturnAllResource(RectTransform con){
		foreach (Transform t in con) {
			if (t.gameObject.activeInHierarchy) {
				t.gameObject.SetActive (false);
				listObj.Add (t.gameObject);
			}

		}
	}

	#endregion

}
