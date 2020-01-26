using System;
using Microsoft.AspNetCore.Mvc;
using GridManager;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web.Http.Cors;

namespace GridViewer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GridController
    {
        public GridController()
        {
        }


      //  [EnableCors(origins: "https://localhost:5001", headers: "*", methods: "*")]
        [HttpGet]
        [Route("Triangle")]
        public Triangle GetTriangle(char row, int column)
        {
            var grid = new Grid();
            var result = grid.GetTriangle(row, column);
            return result;
        }


      //  [EnableCors(origins: "https://localhost:5001", headers: "*", methods: "*")]
        [HttpGet]
        [Route("Position")]
        public Position GetPosition(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            var grid = new Grid();
            
            var result = grid.GetPosition(x1, y1, x2, y2, x3, y3);
            return result;
        }

        [HttpGet]
        [Route("Stuff")]
        public String GetStuff()
        {
            return "yes!";
        }
    }
}
