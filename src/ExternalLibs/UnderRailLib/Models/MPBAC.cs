﻿using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MPBAC")]
    [Serializable]
    public sealed class MPBAC : Condition
    {
        private MPBAC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _mv = info.GetInt32("MPBAC:MV");
            if (DataModelVersion.MinorVersion >= 411)
            {
                _ban = (BaseAbilityEnum)info.GetValue("MPBAC:BAN", typeof(BaseAbilityEnum));
                return;
            }
            var str = info.GetString("MPBAC:BAN");
            Enum.TryParse(str, true, out _ban);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MPBAC:MV", _mv);
            if (DataModelVersion.MinorVersion >= 411)
            {
                info.AddValue("MPBAC:BAN", _ban);
            }
            else
            {
                info.AddValue("MPBAC:BAN", _ban.ToString());
            }
        }

        private int _mv;
        private BaseAbilityEnum _ban;
    }

}
