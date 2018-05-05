using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperBuyData;
using SuperBuyData.model;

namespace SuperBuyApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        //获取商品分类列表
        public FavoriteResponse GetProductCategoryList()
        {
            FavoriteResponse response = new SuperBuyData.ProductData().GetProductCategorysList(1,1);
            return response;
        }
        //根据商品列表ID获取商品信息列表
        public FavoriteProductListResponse GetProductsListByFavoriteId(int favoriteId,int pageIndex, int pageSize){
            FavoriteProductListResponse response = new SuperBuyData.ProductData().GetProductListByFavoriteId(favoriteId, pageIndex, pageSize);
            return response;
        }
        //根据商品优惠链接获取淘口令
        public string GetFavoriteCodeByTitleAndUrl(string title,string coupon_click_url){
            string code = new SuperBuyData.ProductData().GetFavoriteProductCode(title, coupon_click_url);
            return code;
        }
    }
}
