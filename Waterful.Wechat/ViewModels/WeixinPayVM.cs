namespace Waterful.Wechat.ViewModels
{
    public class WeixinPayPayVM
    {
        public string AppId { get; set; }
        public string TimeStamp { get; set; }
        public string NonceStr { get; set; }
        public string PaySign { get; set; }
        public string Package { get; set; }

        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public decimal Amount { get; set; }
    }
}
