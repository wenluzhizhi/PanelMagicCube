using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SlicePic : MonoBehaviour 
{

	public Texture2D oldT;
	public Texture2D newT;

	public RawImage raw;
	public RawImage rawShow;
	void Start () 
	{

		//65,60
		//newT = new Texture2D (128, 95);

	   
		newT = new Texture2D (65, 60);
		for (int i = 0; i < 65; i++)
		{
			for(int j=0;j<60;j++){
				newT.SetPixel (i, j, oldT.GetPixel (i, j));
			}
		}
		newT.Apply ();

	}
	

	void Update () {
	    
	}


	int columns=3;
	int rows=3;
	void OnGUI(){
		if (GUILayout.Button ("ddddddd")) {
			if (raw.texture != null) 
			{
				Texture t = raw.texture;
				int width = t.width;
				int height = t.height;

				Debug.Log (width+"---"+height);
				Texture2D t2d = t as Texture2D;

				if (t2d != null) 
				{
					Debug.Log (t2d.width+"   "+t2d.height);
					int _rows_offset = height / rows;
					int _column_offset = width / columns;

					for (int i = 0; i < rows; i++) {
						for (int j = 0; j < columns; j++)
						{
							int _startx = i * _column_offset;
							int _endx = (i + 1) * _column_offset;
							int _starty = j * _rows_offset;
							int _endy = (j + 1) * _rows_offset;

							Texture2D newt2d = new Texture2D (_column_offset, _rows_offset);

							for(int z=_startx;z<_endx;z++)
							{
								
								for(int w=_starty;w<_endy;w++){
									
										Color c = t2d.GetPixel (z, w);
										newt2d.SetPixel (z, w, c);

								}
							}


							newt2d.Apply ();
							if (newt2d != null) {
								Debug.Log ("生成---"+newt2d.width);
								//byte[] byt = newt2d.EncodeToPNG();
								//然后保存为图片
								//File.WriteAllBytes(Application.dataPath + "/shexiang/" +i+"_"+j + ".png", byt);
								rawShow.texture=newt2d as Texture;
							}


						}
					}
				}
			}
		}
	}
}
