﻿using JFramework.Game;

namespace TiktokGame2Server.Others
{
public partial class LevelNodeCombatService
    {
        public class TiktokAttrNameQuery : IJCombatTurnBasedAttrNameQuery
        {
            public string GetActionPointName()
            {
                return TiktokAttributesType.Speed.ToString();
            }

            public string GetHpAttrName()
            {
                return TiktokAttributesType.Hp.ToString();
            }

            public string GetMaxHpAttrName()
            {
                return TiktokAttributesType.MaxHp.ToString();
            }
        }

    }
}


