using GentrysQuest.Game.Users;
using osu.Framework.Bindables;

namespace GentrysQuest.Game.Database
{
    public class Money
    {
        public bool InfiniteMoney = false;
        private IUser user;

        public Bindable<int> Amount;

        public Money(IUser user)
        {
            this.user = user;
            Amount = new Bindable<int>(user.Money);
        }

        public bool CanAfford(int amount) => InfiniteMoney || Amount.Value >= amount;

        public void Spend(int amount)
        {
            if (!InfiniteMoney) Amount.Value -= amount;
        }

        public void Hand(int amount) => Amount.Value += amount;
    }
}
