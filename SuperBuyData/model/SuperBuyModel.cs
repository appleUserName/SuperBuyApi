using System;
using System.Collections.Generic;
namespace SuperBuyData.model
{
    #region 商品分类列表
    public class Favorite{
        public string favorites_id { get; set; }
        public string favorites_title { get; set; }
        public string type { get; set; }
    }
    public class FavoriteList{
        public List<Favorite> tbk_favorites { get; set; }

    }
    public class FavoriteResult{
        public FavoriteList results { get; set; }
        public int total_results { get; set; }
        public string request_id { get; set; }
    }
    public class FavoriteResponse
    {
        public FavoriteResult tbk_uatm_favorites_get_response { get; set; }
    }
    #endregion
    public class SuperBuyModel
    {

    }
}
