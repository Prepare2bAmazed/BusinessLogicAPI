using System.Collections;

namespace BusinessLogicAPI.Utils;

public static class RulesEngineUtils
{
    public static bool IsValid(IEnumerable list, object targetId, DateTime start, DateTime end)
    {
        foreach (var item in list)
        {
            if (item is IDictionary dictionary &&
                dictionary.Contains("ID") &&
                dictionary.Contains("start") &&
                dictionary.Contains("end"))
            {
                var id = dictionary["ID"]?.ToString();
                if (id == null) continue;

                if (!TryToDateTime(dictionary["start"], out var itemStart) ||
                    !TryToDateTime(dictionary["end"], out var itemEnd))
                {
                    continue;
                }

                if (id == targetId.ToString() && start >= itemStart && end <= itemEnd)
                    return true;

                continue;
            }

            try
            {
                dynamic value = item!;
                var id = value.ID?.ToString();
                if (id == null) continue;

                if (!TryToDateTime((object?)value.start, out var itemStart) ||
                    !TryToDateTime((object?)value.end, out var itemEnd))
                {
                    continue;
                }

                if (id == targetId.ToString() && start >= itemStart && end <= itemEnd)
                    return true;
            }
            catch
            {
            }
        }

        return false;
    }

    private static bool TryToDateTime(object? value, out DateTime date)
    {
        if (value is DateTime dt)
        {
            date = dt;
            return true;
        }

        if (value is string s && DateTime.TryParse(s, out date))
            return true;

        if (value != null && DateTime.TryParse(value.ToString(), out date))
            return true;

        date = default;
        return false;
    }
}
