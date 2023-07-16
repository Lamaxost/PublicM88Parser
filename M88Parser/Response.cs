using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M88Parser
{
    public class MainlineBet
    {
        public string type { get; set; } = default!;
        public double odd { get; set; } = default!;
        public object opt { get; set; } = default!;
    }

    public class EventObject
    {
        public string sport { get; set; } = default!;
        public string is_live { get; set; } = default!;
        public string league { get; set; } = default!;
        public string home_team { get; set; } = default!;
        public string away_team { get; set; } = default!;
        public int red_card_h { get; set; } = default!;
        public int red_card_a { get; set; } = default!;
        public int score_h { get; set; } = default!;
        public int score_a { get; set; } = default!;
        public string date { get; set; } = default!;
        public List<MainlineBet> mainline_bets { get; set; } = new List<MainlineBet> { };
    }
    public class ResponseObject
    {
        public List<EventObject> events { get; set; } = new();
    }

}
