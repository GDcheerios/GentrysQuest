namespace GentrysQuest.Game.Entity.Weapon
{
    public class ChargeAttack
    {
        /// <summary>
        /// How the weapon will function when charging
        /// </summary>
        public AttackPattern ChargePattern = new();

        /// <summary>
        /// How long it will take to charge
        /// </summary>
        public int ChargeTime;

        /// <summary>
        /// If chargeTime should change with attackSpeed.
        /// </summary>
        public bool ChargeBasedOnAttackSpeed;

        /// <summary>
        /// Adjusts how much the attack speed will affect the charge rate.
        /// </summary>
        public float ChargeRateModifier;

        /// <summary>
        /// If the attack will release once fully charged
        /// </summary>
        public bool ReleaseOnCharge;

        /// <summary>
        /// If you must finish the charge before it does anything
        /// </summary>
        public bool MustFinish;

        /// <summary>
        /// Receive thew current charge amount
        /// </summary>
        /// <param name="attackSpeed">The attack speed being put in</param>
        /// <returns></returns>
        private float currentCharge(double attackSpeed) => (float)(CurrentHoldTime / (ChargeTime / (attackSpeed * ChargeRateModifier)));

        private Weapon parent;

        public ChargeAttack(Weapon parent)
        {
            this.parent = parent;
        }

        public float CurrentHoldTime { get; set; }
    }
}
