using System.Linq;

namespace ObisMapper.Fluent.Steps
{
    internal static class AdditionalStep
    {
        internal static bool CheckIsDataAccessAllowed(BaseModelRule modelRule, string logicalName, string tag)
        {
            // TODO: Check another rules for obis code with tag and if not exists use primary
            if (modelRule.LogicalNameModels.Any(x => x.Name.Equals(logicalName)
                                                     && (x.Tag.Equals(tag) || modelRule.IsPrimary)))
                return true;

            return false;
        }
    }
}