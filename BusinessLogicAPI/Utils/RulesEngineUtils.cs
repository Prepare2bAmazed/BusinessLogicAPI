namespace BusinessLogicAPI.Utils;

public static class RulesEngineUtils
{
    // This handles any list that has an 'Id', 'Start', and 'End' property
    public static bool IsValid(IEnumerable<dynamic> list, object targetId, DateTime start, DateTime end)
    {
        foreach (var item in list)
        {
            if (item.Id.ToString() == targetId.ToString() && start >= item.Start && end <= item.End)
                return true;
        }
        return false;
    }
}
