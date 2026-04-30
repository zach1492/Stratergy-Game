
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

public class Map
{
    public int width;
    public int height;
    public Player[] players;
    public bool loadingScene;
    public Tile[,] tiles;

    // Constructor
    public Map(int width, int height, Player[] players, bool loadingScene = false)
    {
        this.width = width;
        this.height = height;
        this.players = players;

        tiles = new Tile[width, height];
        this.loadingScene = loadingScene;


    }

    public void SpawnMap()
    {
        loadingScene = false;
        GenerateOceans();
        GenerateLand();
                GenerateMountains();

                                GenerateCities();

        GenerateBiomes();

        GenerateCoasts();
        GenerateCapitalRivers();

        GenerateRivers();
        GenerateForests();
        //AddNaturalPlainsPatches();

        GenerateResources();

    }

    public void GenerateOceans()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                tiles[x, y] = new Tile("Ocean", null, x, y, "", "");
    }

    public void GenerateCoasts()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y].tileType != "Ocean") continue;
                if (HasAdjacentLand(x, y))
                    tiles[x, y].tileType = "Coast";
            }
    }

    bool HasAdjacentLand(int x, int y)
    {
        foreach (Vector2Int n in GetHexNeighbors(x, y))
        {
            string type = tiles[n.x, n.y].tileType;
            if (type != "Ocean" && type != "Coast")
                return true;
        }
        return false;
    }

    List<Vector2Int> GetHexNeighbors(int x, int y)
    {
        bool odd = (y % 2 == 1);

        int[,] dirsEven =
        {
            { +1,  0 },
            {  0, +1 },
            { -1, +1 },
            { -1,  0 },
            { -1, -1 },
            {  0, -1 }
        };

        int[,] dirsOdd =
        {
            { +1,  0 },
            { +1, +1 },
            {  0, +1 },
            { -1,  0 },
            {  0, -1 },
            { +1, -1 }
        };

        int[,] d = odd ? dirsOdd : dirsEven;

        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = 0; i < 6; i++)
        {
            int nx = x + d[i, 0];
            int ny = y + d[i, 1];

            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                result.Add(new Vector2Int(nx, ny));
        }

        return result;
    }

    public void GenerateLand()
    {
        int continentCount = 0;//width/20;
        int continentSize =0;//width*8;

        for (int i = 0; i < continentCount; i++)
        {
            Vector2Int start = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
            GrowContinent(start, continentSize);
        }
    }

    void GrowContinent(Vector2Int start, int size)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);

        int created = 0;

        while (queue.Count > 0 && created < size)
        {
            Vector2Int pos = queue.Dequeue();
            int x = pos.x;
            int y = pos.y;

            if (x < 0 || x >= width || y < 0 || y >= height)
                continue;

            if (tiles[x, y].tileType == "Plains")
                continue;

            tiles[x, y].tileType = "Plains";
            created++;

            List<Vector2Int> dirs = new List<Vector2Int>()
            {
                new Vector2Int(x+1,y),
                new Vector2Int(x-1,y),
                new Vector2Int(x,y+1),
                new Vector2Int(x,y-1),
                new Vector2Int(x+1,y+1),
                new Vector2Int(x-1,y-1)
            };

            foreach (var d in dirs)
                if (Random.value < 0.6f)
                    queue.Enqueue(d);
        }
    }

