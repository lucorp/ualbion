using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
namespace UAlbion.Base
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Automap : ushort
    {
        TestMapIskai = 100,
        TestMapToronto = 101,
        TestMapToronto2 = 102,
        TestMapSrimalinar = 103,
        TestMapDrinno = 104,
        TestMapSemiBroken = 105,
        TestMapArgim = 106,
        TestMapArgim2 = 107,
        TestMapCeltic = 108,
        TestMapKhamulon = 109,
        Jirinaar = 110,
        HunterClan = 111,
        HunterClanDownstairs = 112,
        JirinaarTownHall = 113,
        DjiKasGuild = 114,
        DjiKasBasement = 115,
        DjiFadhGuild = 116,
        DjiKasBasement2 = 117,
        SnirdArmoury = 118,
        SpiceTrader = 119,
        WaniasShop = 120,
        HouseOfTheWinds = 121,
        OldFormerBuilding = 122,
        HunterClanCellar = 123,
        EmptyCeltHut = 124,
        EmptyCeltHutWide = 125,
        EmptyCeltHutTall = 126,
        SarenasHut = 127,
        PeleitosHut = 128,
        Garris = 129,
        Bragona = 130,
        Tharnos = 131,
        Winion = 132,
        Oibelos = 133,
        Tamno = 134,
        Dranbar = 135,
        BennosProvisions = 136,
        Aretha = 137,
        Rifrako = 138,
        Ferina = 139,
        ArjanoHut = 140,
        Arjano2D = 141,
        ArjanoLibrary = 142,
        Drinno2D = 143,
        Drinno = 144,
        Drinno2 = 145,
        Drinno3 = 146,
        Drinno4 = 147,
        Drinno5 = 148,
        BerosRoom = 149,
        TorontoPart1 = 150,
        TorontoPart2 = 151,
        TorontoPart22 = 152,
        TorontoPart3 = 153,
        Kenget = 154,
        Kenget2 = 155,
        Kenget3 = 156,
        Kenget4 = 157,
        Kenget5 = 158,
        Kenget6 = 159,
        Kenget7 = 160,
        Unnamed = 161,
        Kenget8 = 162,
        TestMapKenget = 163,
        OldFormerBuildingPostFight = 164,
        Unnamed2 = 165,
        LandingOnAlbion = 166,
        JirinaarCombatTrainer = 167,
        JirinaarCave = 168,
        GratogelCave = 169,
        MainiSouthCave = 170,
        MainiNorthCave = 171,
        UmajoCave = 172,
        DjiCantosCave = 173,
        Endgame = 174,
        Unknown175 = 175,
        Unknown176 = 176,
        Unknown177 = 177,
        Unknown178 = 178,
        Unknown179 = 179,
        Unknown180 = 180,
        Unknown181 = 181,
        Unknown182 = 182,
        Unknown183 = 183,
        Unknown184 = 184,
        Unknown185 = 185,
        Unknown186 = 186,
        Unknown187 = 187,
        Unknown188 = 188,
        Unknown189 = 189,
        Broken = 190,
        Unknown191 = 191,
        Unknown192 = 192,
        Unknown193 = 193,
        Unknown194 = 194,
        LoadTestMap = 195,
        Unnamed3 = 196,
        TestMap = 197,
        TestMap2 = 198,
        AlbionShortcutMap = 199,
        Nakiridaani = 200,
        GratogelNorth = 201,
        GratogelSouth = 202,
        Maini = 203,
        Maini2 = 204,
        Maini3 = 205,
        Maini4 = 206,
        Maini5 = 207,
        Unknown208 = 208,
        Unknown209 = 209,
        TestMapOutdoors = 210,
        TestMapGraphics = 211,
        IskaiHolySite = 212,
        Kontos = 213,
        TestMapGraphics2 = 214,
        Umajo = 215,
        Umajo2 = 216,
        Umajo3 = 217,
        Unnamed4 = 218,
        Umajo4 = 219,
        Unknown220 = 220,
        Unknown221 = 221,
        Unknown222 = 222,
        Unknown223 = 223,
        Unknown224 = 224,
        Unknown225 = 225,
        Unknown226 = 226,
        Unknown227 = 227,
        Unknown228 = 228,
        Unknown229 = 229,
        DeviceMakerGuild = 230,
        GemCutterGuild = 231,
        WeaponSmithGuild = 232,
        MinersGuild = 233,
        Prison2D = 234,
        UmajoKenta = 235,
        KylaProvisions = 236,
        UmajoMixedGoods = 237,
        UmajoPrison = 238,
        ErzmineGuestHouse = 239,
        Sojekos = 240,
        KylasHouse = 241,
        MountainPass = 242,
        DeviceMakerDungeon2D = 243,
        DeviceMaker3D = 244,
        DeviceMaker3D2 = 245,
        DeviceMaker3D3 = 246,
        DeviceMakerChamber = 247,
        MineEntrance = 248,
        Unnamed5 = 249,
        Unnamed6 = 250,
        Unnamed7 = 251,
        KounosCave = 252,
        KounosCave2 = 253,
        KounosCave3 = 254,
        KounosCave4 = 255,
        KounosCave5 = 256,
        Unknown257 = 257,
        Unknown258 = 258,
        Unknown259 = 259,
        BelovenoHostel = 260,
        SiobhansHouse = 261,
        SiobhansCellar = 262,
        SouthernResidence = 263,
        Kariah = 264,
        BelovenoTownHall = 265,
        NorthwesternResidence = 266,
        DoloProvisions = 267,
        BaggaEquipment = 268,
        PoschWeapons = 269,
        RioleaMixedGoods = 270,
        RaminaHealer = 271,
        Unnamed8 = 272,
        KounosTrader = 273,
        Darios = 274,
        KounosGuestHouse = 275,
        KontosLabyrinth = 276,
        KontosLabyrinth2 = 277,
        NadjeWeapons = 278,
        SrimalinarMageGuild = 279,
        Arrim = 280,
        Edjirr = 281,
        HolySiteBasement = 282,
        Beloveno = 283,
        Srimalinar = 284,
        Unknown285 = 285,
        Unknown286 = 286,
        Unknown287 = 287,
        Unknown288 = 288,
        Unknown289 = 289,
        TestMapDesert = 290,
        TestMapDjiCantos = 291,
        Unnamed9 = 292,
        TestItems = 293,
        TestMapKengetKamulos = 294,
        TestMapMahinoHouse = 295,
        Unknown296 = 296,
        Unnamed10 = 297,
        Unnamed11 = 298,
        TestMapHouse = 299,
        TorontoBegin = 300,
        TorontoReactor = 301,
        TorontoArrival = 302,
        TorontoDiscovery = 303,
        TorontoDiscoveryWithJoe = 304,
        TorontoReactorWithAI = 305,
        Unknown306 = 306,
        Unknown307 = 307,
        Unknown308 = 308,
        Unknown309 = 309,
        KengetPrison = 310,
        Kenget2D = 311,
        KengetSlaveQuarters = 312,
        Unnamed12 = 313,
        Unknown314 = 314,
        Unknown315 = 315,
        Unknown316 = 316,
        Unknown317 = 317,
        Unknown318 = 318,
        Unknown319 = 319,
        IsleOfPeace = 320,
        Unknown321 = 321,
        CantosHouse = 322,
        Unknown323 = 323,
        Unknown324 = 324,
        Unknown325 = 325,
        Unknown326 = 326,
        Unknown327 = 327,
        Unknown328 = 328,
        Unknown329 = 329,
        Unknown330 = 330,
        Unknown331 = 331,
        Unknown332 = 332,
        Unknown333 = 333,
        Unknown334 = 334,
        Unknown335 = 335,
        Unknown336 = 336,
        Unknown337 = 337,
        Unknown338 = 338,
        Unknown339 = 339,
        Unknown340 = 340,
        Unknown341 = 341,
        Unknown342 = 342,
        Unknown343 = 343,
        Unknown344 = 344,
        Unknown345 = 345,
        Unknown346 = 346,
        Unknown347 = 347,
        Unknown348 = 348,
        Unknown349 = 349,
        Unknown350 = 350,
        Unknown351 = 351,
        Unknown352 = 352,
        Unknown353 = 353,
        Unknown354 = 354,
        Unknown355 = 355,
        Unknown356 = 356,
        Unknown357 = 357,
        Unknown358 = 358,
        Unknown359 = 359,
        Unknown360 = 360,
        Unknown361 = 361,
        Unknown362 = 362,
        Unknown363 = 363,
        Unknown364 = 364,
        Unknown365 = 365,
        Unknown366 = 366,
        Unknown367 = 367,
        Unknown368 = 368,
        Unknown369 = 369,
        Unknown370 = 370,
        Unknown371 = 371,
        Unknown372 = 372,
        Unknown373 = 373,
        Unknown374 = 374,
        Unknown375 = 375,
        Unknown376 = 376,
        Unknown377 = 377,
        Unknown378 = 378,
        Unknown379 = 379,
        Unknown380 = 380,
        Unknown381 = 381,
        Unknown382 = 382,
        Unknown383 = 383,
        Unknown384 = 384,
        Unknown385 = 385,
        Unknown386 = 386,
        Unknown387 = 387,
        Unnamed13 = 388,
        Unnamed14 = 389,
        Unnamed15 = 390,
        Unknown391 = 391,
        Unknown392 = 392,
        Unknown393 = 393,
        Unknown394 = 394,
        Unknown395 = 395,
        Unknown396 = 396,
        Unknown397 = 397,
        Unnamed16 = 398,
        Unnamed17 = 399,
    }
}
