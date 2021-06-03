
namespace LightFramework.UI
{
    public interface IUIBase
    {
        /// <summary>
        /// 绑定的mono
        /// </summary>
        GameUIMono UIMono { get; set; }

        /// <summary>
        /// 是否Awake
        /// </summary>
        bool IsAwake { get; set; }

        /// <summary>
        /// 是否Start
        /// </summary>
        bool IsStart { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        bool IsShow { get; set; }


        /// <summary>
        /// 绑定Mono
        /// </summary>
        void OnBindMono(bool bind);

        /// <summary>
        /// awake初始化: 获取控件, 初始化变量或游戏状态
        /// </summary>
        /// <remarks>
        /// OnInit 总是在 OnStart 前调用
        /// </remarks>
        void OnInit();

        /// <summary>
        /// start初始化: 一般用来给变量赋值
        /// </summary>
        void OnStart();

        /// <summary>
        /// 显示
        /// </summary>
        void OnShow();

        /// <summary>
        /// 隐藏
        /// </summary>
        void OnHide();

        /// <summary>
        /// 销毁
        /// </summary>
        void OnDestroy();

        /// <summary>
        /// 尺寸发生变化
        /// </summary>
        void OnRectTransformDimensionsChange();

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="pause"></param>
        void OnApplicationPause(bool pause);

        /// <summary>
        /// 退出应用
        /// </summary>
        void OnApplicationQuit();

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
    }
}