public void GenerateBiomes()
{
    int snowHeight = Mathf.FloorToInt(height * 0.16f);

    // --- snow same as before ---
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < snowHeight; y++)
            if (tiles[x, y].tileType == "Plains"||tiles[x, y].tileType == "Desert")
                tiles[x, y].tileType = "Snow";

        for (int y = height - snowHeight; y < height; y++)
            if (tiles[x, y].tileType == "Plains"||tiles[x, y].tileType == "Desert")
                tiles[x, y].tileType = "Snow";
    }

    // --- DESERT FIX ---
    /*int desertTarget = width;

    Vector2Int seed;
    do
    {
        seed = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
    }
    while (tiles[seed.x, seed.y].tileType != "Plains");

    Queue<Vector2Int> q = new Queue<Vector2Int>();
    HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

    q.Enqueue(seed);
    visited.Add(seed);

    while (q.Count > 0 && desertTarget > 0)
    {
        Vector2Int p = q.Dequeue();
        int x = p.x;
        int y = p.y;

        if (tiles[x, y].tileType == "Plains")
        {
            tiles[x, y].tileType = "Desert";
            desertTarget--;
        }

        // Spread directions
        Vector2Int[] dirs =
        {
            new Vector2Int(x+1,y),
            new Vector2Int(x-1,y),
            new Vector2Int(x,y+1),
            new Vector2Int(x,y-1),
        };

        foreach (var d in dirs)
        {
            if (!visited.Contains(d) &&
                d.x >= 0 && d.x < width &&
                d.y >= 0 && d.y < height &&
                Random.value < 0.9f)
            {
                visited.Add(d);
                q.Enqueue(d);
            }
        }
    }*/
}

    public void GenerateMountains()
    {
        List<Vector2Int> potentialMountainTiles = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                string type = tiles[x, y].tileType;
                if ((type == "Snow" || type == "Plains"|| type == "Desert") && tiles[x, y].district == null)
                    potentialMountainTiles.Add(new Vector2Int(x, y));
            }

        int mountainCount = Mathf.FloorToInt(potentialMountainTiles.Count * 0.15f);

        System.Random rand = new System.Random();
        potentialMountainTiles = potentialMountainTiles.OrderBy(pos => rand.Next()).ToList();

        for (int i = 0; i < mountainCount; i++)
        {
            Vector2Int mountainTile = potentialMountainTiles[i];
            tiles[mountainTile.x, mountainTile.y].tileType = "Mountain";
        }
    }
