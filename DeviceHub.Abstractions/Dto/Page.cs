using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Abstractions.Dto
{
    /// <summary>
    /// 分页返回对象
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class Page<T>
    {
        /// <summary>
        /// 当前页码（从1开始）
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages =>
            TotalCount == 0
                ? 0
                : (int)Math.Ceiling(TotalCount * 1.0 / PageSize);

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// 当前页数据
        /// </summary>
        public List<T> Data { get; set; } = new();
    }
}
