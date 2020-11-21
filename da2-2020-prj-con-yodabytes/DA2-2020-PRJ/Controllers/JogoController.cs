using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DA2_2020_PRJ.Models;
using Microsoft.AspNetCore.Mvc;


namespace DA2_2020_PRJ.Controllers
{
    public class JogoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(DadosNovoJogo dados)
        {
            if (ModelState.IsValid)
            {
                return View("PreencherNomes", dados);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult PreencherNomes(DadosNovoJogo dadosCompletos)
        {
            if (ModelState.IsValid)
            {
                //criar um novo jogo e enviar como paramêtros o modo e o número de jogadores 
                Jogo novoJogo = new Jogo(dadosCompletos.NumeroJogadores, dadosCompletos.ModoDeJogo, dadosCompletos.NomeJogador1, dadosCompletos.NomeJogador2, dadosCompletos.NomeJogador3, dadosCompletos.NomeJogador4);
                Repository.AdicionarJogo(novoJogo);
                return View("Prototipo", novoJogo);
            }
            else
            {
                return View(dadosCompletos);
            }
        }

        [HttpPost]
        public IActionResult ExecutarAcao(int idJogo, string playerAction, int player1x, int player1y, int player2x, int player2y, int player3x, int player3y, int player4x, int player4y, int resposta)
        {
            Jogo jogoAtual = Repository.CarregarJogo(idJogo);

            jogoAtual.ExecutarAcao(playerAction, player1x, player1y, player2x, player2y, player3x, player3y, player4x, player4y, resposta);

            return View("Prototipo", jogoAtual);

        }
    }
}