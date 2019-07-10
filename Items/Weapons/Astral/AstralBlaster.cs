using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace CalamityMod.Items.Weapons.Astral
{
	public class AstralBlaster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Blaster");
			Tooltip.SetDefault("Fires a homing astral round");
		}

	    public override void SetDefaults()
	    {
	        item.damage = 150;
	        item.crit += 25;
	        item.ranged = true;
	        item.width = 40;
	        item.height = 24;
	        item.useTime = 15;
	        item.useAnimation = 15;
	        item.useStyle = 5;
	        item.noMelee = true;
	        item.knockBack = 2.75f;
            item.value = Item.buyPrice(0, 95, 0, 0);
            item.rare = 9;
	        item.UseSound = SoundID.Item41;
	        item.autoReuse = true;
	        item.shootSpeed = 14f;
	        item.shoot = mod.ProjectileType("AstralRound");
	        item.useAmmo = 97;
	    }
	    
	    public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
	    	Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("AstralRound"), damage, knockBack, player.whoAmI, 0f, 0f);
	    	return false;
		}
	
	    public override void AddRecipes()
	    {
	        ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "AstralBar", 6);
	        recipe.AddTile(TileID.LunarCraftingStation);
	        recipe.SetResult(this);
	        recipe.AddRecipe();
	    }
	}
}
