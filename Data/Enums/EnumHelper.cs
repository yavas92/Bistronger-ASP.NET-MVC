using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

/// <summary>
/// Joren
/// https://stackoverflow.com/questions/13099834/how-to-get-the-display-name-attribute-of-an-enum-member-via-mvc-razor-code
/// </summary>
namespace Bistronger.Data.Enums
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()?
                    .GetMember(enumValue.ToString())?
                    .First()?
                    .GetCustomAttribute<DisplayAttribute>()?
                    .Name;
        }
    }
}