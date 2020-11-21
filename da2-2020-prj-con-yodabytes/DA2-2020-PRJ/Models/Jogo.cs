using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http.Features;
using System.Runtime.CompilerServices;

namespace DA2_2020_PRJ.Models
{
    public class Jogo
    {
        //Propriedades
        public int? Id { get; set; }
        public List<Jogador> Jogadores { get; private set; }
        public List<CasaEspecial> CasasEspeciais { get; private set; }
        public List<Pergunta> Perguntas { get; private set; }
        public int PerguntaFinal { get; set; }
        public int RespostaDada { get; set; }
        public int JogadorAnterior { get; set; }
        public int NumeroJogadores { get; private set; }
        public string ModoDeJogo { get; private set; }
        public int JogadorAtivo { get; private set; }
        public string JogadorQueMove { get; private set; }
        public int ValorDado { get; private set; }
        public bool AnimacaoDado { get; private set; }
        public int MovimentoJogador { get; private set; }
        public string Mensagem { get; private set; }
        public int MovimentoAposMensagem { get; set; }
        public int Player1x { get; set; }
        public int Player1y { get; set; }
        public int Player2x { get; set; }
        public int Player2y { get; set; }
        public int Player3x { get; set; }
        public int Player3y { get; set; }
        public int Player4x { get; set; }
        public int Player4y { get; set; }


        //Construtores
        public Jogo(int numeroJogadores, string modoJogo, string nomeJogador1, string nomeJogador2, string nomeJogador3, string nomeJogador4)
        {
            ModoDeJogo = modoJogo;
            NumeroJogadores = numeroJogadores;
            InicializarJogadores(nomeJogador1, nomeJogador2, nomeJogador3, nomeJogador4);
            JogadorAtivo = 1;

            Player1x = 40;
            Player1y = 520;
            Player2x = 65;
            Player2y = 520;
            Player3x = 40;
            Player3y = 495;
            Player4x = 65;
            Player4y = 495;

            JogadorQueMove = ObterTextoJogador(JogadorAtivo);
            AnimacaoDado = false;
            ValorDado = 6;
            MovimentoJogador = 0;
            Mensagem = "";
            MovimentoAposMensagem = 0;

            CriarEfeitosDeJogo();



        }

        //Métodos
        private void InicializarJogadores(string nomeJogador1, string nomeJogador2, string nomeJogador3, string nomeJogador4)
        {
            Jogadores = new List<Jogador>();
            Jogador jogador1 = new Jogador(nomeJogador1);
            Jogador jogador2 = new Jogador(nomeJogador2);

            //adicionar o jogador à lista de jogadores
            Jogadores.Add(jogador1);
            Jogadores.Add(jogador2);
            if (NumeroJogadores >= 3)
            {
                Jogador jogador3 = new Jogador(nomeJogador3);
                Jogadores.Add(jogador3);
            }
            if (NumeroJogadores == 4)
            {
                Jogador jogador4 = new Jogador(nomeJogador4);
                Jogadores.Add(jogador4);
            }
        }

        public void ExecutarAcao(string playerAction, int player1x, int player1y, int player2x, int player2y, int player3x, int player3y, int player4x, int player4y, int resposta)
        {
            AtualizarCoordenadasXY(player1x, player1y, player2x, player2y, player3x, player3y, player4x, player4y);

            switch (playerAction)
            {
                case "LancarDado": LancarDadoAvancar(); break;
                case "Responder": ValidarResposta(resposta); break;
                default: break;
            }
        }

        private void AtualizarCoordenadasXY(int player1x, int player1y, int player2x, int player2y, int player3x, int player3y, int player4x, int player4y)
        {
            Player1x = player1x;
            Player1y = player1y;
            Player2x = player2x;
            Player2y = player2y;
            Player3x = player3x;
            Player3y = player3y;
            Player4x = player4x;
            Player4y = player4y;
        }

