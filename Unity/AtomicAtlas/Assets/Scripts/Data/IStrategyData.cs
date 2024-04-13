using System.Collections.Generic;

namespace Atlas.Data
{
    public interface IStrategyData
    {
        public IEnumerable<StrategyConfigDefinition> StrategyConfigDefinitions { get; }
        public void Merge(IStrategyData data);
    }
}