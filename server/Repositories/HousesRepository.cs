

namespace csharp_gregslist_api.Repositories;

public class HousesRepository
{
    private readonly IDbConnection _db;
    public HousesRepository(IDbConnection db)
    {
        _db = db;
    }
    internal House CreateHouse(House houseData)
    {
        string sql = @"
        INSERT INTO
        houses(
            sqft,
            bathrooms,
            bedrooms,
            description,
            imgUrl,
            price,
            creatorId
        )
        VALUES(
            @Sqft,
            @Bathrooms,
            @Bedrooms,
            @Description,
            @ImgUrl,
            @Price,
            @CreatorId
        );

        SELECT * FROM houses WHERE id = LAST_INSERT_ID();";

        House house = _db.Query<House>(sql, houseData).FirstOrDefault();
        return house;
    }

    internal House GetHouseById(int houseId)
    {
        string sql = @"
        SELECT houses.*,
        accounts.*
        FROM houses
        JOIN accounts on accounts.id - cars.creatorId
        WHERE houses.id = @houseId;";
        House house = _db.Query<House, Account, House>(sql, (house, account) =>
        {
            house.Creator = account;
            return house;
        }, new { houseId }).FirstOrDefault();
        return house;
    }

    internal List<House> GetHouses()
    {
        string sql = @"
        SELECT
        houses.*,
        accounts.*
        FROM houses
        JOIN accounts on accounts.id = houses.creatorId;";

        List<House> houses = _db.Query<House, Account, House>(sql, (house, account) =>
        {
            house.Creator = account;
            return house;
        }).ToList();
        return houses;
    }

    internal House UpdateHouse(House remodelHouse)
    {
        string sql = @"
        UPDATE houses
        SET
        sqft = @Sqft,
        bathrooms = @Bathrooms,
        bedrooms = @Bedrooms,
        price = @Price,
        description = @Description,
        imgUrl = @ImgUrl
        WHERE id = @Id;
        
        SELECT 
        houses.*,
        accounts.*,
        FROM houses
        JOIN accounts on accounts.id = houses.creatorId
        WHERE houses.id = @Id;";

        House house = _db.Query<House, Account, House>(sql, (house, account) =>
        {
            house.Creator = account;
            return house;
        }, remodelHouse).FirstOrDefault();
        return house;
    }

    internal void TrashHouse(int houseId)
    {
        string sql = "DELETE FROM houses WHERE id = @houseId;";
        _db.Execute(sql, new { houseId });
    }
}