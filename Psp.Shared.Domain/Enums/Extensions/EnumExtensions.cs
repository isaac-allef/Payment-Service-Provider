using System.ComponentModel;

namespace Psp.Shared.Domain.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum input)
    {
        var field = input.GetType().GetField(input.ToString());
        if (Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
        {
            return attribute.Description;
        }
        return string.Empty;
    }
}