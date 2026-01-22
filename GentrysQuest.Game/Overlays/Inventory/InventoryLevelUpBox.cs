using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osuTK.Input;

namespace GentrysQuest.Game.Overlays.Inventory
{
    public partial class InventoryLevelUpBox : InventoryButton
    {
        private Bindable<int> amount = new();
        private bool multiplyTen = false;
        private bool multiplyHundred = false;

        public InventoryLevelUpBox()
            : base("$0")
        {
            amount.BindValueChanged(_ => { SetText($"${amount.ToString()}"); });
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            int actionAmount = 1;
            if (multiplyTen) actionAmount *= 10;
            if (multiplyHundred) actionAmount *= 100;

            switch (e.Button)
            {
                case MouseButton.Left:
                    break;

                case MouseButton.Right:
                    actionAmount = -actionAmount;
                    break;
            }

            if (amount.Value + actionAmount >= 0) amount.Value += actionAmount;
            else amount.Value = 0;
            return base.OnMouseDown(e);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.ControlLeft:
                    multiplyTen = true;
                    break;

                case Key.ShiftLeft:
                    multiplyHundred = true;
                    break;
            }

            return base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            switch (e.Key)
            {
                case Key.ControlLeft:
                    multiplyTen = false;
                    break;

                case Key.ShiftLeft:
                    multiplyHundred = false;
                    break;
            }

            base.OnKeyUp(e);
        }

        public int GetAmount() => amount.Value;
        public void SetAmount(int amount) => this.amount.Value = amount;
        public void IncreaseAmount() => amount.Value++;

        public void DecreaseAmount()
        {
            if (amount.Value > 0) amount.Value--;
        }
    }
}