public void GenerateCities()
{
    List<Vector2Int> capitalPositions = GetCapitalPositions(players.Length);

    for (int i = 0; i < players.Length; i++)
    {
        Player player = players[i];
        Vector2Int pos = capitalPositions[i];

        // Force valid land
            tiles[pos.x, pos.y].tileType = "District";
        tiles[pos.x, pos.y].forestResource = "";
        tiles[pos.x, pos.y].resource = "";
       // tiles[pos.x, pos.y].hasRoad = true;

        tiles[pos.x, pos.y].district = new City();
        tiles[pos.x, pos.y].district.building = new Palace();
        tiles[pos.x, pos.y].owner = player;
        tiles[pos.x, pos.y].unit = new Warrior(player);

        ClaimAdjacentTiles(pos.x, pos.y, player);

        // Terrain blob by tribe
        if (player.tribeType == "Rome" )
            ForceTerrainBlob(pos, "Plains", width*2, 0.2f,0f);
        else if(player.tribeType == "Greece")
            ForceTerrainBlob(pos, "Plains", width*2,0.2f,0.4f);

        else if (player.tribeType == "Eygpt")
            ForceTerrainBlob(pos, "Desert", width*2,0f,0f);
        else if( player.tribeType == "Persia")
            ForceTerrainBlob(pos, "Desert", width*2,0.4f,0f);

    }
}
/*
    public void GenerateCities()
    {
        foreach (Player player in players)
        {
            Vector2Int pos;
            do
            {
                pos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
            }
            while (!IsValidCitySpot(pos) ||!(tiles[pos.x, pos.y].tileType == "Plains"||tiles[pos.x, pos.y].tileType == "Snow"||tiles[pos.x, pos.y].tileType == "Desert"));

            tiles[pos.x, pos.y].district = new City();
            tiles[pos.x, pos.y].tileType = "District";
            tiles[pos.x, pos.y].forestResource="";
            tiles[pos.x, pos.y].resource="";
            tiles[pos.x, pos.y].district.building = new Palace();
            tiles[pos.x, pos.y].owner = player;
            tiles[pos.x, pos.y].unit = new Warrior(player);

            ClaimAdjacentTiles(pos.x, pos.y, player);
        }
    }
*/ 
    public void ClaimAdjacentTiles(int x, int y, Player player)
    {
        int[,] offsetsEven =
        {
            { +1,  0 },
            {  0, -1 },
            { -1, -1 },
            { -1,  0 },
            { -1, +1 },
            {  0, +1 }
        };

        int[,] offsetsOdd =
        {
            { +1,  0 },
            { +1, -1 },
            {  0, -1 },
            { -1,  0 },
            {  0, +1 },
            { +1, +1 }
        };

        bool isOdd = (y % 2 != 0);
        int[,] dirs = isOdd ? offsetsOdd : offsetsEven;

        for (int i = 0; i < 6; i++)
        {
            int nx = x + dirs[i, 0];
            int ny = y + dirs[i, 1];

            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                if(tiles[nx, ny].owner == null)
                tiles[nx, ny].owner = player;
        }
    }
    public void ClaimAdjacentTilesTroops(int x, int y, Player player)
    {
        int[,] offsetsEven =
        {
            { +1,  0 },
            {  0, -1 },
            { -1, -1 },
            { -1,  0 },
            { -1, +1 },
            {  0, +1 }
        };

        int[,] offsetsOdd =
        {
            { +1,  0 },
            { +1, -1 },
            {  0, -1 },
            { -1,  0 },
            {  0, +1 },
            { +1, +1 }
        };

        bool isOdd = (y % 2 != 0);
        int[,] dirs = isOdd ? offsetsOdd : offsetsEven;

        for (int i = 0; i < 6; i++)
        {
            int nx = x + dirs[i, 0];
            int ny = y + dirs[i, 1];

            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                if(tiles[nx, ny].district == null && !(tiles[nx, ny].unit != null && tiles[nx, ny].unit.owner != player))
                tiles[nx, ny].owner = player;
        }
    }
    bool IsValidCitySpot(Vector2Int pos)
    {
        if (tiles[pos.x, pos.y].district is City) return false;

        for (int dx = -2; dx <= 2; dx++)
            for (int dy = -2; dy <= 2; dy++)
            {
                int nx = pos.x + dx;
                int ny = pos.y + dy;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (tiles[nx, ny].district is City)
                        return false;
                }
            }

        return true;
    }
public void GenerateRivers()
{
    int riverAttempts = width/8;  // number of total rivers to try to generate
    int maxSteps = width * height; // safety limit so nothing freezes

    for (int r = 0; r < riverAttempts; r++)
    {
        Vector2Int start = FindRiverStart();
        if (start.x == -1) continue;

        Vector2Int current = start;
        tiles[current.x, current.y].tileType = "River";

        int steps = 0;

        while (steps < maxSteps)
        {
            steps++;

            if (IsAdjacentToCoast(current))
                break;

            List<Vector2Int> nextPossible = GetHexNeighbors(current.x, current.y)
                .Where(n => IsValidRiverTile(n))
                .Where(n => CountAdjacentRivers(n) <= 1) // prevents doubling back
                .ToList();

            if (nextPossible.Count == 0) break;

            Vector2Int next = nextPossible[Random.Range(0, nextPossible.Count)];
            current = next;
            tiles[current.x, current.y].tileType = "River";
        }
    }
}
Vector2Int FindRiverStart()
{
    List<Vector2Int> candidates = new List<Vector2Int>();

    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            string t = tiles[x, y].tileType;

            // Must be a land biome
            if (t != "Snow" && t != "Desert" && t != "Plains") continue;

            // Must NOT be adjacent to a coast
            bool nextToCoast = false;
            foreach (Vector2Int n in GetHexNeighbors(x, y))
            {
                if (tiles[n.x, n.y].tileType == "Coast")
                {
                    nextToCoast = true;
                    break;
                }
            }

            if (!nextToCoast)
                candidates.Add(new Vector2Int(x, y));
        }
    }

    if (candidates.Count == 0)
        return new Vector2Int(-1, -1);

    return candidates[Random.Range(0, candidates.Count)];
}

