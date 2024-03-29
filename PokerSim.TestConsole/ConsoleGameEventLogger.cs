﻿using PokerSim.Engine.Decks;
using PokerSim.Engine.Game;
using PokerSim.Engine.Players;
using System;
using System.Collections.Generic;

namespace PokerSim.TestConsole
{
    public class ConsoleGameEventLogger : IGameEventLogger
    {
        private bool _logEachTurn;
        public ConsoleGameEventLogger(bool logEachTurn = false)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            _logEachTurn = logEachTurn;
        }
        public void Log(HandResult handResult)
        {
            string winners = "Winners: ";
            foreach(var winner in handResult.Winners)
            {
                winners += $"{winner.Player.Name} {winner.Hand.ToString()} ${winner.Winnings}, ";
            }
            Console.WriteLine(winners);

            string losers = "Losers: ";
            foreach (var loser in handResult.Losers)
            {
                losers += $"{loser.Player.Name} {loser.Hand.ToString()}, ";
            }

            Console.WriteLine(losers);
        }

        public void Log(TurnResult turnResult)
        {
            if(_logEachTurn)
            {
                Console.WriteLine(turnResult.ToString());
            }
        }

        public void Log(TexasHoldemStages stage, IEnumerable<Card> communityCards)
        {
            
            Console.WriteLine($"-========= {stage.ToString().ToUpper()} ===========-");
            if(stage != TexasHoldemStages.PreFlop)
            {
                string str = "";
                foreach (var card in communityCards)
                {
                    str += card.ShortName + " ";
                }

                Console.WriteLine(str);
                Console.WriteLine($"-========= {stage.ToString().ToUpper()} ===========-");
            }
           
        }
    }
}
