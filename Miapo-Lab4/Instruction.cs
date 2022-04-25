using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miapo_Lab4
{
    class Instruction
    {
        public string Result { get; set; }

        public string[] Paramethers = new string[] { };

        public string formula;

        public Instruction() { }

        public Instruction(string input,string formula,string[] par)
        {
            Result = input;

            this.Paramethers = par;

            this.formula = formula;

        }
    }
}
