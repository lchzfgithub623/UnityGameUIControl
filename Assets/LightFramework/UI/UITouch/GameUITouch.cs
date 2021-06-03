using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LightFramework.UI
{
    /// <summary>
    /// ui触摸 处理程序
    /// </summary>
    public class GameUITouch : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        #region 公有字段

        /// <summary>
        /// 长安检测阀值, 按下时间超过这个值即表示长按
        /// </summary>
        [HideInInspector]
        public float LongThresholdTime = 0.5f;

        /// <summary>
        /// 双击间隔, 这个是两次点击间隔最短检测时间
        /// </summary>
        [HideInInspector]
        public float DoubleIntervalTime = 0.5f;

        /// <summary>
        /// 点击允许移动距离的平方值
        /// 大于0的值:在目标内按下和弹起时的距离小于等于该值才算点击, 一般为10-50
        /// -1:表示不做距离检测(只要手指一直在目标内部就算点击)
        /// </summary>
        [HideInInspector]
        public float ClickMoveDis = -1;

        #endregion

        #region 私有字段

        private IGameUITouchHandle touchHandle;

        /// <summary>
        /// 点击点是否在对象内
        /// </summary>
        private bool pointInGameObject;

        /// <summary>
        /// 鼠标第一次点击时间
        /// </summary>
        private float firstClickTime;

        /// <summary>
        /// 按下时信息
        /// </summary>
        private PointerEventData pointerEventData;

        /// <summary>
        /// 按下时位置
        /// </summary>
        private Vector2 pointerDownPos;

        /// <summary>
        /// 是否进入长按
        /// </summary>
        private bool inLongPress;

        /// <summary>
        /// 点击次数
        /// </summary>
        private int clickCount;

        private UnityAction<PointerEventData> inAction;
        private UnityAction<PointerEventData> downAction;
        private UnityAction<PointerEventData> longPressAction;
        private UnityAction<PointerEventData> clickAction;
        private UnityAction<PointerEventData> doubleClickAction;
        private UnityAction<bool> upOrExitAction;

        #endregion

        #region 公有属性 注册的事件监听个数

        /// <summary>
        /// 注册的 进入事件监听 个数
        /// </summary>
        public int InActionCount
        {
            get { return inAction != null ? inAction.GetInvocationList().Length : 0; }
        }

        /// <summary>
        ///注册的 按下事件监听 个数 
        /// </summary>
        public int DownActionCount
        {
            get { return downAction != null ? downAction.GetInvocationList().Length : 0; }
        }

        /// <summary>
        /// 注册的 长按事件监听 个数 
        /// </summary>
        public int LongPressActionCount
        {
            get { return longPressAction != null ? longPressAction.GetInvocationList().Length : 0; }
        }

        /// <summary>
        /// 注册的 点击事件监听 个数 
        /// </summary>
        public int ClickActionCount
        {
            get { return clickAction != null ? clickAction.GetInvocationList().Length : 0; }
        }

        /// <summary>
        /// 注册的 双击事件监听 个数 
        /// </summary>
        public int DoubleClickActionCount
        {
            get { return doubleClickAction != null ? doubleClickAction.GetInvocationList().Length : 0; }
        }

        /// <summary>
        /// 注册的 弹起或者离开事件监听 个数 
        /// </summary>
        public int UpOrExitActionCount
        {
            get { return upOrExitAction != null ? upOrExitAction.GetInvocationList().Length : 0; }
        }

        #endregion

        #region 公有方法 注册持有者

        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        /// <param name="touchHandle"></param>
        public void RegisterHandle(IGameUITouchHandle touchHandle)
        {
            this.touchHandle = touchHandle;
        }

        #endregion

        #region 公有方法 添加事件监听

        /// <summary>
        /// 添加 进入事件监听
        /// </summary>
        /// <param name="action"></param>
        public void AddInListener(UnityAction<PointerEventData> action)
        {
            inAction += action;
        }

        /// <summary>
        /// 添加 按下事件监听
        /// </summary>
        /// <param name="action"></param>
        public void AddDownListener(UnityAction<PointerEventData> action)
        {
            downAction += action;
        }

        /// <summary>
        /// 添加 长按事件监听
        /// </summary>
        /// <param name="action"></param>
        public void AddLongPressListener(UnityAction<PointerEventData> action)
        {
            longPressAction += action;
        }

        /// <summary>
        /// 添加 点击事件监听
        /// </summary>
        /// <param name="action"></param>
        public void AddClickListener(UnityAction<PointerEventData> action)
        {
            clickAction += action;
        }

        /// <summary>
        /// 添加 双击事件监听
        /// </summary>
        /// <param name="action"></param>
        public void AddDoubleClickListener(UnityAction<PointerEventData> action)
        {
            doubleClickAction += action;
        }

        /// <summary>
        /// 添加 弹起或者离开事件监听
        /// </summary>
        /// <param name="action">回调参数, true:在目标内弹起点击, false:离开目标</param>
        public void AddUpOrExitListener(UnityAction<bool> action)
        {
            upOrExitAction += action;
        }

        #endregion

        #region 公有方法 替换事件监听

        /// <summary>
        /// 替换 进入事件监听
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public UnityAction<PointerEventData> ReplaceInListener(UnityAction<PointerEventData> action)
        {
            UnityAction<PointerEventData> temp = inAction;
            inAction = action;
            return temp;
        }

        /// <summary>
        /// 替换 按下事件监听
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public UnityAction<PointerEventData> ReplaceDownListener(UnityAction<PointerEventData> action)
        {
            UnityAction<PointerEventData> temp = downAction;
            downAction = action;
            return temp;
        }

        /// <summary>
        /// 替换 长按事件监听
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public UnityAction<PointerEventData> ReplaceLongPressListener(UnityAction<PointerEventData> action)
        {
            UnityAction<PointerEventData> temp = longPressAction;
            longPressAction = action;
            return temp;
        }

        /// <summary>
        /// 替换 点击事件监听
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public UnityAction<PointerEventData> ReplaceClickListener(UnityAction<PointerEventData> action)
        {
            UnityAction<PointerEventData> temp = clickAction;
            clickAction = action;
            return temp;
        }

        /// <summary>
        /// 替换 双击事件监听
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public UnityAction<PointerEventData> ReplaceDoubleClickListener(UnityAction<PointerEventData> action)
        {
            UnityAction<PointerEventData> temp = doubleClickAction;
            doubleClickAction = action;
            return temp;
        }

        /// <summary>
        /// 替换 弹起或离开事件监听
        /// 事件内的参数 true:弹起, false:离开
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public UnityAction<bool> ReplaceUpOrExitListener(UnityAction<bool> action)
        {
            UnityAction<bool> temp = upOrExitAction;
            upOrExitAction = action;
            return temp;
        }

        #endregion

        #region 公有方法 移除事件监听

        /// <summary>
        /// 移除 进入事件监听
        /// </summary>
        /// <param name="action"></param>
        public void RemoveInListener(UnityAction<PointerEventData> action)
        {
            if (action != null)
            {
                inAction -= action;
            }
            else
            {
                inAction = null;
            }
        }

        /// <summary>
        /// 移除 按下事件监听
        /// </summary>
        /// <param name="action"></param>
        public void RemoveDownListener(UnityAction<PointerEventData> action)
        {
            if (action != null)
            {
                downAction -= action;
            }
            else
            {
                downAction = null;
            }
        }

        /// <summary>
        /// 移除 长按事件监听
        /// </summary>
        /// <param name="action"></param>
        public void RemoveLongPressListener(UnityAction<PointerEventData> action)
        {
            if (action != null)
            {
                longPressAction -= action;
            }
            else
            {
                longPressAction = null;
            }
        }

        /// <summary>
        /// 移除 点击事件监听
        /// </summary>
        /// <param name="action"></param>
        public void RemoveClickListener(UnityAction<PointerEventData> action)
        {
            if (action != null)
            {
                clickAction -= action;
            }
            else
            {
                clickAction = null;
            }
        }

        /// <summary>
        /// 移除 双击事件监听
        /// </summary>
        /// <param name="action"></param>
        public void RemoveDoubleClickListener(UnityAction<PointerEventData> action)
        {
            if (action != null)
            {
                doubleClickAction -= action;
            }
            else
            {
                doubleClickAction = null;
            }
        }

        /// <summary>
        /// 移除 弹起或者离开事件监听
        /// </summary>
        /// <param name="action"></param>
        public void RemoveUpOrExitListener(UnityAction<bool> action)
        {
            if (action != null)
            {
                upOrExitAction -= action;
            }
            else
            {
                upOrExitAction = null;
            }
        }

        /// <summary>
        /// 移除所有事件监听
        /// </summary>
        public void RemoveAllListeners()
        {
            inAction = null;
            downAction = null;
            longPressAction = null;
            clickAction = null;
            doubleClickAction = null;
            upOrExitAction = null;
        }
        #endregion

        #region 公有方法 触发事件监听

        /// <summary>
        /// 触发 进入事件监听
        /// </summary>
        /// <param name="e"></param>
        public void TriggerInEvent(PointerEventData e)
        {
            if (inAction != null)
            {
                inAction.Invoke(e);
            }
        }

        /// <summary>
        /// 触发 按下事件监听
        /// </summary>
        /// <param name="e"></param>
        public void TriggerPressEvent(PointerEventData e)
        {
            if (downAction != null)
            {
                downAction.Invoke(e);
            }
        }

        /// <summary>
        /// 触发 长按事件监听
        /// </summary>
        /// <param name="e"></param>
        public void TriggerLongPressEvent(PointerEventData e)
        {
            if (longPressAction != null)
            {
                longPressAction.Invoke(e);
            }
        }

        /// <summary>
        /// 触发 长按事件监听
        /// </summary>
        /// <param name="e"></param>
        public void TriggerClickEvent(PointerEventData e)
        {
            if (clickAction != null)
            {
                clickAction.Invoke(e);
            }
        }

        /// <summary>
        /// 触发 长按事件监听
        /// </summary>
        /// <param name="e"></param>
        public void TriggerDoubleClickEvent(PointerEventData e)
        {
            if (doubleClickAction != null)
            {
                doubleClickAction.Invoke(e);
            }
        }

        /// <summary>
        /// 触发 弹起或者离开事件监听
        /// </summary>
        /// <param name="isUp">true:弹起时在物体内, false:弹起时不在物体内</param>
        public void TriggerUpOrExitEvent(bool isIn)
        {
            if (upOrExitAction != null)
            {
                upOrExitAction.Invoke(isIn);
            }
        }

        #endregion

        #region 接口方法 触摸事件

        public void OnPointerEnter(PointerEventData eventData)
        {
            pointInGameObject = true;
            TriggerInEvent(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isActiveAndEnabled || touchHandle == null || !touchHandle.CanTouch)
            {
                return;
            }
            pointerEventData = eventData as PointerEventData;
            pointerDownPos = pointerEventData.position;
            cancelWaitDoubleClick(false);
            resetLongPressState();
            TriggerPressEvent(pointerEventData);
            if (longPressAction != null)
            {
                StartCoroutine("triggerLongPress");
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopCoroutine("triggerLongPress");
            if (!isActiveAndEnabled || touchHandle == null || !touchHandle.CanTouch || !pointInGameObject)
            {
                return;
            }
            TriggerUpOrExitEvent(true);
            if (inLongPress)
            {
                resetClickState();
                return;
            }
            clickCount++;
            PointerEventData upEventData = eventData as PointerEventData;
            Vector2 upPos = upEventData.pointerCurrentRaycast.screenPosition;
            float dis = Vector2.SqrMagnitude(upPos - pointerDownPos);
            // 判断点击偏移距离, 如果偏移距离过大也不算点击
            if (ClickMoveDis > 0 && dis > ClickMoveDis)
            {
                resetClickState();
                return;
            }
            if (doubleClickAction == null)
            {
                TriggerClickEvent(upEventData);
                resetClickState();
                return;
            }
            float clickTime = Time.realtimeSinceStartup;
            if (clickCount >= 2)
            {
                if ((clickTime - firstClickTime) <= DoubleIntervalTime)
                {
                    TriggerDoubleClickEvent(upEventData);
                }
                else
                {
                    TriggerClickEvent(upEventData);
                }
                resetClickState();
                return;
            }
            firstClickTime = Time.realtimeSinceStartup;
            StartCoroutine("waitDoubleClick");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (pointInGameObject)
            {
                TriggerUpOrExitEvent(false);
            }
            pointInGameObject = false;
            cancelWaitDoubleClick(true);
            resetLongPressState();
            resetClickState();
        }

        private void resetClickState()
        {
            clickCount = 0;
            pointerEventData = null;
        }

        /// <summary>
        /// 触发长按
        /// </summary>
        private IEnumerator triggerLongPress()
        {
            yield return new WaitForSecondsRealtime(LongThresholdTime);
            inLongPress = true;
            TriggerLongPressEvent(pointerEventData);
        }

        /// <summary>
        /// 取消长按状态
        /// </summary>
        private void resetLongPressState()
        {
            inLongPress = false;
            StopCoroutine("triggerLongPress");
        }

        private IEnumerator waitDoubleClick()
        {
            yield return new WaitForSecondsRealtime(0.3f);
            TriggerClickEvent(pointerEventData);
            resetClickState();
        }

        /// <summary>
        /// 取消等待双击
        /// </summary>
        /// <param name="isOut"></param>
        private void cancelWaitDoubleClick(bool isOut)
        {
            StopCoroutine("waitDoubleClick");
            //if (isOut && 1 == clickCount)
            //{
            //    TriggerClickEvent(pointerEventData);
            //}
        }
        #endregion
    }
}