bool IsValidRiverTile(Vector2Int p)
{
    string t = tiles[p.x, p.y].tileType;
    return t == "Snow" || t == "Desert" || t == "Plains";
}
bool IsAdjacentToCoast(Vector2Int p)
{
    foreach (Vector2Int n in GetHexNeighbors(p.x, p.y))
        if (tiles[n.x, n.y].tileType == "Coast")
            return true;

    return false;
}
int CountAdjacentRivers(Vector2Int p)
{
    int count = 0;
    foreach (Vector2Int n in GetHexNeighbors(p.x, p.y))
        if (tiles[n.x, n.y].tileType == "River")
            count++;

    return count;
}
public void GenerateForests()
{
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            bool isArcticOrAntarctic = y < height * 0.06f || y > height * 0.90f;

            Tile tile = tiles[x, y];
            if(isArcticOrAntarctic){
                if(tile.tileType == "Ocean"||tile.tileType == "Coast"||tile.tileType=="River")
                    tile.forestResource = "Ice";
            }
            // Skip tiles with cities, rivers, mountains, oceans, or coasts
            if (tile.district is City || tile.tileType == "River" || tile.tileType == "Mountain" ||
                tile.tileType == "Ocean" || tile.tileType == "Coast")
                continue;



            // Desert → DryForest with 5% chance
            if (tile.tileType == "Desert" && Random.value < 0.03f)
            {
                tile.forestResource = "DryForest";
            }

            else if (tile.tileType == "Snow" && !isArcticOrAntarctic)
            {
                tile.forestResource = "BorealForest";
            }
            // Plains → Tropical/Temperate Forest
            else if (tile.tileType == "Plains")
            {
                tile.forestResource = IsInTropicalZone(x, y) ? "Rainforest" : "TemperateForest";
            }

        }
    }
}
public void AddNaturalPlainsPatches()
{
    int patchCount = width / 8;       // how many patches to spawn
    int patchSize = width / 3;        // how large each patch can grow

    for (int p = 0; p < patchCount; p++)
    {
Vector2Int start = new Vector2Int(-1, -1);

int attempts = 0;

do
{
    start = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
    attempts++;

    if (attempts > 200)
        return; // stop trying if no valid tile exists

}
while (tiles[start.x, start.y].tileType != "Plains" &&
       tiles[start.x, start.y].tileType != "Snow");

        // Only start on land
        if (tiles[start.x, start.y].tileType == "Ocean" ||
            tiles[start.x, start.y].tileType == "Coast" ||
            tiles[start.x, start.y].tileType == "River" ||
            tiles[start.x, start.y].tileType == "Mountain")
            continue;

        // Grow a plain region
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        q.Enqueue(start);

        int created = 0;

        while (q.Count > 0 && created < patchSize)
        {
            Vector2Int pos = q.Dequeue();
            int x = pos.x;
            int y = pos.y;

            if (x < 0 || x >= width || y < 0 || y >= height)
                continue;

            Tile t = tiles[x, y];
            if (t.forestResource == "") 
                continue;
            if (t.tileType == "Snow" ||t.tileType == "Plains")
                t.forestResource = "";

            created++;

            // Spread a bit
            foreach (var n in GetHexNeighbors(x, y))
            {
                if (Random.value < 0.7f)
                    q.Enqueue(n);
            }
        }
    }
}

