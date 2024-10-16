using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Models;
using RecipeAPI.Repositories;
using RecipeAPI.Services;

namespace RecipeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : Controller
    {
        private readonly IRecipeService recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            var recipes = await recipeService.GetRecipesAsync();
            return Ok(recipes);
        }

        [HttpPost]
        public async Task<ActionResult<Recipe>> AddRecipe([FromBody] Recipe recipe)
        {
            try
            {
                recipe.Id = Guid.NewGuid();
                await recipeService.AddRecipeAsync(recipe);
                return CreatedAtAction(nameof(GetRecipeById), new { id = recipe.Id }, recipe);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipeById(Guid id)
        {
            try
            {
                var recipe = await recipeService.GetRecipeByIdAsync(id);
                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("favorite/{id}")]
        public async Task<ActionResult<Recipe>> MakeRecipeFavoriteById(Guid id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("Id was null");
                }

                await recipeService.FavoriteRecipeByIdAsync(id);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("unfavorite/{id}")]
        public async Task<ActionResult<Recipe>> RemoveRecipeFavoriteById(Guid id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("Id was null");
                }

                await recipeService.RemoveFavoriteRecipeByIdAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Recipe>> UpdateRecipe(Guid id, [FromBody] Recipe recipe)
        {
            try
            {
                if (id != recipe.Id)
                {
                    return BadRequest("Url id does not match recipe id");
                }
                await recipeService.UpdateRecipeAsync(recipe);
                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecipe(Guid id)
        {
            try
            {
                await recipeService.DeleteRecipeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
