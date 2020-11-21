using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DA2_2020_PRJ.Models
{
    public static class Repository
    {
        private static List<Jogo> listaJogos = new List<Jogo>();

        public static List<Jogo> ListaDeJogos
        {
            get
            {
                return listaJogos;
            }
        }

        public static void AdicionarJogo(Jogo novoJogo)
        {
            Random randomId = new Random();
            while (novoJogo.Id == null)
            {
                int id = randomId.Next();
                if (IdLivre(id))
                {
                    novoJogo.Id = id;
                    break;
                }
            }
            ListaDeJogos.Add(novoJogo);
        }

        public static bool IdLivre(int id)
        {
            foreach (Jogo novoJogo in listaJogos)
            {
                if (novoJogo.Id == id)
                {
                    return false;
                }
            }
            return true;
        }

        public static Jogo CarregarJogo(int id)
        {
            foreach (Jogo jogo in listaJogos)
            {
                if (jogo.Id == id)
                {
                    return jogo;
                }
            }
            return null;
        }

       

        






    }
}
