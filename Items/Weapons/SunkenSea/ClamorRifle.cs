using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace CalamityMod.Items.Weapons.SunkenSea
{
	public class ClamorRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clamor Rifle");
			Tooltip.SetDefault("Shoots homing energy bolts");
		}

	    public override void SetDefaults()
	    {
			item.damage = 32;
			item.ranged = true;
			item.width = 64;
			item.height = 30;
			item.useTime = 13;
			item.useAnimation = 13;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 2.5f;
			item.value = Item.buyPrice(0, 36, 0, 0);
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PlasmaBolt");
			item.autoReuse = true;
			item.rare = 5;
			item.shoot = mod.ProjectileType("ClamorRifleProj");
			item.shootSpeed = 15f;
			item.useAmmo = 97;
		}
	    
	    public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 0);
		}
		
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("ClamorRifleProj"), damage, knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
		}
	}
}