        private void LancarDadoAvancar()
        {
            Random randomDado = new Random();
            ValorDado = randomDado.Next(1, 7);
            AnimacaoDado = true;
            Avancar();
        }

        private void Avancar()
        {
            Jogador jogador = Jogadores[JogadorAtivo - 1];
            int movimento = ValorDado;
            if (jogador.Casa + ValorDado > 40)
            {
                movimento = 40 - jogador.Casa;                
            }
            jogador.Casa = jogador.Casa + movimento;
            if (jogador.Casa == 40 && ModoDeJogo == "Desafio")
            {
                SelecionarPergunta();
            }
            JogadorQueMove = ObterTextoJogador(JogadorAtivo);
            MovimentoJogador = movimento;
            Mensagem = "";
            MovimentoAposMensagem = 0;
            foreach (CasaEspecial casa in CasasEspeciais)
            {
                if (jogador.Casa == casa.Casa)
                {
                    Mensagem = casa.Mensagem;
                    MovimentoAposMensagem = casa.Movimento;
                    jogador.Casa = jogador.Casa + MovimentoAposMensagem;
                    jogador.JogaNovamente = casa.JogaNovamente;
                    jogador.PerdeuVez = casa.PerdeVez;
                }

            }
            if (jogador.Casa < 40) 
            {
                PassarAoProximoJogador();
            }
            

            
        }

        private string ObterTextoJogador(int numeroJogador)
        {
            switch (numeroJogador)
            {
                case 1: return "one";
                case 2: return "two";
                case 3: return "three";
                case 4: return "four";
                default: return null;
            }
        }

        private void PassarAoProximoJogador()
        {            
            if (Jogadores[JogadorAtivo - 1].JogaNovamente)
            {
                Jogadores[JogadorAtivo - 1].JogaNovamente = false; // limpamos o jogar novamente e está feito (não trocamos a vez)
            }            
            else
            {
                bool proximoEncontrado = false;
                int atual = JogadorAtivo;
                do
                {
                    int proximoNumeroJogador = ObterNumeroJogadorSeguinte(atual);
                    if (!Jogadores[proximoNumeroJogador - 1].PerdeuVez) //se não perdeu a vez fica este
                    {
                        JogadorAtivo = proximoNumeroJogador;
                        proximoEncontrado = true;
                    }                    
                    else // se perdeu a vez, limpamos o perder a vez e passamos ao próximo
                    {
                        Jogadores[proximoNumeroJogador - 1].PerdeuVez = false;
                        atual = proximoNumeroJogador;
                    }
                } while (proximoEncontrado == false);
            }
        }
        
        private int ObterNumeroJogadorSeguinte(int numeroJogador)
        {
            if (numeroJogador < NumeroJogadores)
                return numeroJogador + 1;
            else
                return 1;
        }

