﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M88Parser
{
    public class Datum
    {
        public string spid { get; set; } = default!;
        public List<Datum> data { get; set; } = default!;
        public string no_event { get; set; } = default!;
        public string no_event_sort { get; set; } = default!;
        public string event_name { get; set; } = default!;
        public string no_partai { get; set; } = default!;
        public string no_team_sort { get; set; } = default!;
        public string match_date { get; set; } = default!;
        public string club_home_id { get; set; } = default!;
        public string club_home { get; set; } = default!;
        public string club_away_id { get; set; } = default!;
        public string club_away { get; set; } = default!;
        public string home_score { get; set; } = default!;
        public string away_score { get; set; } = default!;
        public string ev_round { get; set; } = default!;
        public string live_timer { get; set; } = default!;
        public string home_pry { get; set; } = default!;
        public string away_pry { get; set; } = default!;
        public string multi_flags_1 { get; set; } = default!;
        public string multi_flags_2 { get; set; } = default!;
        public string is_neutral { get; set; } = default!;
        public string is_live { get; set; } = default!;
        public string game_type_a { get; set; } = default!;
        public string sub_partai_a { get; set; } = default!;
        public string hdc_ori_a { get; set; } = default!;
        public string hdc_display_a { get; set; } = default!;
        public string price_alert_a { get; set; } = default!;
        public string odds_1_a { get; set; } = default!;
        public string odds_2_a { get; set; } = default!;
        public string price_alert_flag_1_a { get; set; } = default!;
        public string price_alert_flag_2_a { get; set; } = default!;
        public string game_type_b { get; set; } = default!;
        public string sub_partai_b { get; set; } = default!;
        public string hdc_ori_b { get; set; } = default!;
        public string hdc_display_b { get; set; } = default!;
        public string price_alert_b { get; set; } = default!;
        public string odds_1_b { get; set; } = default!;
        public string odds_2_b { get; set; } = default!;
        public string price_alert_flag_1_b { get; set; } = default!;
        public string price_alert_flag_2_b { get; set; } = default!;
        public string game_type_c { get; set; } = default!;
        public string sub_partai_c { get; set; } = default!;
        public string price_alert_c { get; set; } = default!;
        public string odds_1_c { get; set; } = default!;
        public string odds_2_c { get; set; } = default!;
        public string price_alert_flag_1_c { get; set; } = default!;
        public string price_alert_flag_2_c { get; set; } = default!;
        public string game_type_d { get; set; } = default!;
        public string sub_partai_d { get; set; } = default!;
        public string price_alert_d { get; set; } = default!;
        public string odds_1_d { get; set; } = default!;
        public string odds_2_d { get; set; } = default!;
        public string odds_3_d { get; set; } = default!;
        public string price_alert_flag_1_d { get; set; } = default!;
        public string price_alert_flag_2_d { get; set; } = default!;
        public string game_type_e { get; set; } = default!;
        public string sub_partai_e { get; set; } = default!;
        public string hdc_ori_e { get; set; } = default!;
        public string hdc_display_e { get; set; } = default!;
        public string price_alert_e { get; set; } = default!;
        public string odds_1_e { get; set; } = default!;
        public string odds_2_e { get; set; } = default!;
        public string price_alert_flag_1_e { get; set; } = default!;
        public string price_alert_flag_2_e { get; set; } = default!;
        public string game_type_f { get; set; } = default!;
        public string sub_partai_f { get; set; } = default!;
        public string hdc_ori_f { get; set; } = default!;
        public string hdc_display_f { get; set; } = default!;
        public string price_alert_f { get; set; } = default!;
        public string odds_1_f { get; set; } = default!;
        public string odds_2_f { get; set; } = default!;
        public string price_alert_flag_1_f { get; set; } = default!;
        public string price_alert_flag_2_f { get; set; } = default!;
        public string game_type_g { get; set; } = default!;
        public string sub_partai_g { get; set; } = default!;
        public string price_alert_g { get; set; } = default!;
        public string odds_1_g { get; set; } = default!;
        public string odds_2_g { get; set; } = default!;
        public string odds_3_g { get; set; } = default!;
        public string price_alert_flag_1_g { get; set; } = default!;
        public string price_alert_flag_2_g { get; set; } = default!;
        public string game_type_h { get; set; } = default!;
        public string sub_partai_h { get; set; } = default!;
        public string price_alert_h { get; set; } = default!;
        public string odds_1_h { get; set; } = default!;
        public string odds_2_h { get; set; } = default!;
        public string price_alert_flag_1_h { get; set; } = default!;
        public string price_alert_flag_2_h { get; set; } = default!;
        public string game_type_i { get; set; } = default!;
        public string sub_partai_i { get; set; } = default!;
        public string price_alert_i { get; set; } = default!;
        public string odds_1_i { get; set; } = default!;
        public string odds_2_i { get; set; } = default!;
        public string odds_3_i { get; set; } = default!;
        public string price_alert_flag_1_i { get; set; } = default!;
        public string price_alert_flag_2_i { get; set; } = default!;
        public string game_type_j { get; set; } = default!;
        public string sub_partai_j { get; set; } = default!;
        public string price_alert_j { get; set; } = default!;
        public string odds_1_j { get; set; } = default!;
        public string odds_2_j { get; set; } = default!;
        public string odds_3_j { get; set; } = default!;
        public string odds_4_j { get; set; } = default!;
        public string odds_5_j { get; set; } = default!;
        public string price_alert_flag_1_j { get; set; } = default!;
        public string price_alert_flag_2_j { get; set; } = default!;
        public string price_alert_flag_3_j { get; set; } = default!;
        public string price_alert_flag_4_j { get; set; } = default!;
        public string price_alert_flag_5_j { get; set; } = default!;
        public string game_type_k { get; set; } = default!;
        public string sub_partai_k { get; set; } = default!;
        public string price_alert_k { get; set; } = default!;
        public string odds_1_K { get; set; } = default!;
        public string odds_2_K { get; set; } = default!;
        public string odds_3_K { get; set; } = default!;
        public string odds_4_K { get; set; } = default!;
        public string odds_5_K { get; set; } = default!;
        public string odds_6_K { get; set; } = default!;
        public string odds_7_K { get; set; } = default!;
        public string odds_8_K { get; set; } = default!;
        public string odds_9_K { get; set; } = default!;
        public string price_alert_flag_1_k { get; set; } = default!;
        public string price_alert_flag_2_k { get; set; } = default!;
        public string price_alert_flag_3_k { get; set; } = default!;
        public string price_alert_flag_4_k { get; set; } = default!;
        public string price_alert_flag_5_k { get; set; } = default!;
        public string price_alert_flag_6_k { get; set; } = default!;
        public string price_alert_flag_7_k { get; set; } = default!;
        public string price_alert_flag_8_k { get; set; } = default!;
        public string price_alert_flag_9_k { get; set; } = default!;
        public string game_type_l { get; set; } = default!;
        public string sub_partai_l { get; set; } = default!;
        public string odds_1_l { get; set; } = default!;
        public string odds_2_l { get; set; } = default!;
        public string odds_3_l { get; set; } = default!;
        public string odds_4_l { get; set; } = default!;
        public string odds_5_l { get; set; } = default!;
        public string odds_6_l { get; set; } = default!;
        public string odds_7_l { get; set; } = default!;
        public string odds_8_l { get; set; } = default!;
        public string odds_9_l { get; set; } = default!;
        public string odds_10_l { get; set; } = default!;
        public string odds_11_l { get; set; } = default!;
        public string odds_12_l { get; set; } = default!;
        public string odds_13_l { get; set; } = default!;
        public string odds_14_l { get; set; } = default!;
        public string odds_15_l { get; set; } = default!;
        public string odds_16_l { get; set; } = default!;
        public string odds_17_l { get; set; } = default!;
        public string odds_18_l { get; set; } = default!;
        public string odds_19_l { get; set; } = default!;
        public string odds_20_l { get; set; } = default!;
        public string odds_21_l { get; set; } = default!;
        public string odds_22_l { get; set; } = default!;
        public string odds_23_l { get; set; } = default!;
        public string odds_24_l { get; set; } = default!;
        public string odds_25_l { get; set; } = default!;
        public string odds_26_l { get; set; } = default!;
        public string price_alert_flag_1_l { get; set; } = default!;
        public string game_type_m { get; set; } = default!;
        public string sub_partai_m { get; set; } = default!;
        public string odds_1_m { get; set; } = default!;
        public string price_alert_flag_1_m { get; set; } = default!;
        public string game_type_n { get; set; } = default!;
        public string sub_partai_n { get; set; } = default!;
        public string price_alert_n { get; set; } = default!;
        public string odds_1_n { get; set; } = default!;
        public string odds_2_n { get; set; } = default!;
        public string odds_3_n { get; set; } = default!;
        public string odds_4_n { get; set; } = default!;
        public string price_alert_flag_1_n { get; set; } = default!;
        public string price_alert_flag_2_n { get; set; } = default!;
        public string price_alert_flag_3_n { get; set; } = default!;
        public string price_alert_flag_4_n { get; set; } = default!;
        public string game_type_o { get; set; } = default!;
        public string sub_partai_o { get; set; } = default!;
        public string price_alert_o { get; set; } = default!;
        public string odds_1_o { get; set; } = default!;
        public string odds_2_o { get; set; } = default!;
        public string odds_3_o { get; set; } = default!;
        public string odds_4_o { get; set; } = default!;
        public string price_alert_flag_1_o { get; set; } = default!;
        public string price_alert_flag_2_o { get; set; } = default!;
        public string price_alert_flag_3_o { get; set; } = default!;
        public string price_alert_flag_4_o { get; set; } = default!;
        public string more_gt_num { get; set; } = default!;
        public string multi_flags_3 { get; set; } = default!;
        public string row_num { get; set; } = default!;
        public string display_type { get; set; } = default!;
    }

    public class ApiResponse
    {
        public int status { get; set; }
        public List<Datum> data { get; set; } = new List<Datum> { };
    }


}
