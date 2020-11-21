using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DA2_2020_PRJ.Models
{
    public class PowerUp
    {

        public int AspiradorJato { get; set; }
        public int EscudoAtomico { get; set; }
        public int ArmadilhaMagnetica { get; set; }

        public PowerUp()
        {
            AspiradorJato = 3;
            EscudoAtomico = 3;
            ArmadilhaMagnetica = 3;

        }
    }

   
}