        private void CriarCasasEspeciais()
        {
            CasasEspeciais = new List<CasaEspecial>();
            CasaEspecial casa2 = new CasaEspecial(2, "Atravessar a rua, Só na passadeira. Avança até à casa 4", 2, false, false);
            CasaEspecial casa3 = new CasaEspecial(3, "Entre uma peça de fruta e um refrigerante, preferiste a fruta. Tens razão!" +
            " Escolheste bem, mas não te esqueças, a fruta deve ser sempre bem lavada. Avança até à casa 7 para a lavar.", 4, false, false);
            CasaEspecial casa5 = new CasaEspecial(5, "A papeleira está tão cheia, não comporta mais papéis. É melhor ires deitar o papel do" +
                " gelado na próxima papeleira. Avança até à casa 10.", 5, false, false);
            CasaEspecial casa8 = new CasaEspecial(8, "Esqueces-te sempre de lavar os dentes. Por isso tens uma doença chamada cárie." +
                "Tens de ir ao dentista à casa 2.", -6, false, false);
            CasaEspecial casa9 = new CasaEspecial(9, "Bandeira vermelha é sinal de perigo - não se pode tomar banho. Vai jogar com a bola," +
                " mas num sítio onde não incomodes as outras pessoas, na casa 13.", 4, false, false);
            CasaEspecial casa11 = new CasaEspecial(11, "Em casa, fazes a separação de resíduos e coloca-los no ecoponto? Avança até à casa" +
                " 15 e mostra - nos onde eles se encontram.", 4, false, false);
            CasaEspecial casa14 = new CasaEspecial(14, "Os trabalhos de colagem e pintura são divertidos, mas é preciso tomar cuidado, as tintas" +
                " e colas são venenosas. Como não lavaste as mãos, recua até à casa 7 e vai lavá - las.", -7, false, false);
            CasaEspecial casa16 = new CasaEspecial(16, "Toma duches rápidos de 5 minutos, evita os banhos de imersão. Pula para o teu duche" +
                " na casa 20.", 4, false, false);
            CasaEspecial casa19 = new CasaEspecial(19, "O leite é um alimento benéfico para a saúde dos ossos e dentes. Tu bebes leite, estás" +
                " forte e saudável, por isso avança mais rapidamente. Joga outra vez.", 0, false, true);
            CasaEspecial casa21 = new CasaEspecial(21, "O sol favorece o crescimento, mas é preciso muito cuidado com as queimaduras solares. " +
                "Recua até à casa 17 e vai buscar o teu chapéu e aplicar um protetor solar.", -4, false, false);
            CasaEspecial casa22 = new CasaEspecial(22, "Chamaste a atenção da mãe que estava a comprar iogurtes fora do prazo de validade." +
                " Avança até à casa 26 e vai escolher os iogurtes.", 4, false, false);
            CasaEspecial casa23 = new CasaEspecial(23, "Foste andar de bicicleta numa rua onde passam automóveis e quase tiveste um" +
                " acidente. Para te recompores do susto fica uma vez sem jogar.", 0, true, false);
            CasaEspecial casa25 = new CasaEspecial(25, "Danificar os baloiços do parque infantil público é uma grande maroteira." +
                " Mereces um castigo! Volta ao início do jogo.", -24, false, false);
            CasaEspecial casa27 = new CasaEspecial(27, "O cão é um animal nosso amigo, mas pode ser transmissor de doenças graves, " +
                "por isso é preciso cuidar bem dele. Leva - o ao Veterinário na casa 32 para ser vacinado.", 5, false, false);
            CasaEspecial casa29 = new CasaEspecial(29, "Nunca deves revelar o teu nome e morada aos teus parceiros de jogo online." +
                " Joga com precaução e avança 3 casas.", 3, false, false);
            CasaEspecial casa31 = new CasaEspecial(31, "Lembraste o pai que não deve estacionar em cima do passeio. Avança" +
                " até ao parque de estacionamento na casa 35.", 4, false, false);
            CasaEspecial casa33 = new CasaEspecial(33, "Não tomaste o pequeno-almoço antes de ires para a escola, podes ficar doente!" +
                " Ficas em casa a tomar o pequeno - almoço durante uma jogada.", 0, true, false);
            CasaEspecial casa37 = new CasaEspecial(37, "Acabaste de almoçar, só podes tomar banho daqui a 3 horas." +
                "Entretanto põe a tua imaginação à prova e constrói castelos de areia. Enquanto pensas, joga outra vez.", 0, false, true);
            CasaEspecial casa38 = new CasaEspecial(38, "Comeste uma banana. Que bom! Mas atiraste a casca para o chão..." +
                " Apanha - a e vai deitá - la no lixo na casa 34.", -4, false, false);
            CasaEspecial casa39 = new CasaEspecial(39, "Foste ao supermercado com a mãe, sem uma lista de compras, e gastaste muito dinheiro." +
                "Compraste coisas desnecessárias... Para te recompores fica uma vez sem jogar.", 0, true, false);
            CasasEspeciais.Add(casa2);
            CasasEspeciais.Add(casa3);
            CasasEspeciais.Add(casa5);
            CasasEspeciais.Add(casa8);
            CasasEspeciais.Add(casa9);
            CasasEspeciais.Add(casa11);
            CasasEspeciais.Add(casa14);
            CasasEspeciais.Add(casa16);
            CasasEspeciais.Add(casa19);
            CasasEspeciais.Add(casa21);
            CasasEspeciais.Add(casa22);
            CasasEspeciais.Add(casa23);
            CasasEspeciais.Add(casa25);
            CasasEspeciais.Add(casa27);
            CasasEspeciais.Add(casa29);
            CasasEspeciais.Add(casa31);
            CasasEspeciais.Add(casa33);
            CasasEspeciais.Add(casa37);
            CasasEspeciais.Add(casa38);
            CasasEspeciais.Add(casa39);


        }

