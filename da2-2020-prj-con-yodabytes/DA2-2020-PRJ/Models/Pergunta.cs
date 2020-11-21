using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DA2_2020_PRJ.Models
{
    public class Pergunta
    {
        public int ID { get; set; }
        public string Categoria { get; set; }
        public string TextoPergunta { get; set; }
        public string Resposta1 { get; set; }
        public string Resposta2 { get; set; }
        public string Resposta3 { get; set; }
        public string Resposta4 { get; set; }
        public int RespostaCerta { get; set; } // 1 a 4


        public Pergunta(int id, string categoria, string textoPergunta, string resposta1, string resposta2, string resposta3, string resposta4, int respostaCerta)
        {
            ID = id;
            Categoria = categoria;
            TextoPergunta = textoPergunta;
            Resposta1 = resposta1;
            Resposta2 = resposta2;
            Resposta3 = resposta3;
            Resposta4 = resposta4;
            RespostaCerta = respostaCerta;
        }

        
    }

}
