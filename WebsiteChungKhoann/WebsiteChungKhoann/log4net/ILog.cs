using System;

namespace log4net
{
    internal interface ILog
    {
        void Error(string v, Exception ex);
        void InfoFormat(string v, long orderId, long vnpayTranId);
        void InfoFormat(string v, string rawUrl);
        void InfoFormat(string v, long orderId, long vnpayTranId, string vnp_ResponseCode);
    }
}