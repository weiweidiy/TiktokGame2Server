using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 执行器执行参数
    /// </summary>
    public interface IJCombatExecutorExecuteArgs
    {
        List<IJCombatCasterTargetableUnit> TargetUnits { get; set; }

        IJCombatDamageData DamageData { get; set; }

        /// <summary>
        /// 执行参数历史
        /// </summary>
        Dictionary<string, IJCobmatExecuteArgsHistroy> ExecuteArgsHistroy { get; set; }

        void Clear();
    }

    public interface IJCobmatExecuteArgsHistroy
    {

    }

    public class JCombatExecutorExecuteArgsHistroy : IJCobmatExecuteArgsHistroy
    {
        public IJCombatDamageData DamageData { get; set; }
    }

}
