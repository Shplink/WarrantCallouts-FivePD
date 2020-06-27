using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using FivePD.API;

namespace HighRiskArrestWarrant
{
    [CalloutProperties("HighRiskArrestWarrant", "Shplink", "1.1")]
    public class HighRiskArrestWarrant : FivePD.API.Callout
    {
        Ped suspect;

        Random random = new Random();

        internal class WeaponChance
        {
            public int Entry;
            public WeaponHash Weapon;
        }
        WeaponChance[] Weapons = new WeaponChance[]
        {
            new WeaponChance{ Entry = 199,Weapon = WeaponHash.Pistol},
            new WeaponChance{ Entry = 9,Weapon = WeaponHash.CombatPistol},
            new WeaponChance{ Entry = 1,Weapon = WeaponHash.CarbineRifle},
            new WeaponChance{ Entry = 20,Weapon = WeaponHash.Bat},
            new WeaponChance{ Entry = 10,Weapon = WeaponHash.PumpShotgun},
            new WeaponChance{ Entry = 50,Weapon = WeaponHash.SMG}
        };

        public HighRiskArrestWarrant()
        {
            float offsetX = random.Next(100, 600);
            float offsetY = random.Next(100, 600);

            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetOffsetPosition(new Vector3(offsetX, offsetY, 0))));

            this.ShortName = "High Risk Arrest Warrant";
            this.CalloutDescription = "A High Risk Arrest Warrant has been issued. The suspect is to be considered Armed and Dangerous. Respond Code 3";
            this.ResponseCode = 3;
            this.StartDistance = 30f;
        }

        public async override Task OnAccept()
        {
            base.InitBlip();

            suspect = await SpawnPed(GetRandomPed(), Location);

            suspect.Weapons.Give(this.RollWeapon(), 250, true, true);

            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
        }

        public override void OnStart(Ped player)
        {
            base.OnStart(player);
            int x = random.Next(1, 100 + 1);
            if (x < 45)
            {
                suspect.Task.ShootAt(player);
            }
            else if (x > 45 && x <= 85)
            {
                suspect.Task.ReactAndFlee(player);
            }
            else if (x > 85)
            {
                suspect.Task.HandsUp(-1);
            }
        }

        public WeaponHash RollWeapon()
        {
            int overall = 0;
            foreach (var elem in this.Weapons)
                overall += elem.Entry;

            int x = random.Next(1, overall);
            overall = 0;
            for (int i = 0; i < this.Weapons.Length; i++)
            {
                overall += this.Weapons[i].Entry;
                if (overall > x)
                    return this.Weapons[i].Weapon;
            }
            return default;
        }
    }
}