// Adjust your tropical zone logic
bool IsInTropicalZone(int x, int y)
{
    return y > height * 0.35f && y < height * 0.65f;
}
public void GenerateResources()
{
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            Tile tile = tiles[x, y];

            //if (tile.forestResource != "") continue;
            if (tile.district != null) continue;

            // ------------------------------
            //       C R O P S  (YOU ASKED)
            // ------------------------------
            if ((tile.tileType == "Plains" || tile.tileType == "Desert") &&
                IsAdjacentToRiver(new Vector2Int(x, y)))
            {
                // Only if there is NO forest
                if (tile.forestResource == "")
                    if (Random.value < 0.9f) // 35% chance to avoid spawning crops everywhere
                        tile.resource = "Crop";
            }

            // ------------------------------
            //       H O R S E S
            // ------------------------------
            if (tile.tileType == "Plains" && tile.forestResource == "")
                if (Random.value < 0.3f)
                    tile.resource = "Horse";

            // ------------------------------
            //       D E E R
            // ------------------------------
            if (tile.tileType == "Snow" && tile.forestResource == "BorealForest")
                if (Random.value < 0.1f)
                    tile.resource = "Deer";

            // ------------------------------
            //       B O A R
            // ------------------------------
            if (tile.forestResource == "TemperateForest")
                if (Random.value < 0.2f)
                    tile.resource = "Boar";

            // ------------------------------
            //       F I S H
            // (spawn in coastal rivers or lakes)
            // ------------------------------
            if (tile.tileType == "Coast"||tile.tileType == "River")
                if (Random.value < 0.35f)
                    tile.resource = "Fish";
            if (tile.tileType == "Ocean")
                if (Random.value < 0.01f)
                    tile.resource = "Fish";
            if (tile.tileType == "Ocean")
                if (Random.value < 0.05f)
                    tile.resource = "Whale";
            // ------------------------------
            //       M E T A L / O R E
            // ------------------------------
            if (tile.tileType == "Mountain")
                //tile.resource = "Metal";

                if (Random.value < 0.5f)
                    tile.resource = "Metal";

            // ------------------------------
            //   P E N G U I N S (fun extra)
            // ------------------------------
            if (tile.tileType == "Snow" && tile.forestResource=="")
                if (Random.value < 0.05f)
                    tile.resource = "Penguin";
        }
    }
}
bool IsAdjacentToRiver(Vector2Int p)
{
    foreach (Vector2Int n in GetHexNeighbors(p.x, p.y))
        if (tiles[n.x, n.y].tileType == "River")
            return true;

    return false;
}

public bool IsConnectedToCapitalByRoad(Tile startTile, Player player)
{
    Tile capital = GetCapitalTile(player);
    if (capital == null) return false;

    // BOTH must have roads
    if (!startTile.hasRoad || !capital.hasRoad)
        return false;

    Queue<Tile> q = new Queue<Tile>();
    HashSet<Tile> visited = new HashSet<Tile>();

    q.Enqueue(startTile);
    visited.Add(startTile);

    while (q.Count > 0)
    {
        Tile current = q.Dequeue();

        if (current == capital)
            return true;

        foreach (Vector2Int n in GetHexNeighbors(current.x, current.y))
        {
            Tile next = tiles[n.x, n.y];

            if (visited.Contains(next)) continue;
            if (next.owner != player) continue;
            if (!next.hasRoad && !next.hasTradeRoute) continue;   // 🔒 road required everywhere

            visited.Add(next);
            q.Enqueue(next);
        }
    }

    return false;
}

public bool IsConnectedToCapitalByTrain(Tile startTile, Player player)
{
    Tile capital = GetCapitalTile(player);
    if (capital == null) return false;

    // BOTH must have train tracks
    if (!startTile.hasTrainTrack || !capital.hasTrainTrack)
        return false;

    Queue<Tile> q = new Queue<Tile>();
    HashSet<Tile> visited = new HashSet<Tile>();

    q.Enqueue(startTile);
    visited.Add(startTile);

    while (q.Count > 0)
    {
        Tile current = q.Dequeue();

        if (current == capital)
            return true;

        foreach (Vector2Int n in GetHexNeighbors(current.x, current.y))
        {
            Tile next = tiles[n.x, n.y];

            if (visited.Contains(next)) continue;
            if (next.owner != player) continue;
            if (!next.hasTrainTrack) continue; // 🔒 track required everywhere

            visited.Add(next);
            q.Enqueue(next);
        }
    }

    return false;
}

