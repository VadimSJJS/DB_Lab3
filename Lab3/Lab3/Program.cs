using Lab3;
using Lab3.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var services = builder.Services;

        string connection = builder.Configuration.GetConnectionString("SqlServerConnection");
        services.AddDbContext<PharmacyContext>(options => options.UseSqlServer(connection));



        services.AddMemoryCache();

        services.AddDistributedMemoryCache();
        services.AddScoped<CashedPharmacyDb>();
        services.AddSession();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();

        var app = builder.Build();
        app.UseSession();


        app.Map("/info", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {

                string strResponse = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>����������:</H1>";
                strResponse += "<BR> ������: " + context.Request.Host;
                strResponse += "<BR> ����: " + context.Request.PathBase;
                strResponse += "<BR> ��������: " + context.Request.Protocol;
                strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";

                await context.Response.WriteAsync(strResponse);
            });
        });

        app.Map("/incoming", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CashedPharmacyDb cachedSubsCityDb = context.RequestServices.GetService<CashedPharmacyDb>();
                TableWriter tableWriter = new TableWriter();

                IEnumerable<Incoming> incomings = cachedSubsCityDb.GetSubscription(CacheKey.Incoming);
                string HtmlString = tableWriter.WriteTable(incomings);

                Console.WriteLine(HtmlString == null);

                await context.Response.WriteAsync(HtmlString);
            });
        });

        app.Map("/searchIncomingPrice", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CashedPharmacyDb cachedPharmacyDb = context.RequestServices.GetService<CashedPharmacyDb>();
                IEnumerable<Incoming> incomings = cachedPharmacyDb.GetSubscription(CacheKey.Incoming);

                TableWriter tableWriter = new TableWriter();

                string formHtml = "<form method='post' action='/searchIncomingPrice'>" +
                                  "<label>Price:</label>";



                if (context.Request.Cookies.TryGetValue("name", out var input_value))
                {
                    formHtml += $"<input type='number' name='price' value='{input_value}'><br><br>" +
                               "<input type='submit' value='�����'>" +
                               "</form>";
                }
                else
                {
                    formHtml += "<input type='number' name='price'><br><br>" +
                                "<input type='submit' value='�����'>" +
                                "</form>";
                }


                if (context.Request.Method == "POST")
                {
                    decimal price = Decimal.Parse(context.Request.Form["price"]);

                    context.Response.Cookies.Append("price", price.ToString());

                    IEnumerable<Incoming> subscriptionsByPublications = incomings.Where(s => s.Price == price);

                    string HtmlString = tableWriter.WriteTable(subscriptionsByPublications, formHtml);

                    await context.Response.WriteAsync(HtmlString);
                }
                else
                {
                    string HtmlString = tableWriter.WriteTable(incomings, formHtml);

                    await context.Response.WriteAsync(HtmlString);
                }
            });
        });



        
        app.Map("/searchIncomingCount", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CashedPharmacyDb cachedSubsCityDb = context.RequestServices.GetService<CashedPharmacyDb>();
                IEnumerable<Incoming> subscriptions = cachedSubsCityDb.GetSubscription(CacheKey.Incoming);

                TableWriter tableWriter = new TableWriter();

                string formHtml = "<form method='post' action='/searchIncomingCount'>" +
                                    "<label for='name'>����������� ����������:</label>";


                if (context.Session.Keys.Contains("duration"))
                {
                    int duration = Int32.Parse(context.Session.GetString("duration"));

                    formHtml += $"<input type='number' name='duration' value='{duration}'><br><br>" +
                                "<input type='submit' value='�����'>" +
                                 "</form>";
                }
                else
                {
                    formHtml += "<input type='number' name='duration'><br><br>" +
                                "<input type='submit' value='�����'>" +
                                 "</form>";
                }

                if (context.Request.Method == "POST")
                {
                    int duration = Int32.Parse(context.Request.Form["duration"]);

                    context.Session.SetString("duration", duration.ToString());

                    IEnumerable<Incoming> subscriptionsByPublications = subscriptions.Where(s => s.Count >= duration);

                    string HtmlString = tableWriter.WriteTable(subscriptionsByPublications, formHtml);


                    await context.Response.WriteAsync(HtmlString);
                }
                else
                {

                    string HtmlString = tableWriter.WriteTable(subscriptions, formHtml);


                    await context.Response.WriteAsync(HtmlString);
                }
            });
        });

        app.Run((context) =>
        {

            CashedPharmacyDb cachedPharmacyDb = context.RequestServices.GetService<CashedPharmacyDb>();

            cachedPharmacyDb.AddIncomingToCache(CacheKey.Incoming);

            string HtmlString = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
            "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<BODY><H1>�������</H1>";
            HtmlString += "<H2>������ �������� � ��� �������</H2>";
            HtmlString += "<BR><A href='/'>�������</A></BR>";
            HtmlString += "<BR><A href='/info'>����������</A></BR>";
            HtmlString += "<BR><A href='/incoming'>������</A></BR>";
            HtmlString += "<BR><A href='/searchIncomingPrice'>������(�����)</A></BR>";
            HtmlString += "<BR><A href='/searchIncomingCount'>������(����� 2.0)</A></BR>";

            return context.Response.WriteAsync(HtmlString);

        });

        app.Run();
    }
}


