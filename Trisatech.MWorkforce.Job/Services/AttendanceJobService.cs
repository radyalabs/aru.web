using Npgsql;

namespace Trisatech.MWorkforce.Job.Services;
public interface IAttendanceJobService {
    public Task AutomaticCheckout();
}

public class AttendanceJobService : IAttendanceJobService
{
    private readonly IConfiguration _config;
    public AttendanceJobService(IConfiguration config)
    {
        _config = config;
    }
    public async Task AutomaticCheckout()
    {
        await using var conn = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();
 
        var query = $"UPDATE public.\"AssignmentGroups\" set \"EndTime\" = current_timestamp where \"EndTime\"  is null and to_char(\"StartTime\", 'yyyy-MM-dd') = '{DateTime.UtcNow.ToString("yyyy-MM-dd")}'";
 
        await using (var cmd = new NpgsqlCommand(query, conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }
    }
}