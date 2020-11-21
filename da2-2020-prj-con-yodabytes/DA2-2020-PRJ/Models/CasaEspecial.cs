using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DA2_2020_PRJ.Models
{
    public class CasaEspecial
    {
        public int Casa { get; set; }
        public string Mensagem { get; set; }
        public int Movimento { get; set; }
        public bool PerdeVez { get; set; }
        public bool JogaNovamente { get; set; }

        
        
        



        public CasaEspecial(int casa, string mensagem, int movimento, bool perdeVez, bool jogaNovamente)
        {
            Casa = casa;
            Mensagem = mensagem;
            Movimento = movimento;
            PerdeVez = perdeVez;
            JogaNovamente = jogaNovamente;

            if (PerdeVez)
                JogaNovamente = false; //garantir consistência interna

        }

        
    }
}
