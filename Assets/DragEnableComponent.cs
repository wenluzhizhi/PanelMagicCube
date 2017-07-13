using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace MySwing.Balance
{

	[RequireComponent (typeof(RectTransform))]
	public class DragEnableComponent : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
	{
		


		#region var

		[SerializeField] private RectTransform parentRect;


		[SerializeField] private BoxCollider2D boxCollider2D;
		[SerializeField] private AdjustBoxcollider adjustBox;
		[SerializeField] private Rigidbody2D rigid2D;


		public bool isEnableDrag = true;

		#endregion

		//持有本物体的引用
		[SerializeField] private RectTransform myRect;

		/// <summary>
		/// Start this instance. 初始化拖拽组件。
		/// </summary>
		void Start ()
		{
			this.gameObject.name = "drag_item_" + this.gameObject.name;
			myRect = this.gameObject.GetComponent<RectTransform> ();
			boxCollider2D = this.gameObject.AddMissingComponent<BoxCollider2D> ();
			boxCollider2D.isTrigger = true;
			adjustBox = this.gameObject.AddMissingComponent<AdjustBoxcollider> ();
			adjustBox.boxcolliderRatio = 0.6f;
			rigid2D = this.gameObject.AddMissingComponent<Rigidbody2D> ();
			rigid2D.gravityScale = 0.0f;
			if (adjustBox != null) {
				adjustBox.Rest ();
			}
			RawSize = myRect.rect.size;
			RawAnchorMin = myRect.anchorMin;
			RawAnchorMax = myRect.anchorMax;
			RawPivot = myRect.pivot;
			RawPos = myRect.anchoredPosition;

		}

		/// <summary>
		/// Adds the canvas.由于需要将拖拽的物体放置在所有UI的最上层，所以拖拽之前将该物体添加canvas 组建，
		/// 当拖拽结束后，需要将添加的canvs组件销毁，（使用enable=false 无效）
		/// </summary>
		/// <param name="isTrue">If set to <c>true</c> is true.</param>
		/// <param name="go">Go.</param>
		public void AddCanvas (bool isTrue, GameObject go)
		{
			Canvas ca;
			GraphicRaycaster gpr;
			//Debug.Log ("3333-------");
			if (isTrue) {
				ca = go.AddMissingComponent<Canvas> ();
				ca.overrideSorting = true;
				ca.sortingOrder = 2;
				gpr = go.AddMissingComponent<GraphicRaycaster> ();
				//Debug.Log ("444444-------");

			} else {
				gpr = go.GetComponent<GraphicRaycaster> ();
				if (gpr != null) {
					try {
						//Debug.Log ("555555-------");
						DestroyImmediate (gpr);
					} catch (System.Exception e1) {
						Debug.Log ("addcans==2" + e1.ToString ());
					}

				}
				ca = go.GetComponent<Canvas> ();
				if (ca != null) {
					try {

						//Debug.Log ("66666-------");
						DestroyImmediate (ca);
					} catch (System.Exception e1) {
						Debug.Log ("addcans==1" + e1.ToString ());
					}
				}

			}
		}


		#region  拖拽过程  拖拽开始时记录当前要拖拽物体的与其父亲物体的的关系，如果遇到可替换物体则交换与父亲物体的的位置关系，如果未遇到则则按照保存的关系数据返回。

		//拖拽时机：拖拽结束时检查是否满足替换条件
		[SerializeField] private Vector2 RawSize;
		[SerializeField] private Vector2 RawPos;
		[SerializeField] private Vector2 RawAnchorMin;
		[SerializeField] private Vector2 RawAnchorMax;
		[SerializeField] private Vector2 RawPivot;

		//开始拖动时，应该记录当前位置，及层级关系
		void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
		{
			if (!isEnableDrag)
				return;
			this.parentRect = this.gameObject.transform.parent.GetComponent<RectTransform> ();
			AddCanvas (true, myRect.gameObject);
			RawSize = myRect.rect.size;

			myRect.anchorMin = Vector2.one * 0.5f;
			myRect.anchorMax = Vector2.one * 0.5f;
			myRect.pivot = Vector2.one * 0.5f;
			//this.RawPos = myRect.anchoredPosition;
			myRect.sizeDelta = RawSize;
			SetDraggedPosition (eventData);


		}

		public void OnDrag (PointerEventData eventData)
		{
			if (!isEnableDrag)
				return;
			SetDraggedPosition (eventData);

		}



		void IEndDragHandler.OnEndDrag (PointerEventData eventData)
		{
			if (!isEnableDrag)
				return;
			isEnableDrag = false;
			if (switchCollider != null) {
				try 
				{
					#region   替换逻辑
					//Debug.Log ("找到替换的物体。。。。" + Time.time);
					RectTransform t = switchCollider.gameObject.GetComponent<RectTransform> ();
					Vector2 _rawSize = t.rect.size;
					Vector2 _rawOffsetMax = t.offsetMax;
					Vector2 _rawOffsetMin = t.offsetMin;
					Vector2 _rawAnchorMin = t.anchorMin;
					Vector2 _rawAnchorMax = t.anchorMax;
					Vector2 _rawPivot = t.pivot;

					Transform t_p = t.gameObject.transform.parent;


					t.SetParent (myRect.transform.parent, true);
					t.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, RawSize.x);
					t.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, RawSize.y);
					EndragRect (t);
					t.gameObject.GetComponent<DragEnableComponent> ().isEnableDrag = true;

					AddCanvas (false, myRect.gameObject);
					myRect.SetParent (t_p, false);
					myRect.pivot = _rawPivot;
					myRect.anchorMax = _rawAnchorMax;
					myRect.anchorMin = _rawAnchorMin;
					myRect.offsetMin = _rawOffsetMin;
					myRect.offsetMax = _rawOffsetMax;
					myRect.gameObject.GetComponent<DragEnableComponent> ().isEnableDrag = true;

					#endregion 
				} catch (System.Exception e1) {
					Debug.Log ("替换出现异常：" + Time.time);
				}
			} else {
				//SetDraggedPosition (eventData);
				EndragRect (myRect);
				isEnableDrag = true;
			}
			MainUIContrller.Instance.CaculateState ();

		}

		private void EndragRect (RectTransform t)
		{
			t.DOAnchorPos (this.RawPos, 0.2f, false).OnComplete (delegate () {

				t.pivot = RawPivot; 
				t.anchorMin = RawAnchorMin;
				t.anchorMax = RawAnchorMax;

				t.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, RawSize.x);
				t.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, RawSize.y);
				AddCanvas (false, t.gameObject);
				Image i = t.gameObject.GetComponent<Image> ();
				if (i != null){
					i.SetAllDirty ();
				}
				RawImage r = t.gameObject.GetComponent<RawImage> ();
				if (r != null){
					r.SetAllDirty();
				}
			});
		}

	





		private void SetDraggedPosition (PointerEventData eventData)
		{
			var rt = gameObject.GetComponent<RectTransform> ();
			Vector3 globalMousePos;
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle (rt, eventData.position, eventData.pressEventCamera, out globalMousePos)) {
				rt.position = globalMousePos;
			}
		}

		#endregion

	
		#region  碰撞检测

		public Collider2D switchCollider = null;

		void OnTriggerEnter2D (Collider2D collider)
		{
			if (collider.name.StartsWith ("drag_item_")) {
				switchCollider = collider;
			}


		   
		}

		void OnTriggerExit2D (Collider2D collider)
		{
			if (collider.name.StartsWith ("drag_item_")) {
				switchCollider = null;
			}
		}

		void OnTriggerStay2D (Collider2D collider)
		{
			if (collider.name.StartsWith ("drag_item_")) {
				switchCollider = collider;
			}
		}

		#endregion
	
	}

}
