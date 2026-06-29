using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DeviceHub.Utils
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取单个枚举的 Description
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault();

            return attr?.Description ?? value.ToString();
        }

        /// <summary>
        /// 获取枚举列表
        /// Key：枚举值
        /// Value：Description
        /// </summary>
        public static List<KeyValue> GetKeyValues<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new KeyValue
                {
                    Key = Convert.ToInt64(e).ToString(),
                    Value = ((Enum)(object)e).GetDescription()
                })
                .ToList();
        }
        public class KeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