        private void CriarPerguntas()
        {
            Perguntas = new List<Pergunta>();
            Pergunta pr1 = new Pergunta(1, "Alimentação", "Identifica qual a maior fatia da roda dos alimentos ?", "Fruta", "Hortícolas",
                "Leite e Derivados", "Cereais e derivados, Tubérculos", 4);
            Pergunta pr2 = new Pergunta(2, "Alimentação", "Quantas vezes devemos comer por dia?", "6 vezes, pequeno-almoço, lanche da manhã," +
                " almoço, lanche da tarde, jantar e ceia", "3 vezes, pequeno almoço, almoço e jantar",
                "4 vezes, pequeno-almoço, almoço, lanche da tarde e jantar ", "2 vezes, Almoço e Jantar", 1);
            Pergunta pr3 = new Pergunta(3, "Saúde", "A que horas deves evitar a exposição solar?", "Entre as 8 e as 9h", "Entre as 12 e as 14h",
                "Entre as 11 e as 17h", "Entre as 12 e as 16:30h", 3);
            Pergunta pr4 = new Pergunta(4, "Saúde", "Quantas vezes devemos lavar os dentes por dia?", "Apenas 1 vez", "2 vezes é suficiente",
                "3 vezes ou mais", "Não é preciso, se não comer doces", 3);
            Pergunta pr5 = new Pergunta(5, "Poupança", "O Tiago recebeu 50€ de presente dos seus avós nos anos e quer comprar um jogo para" +
                " o computador que custa 30€, mas a sua mãe só o deixa gastar metade do dinheiro que ele recebeu, será que o Tiago consegue comprar o jogo?"
                , "Sim, e ainda lhe sobra 5€", "não, porque ele já não quer o jogo", "sim, e ainda lhe sobra 10€", "não, porque ficam a faltar 5€", 4);
            Pergunta pr6 = new Pergunta(6, "Poupança", "O Manuel está a poupar dinheiro para comparar um computador para usar para fazer os trabalhos da" +
                " escola que custa 50€ e recebeu 80€ dos seus pais, mas quando ia para comprar o computador, viu na loja um jogo que custava 30€, e quis logo comprar " +
                "o jogo também, mas depois pensou que não queria ficar sem o seu dinheiro, o que deve fazer o Manuel para não ficar sem dinheiro?"
                , "Pode comprar os 2 porque têm dinheiro que chegue", "É melhor comprar o computador primeiro para depois poupar mais dinheiro para o jogo",
                "O jogo é mais importante por isso o melhor é comprar só o jogo", "Todas estão erradas", 2);
            Pergunta pr7 = new Pergunta(7, "Consumo", "O joão quer fazer um sumo de laraja, para os seus 2 amigos, cada sumo leva 2 laranjas, quantas laranjas" +
                " precisa de comprar o João na mercearia ?", "4 laranjas", "2 laranjas", "10 laranja", "5 laranjas", 1);
            Pergunta pr8 = new Pergunta(8, "Consumo", "A Maria quer fazer um bolo que leva uma dúzia de ovos, quantos ovos leva o bolo?", "6","12", "20", "3", 2);
            Pergunta pr9 = new Pergunta(9, "Ambiente", "Qual o ecoponto onde colocamos as latas?", "Verde", "Azul", "Amarelo", "Lixo normal", 3);
            Pergunta pr10 = new Pergunta(10, "Ambiente", "No nosso planeta, qual o tipo de água que existe em maior quantidade ? ", "Doce", "Salgada", "Agridoce", "Todas estão erradas", 1);
            Perguntas.Add(pr1);
            Perguntas.Add(pr2);
            Perguntas.Add(pr3);
            Perguntas.Add(pr4);
            Perguntas.Add(pr5);
            Perguntas.Add(pr6);
            Perguntas.Add(pr7);
            Perguntas.Add(pr8);
            Perguntas.Add(pr9);
            Perguntas.Add(pr10);

        }
        private void ValidarResposta(int resposta)
        {
            RespostaDada = 0;
            MovimentoJogador = 0;
            Mensagem = "";
            MovimentoAposMensagem = 0;
            Jogador jogador = Jogadores[JogadorAtivo-1];
            RespostaDada = resposta;
            if (RespostaDada != Perguntas[PerguntaFinal].RespostaCerta)
            {
                Mensagem = "Erraste a pergunta final, recua 10 casas.";
                MovimentoAposMensagem = -10;
                jogador.Casa = jogador.Casa + MovimentoAposMensagem;
                jogador.JogaNovamente = false;
                jogador.PerdeuVez = false;
                PassarAoProximoJogador();
            }

        }
        private void SelecionarPergunta()
        {
            Random randomPergunta = new Random();
            PerguntaFinal = randomPergunta.Next(0, 9);
        }
        private void CriarEfeitosDeJogo()
        {
            CriarCasasEspeciais();

            if (ModoDeJogo == "Desafio")
            {
                CriarPerguntas();
            }
        }

