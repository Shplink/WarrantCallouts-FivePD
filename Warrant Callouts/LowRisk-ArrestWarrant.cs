using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using FivePD.API;

namespace LowRiskArrestWarrant
{
    [CalloutProperties("LowRiskArrestWarrant", "Shplink", "1.1")]
    public class LRArrestWarrant : FivePD.API.Callout
    {
        Ped suspect;

        public LRArrestWarrant()
        {
            Random random = new Random();
            float offsetX = random.Next(100, 600);
            float offsetY = random.Next(100, 600);

            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetOffsetPosition(new Vector3(offsetX, offsetY, 0))));

            this.ShortName = "Low-Risk Arrest Warrant";
            this.CalloutDescription = "A Low-Risk Arrest Warrant has been issued by the State. Respond Code 2";
            this.ResponseCode = 2;
            this.StartDistance = 30f;
        }
        public async override Task OnAccept()
        {
            this.InitBlip();

            suspect = await SpawnPed(GetRandomPed(), Location);

            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
        }

        public override void OnStart(Ped player)
        {
            base.OnStart(player);

            Random random = new Random();
            int x = random.Next(1, 100 + 1);
            if (x < 30)
            {
                suspect.Task.FightAgainst(player);
            }
            else if (x > 30 && x <= 75)
            {
                suspect.Task.ReactAndFlee(player);
            }
            else if (x > 75)
            {
                suspect.Task.HandsUp(-1);
            }
        }
    }
}
