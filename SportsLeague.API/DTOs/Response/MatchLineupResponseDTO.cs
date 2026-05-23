namespace SportsLeague.API.DTOs.Response;

public class MatchLineupResponseDTO
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int TeamId { get; set; }
    public int Number { get; set; }
    public bool IsStarter { get; set; }
    public string Position { get; set; } = string.Empty;
}
