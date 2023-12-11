using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo1225.Dtos;
using WebApiDemo1225.Entities;

namespace WebApiDemo1225.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        //localhost:7076/api/Player      GET 
        //localhost:7076/api/Player/1    GET
        //localhost:7076/api/Player {"id":1,"name":"Leyla"}    POST
        //localhost:7076/api/Player/1 {"id":1,"name":"Aysel"}  PUT
        //localhost:7076/api/Player/1    DELETE

        public static List<Player> Players { get; set; } = new List<Player>
        {
            new Player
            {
                Id=1,
                 City="Baku",
                  PlayerName="Leyla",
                   Score=99
            },
            new Player
            {
                Id=2,
                 City="Gence",
                  PlayerName="Arif",
                   Score=90
            },
            new Player
            {
                Id=3,
                 City="Sumqayit",
                  PlayerName="Eli",
                   Score=77
            }
        };

        [HttpGet]
        public IEnumerable<PlayerDto> Get()
        {
            var result = Players.Select(x =>
            {
                return new PlayerDto
                {
                    Id = x.Id,
                    PlayerName = x.PlayerName,
                    Score = x.Score
                };
            });
            return result;
        }

        [HttpGet("BestStudents")]
        public IEnumerable<PlayerDto> GetBestStudents()
        {
            var result = Players.Where(p=>p.Score>=85).Select(x =>
            {
                return new PlayerDto
                {
                    Id = x.Id,
                    PlayerName = x.PlayerName,
                    Score = x.Score
                };
            });
            return result;
        }

        [HttpGet("Search")]
        public IEnumerable<PlayerExtendDto> Search(string key)
        {
            var keyResult = key.ToLower().Trim();
            var result = Players.Where(p => p.City.ToLower().Contains(keyResult)
            || p.PlayerName.ToLower().Contains(keyResult)).Select(x =>
            {
                return new PlayerExtendDto
                {
                    Id = x.Id,
                    PlayerName = x.PlayerName,
                    Score = x.Score,
                    City = x.City
                };
            });
            return result;
        }

        //[HttpGet("{id}")]
        //public PlayerDto? Get(int id)
        //{
        //    var player = Players.FirstOrDefault(x => x.Id == id);
        //    if (player != null)
        //    {
        //        var dataToReturn = new PlayerDto
        //        {
        //            Id = player.Id,
        //            PlayerName = player.PlayerName,
        //            Score = player.Score
        //        };
        //        return dataToReturn;
        //    }
        //    return null;
        //}

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var player = Players.FirstOrDefault(x => x.Id == id);
            if (player != null)
            {
                var dataToReturn = new PlayerDto
                {
                    Id = player.Id,
                    PlayerName = player.PlayerName,
                    Score = player.Score
                };
                return Ok(dataToReturn);
            }
            return NotFound();
        }


        [HttpPost]
        public IActionResult Post([FromBody] PlayerAddDto dto)
        {
            try
            {
                var player = new Player
                {
                    City = dto.City,
                    PlayerName = dto.PlayerName,
                    Score = dto.Score,
                    Id = dto.Id
                };
                Players.Add(player);
                return Ok(player);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] PlayerAddDto dto) {
            try
            {
                var item=Players.FirstOrDefault(x=>x.Id == id);
                if (item == null)
                {
                    return NotFound();
                }
                item.City=dto.City;
                item.PlayerName=dto.PlayerName;
                item.Score=dto.Score;

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var item=Players.FirstOrDefault(p=>p.Id==id);
                if (item == null)
                {
                    return NotFound();
                }
                Players.Remove(item);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
