using PokerSim.Engine.Game;
using PokerSim.Engine.Players;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace PokerSim.SamplePlayerPlugin
{
    public class SamplePlayerPluginConfiguration
    {
        public double CheckCallProb { get; set; }
        public double RaiseProb { get; set; }
    }
        
    public class SamplePlayerPlugin : PlayerPlugin
    {
        private SamplePlayerPluginConfiguration _configuration;
        private Random _random = new Random();

        public SamplePlayerPlugin() :
            base("SamplePlayerPlugin")
        {
            _configuration = LoadConfiguration();
        }

        private SamplePlayerPluginConfiguration LoadConfiguration()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("player-configuration.json"));
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    return JsonSerializer.Deserialize<SamplePlayerPluginConfiguration>(result);
                }

            }
            catch(Exception ex)
            {
                return new SamplePlayerPluginConfiguration();
            }
        }

        public override TurnResult TakeTurn(IPlayerTurnState state)
        {
            double p = _random.NextDouble();
            if (p < _configuration.RaiseProb)
            {
                return TurnResult.Raise(this, state.Blinds * 3);
            }
            if (p < _configuration.RaiseProb + _configuration.CheckCallProb)
            {
                return TurnResult.CheckOrCallAny(this);
            }

            if (state.CurrentBet == 0)
            {
                return TurnResult.CheckOrCallAny(this);
            }
            return TurnResult.Fold(this);

        }
    }
}
