using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class OpenImages : MonoBehaviour 
{

	public int startIndex=0;
	public int endIndex=4;
	public int offset = 4;
	public List<string> listImages = new List<string> ();
	public FileItemUI[] fileImageGird=new FileItemUI[]{};
	public void OnOpenImageList(List<string> list)
	{


		listImages = list;
		opneImages ();

	}

	List<Texture2D> listT=new List<Texture2D>();
	private void opneImages(){
		for (int i = 0; i < fileImageGird.Length; i++) {
			fileImageGird [i].gameObject.SetActive (false);
			if (i < listT.Count) {
				if(listT[i]!=null)
					DestroyImmediate (listT[i]);
			}
		}
		TipManager.Instance.show ("。。。。。。。。。");
	    for (int i = startIndex; i < endIndex; i++) 
	    {
			
		    if (i < listImages.Count) 
			{
				int k = i - startIndex;
				string str=listImages[i];
	        	byte[] bys =File.ReadAllBytes (str);
				if (k < listT.Count) {
					listT [k] = new Texture2D (300, 300);
				} else {
					listT.Add (new Texture2D (300, 300));
				}

				listT[k].LoadImage (bys);
				listT[k].Apply ();
			

				fileImageGird [k].gameObject.SetActive (true);
				fileImageGird [k].showImagRawImage.texture = listT[k];
				fileImageGird [k].name = str;
				//DestroyImmediate (t);
			}
	    }
		TipManager.Instance.OnClickClose ();


	}



	public void OnClickNextImage(){
		
		startIndex += offset;
		endIndex += offset;
		if (endIndex > listImages.Count) {
			endIndex = listImages.Count;
		}
		if (startIndex >= endIndex) 
		{
			if (listImages.Count >= offset) {
				startIndex = endIndex - offset;
			}
			else {
				startIndex = endIndex - listImages.Count;
			}

		}

		opneImages ();
	}


	public void OnClickPreviousImage(){
		startIndex -=offset;
		endIndex -= offset;
		if (startIndex < 0)
			startIndex = 0;
		if (startIndex >=endIndex) 
		{
			if (listImages.Count >= offset)
			{
				endIndex = startIndex+offset;
			}
			else {
				endIndex= startIndex + listImages.Count;
			}

		}
		opneImages ();
	}

	public void OnClickImage(GameObject go){
		FileItemUI it=go.GetComponent<FileItemUI>();
		Debug.Log (it.name);
		MainUIContrller.Instance.SelectFilePic (it.name);
		TipManager.Instance.show ("图片已经选择。。。。。");
	}

}
