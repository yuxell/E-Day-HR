using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Utilities
{
    public static class EnumHelper
    {
        public static SelectList GetSelectListFromEnum<T>() where T : Enum
        {
            var list = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new
                {
                    ID = Convert.ToInt32(e),
                    Name = e.GetType()
                            .GetMember(e.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
                });

            return new SelectList(list, "ID", "Name");
        }

        private static string GetEnumDisplayName<T>(T enumValue) where T : Enum
        {
            var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            var displayAttr = member?.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.Name ?? enumValue.ToString();
        }

        public static string GetDisplayName<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            var displayAttribute = member?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? enumValue.ToString();
        }
    }
}
