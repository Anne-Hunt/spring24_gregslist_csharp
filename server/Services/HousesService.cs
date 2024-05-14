

namespace csharp_gregslist_api.Services;

public class HousesService
{

    private readonly HousesRepository _repository;
    public HousesService(HousesRepository repository)
    {
        _repository = repository;
    }
    internal House CreateHouse(House houseData)
    {
        House house = _repository.CreateHouse(houseData);
        return house;
    }

    internal List<House> GetHouses()
    {
        List<House> houses = _repository.GetHouses();
        return houses;
    }

    internal House GetHouseById(int houseId)
    {
        House house = _repository.GetHouseById(houseId);
        if (house == null)
        {
            throw new Exception("No house at that address, bro!");
        }
        return house;
    }


    internal House UpdateHouse(int houseId, string userId, House houseData)
    {
        House remodelHouse = GetHouseById(houseId);

        if (remodelHouse.CreatorId != userId)
        {
            throw new Exception("Can't remodel what you don't own!");
        }

        remodelHouse.Bathrooms = houseData.Bathrooms ?? remodelHouse.Bathrooms;
        remodelHouse.Bedrooms = houseData.Bedrooms ?? remodelHouse.Bedrooms;
        remodelHouse.Description = houseData.Description ?? remodelHouse.Description;
        remodelHouse.ImgUrl = houseData.ImgUrl ?? remodelHouse.ImgUrl;
        remodelHouse.Price = houseData.Price ?? remodelHouse.Price;
        remodelHouse.Sqft = houseData.Sqft ?? remodelHouse.Sqft;

        House fixedHouse = _repository.UpdateHouse(remodelHouse);
        return fixedHouse;
    }

    internal string TrashHouse(int houseId, string userId)
    {
        House houseTrash = GetHouseById(houseId);
        if (houseTrash.CreatorId != userId)
        {
            throw new Exception("You can't destroy what isn't yours!");
        }

        _repository.TrashHouse(houseId);

        return $"houseTrash.Id has been deleted";
    }

}