public Tile GetCapitalTile(Player player)
{
    for (int x = 0; x < width; x++)
    for (int y = 0; y < height; y++)
    {
        Tile t = tiles[x, y];
        if (t.owner == player &&
            t.district is City &&
            t.district.building is Palace)
            return t;
    }
    return null;
}
void ForceTerrainBlob(Vector2Int start, string terrainType, int size, float mountainRatio, float coastRatio)
{
    Queue<Vector2Int> q = new Queue<Vector2Int>();
    HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

    q.Enqueue(start);
    visited.Add(start);

    int created = 0;

    while (q.Count > 0 && created < size)
    {
        Vector2Int p = q.Dequeue();
        Tile t = tiles[p.x, p.y];

        if (t.tileType != "District" &&
            t.tileType != "" &&
            t.tileType != "" &&
            t.tileType != "River")
        {
            float randomValue = UnityEngine.Random.value; // Generates a float between 0 and 1

            if (randomValue < mountainRatio)
            {
                t.tileType = "Mountain"; // Set the tile type to Mountain
            }
            else if (randomValue < mountainRatio + coastRatio)
            {
                t.tileType = "Coast"; // Set the tile type to Coast
            }
            else
            {
                t.tileType = terrainType; // Otherwise, set it to the specified terrain type
            }
            t.forestResource = "";
            created++;
            
        }

        foreach (Vector2Int n in GetHexNeighbors(p.x, p.y))
        {
            if (!visited.Contains(n) && Random.value < 0.7f)
            {
                visited.Add(n);
                q.Enqueue(n);
            }
        }
    }
}

