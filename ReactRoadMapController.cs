using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ReactJS_RoadMap;

namespace ReactJS_RoadMap
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReactProgressController(IConfiguration configuration) : ControllerBase
    {
        private readonly string _connStr = configuration.GetConnectionString("MyDb");

        [HttpGet]
        public IActionResult GetAll()
       
        {
            List<ProgramModel> progressList = new List<ProgramModel>();

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string query = "SELECT DayNumber, Title, Completed, EntryDate, UpdateDate FROM ReactProgress ORDER BY DayNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
               

                while (dr.Read())
                {
                    ProgramModel objrt = new ProgramModel
                    {
                        DayNumber = Convert.ToInt32(dr["DayNumber"]),
                        Title = dr["Title"].ToString(),
                        Completed = Convert.ToBoolean(dr["Completed"]),
                        EntryDate = Convert.ToDateTime(dr["EntryDate"]),
                        UpdateDate = dr["UpdateDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["UpdateDate"])
                    };

                    progressList.Add(objrt);
                }
            }

            return Ok(progressList);
        }

        public class UpdateRequest
        {
            public int DayNumber { get; set; }
            public bool Completed { get; set; }
        }

        [HttpPost("update")]
        public IActionResult UpdateDay([FromBody] UpdateRequest request)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string query = @"UPDATE ReactProgress 
                             SET Completed = @Completed, UpdateDate = GETDATE() 
                             WHERE DayNumber = @DayNumber";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DayNumber", request.DayNumber);
                cmd.Parameters.AddWithValue("@Completed", request.Completed);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound($"Day {request.DayNumber} not found.");
            }

            return Ok(new { message = "Progress updated." });
        }

    }
}
