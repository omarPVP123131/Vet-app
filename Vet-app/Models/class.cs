using VeterinaryManagementSystem.Models;

public class Client
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DocumentNumber { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class Patient
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; }
    public double? Weight { get; set; }
    public string Color { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    // Relaciones
    public Client Client { get; set; }
}


public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int VeterinarianId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Relaciones
    public Patient Patient { get; set; }
    public User Veterinarian { get; set; }
}


public class MedicalRecord
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int VeterinarianId { get; set; }
    public DateTime ConsultationDate { get; set; }
    public string Diagnosis { get; set; }
    public string Treatment { get; set; }
    public string Observations { get; set; }
    public DateTime? NextCheckupDate { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relaciones
    public Patient Patient { get; set; }
    public User Veterinarian { get; set; }
}

public class Purchase
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public int UserId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public double Total { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relaciones
    public Supplier Supplier { get; set; }
    public User User { get; set; }
}

public class PurchaseDetail
{
    public int Id { get; set; }
    public int PurchaseId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }

    // Relaciones
    public Purchase Purchase { get; set; }
    public Product Product { get; set; }
}

public class Sale
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int UserId { get; set; }
    public DateTime SaleDate { get; set; }
    public double Subtotal { get; set; }
    public double Tax { get; set; }
    public double Total { get; set; }
    public string PaymentMethod { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relaciones
    public Client Client { get; set; }
    public User User { get; set; }
}

public class SaleDetail
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double Discount { get; set; }

    // Relaciones
    public Sale Sale { get; set; }
    public Product Product { get; set; }
}

public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relaciones
    public User User { get; set; }
}