/*List<Vector2Int> GetCapitalPositions(int count)
{
    List<Vector2Int> positions = new List<Vector2Int>();

    int marginX = width / 3;
    int marginY = height /3;

    if (count == 1)
    {
        positions.Add(new Vector2Int(width / 2, height / 2));
    }
    else if (count == 2)
    {
        positions.Add(new Vector2Int(marginX, height / 2));
        positions.Add(new Vector2Int(width - marginX, height / 2));
    }
    else if (count == 3)
    {
        positions.Add(new Vector2Int(width / 2, marginY));
        positions.Add(new Vector2Int(marginX, height - marginY));
        positions.Add(new Vector2Int(width - marginX, height - marginY));
    }
    else if (count == 4)
    {
        positions.Add(new Vector2Int(marginX, marginY));
        positions.Add(new Vector2Int(width - marginX, marginY));
        positions.Add(new Vector2Int(marginX, height - marginY));
        positions.Add(new Vector2Int(width - marginX, height - marginY));
    }

    return positions;
*/
List<Vector2Int> GetCapitalPositions(int count)
{
    List<Vector2Int> positions = new List<Vector2Int>();

    int minDistance = 6;
    int attempts = 0;
int polarMargin = Mathf.FloorToInt(height * 0.20f);
    while (positions.Count < count && attempts < 500000)
    {
        attempts++;

        Vector2Int pos = new Vector2Int(
            Random.Range(2, width-2),
            Random.Range(polarMargin, height-polarMargin)
        );

        // Must be valid land
        string type = tiles[pos.x, pos.y].tileType;
        //if (type != "Plains" && type != "Snow" && type != "Desert")
            //continue;

        bool tooClose = false;

        foreach (Vector2Int existing in positions)
        {
            if (Vector2Int.Distance(pos, existing) < minDistance)
            {
                tooClose = true;
                break;
            }
        }

        if (!tooClose)
            positions.Add(pos);
    }

    return positions;
}
void GenerateCapitalRivers()
{
    foreach (Player player in players)
    {
        if(player.tribeType == "Greece")
            return;
        Tile capital = GetCapitalTile(player);
        if (capital == null) continue;

        Vector2Int capPos = new Vector2Int(capital.x, capital.y);

        // Find valid adjacent tile to start river
        List<Vector2Int> candidates = GetHexNeighbors(capPos.x, capPos.y)
            .Where(n =>
                tiles[n.x, n.y].tileType == "Plains" ||
                tiles[n.x, n.y].tileType == "Desert" ||
                tiles[n.x, n.y].tileType == "Snow")
            .ToList();

        if (candidates.Count == 0)
            continue;

        Vector2Int start = candidates[Random.Range(0, candidates.Count)];

        GrowRiverFrom(start);
    }
}
void GrowRiverFrom(Vector2Int start)
{
    int maxSteps = width * height;
    Vector2Int current = start;

    tiles[current.x, current.y].tileType = "River";

    int steps = 0;

    while (steps < maxSteps)
    {
        steps++;

        if (IsAdjacentToCoast(current))
            break;

        List<Vector2Int> nextPossible = GetHexNeighbors(current.x, current.y)
            .Where(n => IsValidRiverTile(n))
            .Where(n => CountAdjacentRivers(n) <= 1)
            .ToList();

        if (nextPossible.Count == 0)
            break;

        Vector2Int next = nextPossible[Random.Range(0, nextPossible.Count)];
        current = next;
        tiles[current.x, current.y].tileType = "River";
    }
}
public void DrawLoadingScreenBackground()
{
    // Step 1: Base ocean
    for (int x = 0; x < width; x++)
    for (int y = 0; y < height; y++)
        tiles[x, y] = new Tile("Ocean", null, x, y, "", "");

    // Step 2: Ice gradient (left side fully ice → fades out)
    for (int x = 0; x < width; x++)
    {
        float iceChance = 1f - ((float)x / (width * 0.45f)); // fade across ~45% of map
        iceChance = Mathf.Clamp01(iceChance);

        for (int y = 0; y < height; y++)
        {
            if (Random.value < iceChance)
                tiles[x, y].forestResource = "Ice";
        }
    }

    // Step 3: Spawn a few islands
    int islandCount = Random.Range(2, 6);

    for (int i = 0; i < islandCount; i++)
    {
        int ix = Random.Range(width / 3, width - 3);
        int iy = Random.Range(3, height - 3);

        CreateLoadingIsland(ix, iy);
    }
}

void CreateLoadingIsland(int cx, int cy)
{
    int size = Random.Range(4, 8);

    Queue<Vector2Int> q = new Queue<Vector2Int>();
    HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

    q.Enqueue(new Vector2Int(cx, cy));
    visited.Add(new Vector2Int(cx, cy));

    int created = 0;

    while (q.Count > 0 && created < size)
    {
        Vector2Int p = q.Dequeue();

        if (p.x < 1 || p.x >= width-1 || p.y < 1 || p.y >= height-1)
            continue;

        Tile t = tiles[p.x, p.y];

        if (t.tileType == "Ocean")
        {
            // Snow island or mountain island
            if (Random.value < 0.6f)
                t.tileType = "Snow";
            else
                t.tileType = "Mountain";

            t.forestResource = "";
            created++;

            foreach (Vector2Int n in GetHexNeighbors(p.x, p.y))
            {
                if (!visited.Contains(n) && Random.value < 0.7f)
                {
                    visited.Add(n);
                    q.Enqueue(n);
                }
            }
        }
    }

    // Step 4: Add coast around island
    foreach (Vector2Int n in GetHexNeighbors(cx, cy))
    {
        Tile t = tiles[n.x, n.y];
        if (t.tileType == "Ocean")
            t.tileType = "Coast";
    }
}}