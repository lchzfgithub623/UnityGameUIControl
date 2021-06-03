
namespace LightFramework.UI
{
    /// <summary>
    /// 可选项选择结果类型
    /// </summary>
    public enum SelectableResultType
    {
        /// <summary>
        /// 无意义
        /// </summary>
        None,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 不能被选中, onBeforeSelect返回结果过
        /// </summary>
        CannotBeSelected,
        /// <summary>
        /// 最大可选数量为0
        /// </summary>
        MaxSelectNumZero,
        /// <summary>
        /// 超过最大可选数量
        /// </summary>
        OverMaxSelectNum,
        /// <summary>
        /// 无法取消最后选中项
        /// </summary>
        CannotUnselectLast,
        /// <summary>
        /// 重复选中
        /// </summary>
        RepeatedSelect,
        /// <summary>
        /// 没有设置SelectHandler
        /// </summary>
        NoHandler,
    }
}
