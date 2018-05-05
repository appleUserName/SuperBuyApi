using System;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SuperBuyData.model;
namespace SuperBuyData
{
    public class ProductData
    {
        //https://temai.m.taobao.com/index.htm?pid=mm_131834139_44742922_497978280   微信
        //https://ai.taobao.com?pid=mm_131834139_44656655_507794294  网站
        public string url = "https://eco.taobao.com/router/rest?";
        //public string url = "http://gw.api.taobao.com/router/rest?";
        public string appkey = "24871297";
        public string secret = "a842a32ea65dfd3440186824b624c769";
        public string adzoneId = "507794294";
        public string sign_method = "md5";
        #region 获取时间戳
        public string GetNowTime()
        {
            string timer = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return timer;
        }
        #endregion

        #region 获取签名
        public static string SignTopRequest(IDictionary<string, string> parameters, string secret, string signMethod)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            if (Constants.SIGN_METHOD_MD5.Equals(signMethod))
            {
                query.Append(secret);
            }
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }

            // 第三步：使用MD5/HMAC加密
            byte[] bytes;
            if (Constants.SIGN_METHOD_HMAC.Equals(signMethod))
            {
                HMACMD5 hmac = new HMACMD5(Encoding.UTF8.GetBytes(secret));
                bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }
            else
            {
                query.Append(secret);
                MD5 md5 = MD5.Create();
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }
        #endregion

        #region 获取公共参数
        public Dictionary<string, string> GetPublicObjs(string method, out string urlStr)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string timer = GetNowTime();
            urlStr = url + "method=" + method + "&app_key=" + appkey + "&timestamp=" + timer + "&format=json&v=2.0&sign_method=md5";
            parameters.Add("method", method);
            parameters.Add("app_key", appkey);
            parameters.Add("sign_method", "md5");
            parameters.Add("timestamp", timer);
            parameters.Add("format", "json");
            parameters.Add("v", "2.0");
            return parameters;
        }
        #endregion

        #region 获取选品库列表
        public FavoriteResponse GetProductCategorysList(int pageIndex, int pageSize)
        {
            string urlStr;
            Dictionary<string, string> parameters = GetPublicObjs("taobao.tbk.uatm.favorites.get", out urlStr);
            parameters.Add("page_no", pageIndex.ToString());
            parameters.Add("page_size", pageSize.ToString());
            parameters.Add("fields", "favorites_title,favorites_id");
            string finalUrl = urlStr + "&page_no=" + pageIndex + "&page_size=" + pageSize + "&fields=favorites_title,favorites_id" + "&sign=" + SignTopRequest(parameters, secret, sign_method);
            var type = "utf-8";
            System.Net.WebRequest wReq = System.Net.WebRequest.Create(finalUrl);
            System.Net.WebResponse wResp = wReq.GetResponse();
            System.IO.Stream respStream = wResp.GetResponseStream();
            string data = "";
            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
            {
                data = reader.ReadToEnd();
            }

            data = Regex.Unescape(data);
            FavoriteResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FavoriteResponse>(data);
            return response;
        }
        #endregion

        #region 根据选品库ID获取商品列表
        public FavoriteProductListResponse GetProductListByFavoriteId(int favotiteId, int pageIndex, int pageSize)
        {
            string urlStr;
            Dictionary<string, string> parameters = GetPublicObjs("taobao.tbk.uatm.favorites.item.get", out urlStr);
            parameters.Add("page_no", pageIndex.ToString());
            parameters.Add("page_size", pageSize.ToString());
            parameters.Add("adzone_id", adzoneId);
            parameters.Add("favorites_id", favotiteId.ToString());
            parameters.Add("fields", "num_iid,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url,seller_id,volume,nick,shop_title,zk_final_price_wap,event_start_time,event_end_time,tk_rate,status,type,coupon_click_url,coupon_total_count,coupon_remain_count,coupon_info");
            string finalUrl = urlStr + "&page_no=" + pageIndex + "&page_size=" + pageSize + "&adzone_id=" + adzoneId + "&favorites_id=" + favotiteId + "&fields=num_iid,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url,seller_id,volume,nick,shop_title,zk_final_price_wap,event_start_time,event_end_time,tk_rate,status,type,coupon_click_url,coupon_total_count,coupon_remain_count,coupon_info" + "&sign=" + SignTopRequest(parameters, secret, sign_method);
            var type = "utf-8";
            System.Net.WebRequest wReq = System.Net.WebRequest.Create(finalUrl);
            System.Net.WebResponse wResp = wReq.GetResponse();
            System.IO.Stream respStream = wResp.GetResponseStream();
            string data = "";
            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
            {
                data = reader.ReadToEnd();
            }
            data = Regex.Unescape(data);
            FavoriteProductListResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FavoriteProductListResponse>(data);

            return response;
        }
        #endregion

        #region 获取商品淘口令
        public string GetFavoriteProductCode(string shortTitle,string url){
            string urlStr;
            Dictionary<string, string> parameters = GetPublicObjs("taobao.tbk.tpwd.create", out urlStr);
            parameters.Add("text", shortTitle);
            byte[] buffer = Encoding.UTF8.GetBytes(url);
            parameters.Add("url", url);
            string finalUrl = urlStr + "&text=" + shortTitle + "&url=" + Encoding.UTF8.GetString(buffer, 0, buffer.Length) + "&sign=" + SignTopRequest(parameters, secret, sign_method);
            var type = "utf-8";
            System.Net.WebRequest wReq = System.Net.WebRequest.Create(finalUrl);
            System.Net.WebResponse wResp = wReq.GetResponse();
            System.IO.Stream respStream = wResp.GetResponseStream();
            string data = "";
            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
            {
                data = reader.ReadToEnd();
            }
            data = Regex.Unescape(data);
            return data;
        }
        #endregion

    }
}
