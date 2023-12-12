namespace PeopleCalcBot
{
    public class Program
    {
        static async Task Main()
        {
            var controller = new Controller();
            await controller.ControllerAsync();
        }
    }
}