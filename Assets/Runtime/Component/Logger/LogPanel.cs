//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using System.Diagnostics;
//using System;

//namespace Cat.Core
//{
//    public class LogPanel : MonoBehaviour,IBeginDragHandler, IDragHandler
//    {
//        TextMeshProUGUI _textmesh;
//        RectTransform _rt;
//        RectTransform _bthandle;
//        RectTransform _logo;
//        Button _btenlarge;
//        Button _btreduce;
//        Button _btrecycle;
//        Button _btclose;
//        bool _logoIsup = true;
//        bool _handleIsRight = true;
//        private void Awake()
//        {
//            try
//            {
//                _textmesh = GameObject.Find("Viewport/Content/TextMeshPro Text").GetComponent<TextMeshProUGUI>();
//                _btenlarge = GameObject.Find("BtHandle/_btenlarge").GetComponent<Button>();
//                _btreduce = GameObject.Find("BtHandle/_btreduce").GetComponent<Button>();
//                _btrecycle = GameObject.Find("BtHandle/_btrecycle").GetComponent<Button>();
//                _btclose = GameObject.Find("BtHandle/_btclose").GetComponent<Button>();
//                _rt = this.GetComponent<RectTransform>();
//                _bthandle = GameObject.Find("BtHandle").GetComponent<RectTransform>();
//                _logo = GameObject.Find("Logo").GetComponent<RectTransform>();
//                _rt.anchorMin = Vector2.zero;//(0.0)左下
//                _rt.anchorMax = Vector2.zero;//(0.0)左下
//                _rt.pivot = Vector2.zero;//锚点左下
//                _bthandle.anchorMin = Vector2.right;//(1.0)右下
//                _bthandle.anchorMax = Vector2.right;//(1.0)右下
//                _bthandle.pivot = Vector2.zero;//锚点右下
//                _logo.anchorMin = Vector2.up;//(0.1)依靠左上
//                _logo.anchorMax = Vector2.up;//(0.1)依靠左上
//                _logo.pivot = Vector2.zero;//锚点左下
//                if (_textmesh == null)
//                {
//                    throw new System.Exception("Log Panel is crash ! Unable to locate the TextMeshProUGUI component");
//                }
//                _textmesh.Register();
//                _btenlarge.onClick.AddListener(_btenlargeOnclick);
//                _btreduce.onClick.AddListener(_btreduceOnclick);
//                _btrecycle.onClick.AddListener(_btrecycleOnclick);
//                _btclose.onClick.AddListener(_btcloseOnclick);
//            }
//            catch (System.Exception e)
//            {
//                Debug.LogError(e.ToString());
//                this.enabled = false;
//            }
//        }

//        float minX, maxX, minY, maxY;
//        Vector3 offset = Vector3.zero;
//        public void OnBeginDrag(PointerEventData eventData)
//        {
//            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_rt, eventData.position, eventData.enterEventCamera, out Vector3 globalMousePos))
//            {
//                offset = _rt.position - globalMousePos;
//            }
//        }
//        public void OnDrag(PointerEventData eventData)
//        {
//            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_rt, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
//            {
//                minX = _rt.rect.width * _rt.pivot.x * _rt.localScale.x;
//                maxX = Screen.width - (_rt.rect.width * (1 - _rt.pivot.x)) * _rt.localScale.x;

//                minY = _rt.rect.height * _rt.pivot.y * _rt.localScale.y;
//                maxY = Screen.height - (_rt.rect.height * (1 - _rt.pivot.y)) * _rt.localScale.y;

//                Vector3 v = globalMousePos + offset;
//                v.x = Mathf.Clamp(v.x, minX, maxX);
//                v.y = Mathf.Clamp(v.y, minY, maxY);
//                _rt.position = v;

//                #region fix the _logo and _bthanle position
//                /*
//                 * 边界情况处理下另外_logo和_bthanle
//                 * 测了一下，这段费时0.002-0.003ms左右
//                 */
//                //上边界
//                bool upBorder = v.y > maxY - (_logo.rect.height * (1 - _logo.pivot.y) * _logo.localScale.y);
//                //右边界
//                bool rightBorder = v.x > maxX - (_bthandle.rect.width * (1 - _bthandle.pivot.x) * _bthandle.localScale.x);
//                //左边界
//                bool leftBorder = v.x < minX + (_bthandle.rect.width * (1 - _bthandle.pivot.x) * _bthandle.localScale.x);
//                if ((upBorder && _logoIsup)||(upBorder && rightBorder)|| (upBorder && leftBorder))//_rt已经进入上边界，且logo在上面  或者 _rt已经进入上边界且操作版进入右边界
//                {
//                    Vector3 _logov = rightBorder ?
//                        new Vector3(v.x - _logo.rect.width, v.y + _rt.rect.height - _logo.rect.height, 0)
//                        :
//                        new Vector3(v.x + _rt.rect.width, v.y + _rt.rect.height - _logo.rect.height, 0);
//                    _logo.position = _logov;
//                    _logoIsup = false;
//                }
//                else if (!upBorder && !_logoIsup)//_rt已经离开上边界，且logo不在up(默认一直在上面)
//                {
//                    _logo.position = new Vector3(v.x, v.y + _rt.rect.height, 0);
//                    _logoIsup = true;
//                }
//                if (rightBorder && _handleIsRight)
//                {
//                    _bthandle.position = new Vector3(v.x - _bthandle.rect.width, v.y, 0);
//                    _handleIsRight = false;
//                }
//                else if (!rightBorder && !_handleIsRight)
//                {
//                    _bthandle.position = new Vector3(v.x + _rt.rect.width, v.y, 0);
//                    _handleIsRight = true;
//                }
//                #endregion
//            }
//        }
//        public void _btenlargeOnclick()
//        {
//            _textmesh.fontSize += 1;
//        }
//        public void _btreduceOnclick()
//        {
//            _textmesh.fontSize -= 1;
//        }
//        public void _btrecycleOnclick()
//        {
//            _textmesh.text = string.Empty;
//        }
//        private void _btcloseOnclick()
//        {
//            this.gameObject.SetActive(false);
//        }
//        private void OnDestroy()
//        {
//            _textmesh.UnRegister();
//        }
//    }
//}
