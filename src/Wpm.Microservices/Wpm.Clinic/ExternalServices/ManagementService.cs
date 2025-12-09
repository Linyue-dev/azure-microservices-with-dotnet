namespace Wpm.Clinic.ExternalServices
{
    public class ManagementService(HttpClient client)
    {
        public async Task<PetInfo> GetPetInfo(int id)
        {
            var petInfo = await client.GetFromJsonAsync<PetInfo>($"/api/pets/{id}");

            return petInfo;
        }
    }

    public record PetInfo(int Id, String Name, int Age, int BreedId);
}
