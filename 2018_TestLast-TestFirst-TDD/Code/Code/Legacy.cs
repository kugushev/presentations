using System;
using System.Collections.Generic;
using System.Text;

namespace Code
{
    public class Legacy
    {
        private dynamic legacyService;
        public void LegacyMethod(dynamic legacy, dynamic legacyLegacy, dynamic le, dynamic ga, dynamic cy)
        {            
            legacy.Legacy();
            var legacy1 = legacy.Legacy + legacyLegacy;
            legacy1.Legacy(le, ga, cy);
            legacy.Shape = legacy.Shape?.ToKitty();
            var legacy42 = legacy
                .Where(l => l.IsLeagacy)
                .Select(l => l.Legacy);
            legacyService.MakeLegacy(legacy42);
            NullReferenceException
        }

        [Test]
        public void Draw_LineIsLast_ReturnsKitty();

        [Test]
        public void Draw_NoShape_DoNothing();

    }
}
