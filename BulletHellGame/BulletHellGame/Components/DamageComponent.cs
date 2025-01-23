using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellGame.Components
{
    public class DamageComponent : IComponent
    {
        public int BaseDamage { get; private set; }
        public float Multiplier { get; private set; } = 1.0f; // For scaling damage
        public Func<int, int>? DamageModifier { get; set; } // Optional: Dynamic damage logic

        public DamageComponent(int baseDamage)
        {
            BaseDamage = baseDamage;
        }

        public int CalculateDamage()
        {
            int damage = (int)(BaseDamage * Multiplier);
            return DamageModifier != null ? DamageModifier(damage) : damage;
        }

        public void Update(GameTime gameTime)
        {
            // Update logic (if necessary)
        }
    }

}
