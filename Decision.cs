using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace N1_SO_KAZAR
{
    public class Decision
    {
        public List<Dictionary<string, int>> decisionTable { get; set; }
        public Decision(List<string[]> vrstice)
        {
            decisionTable = decisionMakingMethods.CSVtoDecision(vrstice);
        }
    }
}
