using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace CalamityMod.Items.Weapons.SlimeGod
{
	public class GunkShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gunk Shot");
			Tooltip.SetDefault("Shoots a spread of bullets");
		}

	    public override void SetDefaults()
	    {
			item.damage = 30;
			item.ranged = true;
			item.width = 42;
			item.height = 18;
			item.useTime = 32;
			item.useAnimation = 32;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 3.5f;
            item.value = Item.buyPrice(0, 12, 0, 0);
            item.rare = 4;
			item.UseSound = SoundID.Item36;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 3.5f;
			item.useAmmo = 97;
		}
		
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{    
		    int num6 = Main.rand.Next(3, 5);
		    for (int index = 0; index < num6; ++index)
		    {
		        float SpeedX = speedX + (float) Main.rand.Next(-25, 26) * 0.05f;
		        float SpeedY = speedY + (float) Main.rand.Next(-25, 26) * 0.05f;
		        Projectile.NewProjectile(position.X, position.Y, SpeedX, SpeedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
		    }
		    return false;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PurifiedGel", 18);
            recipe.AddIngredient(ItemID.Gel, 15);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
