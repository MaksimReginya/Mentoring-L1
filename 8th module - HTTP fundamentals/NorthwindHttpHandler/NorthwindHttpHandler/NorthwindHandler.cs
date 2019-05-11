using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using NorthwindHttpHandler.ReportGenerator;

namespace NorthwindHttpHandler
{
	public class NorthwindHandler : IHttpHandler
	{
		private readonly string[] _xmlAcceptTypes = { "text/xml", "application/xml" };

		public bool IsReusable => true;

		public void ProcessRequest(HttpContext context)
		{
			NameValueCollection queryString;
			if (context.Request.QueryString != null)
			{
				queryString = context.Request.QueryString;
			}
			else
			{
				queryString = this.ParseRequestBody(context.Request);
			}

			ReportFormat format = this.ParseReportFormat(context.Request);
			//StringBuilder s = new StringBuilder();
			using (DataModel model = new DataModel())
			{
				Generator generator = new Generator(model.Orders.AsQueryable(), queryString, format);
				/*foreach (var item in model.Orders)
				{
					s.Append($"{item.OrderDate}, {item.CustomerID}");
				}*/
			}

			
			//context.Response.Output.WriteLine(s.ToString());
		}

		private ReportFormat ParseReportFormat(HttpRequest request)
		{
			if (_xmlAcceptTypes.Intersect(request.AcceptTypes).Any())
			{
				return ReportFormat.Xml;
			}

			return ReportFormat.Xlsx;
		}

		private NameValueCollection ParseRequestBody(HttpRequest request)
		{
			string input;

			using (StreamReader reader = new StreamReader(request.InputStream))
			{
				input = reader.ReadToEnd();
			}

			return HttpUtility.ParseQueryString(input);
		}
	}
}