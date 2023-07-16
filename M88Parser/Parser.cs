using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParsingWebTools;
using ParsingWebTools.RequestOptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M88Parser
{
    internal class Parser
    {
        private readonly WebScrapper _scrapper;
        private ILogger _log;

        public Parser(WebScrapper scrapper, ILogger<Parser> log)
        {
            _scrapper = scrapper;
            _log = log;
        }
        public async Task<ResponseObject?> ParseLiveSoccerMatches()
        {
            string json = await GetLiveSoccerJson();
            //string json = File.ReadAllText("testJson.txt");
            var apiObject = JsonConvert.DeserializeObject<ApiResponse>(json);
            var responseObject = new ResponseObject();

            foreach (var eventData in apiObject.data[0].data)
            {
                try
                {
                    EventObject eventObject = GetEventObject(responseObject, eventData);
                    SetOdds(eventData, eventObject);
                    OrderMainlineBets(responseObject, eventData, eventObject);
                }
                catch (Exception e)
                {
                    _log.LogError(" error:{message} |\n {trace: {trace}}", e.GetInnerMessages(), e.GetInnerStackTraces());
                }
            }
            return responseObject;
        }

        private static void OrderMainlineBets(ResponseObject responseObject, Datum eventData, EventObject eventObject)
        {
            eventObject.mainline_bets = eventObject.mainline_bets.OrderBy(b => b.type).ToList();
            if (!String.IsNullOrWhiteSpace(eventData.club_home))
            {
                responseObject.events.Add(eventObject);
            }
        }

        private void SetOdds(Datum eventData, EventObject eventObject)
        {
            // Full Time Handicap
            // FT_HDP
            Set_FT_HDP(eventData, eventObject);

            // First Half Handicap
            // FH_HDP
            Set_FH_HDP(eventData, eventObject);

            //Full Time Over Under
            //FT_OU
            Set_FT_OU(eventData, eventObject);

            //First Hal Over Under
            //FH_OU
            Set_FH_OU(eventData, eventObject);


            // Full Time 1 X 2 
            // FT_1X2
            Set_FT_1X2(eventData, eventObject);

            // First Half 1 X 2
            // FH_1X2
            Set_FH_1X2(eventData, eventObject);
        }

        private static EventObject GetEventObject(ResponseObject responseObject, Datum eventData)
        {
            EventObject eventObject = null;

            if (String.IsNullOrWhiteSpace(eventData.club_home))
            {
                eventObject = responseObject.events.Last();
            }
            else
            {
                eventObject = CreateEventObject(eventData);
            }

            Set_League(responseObject, eventObject);
            return eventObject;
        }

        private static void Set_League(ResponseObject responseObject, EventObject eventObject)
        {
            if (string.IsNullOrEmpty(eventObject.league))
            {
                eventObject.league = responseObject.events.Last().league;
            }
        }

        private void Set_FT_HDP(Datum eventData, EventObject eventObject)
        {
            object FT_HDP_a_opt = null;
            if (String.IsNullOrEmpty(eventData.hdc_display_a.Split("|").Skip(0).FirstOrDefault()))
            {
                FT_HDP_a_opt = ConvertHdpOpt(eventData.hdc_display_a.Split("|").Skip(1).FirstOrDefault());
                if (FT_HDP_a_opt != null)
                {
                    var opt = FT_HDP_a_opt as double?;
                    FT_HDP_a_opt = -opt;
                }
            }
            else
            {
                FT_HDP_a_opt = ConvertHdpOpt(eventData.hdc_display_a.Split("|").Skip(0).FirstOrDefault());
            }
            if (!string.IsNullOrEmpty(eventData.odds_1_a) && eventData.odds_1_a != "|")
            {
                eventObject.mainline_bets.Add(new MainlineBet { type = "FT_HDP_a", odd = Convert.ToDouble(eventData.odds_2_a.Split("|")[0], CultureInfo.InvariantCulture), opt = FT_HDP_a_opt ?? "-" });
            }
            if (!string.IsNullOrEmpty(eventData.odds_2_a) && eventData.odds_2_a != "|")
            {

                if (FT_HDP_a_opt != null)
                {
                    var opt = FT_HDP_a_opt as double?;
                    eventObject.mainline_bets.Add(new MainlineBet { type = "FT_HDP_h", odd = Convert.ToDouble(eventData.odds_1_a.Split("|")[0], CultureInfo.InvariantCulture), opt = -opt });
                }
                else
                {
                    eventObject.mainline_bets.Add(new MainlineBet { type = "FT_HDP_h", odd = Convert.ToDouble(eventData.odds_1_a.Split("|")[0], CultureInfo.InvariantCulture), opt = "-" });
                }
            }
        }

        private static EventObject CreateEventObject(Datum eventData)
        {
            EventObject eventObject = new EventObject()
            {
                sport = "Soccer",
                is_live = "1",
                home_team = eventData.club_home,
                away_team = eventData.club_away,

                score_a = int.Parse(eventData.away_score.Replace("_0", "")),
                score_h = int.Parse(eventData.home_score.Replace("0_", "")),
                red_card_a = int.Parse(eventData.away_pry.Replace("_0", "")),
                red_card_h = int.Parse(eventData.home_pry.Replace("_0", "")),
                league = eventData.event_name.Split("|").First(),
                mainline_bets = new List<MainlineBet>()
            };
            
            if (eventData.ev_round == "1")
            {
                var lifeTimerPlus1 = (int.Parse(eventData.live_timer.Replace("`", "").Trim()) + 1) + "'";
                eventObject.date = "1H " + lifeTimerPlus1;
            }
            if (eventData.ev_round == "2")
            {
                eventObject.date = "HT";
            }
            if (eventData.ev_round == "3")
            {
                var lifeTimerPlus1 = (int.Parse(eventData.live_timer.Replace("`", "").Trim()) + 1) + "'";
                eventObject.date = "2H " + lifeTimerPlus1;
            }

            return eventObject;
        }

        private static void Set_FH_1X2(Datum eventData, EventObject eventObject)
        {
            if (!string.IsNullOrEmpty(eventData.odds_1_g) && !String.IsNullOrWhiteSpace(eventData.club_home) && eventData.odds_2_g != "|")
            {
                eventObject.mainline_bets.Add(new MainlineBet { type = "FH_1X2_h", odd = Convert.ToDouble(eventData.odds_1_g.Split("|")[0], CultureInfo.InvariantCulture), opt = "-" });
            }
            if (!string.IsNullOrEmpty(eventData.odds_3_g) && !String.IsNullOrWhiteSpace(eventData.club_home) && eventData.odds_3_g != "|")
            {
                eventObject.mainline_bets.Add(new MainlineBet { type = "FH_1X2_a", odd = Convert.ToDouble(eventData.odds_3_g.Split("|")[0], CultureInfo.InvariantCulture), opt = "-" });
            }
            if (!string.IsNullOrEmpty(eventData.odds_2_g) && !String.IsNullOrWhiteSpace(eventData.club_home) && eventData.odds_2_g != "|")
            {
                eventObject.mainline_bets.Add(new MainlineBet { type = "FH_1X2_d", odd = Convert.ToDouble(eventData.odds_2_g.Split("|")[0], CultureInfo.InvariantCulture), opt = "-" });
            }
        }

        private static void Set_FT_1X2(Datum eventData, EventObject eventObject)
        {
            if (!string.IsNullOrEmpty(eventData.odds_1_d) && !String.IsNullOrWhiteSpace(eventData.club_home) && eventData.odds_1_d != "|")
            {
                eventObject.mainline_bets.Add(new MainlineBet { type = "FT_1X2_h", odd = Convert.ToDouble(eventData.odds_1_d.Split("|")[0], CultureInfo.InvariantCulture), opt = "-" });
            }
            if (!string.IsNullOrEmpty(eventData.odds_3_d) && !String.IsNullOrWhiteSpace(eventData.club_home) && eventData.odds_3_d != "|")
            {
                eventObject.mainline_bets.Add(new MainlineBet { type = "FT_1X2_a", odd = Convert.ToDouble(eventData.odds_3_d.Split("|")[0], CultureInfo.InvariantCulture), opt = "-" });
            }
            if (!string.IsNullOrEmpty(eventData.odds_2_d) && !String.IsNullOrWhiteSpace(eventData.club_home) && eventData.odds_2_d != "|")
            {
                eventObject.mainline_bets.Add(new MainlineBet { type = "FT_1X2_d", odd = Convert.ToDouble(eventData.odds_2_d.Split("|")[0], CultureInfo.InvariantCulture), opt = "-" });
            }
        }

        private void Set_FH_OU(Datum eventData, EventObject eventObject)
        {
            object FH_OU_a_opt = null;
            if (String.IsNullOrEmpty(eventData.hdc_display_f.Split("|").Skip(0).FirstOrDefault()))
            {
                FH_OU_a_opt = ConvertHdpOpt(eventData.hdc_display_f.Split("|").Skip(1).FirstOrDefault());
                if (FH_OU_a_opt != null)
                {
                    var opt = FH_OU_a_opt as double?;
                    FH_OU_a_opt = -opt;
                }
            }
            else
            {
                FH_OU_a_opt = ConvertHdpOpt(eventData.hdc_display_f.Split("|").Skip(0).FirstOrDefault());
            }

            if (!string.IsNullOrEmpty(eventData.odds_1_f) && eventData.odds_1_f != "|")
            {
                eventObject.mainline_bets.Add(new MainlineBet { type = "FH_OU_o", odd = Convert.ToDouble(eventData.odds_1_f.Split("|")[0], CultureInfo.InvariantCulture), opt = FH_OU_a_opt ?? "-" });
            }
            if (!string.IsNullOrEmpty(eventData.odds_2_f) && eventData.odds_2_f != "|")
            {
                if (FH_OU_a_opt != null)
                {
                    var opt = FH_OU_a_opt as double?;
                    eventObject.mainline_bets.Add(new MainlineBet { type = "FH_OU_u", odd = Convert.ToDouble(eventData.odds_2_f.Split("|")[0], CultureInfo.InvariantCulture), opt = opt });
                }
                else
                {
                    eventObject.mainline_bets.Add(new MainlineBet { type = "FH_OU_u", odd = Convert.ToDouble(eventData.odds_2_f.Split("|")[0], CultureInfo.InvariantCulture), opt = "-" });
                }
            }
        }

        private void Set_FT_OU(Datum eventData, EventObject eventObject)
        {
            object FT_OU_a_opt = null;
            if (String.IsNullOrEmpty(eventData.hdc_display_b.Split("|").Skip(0).FirstOrDefault()))
            {
                FT_OU_a_opt = ConvertHdpOpt(eventData.hdc_display_b.Split("|").Skip(1).FirstOrDefault());
                if (FT_OU_a_opt != null)
                {
                    var opt = FT_OU_a_opt as double?;
                    FT_OU_a_opt = -opt;
                }
            }
            else
            {
                FT_OU_a_opt = ConvertHdpOpt(eventData.hdc_display_b.Split("|").Skip(0).FirstOrDefault());
            }
            if (!string.IsNullOrEmpty(eventData.odds_1_b) && eventData.odds_1_b != "|")
            {
                eventObject.mainline_bets.Add(new MainlineBet { type = "FT_OU_o", odd = Convert.ToDouble(eventData.odds_1_b.Split("|")[0], CultureInfo.InvariantCulture), opt = FT_OU_a_opt ?? "-" });
            }
            if (!string.IsNullOrEmpty(eventData.odds_2_b) && eventData.odds_2_b != "|")
            {
                if (FT_OU_a_opt != null)
                {
                    var opt = FT_OU_a_opt as double?;
                    eventObject.mainline_bets.Add(new MainlineBet { type = "FT_OU_u", odd = Convert.ToDouble(eventData.odds_2_b.Split("|")[0], CultureInfo.InvariantCulture), opt = opt });
                }
                else
                {
                    eventObject.mainline_bets.Add(new MainlineBet { type = "FT_OU_u", odd = Convert.ToDouble(eventData.odds_2_b.Split("|")[0], CultureInfo.InvariantCulture), opt = "-" });
                }
            }
        }

        private void Set_FH_HDP(Datum eventData, EventObject eventObject)
        {
            object FH_HDP_a_opt = null;
            if (String.IsNullOrEmpty(eventData.hdc_display_e.Split("|").Skip(0).FirstOrDefault()))
            {
                FH_HDP_a_opt = ConvertHdpOpt(eventData.hdc_display_e.Split("|").Skip(1).FirstOrDefault());
                if (FH_HDP_a_opt != null)
                {
                    var opt = FH_HDP_a_opt as double?;
                    FH_HDP_a_opt = -opt;
                }
            }
            else
            {
                FH_HDP_a_opt = ConvertHdpOpt(eventData.hdc_display_e.Split("|").Skip(0).FirstOrDefault());
            }
            if (!string.IsNullOrEmpty(eventData.odds_1_e) && eventData.odds_1_e != "|")
            {
                eventObject.mainline_bets.Add(new MainlineBet { type = "FH_HDP_a", odd = Convert.ToDouble(eventData.odds_2_e.Split("|")[0], CultureInfo.InvariantCulture), opt = FH_HDP_a_opt ?? "-" });
            }
            if (!string.IsNullOrEmpty(eventData.odds_2_e) && eventData.odds_2_e != "|")
            {
                if (FH_HDP_a_opt != null)
                {
                    var opt = FH_HDP_a_opt as double?;
                    eventObject.mainline_bets.Add(new MainlineBet { type = "FH_HDP_h", odd = Convert.ToDouble(eventData.odds_1_e.Split("|")[0], CultureInfo.InvariantCulture), opt = -opt });
                }
                else
                {
                    eventObject.mainline_bets.Add(new MainlineBet { type = "FH_HDP_h", odd = Convert.ToDouble(eventData.odds_1_e.Split("|")[0], CultureInfo.InvariantCulture), opt = "-" });
                }
            }
        }

        private object ConvertHdpOpt(string? s)
        {
            try
            {
                if(String.IsNullOrEmpty(s))
                {
                    return null;
                }
                else
                {
                    var opt = s.Split("|").First();
                    if (!opt.Contains("-"))
                    {
                        return Convert.ToDouble(opt, CultureInfo.InvariantCulture);
                    }
                    var firstOpt = Convert.ToDouble(opt.Split("-").First(), CultureInfo.InvariantCulture);
                    firstOpt += 0.25;
                    return firstOpt;
                }
            }
            catch(Exception e)
            {
                _log.LogError(e.Message);
                return 0;
            }
        }
        private async Task<string> GetLiveSoccerJson()
        {
            var content = new StringContent("{\"sport_id\":\"10\",\"lang_id\":\"en\",\"display_type\":\"11\",\"view_type\":\"0\",\"leagues\":\"999\",\"odds_type\":\"3\",\"odds_group\":\"3\",\"view_date\":\"0\",\"time_filter\":\"0\",\"category\":\"0\",\"branch_code\":\"001\",\"currency\":\"\",\"is_full\":\"true\",\"prev_params\":\"\",\"version\":\"\",\"top_x\":0,\"is_stream\":\"false\",\"is_live_center\":\"false\"}");
            var headers = new Dictionary<string, string>()
            {
                {"authority", "msports.m88.com"},
                {"accept", "application/json, text/plain, */*"},
                {"authorization", "Bearer GUEST"},
                {"origin", "https://msports.m88.com"},
                {"referer", "https://msports.m88.com/app/v2/"},
                {"content-type","application/json"}
            };
            var postOptions = new PostOptions()
            {
                AdditionalHeaders = headers,
                Content = content
            };
            var responseText = await _scrapper.Post("https://msports.m88.com/api/v1/M88/data/main", postOptions);
            return responseText;
        }
    }
}
