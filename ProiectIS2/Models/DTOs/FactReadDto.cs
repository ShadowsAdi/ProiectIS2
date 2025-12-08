namespace ProiectIS2.Models.DTOs;

public class FactReadDto
{
    public int Id { get; set; }
    public string FactText { get; set; } // Putem redenumi proprietățile pentru claritate
    public string CategoryName { get; set; } // Returnăm numele categoriei, nu doar ID-ul
    public DateTime CreatedDate { get; set; }
}