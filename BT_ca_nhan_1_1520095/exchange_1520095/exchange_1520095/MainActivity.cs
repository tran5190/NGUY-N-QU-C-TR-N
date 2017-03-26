using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;

namespace exchange_1520095
{
	[Activity(Label = "UDDD_exchangeRate_1520095", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		string url; //= "http://www.apilayer.net/api/live?access_key=790a8431355181941a110a9152e5515f&format=1";
		JsonValue json;
		EditText VNDmoney;
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);
			VNDmoney = FindViewById<EditText>(Resource.Id.VNDnumber);
			Button button = FindViewById<Button>(Resource.Id.getRateButton);

			button.Click += async (sender, e) =>
			{
				url = "http://www.apilayer.net/api/live?access_key=790a8431355181941a110a9152e5515f&format=1";

				json = await FetchWeatherAsync(url);
				Console.Out.WriteLine("out...");
				ParseAndDisplay(json);
			};
		}

		private async Task<JsonValue> FetchWeatherAsync(string url)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
			Console.Out.WriteLine("haha...1");
			request.ContentType = "application/json";
			Console.Out.WriteLine("haha...2");
			request.Method = "GET";
			using (WebResponse response = await request.GetResponseAsync())
			{
				using (Stream stream = response.GetResponseStream())
				{
					JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
					Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());
					return jsonDoc;
				}
			}
		}
		private float exchangeVND(float InputVND, string MoneyName)
		{
			JsonValue usd_rate = json["quotes"];
			//Console.Out.WriteLine("haha...1");
			float usdOUT = usd_rate["USD" + MoneyName];
			float USD_VND = usd_rate["USDVND"];
			//Console.Out.WriteLine("haha...2");
			float result = usdOUT * InputVND / USD_VND;
			Console.Out.WriteLine("result: " + result);
			return result;
		}
		private void ParseAndDisplay(JsonValue json)
		{
			TextView usd = FindViewById<TextView>(Resource.Id.USDnumber);
			usd.Text = exchangeVND(float.Parse(VNDmoney.Text.ToString()), "USD").ToString();

			TextView eur = FindViewById<TextView>(Resource.Id.EURnumber);
			eur.Text = exchangeVND(float.Parse(VNDmoney.Text.ToString()), "EUR").ToString();

			TextView yen = FindViewById<TextView>(Resource.Id.YENnumber);
			yen.Text = exchangeVND(float.Parse(VNDmoney.Text.ToString()), "JPY").ToString();

			TextView cny = FindViewById<TextView>(Resource.Id.CNYnumber);
			cny.Text = exchangeVND(float.Parse(VNDmoney.Text.ToString()), "CNY").ToString();

		}
	}
}
