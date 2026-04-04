using GentrysQuest.Game.Online.API.Requests.Gameplay;
using System.Threading.Tasks;

namespace GentrysQuest.Game.Location;

public class Visitation
{
    public string ID { get; set; }
    public int UserID { get; set; }
    public int LocationID { get; set; }

    public Task DepartAsync()
    {
        if (string.IsNullOrWhiteSpace(ID))
            return Task.CompletedTask;

        return new DepartRequest(ID).PerformAsync();
    }

    public void Depart()
    {
        _ = DepartAsync();
    }
}
