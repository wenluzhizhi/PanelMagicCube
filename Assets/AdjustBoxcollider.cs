using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MySwing.Balance
{
	
	public class AdjustBoxcollider :UIBehaviour
	{
		public BoxCollider2D box2d;
		public RectTransform myRect;
		public float boxcolliderRatio=1.0f;
		protected override void OnRectTransformDimensionsChange ()
		{
			base.OnRectTransformDimensionsChange ();
			if (myRect == null)
				myRect = this.GetComponent<RectTransform> ();
			if (myRect == null)
				return;
			if (box2d == null)
				box2d = this.gameObject.GetComponent<BoxCollider2D> ();
			if (box2d == null)
				return;
		
			Vector2 _offsetV=new Vector2 (0.5f,0.5f)-myRect.pivot;
			box2d.offset = new Vector2 (myRect.rect.size.x*_offsetV.x,myRect.rect.size.y*_offsetV.y);
			box2d.size = myRect.rect.size*boxcolliderRatio;
		}


		public void Rest(){
			OnRectTransformDimensionsChange ();
		}
	}


}
