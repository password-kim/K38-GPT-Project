using Newtonsoft.Json;
using Slack.Webhook;
using System.Text;

private static async Task SendSlackMessage(string message)
{
    using (var client = new HttpClient())
    {
        var content = new StringContent(JsonConvert.SerializeObject(new
        {
            text = message
        }), Encoding.UTF8, "application/json");

        var result = await client.PostAsync("YOUR_SLACK_WEBHOOK_URL", content);
    }
}

private static void ScheduleTask(Action task)
{
    var nextRun = GetNextRun();
    var timer = new Timer(x =>
    {
        task.Invoke();
        ScheduleTask(task);
    }, null, nextRun - DateTime.Now, TimeSpan.FromMilliseconds(-1));
}

private static DateTime GetNextRun()
{
    var now = DateTime.Now;
    var nextRun = new DateTime(now.Year, now.Month, now.Day, 21, 30, 0, DateTimeKind.Local);
    if (nextRun < now)
    {
        nextRun = nextRun.AddDays(1);
    }
    while (nextRun.DayOfWeek == DayOfWeek.Saturday || nextRun.DayOfWeek == DayOfWeek.Sunday)
    {
        nextRun = nextRun.AddDays(1);
    }

    return nextRun;
}