        private Jogador JogadorAvancado()
        {
            Jogador jogador_mais_avancado;
            jogador_mais_avancado = Jogadores[0];
            foreach(Jogador jogador in Jogadores)
            {
               if(jogador.Casa > jogador_mais_avancado.Casa)
                {
                    jogador_mais_avancado = jogador;
                }
            }
            return jogador_mais_avancado;
        }

        private void AspiradorJato()
        {
            Jogador jogador_mais_avancado = JogadorAvancado();
            if (jogador_mais_avancado.Casa >= 7)
            {
                jogador_mais_avancado.Casa = jogador_mais_avancado.Casa - 6;
                foreach (CasaEspecial casa in CasasEspeciais)
                {
                    if (jogador_mais_avancado.Casa == casa.Casa)
                    {
                        Mensagem = "Casa Especial sem efeito";
                        MovimentoAposMensagem = 0;
                        jogador_mais_avancado.Casa = jogador_mais_avancado.Casa + MovimentoAposMensagem;
                        jogador_mais_avancado.JogaNovamente = false;
                        jogador_mais_avancado.PerdeuVez = false;
                    }

                }
                PassarAoProximoJogador();
            }
        }


            private void EscudoAtomico()
            {
                Jogador jogador = Jogadores[JogadorAtivo - 1];
                foreach (CasaEspecial casa in CasasEspeciais)
                {
                    if (jogador.Casa == casa.Casa)
                    {
                            Mensagem = "Estás Imune";
                            MovimentoAposMensagem = 0;
                            jogador.Casa = jogador.Casa + MovimentoAposMensagem;
                            jogador.JogaNovamente = true;
                            jogador.PerdeuVez = false;
                        
                    }

                }
            }

        private void ArmadilhaMagnetica()
        {
            Jogador jogador_mais_avancado = JogadorAvancado();
            
        }





        

    }
}


