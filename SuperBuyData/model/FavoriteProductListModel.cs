using System;
using System.Collections.Generic;
namespace SuperBuyData.model
{
    public class TbkItem
    {
        public string coupon_click_url { get; set; }
        public string coupon_info { get; set; }
        public string coupon_remain_count { get; set; }
        public string coupon_total_count { get; set; }
        public string event_end_time { get; set; }
        public string event_start_time { get; set; }
        public string item_url { get; set; }
        public string nick { get; set; }
        public string num_iid { get; set; }
        public string pict_url { get; set; }
        public  string provcity { get; set; }
        public string reserve_price { get; set; }
        public string seller_id { get; set; }
        public string shop_title { get; set; }
        public Dictionary<string, List<string>> small_images { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string tk_rate { get; set; }
        public string type { get; set; }
        public string user_type { get; set; }
        public string volume { get; set; }
        public string zk_final_price { get; set; }
        public string zk_final_price_wap { get; set; }

    }
    public class TbkItemResult{
        public List<TbkItem> uatm_tbk_item { get; set; }
    }

    public class FavoriteProductListModel
    {
        public TbkItemResult results { get; set; }
        public int total_results { get; set; }
        public string request_id { get; set; }
    }
    public class FavoriteProductListResponse
    {
        public FavoriteProductListModel tbk_uatm_favorites_item_get_response;
    }
}
