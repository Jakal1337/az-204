using System.Collections.Generic;
using System.Linq;
using FoodApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodApi
{
    [Route ("[controller]")]
    [ApiController]
    public class FoodController : ControllerBase {


        public FoodController (FoodDBContext context, AILogger l) {
            ctx = context;
            logger = l;
        }

        private FoodDBContext ctx;
        private AILogger logger;

        // http://localhost:PORT/food
        [HttpGet ()]
        public IEnumerable<FoodItem> GetFood () {
            return ctx.Food.ToArray ();
        }

        // http://localhost:PORT/food/3
        [HttpGet ("{id}")]
        public FoodItem GetById (int id) {
            return ctx.Food.FirstOrDefault (v => v.ID == id);
        }

        // http://localhost:PORT/food
        [HttpPost ()]
        public FoodItem SaveFood (FoodItem item) {

            if(item.ID==0){
                ctx.Food.Add (item);
            }else{
                ctx.Food.Attach(item);
                ctx.Entry(item).State = EntityState.Modified;
            }           
            ctx.SaveChanges ();
            return item;
        }

        // http://localhost:PORT/food
        [HttpDelete ("{id}")]
        public ActionResult Delete (int id) {
            var v = GetById (id);
            if (v != null) {
                ctx.Remove (v);
                ctx.SaveChanges ();
                logger.LogEvent("delete-food",id.ToString());
            }
            return Ok ();
        }
    }
}