using Newtonsoft.Json;
using System.Text;
using System.Web;
using WeatherConsoleFD;

Console.OutputEncoding = Encoding.UTF8;
var apiKey = "de652bea5fc39e262e384811c44b47a7";
var client = new HttpClient();

while (true)
{
    Console.Write(" Введите название города, где хотите узнать погоду: ");
    var city = Console.ReadLine();
    var response = await client.GetAsync(@$"https://api.openweathermap.org/data/2.5/forecast?q={HttpUtility.UrlEncode(city)}&appid={apiKey}&units=metric&lang=ru");

    if (response.IsSuccessStatusCode)
    {
        var result = await response.Content.ReadAsStringAsync();
        var model = JsonConvert.DeserializeObject<WeatherInf>(result);
        Console.Clear();
        Console.WriteLine(
            $" Погода в городе: {model.city.name}, {model.city.country} на {DateTime.Now} - {model.list[0].weather[0].description}\n\n" +
            $" Температура: {Math.Round(model.list[0].main.temp, 1)}°С\n" +
            $" Ощущается как: {Math.Round(model.list[0].main.feels_like, 1)}°С\n" +
            $" Скорость ветра: {model.list[0].wind.speed}м/с\n" +
            $" Влажность: {model.list[0].main.humidity}%\n" +
            $" Давление: {Math.Round(model.list[0].main.grnd_level / 1.33322, 2)} мм ртутного столба" +
            $"\n\n\n Прогноз погоды на 4 дня:\n");

        DateTime somedays;
        int wd = 0;
        List sumdays;
        for (int i = 0; i < 4; i++)
        {
            sumdays = model.list[wd];
            somedays = DateTime.Parse(sumdays.dt_txt);
            Console.ForegroundColor = ConsoleColor.Red;   //сильнейшее желание выделить цветом
            Console.WriteLine($" {somedays.ToShortDateString()} | {somedays.ToString("dddd")[0].ToString() + somedays.ToString("dddd").Substring(1)}");
            Console.ResetColor();   //отмена цвета (очевидно)
            Console.WriteLine($" ↑ {Math.Round(sumdays.main.temp_max, 1)} | ↓ {Math.Round(sumdays.main.temp_min, 1)}");
            Console.WriteLine($" {(sumdays.weather[0].description)[0].ToString() + (sumdays.weather[0].description).Substring(1)}");
            Console.WriteLine(" ");
            wd += 8;
        }
    }

    else
    {
        Console.WriteLine("Или город введен неверно, или такого нет...");
    }
    Console.ReadLine();
    Console.Clear();
}
