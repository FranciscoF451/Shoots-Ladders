using System;
using System.Collections.Generic;

namespace DA2_2020_PRJ.Models
{
    public class Jogador
    {
        public string Nome { get; set; }
        public int Casa { get; set; }
        public bool PerdeuVez { get; set; }
        public bool JogaNovamente { get; set; }
        public  PowerUp PowerUps { get; private set; }
        public Jogador(string nome)
        {
            Nome = nome;
            Casa = 1;
            PerdeuVez = false;
            JogaNovamente = false;
            //PowerUp PowerUps = new PowerUp;
        }


    }
}
