namespace Lab3.Services
{
    public class TableWriter
    {
        public string WriteTable(IEnumerable<Incoming> subscriptions, params object[] addons)
        {
            string HtmlString = "<HTML><HEAD><TITLE>Производители</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
            foreach (string addon in addons)
            {
                HtmlString += addon;
            }
            HtmlString += "<BODY><H1>Таблица прихода</H1><TABLE BORDER=1>";
            HtmlString += "<TR>";
            HtmlString += "<TH>ID</TH>";
            HtmlString += "<TH>MedicineNameId</TH>";
            HtmlString += "<TH>ArrivalDate</TH>";
            HtmlString += "<TH>Count</TH>";
            HtmlString += "<TH>Provider</TH>";
            HtmlString += "<TH>Price</TH>";
            HtmlString += "</TR>";
            foreach (var subscription in subscriptions)
            {
                HtmlString += "<TR>";
                HtmlString += "<TD>" + subscription.Id + "</TD>";
                HtmlString += "<TD>" + subscription.MedicineNameId + "</TD>";
                HtmlString += "<TD>" + subscription.ArrivalDate + "</TD>";
                HtmlString += "<TD>" + subscription.Count + "</TD>";
                HtmlString += "<TD>" + subscription.Provider + "</TD>";
                HtmlString += "<TD>" + subscription.Price + "</TD>";
                HtmlString += "</TR>";
            }
            HtmlString += "</TABLE>";
            HtmlString += "<BR><A href='/'>Главная</A></BR>";
            HtmlString += "</BODY></HTML>";

            return HtmlString;
        }
    }
}
